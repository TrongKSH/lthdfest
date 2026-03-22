using System.Globalization;
using System.Net;
using FestivalApi.Services;
using Google;
using Microsoft.AspNetCore.Mvc;

namespace FestivalApi.Controllers;

[ApiController]
[Route("api/ticket-payment-proofs")]
public sealed class TicketPaymentProofsController : ControllerBase
{
    private readonly GoogleDriveSheetsPaymentService _storage;
    private readonly ILogger<TicketPaymentProofsController> _logger;

    public TicketPaymentProofsController(
        GoogleDriveSheetsPaymentService storage,
        ILogger<TicketPaymentProofsController> logger)
    {
        _storage = storage;
        _logger = logger;
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

        var form = await Request.ReadFormAsync(cancellationToken).ConfigureAwait(false);
        var file = form.Files.GetFile("file");
        var requireFile = _storage.HasFileStorage;

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
            if (!ct.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid file type",
                    Detail = "Only image uploads are allowed.",
                    Status = StatusCodes.Status400BadRequest,
                });
            }
        }

        var fullName = form["fullName"].ToString().Trim();
        var phone = form["phone"].ToString().Trim();
        var email = form["email"].ToString().Trim();
        var purchaseType = form["purchaseType"].ToString().Trim();
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

        if (!int.TryParse(qtyRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var qty) || qty < 1)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid quantity",
                Detail = "qty must be a positive integer.",
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
                        cancellationToken)
                    .ConfigureAwait(false);
            }

            return Created(string.Empty, new TicketPaymentProofResponseDto
            {
                DriveFileId = result.DriveFileId,
                DriveWebViewLink = result.DriveWebViewLink,
                GcsObjectUri = result.GcsObjectUri,
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
}

public sealed class TicketPaymentProofResponseDto
{
    public string? DriveFileId { get; set; }
    public string? DriveWebViewLink { get; set; }

    /// <summary>Set when proofs are stored in Google Cloud Storage (e.g. personal projects without Shared Drive).</summary>
    public string? GcsObjectUri { get; set; }
}
