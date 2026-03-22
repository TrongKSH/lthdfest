using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using FestivalApi.Options;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Upload;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;

namespace FestivalApi.Services;

public sealed class GoogleDriveSheetsPaymentService
{
    private const string DevStorageReadWrite = "https://www.googleapis.com/auth/devstorage.read_write";

    private readonly GooglePaymentOptions _options;
    private readonly ILogger<GoogleDriveSheetsPaymentService> _logger;

    public GoogleDriveSheetsPaymentService(
        IOptions<GooglePaymentOptions> options,
        ILogger<GoogleDriveSheetsPaymentService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>True when service account + spreadsheet id are set (sheet append works).</summary>
    public bool IsConfigured => _options.IsSheetsConfigured;

    /// <summary>True when GCS bucket or Drive folder is configured (image upload enabled).</summary>
    public bool HasFileStorage => _options.HasFileStorage;

    public async Task<TicketPaymentProofResult> StoreProofAsync(
        Stream? fileStream,
        string contentType,
        string originalFileName,
        string fullName,
        string phone,
        string email,
        string ticketCode,
        int qty,
        CancellationToken cancellationToken = default)
    {
        if (!_options.IsSheetsConfigured)
            throw new InvalidOperationException("Google Sheets payment config is not set.");

        TicketPaymentProofResult proofMeta = new(null, null, null, null);
        var paymentProofLinkLabel =
            $"{SanitizePhoneForFileName(phone)}_{SanitizeTicketCodeForFileName(ticketCode)}_{qty}";

        if (_options.HasFileStorage)
        {
            if (fileStream == null)
                throw new InvalidOperationException("File stream is required when file storage is configured.");

            var phoneSeg = SanitizePhoneForFileName(phone);
            var safeTicket = SanitizeTicketCodeForFileName(ticketCode);
            var ext = GetSafeImageExtension(originalFileName, contentType);
            var baseFileName = $"{phoneSeg}_{safeTicket}_{qty}";
            var daySegment = GetVietnamDateFolderName();

            if (!string.IsNullOrWhiteSpace(_options.GcsBucketName))
            {
                proofMeta = await UploadToGcsAsync(
                        fileStream,
                        contentType,
                        baseFileName,
                        ext,
                        daySegment,
                        cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                proofMeta = await UploadToDriveAsync(
                        fileStream,
                        contentType,
                        baseFileName,
                        ext,
                        daySegment,
                        cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        else
        {
            if (fileStream != null)
            {
                await fileStream.CopyToAsync(Stream.Null, cancellationToken).ConfigureAwait(false);
                _logger.LogInformation(
                    "Payment proof image received but no GCS/Drive configured; row will be appended to Sheet only.");
            }
        }

        await AppendSheetRowAsync(
                fullName,
                phone,
                email,
                ticketCode,
                qty,
                proofMeta.PaymentProofUrl,
                paymentProofLinkLabel,
                cancellationToken)
            .ConfigureAwait(false);

        return proofMeta;
    }

    private async Task<TicketPaymentProofResult> UploadToGcsAsync(
        Stream fileStream,
        string contentType,
        string baseFileName,
        string ext,
        string daySegment,
        CancellationToken cancellationToken)
    {
        var credential = CreateCredential(DevStorageReadWrite);
        var storage = StorageClient.Create(credential);

        var prefix = (_options.GcsObjectPrefix ?? "payment-proofs").Trim().Trim('/');
        var objectName = string.IsNullOrEmpty(prefix)
            ? $"{daySegment}/{baseFileName}{ext}"
            : $"{prefix}/{daySegment}/{baseFileName}{ext}";

        await storage
            .UploadObjectAsync(_options.GcsBucketName, objectName, contentType, fileStream, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gs = $"gs://{_options.GcsBucketName}/{objectName}";
        var httpsUrl = BuildGcsObjectHttpsUrl(_options.GcsBucketName!, objectName);
        _logger.LogInformation("Uploaded payment proof to GCS {Uri}", gs);

        return new TicketPaymentProofResult(null, null, gs, httpsUrl);
    }

    private async Task<TicketPaymentProofResult> UploadToDriveAsync(
        Stream fileStream,
        string contentType,
        string baseFileName,
        string ext,
        string daySegment,
        CancellationToken cancellationToken)
    {
        var credential = CreateCredential(DriveService.Scope.Drive, SheetsService.Scope.Spreadsheets);

        var drive = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "LTHD Festival API",
        });

        var rootId = _options.DrivePaymentRootFolderId!;
        var dayFolderId = await GetOrCreateDayFolderAsync(drive, rootId, daySegment, cancellationToken)
            .ConfigureAwait(false);

        var driveFileName = $"{baseFileName}{ext}";

        var fileMetadata = new Google.Apis.Drive.v3.Data.File
        {
            Name = driveFileName,
            Parents = new List<string> { dayFolderId },
        };

        var createRequest = drive.Files.Create(fileMetadata, fileStream, contentType);
        createRequest.Fields = "id, webViewLink";
        createRequest.SupportsAllDrives = true;
        var upload = await createRequest.UploadAsync(cancellationToken).ConfigureAwait(false);
        if (upload.Status != UploadStatus.Completed || createRequest.ResponseBody == null)
        {
            _logger.LogError("Drive upload failed: {Status} {Error}", upload.Status, upload.Exception);
            throw new IOException($"Drive upload failed: {upload.Status}");
        }

        var driveFile = createRequest.ResponseBody;
        var link = driveFile.WebViewLink;
        return new TicketPaymentProofResult(driveFile.Id, link, null, link);
    }

    private async Task AppendSheetRowAsync(
        string fullName,
        string phone,
        string email,
        string ticketCode,
        int qty,
        string? paymentProofUrl,
        string paymentProofLinkLabel,
        CancellationToken cancellationToken)
    {
        var credential = CreateCredential(SheetsService.Scope.Spreadsheets);

        var sheets = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "LTHD Festival API",
        });

        var timestamp = DateTimeOffset.UtcNow.ToString("O", CultureInfo.InvariantCulture);
        // Append needs a bounded A1 range: unbounded "Sheet1!A:G" returns "Unable to parse range" on the API.
        var sheetTitle = FormatSheetTitleForAppend(_options.SheetsTabName);
        const int appendMaxRow = 100_000;
        var range = $"{sheetTitle}!A1:G{appendMaxRow}";
        var valueRange = new ValueRange
        {
            Values = new IList<object>[]
            {
                new List<object>
                {
                    timestamp,
                    fullName,
                    phone,
                    email,
                    ticketCode,
                    qty,
                    string.IsNullOrWhiteSpace(paymentProofUrl)
                        ? string.Empty
                        : paymentProofLinkLabel,
                },
            },
        };

        var appendRequest = sheets.Spreadsheets.Values.Append(valueRange, _options.SheetsSpreadsheetId, range);
        appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
        appendRequest.InsertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;

        var appendResponse = await appendRequest.ExecuteAsync(cancellationToken).ConfigureAwait(false);
        var updatedRange = appendResponse.Updates?.UpdatedRange;
        if (string.IsNullOrEmpty(updatedRange)
            || !TrySplitSheetTitleAndCellRange(updatedRange, out var parsedSheetTitle, out var parsedCellRange))
        {
            return;
        }

        try
        {
            var spreadsheet = await sheets.Spreadsheets.Get(_options.SheetsSpreadsheetId!)
                .ExecuteAsync(cancellationToken)
                .ConfigureAwait(false);

            await ClearBoldOnAppendedRangeAsync(
                    sheets,
                    _options.SheetsSpreadsheetId!,
                    spreadsheet,
                    parsedSheetTitle,
                    parsedCellRange,
                    cancellationToken)
                .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(paymentProofUrl))
            {
                await ApplyPaymentProofLinkToColumnGAsync(
                        sheets,
                        _options.SheetsSpreadsheetId!,
                        spreadsheet,
                        parsedSheetTitle,
                        parsedCellRange,
                        paymentProofUrl,
                        paymentProofLinkLabel,
                        cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Post-append sheet formatting (bold / PaymentProof link) failed; the row was still appended.");
        }
    }

    /// <summary>
    /// New rows often inherit bold from the header row / table style; force regular weight on the appended range.
    /// </summary>
    private async Task ClearBoldOnAppendedRangeAsync(
        SheetsService sheets,
        string spreadsheetId,
        Spreadsheet spreadsheet,
        string sheetTitle,
        string cellRange,
        CancellationToken cancellationToken)
    {
        var grid = TryBuildGridRangeFromA1(spreadsheet, sheetTitle, cellRange);
        if (grid == null)
        {
            _logger.LogWarning("Could not build grid range from {Sheet}!{Cells}", sheetTitle, cellRange);
            return;
        }

        var batch = new BatchUpdateSpreadsheetRequest
        {
            Requests = new List<Request>
            {
                new()
                {
                    RepeatCell = new RepeatCellRequest
                    {
                        Range = grid,
                        Cell = new CellData
                        {
                            UserEnteredFormat = new CellFormat
                            {
                                TextFormat = new TextFormat { Bold = false },
                            },
                        },
                        Fields = "userEnteredFormat.textFormat.bold",
                    },
                },
            },
        };

        try
        {
            await sheets.Spreadsheets.BatchUpdate(batch, spreadsheetId)
                .ExecuteAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not clear bold on appended row; values were still written.");
        }
    }

    /// <summary>
    /// Turns column G into a real Sheets hyperlink (display text already appended) — avoids <c>=HYPERLINK</c> locale errors (, vs ;).
    /// </summary>
    private async Task ApplyPaymentProofLinkToColumnGAsync(
        SheetsService sheets,
        string spreadsheetId,
        Spreadsheet spreadsheet,
        string sheetTitle,
        string cellRange,
        string paymentProofUrl,
        string paymentProofLinkLabel,
        CancellationToken cancellationToken)
    {
        const int columnGZeroBased = 6;
        var gridG = TryBuildGridRangeForColumnIndex(spreadsheet, sheetTitle, cellRange, columnGZeroBased);
        if (gridG == null)
        {
            _logger.LogWarning(
                "Could not build column G grid range from {Sheet}!{Cells} for hyperlink",
                sheetTitle,
                cellRange);
            return;
        }

        var rowCount = (int)(gridG.EndRowIndex ?? 0) - (int)(gridG.StartRowIndex ?? 0);
        if (rowCount < 1)
            return;

        var rows = new List<RowData>(rowCount);
        for (var i = 0; i < rowCount; i++)
        {
            rows.Add(
                new RowData
                {
                    Values = new List<CellData>
                    {
                        new()
                        {
                            UserEnteredValue = new ExtendedValue { StringValue = paymentProofLinkLabel },
                            UserEnteredFormat = new CellFormat
                            {
                                TextFormat = new TextFormat
                                {
                                    Link = new Link { Uri = paymentProofUrl },
                                },
                            },
                        },
                    },
                });
        }

        var batch = new BatchUpdateSpreadsheetRequest
        {
            Requests = new List<Request>
            {
                new()
                {
                    UpdateCells = new UpdateCellsRequest
                    {
                        Range = gridG,
                        Rows = rows,
                        Fields = "userEnteredValue,userEnteredFormat.textFormat.link",
                    },
                },
            },
        };

        try
        {
            await sheets.Spreadsheets.BatchUpdate(batch, spreadsheetId)
                .ExecuteAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not apply PaymentProof hyperlink; plain text label was still written.");
        }
    }

    private static bool TrySplitSheetTitleAndCellRange(string updatedRangeA1, out string sheetTitle, out string cellRange)
    {
        sheetTitle = "";
        cellRange = "";
        if (string.IsNullOrWhiteSpace(updatedRangeA1))
            return false;

        var s = updatedRangeA1.Trim();
        if (s.StartsWith("'", StringComparison.Ordinal))
        {
            for (var i = 1; i < s.Length; i++)
            {
                if (s[i] == '\'')
                {
                    if (i + 1 < s.Length && s[i + 1] == '\'')
                    {
                        i++;
                        continue;
                    }

                    if (i + 1 < s.Length && s[i + 1] == '!')
                    {
                        sheetTitle = s.Substring(1, i - 1).Replace("''", "'", StringComparison.Ordinal);
                        cellRange = s[(i + 2)..];
                        return cellRange.Length > 0;
                    }

                    return false;
                }
            }

            return false;
        }

        var bang = s.IndexOf('!', StringComparison.Ordinal);
        if (bang <= 0)
            return false;
        sheetTitle = s[..bang];
        cellRange = s[(bang + 1)..];
        return cellRange.Length > 0;
    }

    private static GridRange? TryBuildGridRangeFromA1(
        Spreadsheet spreadsheet,
        string sheetTitle,
        string cellRangePart)
    {
        var sheet = spreadsheet.Sheets?.FirstOrDefault(sh => sh.Properties?.Title == sheetTitle);
        if (sheet?.Properties?.SheetId == null)
            return null;

        var sheetId = sheet.Properties.SheetId.Value;

        var m = Regex.Match(
            cellRangePart.Trim(),
            @"^([A-Za-z]+)(\d+):([A-Za-z]+)(\d+)$",
            RegexOptions.CultureInvariant);
        if (!m.Success)
            return null;

        var c1 = ColumnLettersToZeroBasedIndex(m.Groups[1].Value);
        var r1 = int.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture);
        var c2 = ColumnLettersToZeroBasedIndex(m.Groups[3].Value);
        var r2 = int.Parse(m.Groups[4].Value, CultureInfo.InvariantCulture);

        return new GridRange
        {
            SheetId = sheetId,
            StartRowIndex = r1 - 1,
            EndRowIndex = r2,
            StartColumnIndex = c1,
            EndColumnIndex = c2 + 1,
        };
    }

    /// <summary>Same rows as the append A1 range, restricted to one column (e.g. G = index 6).</summary>
    private static GridRange? TryBuildGridRangeForColumnIndex(
        Spreadsheet spreadsheet,
        string sheetTitle,
        string cellRangePart,
        int columnIndexZeroBased)
    {
        var sheet = spreadsheet.Sheets?.FirstOrDefault(sh => sh.Properties?.Title == sheetTitle);
        if (sheet?.Properties?.SheetId == null)
            return null;

        var sheetId = sheet.Properties.SheetId.Value;

        var m = Regex.Match(
            cellRangePart.Trim(),
            @"^([A-Za-z]+)(\d+):([A-Za-z]+)(\d+)$",
            RegexOptions.CultureInvariant);
        if (!m.Success)
            return null;

        var r1 = int.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture);
        var r2 = int.Parse(m.Groups[4].Value, CultureInfo.InvariantCulture);
        var rowStart = Math.Min(r1, r2);
        var rowEnd = Math.Max(r1, r2);

        return new GridRange
        {
            SheetId = sheetId,
            StartRowIndex = rowStart - 1,
            EndRowIndex = rowEnd,
            StartColumnIndex = columnIndexZeroBased,
            EndColumnIndex = columnIndexZeroBased + 1,
        };
    }

    private static int ColumnLettersToZeroBasedIndex(string letters)
    {
        var n = 0;
        foreach (var c in letters.ToUpperInvariant())
        {
            if (c is < 'A' or > 'Z')
                throw new FormatException($"Invalid column letters: {letters}");
            n = n * 26 + (c - 'A' + 1);
        }

        return n - 1;
    }

    private GoogleCredential CreateCredential(params string[] scopes)
    {
        var normalizedJson = GoogleServiceAccountJsonNormalizer.Normalize(_options.ServiceAccountJson);
        var jsonBytes = Encoding.UTF8.GetBytes(normalizedJson);
        using var credentialStream = new MemoryStream(jsonBytes, writable: false);
#pragma warning disable CS0618
        return GoogleCredential.FromStream(credentialStream).CreateScoped(scopes);
#pragma warning restore CS0618
    }

    private static string GetVietnamDateFolderName()
    {
        var offset = TimeSpan.FromHours(7);
        var local = DateTimeOffset.UtcNow.ToOffset(offset);
        return local.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    private async Task<string> GetOrCreateDayFolderAsync(
        DriveService drive,
        string parentId,
        string folderName,
        CancellationToken cancellationToken)
    {
        var safeName = folderName.Replace("'", "\\'", StringComparison.Ordinal);
        var q = $"mimeType = 'application/vnd.google-apps.folder' and '{parentId}' in parents and name = '{safeName}' and trashed = false";
        var list = drive.Files.List();
        list.Q = q;
        list.Fields = "files(id, name)";
        list.Spaces = "drive";
        list.SupportsAllDrives = true;
        list.IncludeItemsFromAllDrives = true;
        list.Corpora = "allDrives";
        var found = await list.ExecuteAsync(cancellationToken).ConfigureAwait(false);
        var existing = found.Files?.FirstOrDefault();
        if (existing?.Id != null)
            return existing.Id;

        var meta = new Google.Apis.Drive.v3.Data.File
        {
            Name = folderName,
            MimeType = "application/vnd.google-apps.folder",
            Parents = new List<string> { parentId },
        };
        var create = drive.Files.Create(meta);
        create.Fields = "id";
        create.SupportsAllDrives = true;
        var created = await create.ExecuteAsync(cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrEmpty(created.Id))
            throw new IOException("Failed to create day folder in Drive.");
        return created.Id;
    }

    private static string SanitizePhoneForFileName(string phone)
    {
        var digits = Regex.Replace(phone ?? "", @"\D", "");
        return string.IsNullOrEmpty(digits) ? "unknown" : digits;
    }

    private static string SanitizeTicketCodeForFileName(string ticketCode)
    {
        var s = Regex.Replace(ticketCode ?? "", @"[^A-Za-z0-9_-]", "");
        return string.IsNullOrEmpty(s) ? "TICKET" : s;
    }

    private static string GetSafeImageExtension(string originalName, string contentType)
    {
        var ext = Path.GetExtension(originalName);
        if (!string.IsNullOrEmpty(ext) && ext.Length <= 6 && ext.StartsWith('.'))
        {
            var lower = ext.ToLowerInvariant();
            if (lower is ".png" or ".jpg" or ".jpeg" or ".webp" or ".gif" or ".heic")
                return lower == ".jpeg" ? ".jpg" : lower;
        }

        return contentType switch
        {
            "image/png" => ".png",
            "image/webp" => ".webp",
            "image/gif" => ".gif",
            "image/heic" or "image/heif" => ".heic",
            _ => ".jpg",
        };
    }

    /// <summary>
    /// Public HTTPS URL for the object (opens in browser if the object/bucket allows public access; otherwise 403 for anonymous users).
    /// </summary>
    private static string BuildGcsObjectHttpsUrl(string bucket, string objectName)
    {
        var encodedObject = string.Join(
            "/",
            objectName.Split('/', StringSplitOptions.RemoveEmptyEntries).Select(Uri.EscapeDataString));
        return $"https://storage.googleapis.com/{Uri.EscapeDataString(bucket)}/{encodedObject}";
    }

    private static string EscapeSheetNameForRange(string tabName)
    {
        if (string.IsNullOrEmpty(tabName))
            return "Sheet1";
        if (!Regex.IsMatch(tabName, @"[^A-Za-z0-9_]"))
            return tabName;
        return "'" + tabName.Replace("'", "''", StringComparison.Ordinal) + "'";
    }

    /// <summary>
    /// A1 sheet title for <c>values.append</c>. Unbounded ranges like <c>Sheet1!A:G</c> return 400 Unable to parse range — use <c>A1:G{N}</c>.
    /// </summary>
    private static string FormatSheetTitleForAppend(string? tabName)
    {
        var t = string.IsNullOrWhiteSpace(tabName) ? "Sheet1" : tabName.Trim();
        if (!Regex.IsMatch(t, @"[^A-Za-z0-9_]"))
            return t;
        return "'" + t.Replace("'", "''", StringComparison.Ordinal) + "'";
    }
}

public sealed record TicketPaymentProofResult(
    string? DriveFileId,
    string? DriveWebViewLink,
    string? GcsObjectUri,
    /// <summary>URL written to the Sheet <c>PaymentProof</c> column (Drive web view or GCS HTTPS).</summary>
    string? PaymentProofUrl);
