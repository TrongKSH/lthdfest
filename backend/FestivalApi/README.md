# Festival API

## Payment proof (`POST /api/ticket-payment-proofs`)

Multipart form fields: `fullName`, `phone`, `email`, `purchaseType`, `qty`, and optionally **`file`** (image).

### Sheets first (no bucket / no Drive yet)

You only need:

| Config key | Env var (example) |
|------------|-------------------|
| `Google:Payment:ServiceAccountJson` | `Google__Payment__ServiceAccountJson` |
| `Google:Payment:SheetsSpreadsheetId` | `Google__Payment__SheetsSpreadsheetId` |
| `Google:Payment:SheetsTabName` | `Google__Payment__SheetsTabName` (default `Sheet1`) |

1. Create a Google Sheet with row 1: `Timestamp`, `Full Name`, `Phone`, `Email`, `TicketType`, `Quantity`.
2. **Share** the spreadsheet with the service account email (**Editor**).
3. Either **do not** set `GcsBucketName` / `DrivePaymentRootFolderId`, **or** set **`DisableFileStorage` to `true`** if those IDs are still in user secrets from earlier tests.

**Important:** If you previously set `DrivePaymentRootFolderId` (personal Drive folder), the API will try **Drive upload first** and you can get **403 storage quota** from Google. For sheets-only, run:

```bash
dotnet user-secrets set "Google:Payment:DisableFileStorage" "true"
```

(or remove `DrivePaymentRootFolderId` entirely).

Then each successful POST **appends one row** (A–F). The **`file`** field is **optional** when file storage is off; if the client sends an image, it is read and discarded (not stored) until you enable storage.

When you add a bucket or Shared Drive folder later, set `DisableFileStorage` to `false`, set `GcsBucketName` and/or `DrivePaymentRootFolderId`, and the API will **require** `file` and persist the image.

---

### Optional: store images (later)

| Option | Config |
|--------|--------|
| **Google Cloud Storage** | `GcsBucketName` (+ optional `GcsObjectPrefix`) |
| **Google Drive (Shared Drive)** | `DrivePaymentRootFolderId` |

See earlier sections in git history or docs for IAM and Shared Drive setup.

---

## Local secrets (development)

```bash
cd backend/FestivalApi
dotnet user-secrets init   # once
dotnet user-secrets set "Google:Payment:ServiceAccountJson" "$(cat /path/to/key.json)"
dotnet user-secrets set "Google:Payment:SheetsSpreadsheetId" "YOUR_SHEET_ID"
```

Use **`dotnet user-secrets`** (with an **s** in `user-secrets`).

### Troubleshooting

| Symptom | Check |
|---------|--------|
| **503** | Service account JSON + spreadsheet id set? Sheet shared with SA? |
| **502** on append | Sheet id, tab name, and APIs (Sheets API enabled in GCP). |
| **403** / Drive quota | You enabled Drive upload without Shared Drive — use GCS or sheets-only until bucket exists. |
