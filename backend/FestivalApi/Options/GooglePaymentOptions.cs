namespace FestivalApi.Options;

/// <summary>
/// Maps to configuration section <c>Google:Payment</c> and env vars like <c>Google__Payment__ServiceAccountJson</c>.
/// </summary>
public sealed class GooglePaymentOptions
{
    public const string SectionName = "Google:Payment";

    /// <summary>Full service account JSON (preferred as a secret).</summary>
    public string? ServiceAccountJson { get; set; }

    /// <summary>Parent Drive folder ID (daily subfolders created inside). Not needed if <see cref="GcsBucketName"/> is set.</summary>
    public string? DrivePaymentRootFolderId { get; set; }

    /// <summary>
    /// Optional Google Cloud Storage bucket for payment images (same GCP project as the service account).
    /// Use this for personal projects: service accounts cannot use consumer Google Drive without a Shared Drive.
    /// </summary>
    public string? GcsBucketName { get; set; }

    /// <summary>Object prefix inside the bucket, e.g. <c>payment-proofs</c> (no leading slash).</summary>
    public string GcsObjectPrefix { get; set; } = "payment-proofs";

    public string? SheetsSpreadsheetId { get; set; }

    public string SheetsTabName { get; set; } = "Sheet1";

    /// <summary>
    /// Optional numeric sheet/tab id (see sheet properties in the Sheets UI / API). When set, post-append
    /// formatting skips loading full spreadsheet metadata; otherwise the tab id is cached after the first fetch.
    /// </summary>
    public int? SheetsTabSheetId { get; set; }

    /// <summary>Vietnam is UTC+7 (no DST). Used for daily folder name yyyy-MM-dd.</summary>
    public int VietnamUtcOffsetHours { get; set; } = 7;

    /// <summary>Minimum config: append rows to the spreadsheet (service account + sheet id).</summary>
    public bool IsSheetsConfigured =>
        !string.IsNullOrWhiteSpace(ServiceAccountJson)
        && !string.IsNullOrWhiteSpace(SheetsSpreadsheetId);

    /// <summary>
    /// When <c>true</c>, never upload to GCS or Drive — only append the Sheet row — even if
    /// <see cref="GcsBucketName"/> or <see cref="DrivePaymentRootFolderId"/> is still set in secrets.
    /// Use this for sheets-first testing (avoids Drive 403 for personal Google accounts).
    /// </summary>
    public bool DisableFileStorage { get; set; }

    /// <summary>
    /// Secret used to sign short-lived mobile receipt resume tokens.
    /// Set with env var <c>Google__Payment__ResumeTokenSecret</c>.
    /// </summary>
    public string? ResumeTokenSecret { get; set; }

    /// <summary>TTL for resume tokens in minutes.</summary>
    public int ResumeTokenTtlMinutes { get; set; } = 60;

    /// <summary>Optional: persist the image to GCS or Drive.</summary>
    public bool HasFileStorage =>
        !DisableFileStorage
        && (!string.IsNullOrWhiteSpace(GcsBucketName)
            || !string.IsNullOrWhiteSpace(DrivePaymentRootFolderId));

    public bool HasResumeTokenSecret => !string.IsNullOrWhiteSpace(ResumeTokenSecret);
}
