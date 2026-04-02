using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using FestivalApi.Services;
using Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Text.Json.Serialization;

namespace FestivalApi.Controllers;

[ApiController]
[Route("api/ticket-payment-proofs")]
[EnableRateLimiting("payment")]
public sealed class TicketPaymentProofsController : ControllerBase
{
    private static readonly HashSet<string> AllowedMerchSizes = new(StringComparer.OrdinalIgnoreCase)
    {
        "XS", "S", "M", "L", "XL", "XXL",
    };

    private readonly GoogleDriveSheetsPaymentService _storage;
    private readonly TicketPaymentProofResumeTokenService _resumeTokens;
    private readonly ResumeProofCompletionTracker _resumeCompletion;
    private readonly ILogger<TicketPaymentProofsController> _logger;

    public TicketPaymentProofsController(
        GoogleDriveSheetsPaymentService storage,
        TicketPaymentProofResumeTokenService resumeTokens,
        ResumeProofCompletionTracker resumeCompletion,
        ILogger<TicketPaymentProofsController> logger)
    {
        _storage = storage;
        _resumeTokens = resumeTokens;
        _resumeCompletion = resumeCompletion;
        _logger = logger;
    }

    [HttpPost("resume")]
    [ProducesResponseType(typeof(TicketPaymentProofResumeResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public IActionResult CreateResumeToken([FromBody] TicketPaymentProofResumeRequestDto request)
    {
        if (!_resumeTokens.IsConfigured)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new ProblemDetails
            {
                Title = "Resume token is not configured",
                Detail = "Set Google:Payment:ResumeTokenSecret to enable mobile receipt upload links.",
                Status = StatusCodes.Status503ServiceUnavailable,
            });
        }

        var fullName = request.FullName?.Trim() ?? string.Empty;
        var phone = request.Phone?.Trim() ?? string.Empty;
        var email = request.Email?.Trim() ?? string.Empty;
        var purchaseType = request.PurchaseType?.Trim() ?? string.Empty;
        var merchSize = request.MerchSize?.Trim() ?? string.Empty;
        var qtyDec = request.Qty;

        if (qtyDec < 1 || qtyDec != decimal.Truncate(qtyDec))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid quantity",
                Detail = "qty must be a positive whole number.",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        var qty = (int)qtyDec;

        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(phone)
            || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(purchaseType))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Missing fields",
                Detail = "fullName, phone, email, and purchaseType are required.",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        var ticketCode = TicketPurchaseTypeMapper.TryGetTicketCode(purchaseType);
        if (ticketCode == null)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid purchase type",
                Detail = "purchaseType is not a supported ticket option.",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        if (!TryNormalizeMerchSizeCsv(ticketCode, qty, merchSize, out var normalizedMerchSize, out var merchError))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid merch size",
                Detail = merchError,
                Status = StatusCodes.Status400BadRequest,
            });
        }

        var now = DateTimeOffset.UtcNow;
        var expiresAt = _resumeTokens.GetExpiresAtUtc(now);
        var token = _resumeTokens.CreateToken(fullName, phone, email, purchaseType, qty, normalizedMerchSize, expiresAt);
        return Ok(new TicketPaymentProofResumeResponseDto
        {
            ResumeToken = token,
            ExpiresAtUtc = expiresAt,
        });
    }

    /// <summary>Whether a proof was successfully submitted for this resume token (for desktop polling while phone uploads).</summary>
    [HttpGet("resume-status")]
    [EnableRateLimiting("polling")]
    [ProducesResponseType(typeof(TicketPaymentProofResumeStatusDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public IActionResult GetResumeStatus([FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Missing token",
                Detail = "Query parameter \"token\" is required.",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        if (!_resumeTokens.IsConfigured)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new ProblemDetails
            {
                Title = "Resume token is not configured",
                Detail = "Set Google:Payment:ResumeTokenSecret to enable resume status checks.",
                Status = StatusCodes.Status503ServiceUnavailable,
            });
        }

        if (_resumeCompletion.IsCompleted(token))
        {
            return Ok(new TicketPaymentProofResumeStatusDto { Submitted = true });
        }

        if (_resumeTokens.TryReadToken(token, DateTimeOffset.UtcNow, out _))
        {
            return Ok(new TicketPaymentProofResumeStatusDto { Submitted = false });
        }

        return BadRequest(new ProblemDetails
        {
            Title = "Invalid resume token",
            Detail = "token is invalid or expired.",
            Status = StatusCodes.Status400BadRequest,
        });
    }

    /// <summary>Accept payment screenshot + purchase fields; append Google Sheet row. Uploads image to GCS/Drive when configured.</summary>
    [HttpPost]
    [RequestSizeLimit(10_485_760)]
    [RequestFormLimits(MultipartBodyLengthLimit = 10_485_760)]
    [ProducesResponseType(typeof(TicketPaymentProofResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> PostAsync(CancellationToken cancellationToken)
    {
        if (!_storage.IsConfigured)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new ProblemDetails
            {
                Title = "Google Sheets not configured",
                Detail =
                    "Set Google:Payment:ServiceAccountJson and Google:Payment:SheetsSpreadsheetId, "
                    + "and share the spreadsheet with the service account (Editor). "
                    + "Optional later: GcsBucketName or DrivePaymentRootFolderId to store images.",
                Status = StatusCodes.Status503ServiceUnavailable,
            });
        }

        IFormCollection form;
        try
        {
            form = await Request.ReadFormAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not read multipart form for ticket payment proof");
            return BadRequest(new ProblemDetails
            {
                Title = "Could not read upload",
                Detail =
                    "The server could not read the upload (multipart form). "
                    + "Try again, use another browser, or use a smaller image (max 10 MB).",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        var file = form.Files.GetFile("file");
        var requireFile = _storage.HasFileStorage;
        var resumeToken = form["resumeToken"].ToString().Trim();

        if (requireFile && (file == null || file.Length == 0))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Missing file",
                Detail = "Multipart field 'file' with an image is required when GCS or Drive storage is configured.",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        if (file != null && file.Length > 0)
        {
            if (file.Length > 10_485_760)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge, new ProblemDetails
                {
                    Title = "File too large",
                    Detail = "Maximum size is 10 MB.",
                    Status = StatusCodes.Status413PayloadTooLarge,
                });
            }

            var ct = file.ContentType ?? "";
            if (ct.StartsWith("image/svg", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid file type",
                    Detail = "SVG uploads are not allowed.",
                    Status = StatusCodes.Status400BadRequest,
                });
            }

            if (!ct.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid file type",
                    Detail = "Only image uploads are allowed.",
                    Status = StatusCodes.Status400BadRequest,
                });
            }

            if (!await IsValidImageByMagicBytesAsync(file, cancellationToken))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid file content",
                    Detail = "The uploaded file does not appear to be a valid image.",
                    Status = StatusCodes.Status400BadRequest,
                });
            }
        }

        string fullName;
        string phone;
        string email;
        string purchaseType;
        int qty;
        string merchSize;

        if (!string.IsNullOrWhiteSpace(resumeToken))
        {
            if (!_resumeTokens.IsConfigured)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new ProblemDetails
                {
                    Title = "Resume token is not configured",
                    Detail = "Set Google:Payment:ResumeTokenSecret to enable resume-token proof submission.",
                    Status = StatusCodes.Status503ServiceUnavailable,
                });
            }

            if (!_resumeTokens.TryReadToken(resumeToken, DateTimeOffset.UtcNow, out var claims))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid resume token",
                    Detail = "resumeToken is invalid or expired.",
                    Status = StatusCodes.Status400BadRequest,
                });
            }

            fullName = claims.FullName;
            phone = claims.Phone;
            email = claims.Email;
            purchaseType = claims.PurchaseType;
            qty = claims.Qty;
            merchSize = claims.MerchSize;
        }
        else
        {
            fullName = form["fullName"].ToString().Trim();
            phone = form["phone"].ToString().Trim();
            email = form["email"].ToString().Trim();
            purchaseType = form["purchaseType"].ToString().Trim();
            merchSize = form["merchSize"].ToString().Trim();
            var qtyRaw = form["qty"].ToString().Trim();

            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(phone)
                || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(purchaseType))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Missing fields",
                    Detail = "fullName, phone, email, and purchaseType are required.",
                    Status = StatusCodes.Status400BadRequest,
                });
            }

            if (!int.TryParse(qtyRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out qty) || qty < 1)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid quantity",
                    Detail = "qty must be a positive integer.",
                    Status = StatusCodes.Status400BadRequest,
                });
            }
        }

        var ticketCode = TicketPurchaseTypeMapper.TryGetTicketCode(purchaseType);
        if (ticketCode == null)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid purchase type",
                Detail = "purchaseType is not a supported ticket option.",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        if (!TryNormalizeMerchSizeCsv(ticketCode, qty, merchSize, out var normalizedMerchSize, out var merchError))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid merch size",
                Detail = merchError,
                Status = StatusCodes.Status400BadRequest,
            });
        }

        try
        {
            TicketPaymentProofResult result;
            if (file != null && file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var contentType = file.ContentType ?? "";
                result = await _storage.StoreProofAsync(
                        stream,
                        contentType,
                        file.FileName,
                        fullName,
                        phone,
                        email,
                        ticketCode,
                        qty,
                        normalizedMerchSize,
                        cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                result = await _storage.StoreProofAsync(
                        null,
                        string.Empty,
                        string.Empty,
                        fullName,
                        phone,
                        email,
                        ticketCode,
                        qty,
                        normalizedMerchSize,
                        cancellationToken)
                    .ConfigureAwait(false);
            }

            if (!string.IsNullOrWhiteSpace(resumeToken))
            {
                _resumeCompletion.MarkCompleted(resumeToken);
            }

            return Created(string.Empty, new TicketPaymentProofResponseDto
            {
                DriveFileId = result.DriveFileId,
                DriveWebViewLink = result.DriveWebViewLink,
                GcsObjectUri = result.GcsObjectUri,
                PaymentProofUrl = result.PaymentProofUrl,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Google Drive/Sheets payment proof failed");

            if (IsDriveStorageQuotaForbidden(ex))
            {
                return StatusCode(StatusCodes.Status502BadGateway, new ProblemDetails
                {
                    Title = "Google Drive: service account has no storage",
                    Detail =
                        "Personal My Drive cannot store files for a service account (403 storage quota). "
                        + "Create or use a Google Shared Drive (Team Drive), add this service account as Content manager (or Manager), "
                        + "put your payment folder inside that Shared Drive, and set DrivePaymentRootFolderId to that folder’s ID.",
                    Status = StatusCodes.Status502BadGateway,
                });
            }

            return StatusCode(StatusCodes.Status502BadGateway, new ProblemDetails
            {
                Title = "Upstream storage error",
                Detail = "Could not complete Google Drive or Sheets operation.",
                Status = StatusCodes.Status502BadGateway,
            });
        }
    }

    private static bool IsDriveStorageQuotaForbidden(Exception ex)
    {
        for (var e = ex; e != null; e = e.InnerException)
        {
            if (e is GoogleApiException gae
                && gae.HttpStatusCode == HttpStatusCode.Forbidden
                && (gae.Message.Contains("storage quota", StringComparison.OrdinalIgnoreCase)
                    || gae.Message.Contains("storageQuotaExceeded", StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
        }

        return false;
    }

    private static bool TryNormalizeMerchSizeCsv(
        string ticketCode,
        int qty,
        string merchSizeCsv,
        out string normalizedMerchSize,
        out string? error)
    {
        normalizedMerchSize = string.Empty;
        error = null;

        var requiresMerchSize = ticketCode.Equals("LTHD-TMH", StringComparison.OrdinalIgnoreCase)
            || ticketCode.Equals("LTHD-VIP", StringComparison.OrdinalIgnoreCase);
        var raw = merchSizeCsv?.Trim() ?? string.Empty;

        if (!requiresMerchSize)
        {
            normalizedMerchSize = string.Empty;
            return true;
        }

        if (string.IsNullOrWhiteSpace(raw))
        {
            error = "merchSize is required for this ticket type.";
            return false;
        }

        var parts = raw
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(static s => s.ToUpperInvariant())
            .ToArray();

        if (parts.Length != qty)
        {
            error = "merchSize must contain exactly qty entries for this ticket type.";
            return false;
        }

        foreach (var part in parts)
        {
            if (!AllowedMerchSizes.Contains(part))
            {
                error = "merchSize entries must be one of: XS,S,M,L,XL,XXL.";
                return false;
            }
        }

        normalizedMerchSize = string.Join(",", parts);
        return true;
    }

    private static readonly byte[] JpegMagic = [0xFF, 0xD8, 0xFF];
    private static readonly byte[] PngMagic = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];
    private static readonly byte[] Gif87Magic = "GIF87a"u8.ToArray();
    private static readonly byte[] Gif89Magic = "GIF89a"u8.ToArray();
    private static readonly byte[] WebpRiff = "RIFF"u8.ToArray();
    private static readonly byte[] WebpType = "WEBP"u8.ToArray();
    private static readonly byte[] BmpMagic = [0x42, 0x4D];
    private static readonly byte[] FtypBox = "ftyp"u8.ToArray();

    private static readonly HashSet<string> HeicAvifBrands = new(StringComparer.Ordinal)
    {
        "heic", "heix", "hevc", "hevx", "heim", "heis",
        "mif1", "msf1", "avif", "avis",
    };

    private static async Task<bool> IsValidImageByMagicBytesAsync(IFormFile file, CancellationToken ct)
    {
        const int headerSize = 12;
        var header = new byte[headerSize];

        await using var stream = file.OpenReadStream();
        var bytesRead = await stream.ReadAsync(header.AsMemory(0, headerSize), ct);
        if (bytesRead < 3)
            return false;

        if (header.AsSpan(0, 3).SequenceEqual(JpegMagic)) return true;
        if (bytesRead >= 8 && header.AsSpan(0, 8).SequenceEqual(PngMagic)) return true;
        if (bytesRead >= 6 && (header.AsSpan(0, 6).SequenceEqual(Gif87Magic) || header.AsSpan(0, 6).SequenceEqual(Gif89Magic))) return true;
        if (bytesRead >= 12 && header.AsSpan(0, 4).SequenceEqual(WebpRiff) && header.AsSpan(8, 4).SequenceEqual(WebpType)) return true;
        if (bytesRead >= 2 && header.AsSpan(0, 2).SequenceEqual(BmpMagic)) return true;

        // HEIC / HEIF / AVIF: ISO BMFF ftyp box at bytes 4-7, brand at bytes 8-11
        if (bytesRead >= 12
            && header.AsSpan(4, 4).SequenceEqual(FtypBox)
            && HeicAvifBrands.Contains(System.Text.Encoding.ASCII.GetString(header, 8, 4)))
        {
            return true;
        }

        return false;
    }
}

public sealed class TicketPaymentProofResumeRequestDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PurchaseType { get; set; } = string.Empty;

    public string MerchSize { get; set; } = string.Empty;

    /// <summary>Whole-number quantity; uses <see cref="decimal"/> so non-integers get a clear 400 instead of JSON model errors.</summary>
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal Qty { get; set; }
}

public sealed class TicketPaymentProofResumeResponseDto
{
    public string ResumeToken { get; set; } = string.Empty;
    public DateTimeOffset ExpiresAtUtc { get; set; }
}

public sealed class TicketPaymentProofResumeStatusDto
{
    public bool Submitted { get; set; }
}

public sealed class TicketPaymentProofResponseDto
{
    public string? DriveFileId { get; set; }
    public string? DriveWebViewLink { get; set; }

    /// <summary>Set when proofs are stored in Google Cloud Storage (e.g. personal projects without Shared Drive).</summary>
    public string? GcsObjectUri { get; set; }

    /// <summary>Same URL appended to the Sheet <c>PaymentProof</c> column (GCS HTTPS or Drive web view).</summary>
    public string? PaymentProofUrl { get; set; }
}
