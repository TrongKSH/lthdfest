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

### Google Workspace + Drive (store receipt images)

Personal Gmail **My Drive** cannot hold uploads from a **service account** (403 storage quota). With **Google Workspace**, use a **Shared drive** (Team Drive) so files live under the org, not the service account’s empty Drive.

1. **Google Cloud (same project as the service account)**  
   Enable **Google Drive API** (and **Google Sheets API** if not already).

2. **In Google Drive (Workspace)**  
   - Open **Shared drives** → **New** → create one (e.g. `LTHD payment proofs`).  
   - **Manage members** → add the **service account email** (`…@….iam.gserviceaccount.com`) with **Content manager** or **Manager**.

3. **Folder for uploads**  
   Inside that Shared drive, create a folder (e.g. `uploads`). Open it and copy the ID from the URL:  
   `https://drive.google.com/drive/folders/THIS_IS_THE_FOLDER_ID`

4. **Configuration** (local or Render env vars)

   | Key | Value |
   |-----|--------|
   | `Google__Payment__DrivePaymentRootFolderId` | That folder ID |
   | `Google__Payment__DisableFileStorage` | `false` or **remove** the variable |
   | `Google__Payment__GcsBucketName` | Leave **empty** if you are not using GCS |

   The API creates a subfolder per day (`yyyy-MM-dd`, Vietnam UTC+7) under that root and names files `{phone}_{ticketCode}_{qty}.{ext}`.

5. **Spreadsheet**  
   Still share the Sheet with the service account (**Editor**); row 1 headers unchanged.

6. **Frontend**  
   In production, `paymentProofImageOptional` should be **`false`** so users must attach an image when the API requires `file`.

**Alternative (no Workspace):** use **Google Cloud Storage** (`GcsBucketName`) instead of Drive — same service account, no Shared drive.

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
| **`PKCS8 data must be contained within BEGIN PRIVATE KEY`** (Render/env) | The `Google__Payment__ServiceAccountJson` value is mangled. Use the **full** JSON from GCP. On Render, paste as a **Secret**; prefer **one line**: `jq -c . your-key.json` then paste the output. Do not strip `private_key` or break newlines inside the JSON string. |

