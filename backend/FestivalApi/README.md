# Festival API

## Payment proof (`POST /api/ticket-payment-proofs`)

Multipart form fields:

- **Standard:** `fullName`, `phone`, `email`, `purchaseType`, `qty`, and optionally **`file`** (image).
- **Resume (phone):** `resumeToken` **instead of** the text fields above, plus optionally **`file`**. The token is issued by `POST /api/ticket-payment-proofs/resume` (see below).

---

## Mobile receipt upload (resume token)

When buyers pay by scanning the **VietQR** on a desktop, the receipt screenshot lives on the **phone**. The web app can show a **second QR** that opens `/tickets?step=receipt&token=…` on the phone so they upload without re-entering name/phone/email.

**Backend requirements**

| Config key | Env var (example) |
|------------|-------------------|
| `Google:Payment:ResumeTokenSecret` | `Google__Payment__ResumeTokenSecret` |
| `Google:Payment:ResumeTokenTtlMinutes` | `Google__Payment__ResumeTokenTtlMinutes` (optional; default **60**, clamped **5–240**) |

1. Set **`ResumeTokenSecret`** to a long random string (at least ~32 bytes of entropy), e.g. `openssl rand -base64 32`. **Do not commit** this value; use user secrets locally and a secret manager / env var in production.
2. The secret must be **identical** on every API instance (stateless HMAC signing).
3. **`POST /api/ticket-payment-proofs/resume`** (JSON body: `fullName`, `phone`, `email`, `purchaseType`, `qty`) returns `{ "resumeToken": "...", "expiresAtUtc": "..." }`. If the secret is missing, the API returns **503** with a clear problem detail.
4. **`POST /api/ticket-payment-proofs`** with multipart field **`resumeToken`** replays the stored buyer + line item from the token; image upload and GCS/Sheets behavior are unchanged.

**Frontend:** the transfer step calls `/resume` and builds `https://<your-site>/tickets?step=receipt&token=<token>` for the second QR. Ensure CORS allows your site origin (existing `Cors:AllowedOrigins` / `CORS_ALLOWED_ORIGINS`).

### Desktop sync after phone upload

While the transfer page stays open on a **desktop** browser, it polls **`GET /api/ticket-payment-proofs/resume-status?token={resumeToken}`** every few seconds. After a successful phone upload, the response is `{ "submitted": true }` and the desktop shows the same success state as a local upload.

- If **`submitted`** is false and the token is still valid → **200**.
- If the token is invalid or expired and the proof was never recorded → **400**.

Completion is stored in **`IMemoryCache`** on the API host for **24 hours** (keyed by a SHA-256 hash of the token, not the raw token). **Multi-instance caveat:** with **multiple Cloud Run instances**, the phone’s request might hit instance A while the desktop polls instance B until one poll hits the same instance that stored the completion—or until concurrency settles. For a single instance (common for small services), sync is reliable. For strict cross-instance behavior, use a shared store (e.g. Redis) later.

### GCP: add `ResumeTokenSecret` (Secret Manager + Cloud Run)

Use the same GCP project as your API (e.g. where Cloud Run is deployed). Secret **name** below matches [cloudbuild.yaml](cloudbuild.yaml) (`google-payment-resume-token-secret` → env var `Google__Payment__ResumeTokenSecret`).

**1. Generate a value (on your machine)**

```bash
openssl rand -base64 32
```

Copy the output; that string is the **secret payload** (not the Secret Manager resource name).

**2. Create the secret in Secret Manager (Console)**

1. Open [Google Cloud Console](https://console.cloud.google.com/) → select your **project**.
2. **Security** → **Secret Manager** (or search “Secret Manager”).
3. If prompted, **Enable** the **Secret Manager API**.
4. **Create secret**:
   - **Name:** `google-payment-resume-token-secret` (must match Cloud Build substitution `_RESUME_TOKEN_SECRET` unless you change [cloudbuild.yaml](cloudbuild.yaml)).
   - **Secret value:** paste the string from step 1.
   - **Region:** use your usual choice (e.g. same as Cloud Run: `asia-southeast1`), or **automatic** if you use global secrets.
5. Create.

**3. Let Cloud Run read the secret (IAM)**

The **runtime** service account of your Cloud Run service (often the default compute SA: `PROJECT_NUMBER-compute@developer.gserviceaccount.com`, or a custom SA on the service) must have **Secret Manager Secret Accessor** on this secret:

- **Secret Manager** → open `google-payment-resume-token-secret` → **Permissions** → **Grant access** → add that service account with role **Secret Manager Secret Accessor**.

If you deploy via Cloud Build and `--set-secrets`, the **Cloud Build** service account also needs permission to **reference** the secret at deploy time (often **Secret Manager Admin** or **Secret Manager Secret Accessor** on that secret, depending on your org policy).

**4. Wire it to Cloud Run**

- **If you use this repo’s Cloud Build:** the next deploy from [cloudbuild.yaml](cloudbuild.yaml) injects `Google__Payment__ResumeTokenSecret` from `google-payment-resume-token-secret:latest`. Create the Secret Manager secret **before** the deploy that adds this mapping.
- **If you configure by hand:** **Cloud Run** → your service → **Edit & deploy new revision** → **Variables & secrets** → **Secrets** → **Reference a secret** → name `Google__Payment__ResumeTokenSecret`, secret `google-payment-resume-token-secret`, version **latest**.

**5. (Optional) TTL in minutes**

Default in the app is **60** minutes (configurable **5–240**). To override in GCP:

- Either add a **plain environment variable** on Cloud Run: `Google__Payment__ResumeTokenTtlMinutes` = `90`,  
- Or create another Secret Manager secret (e.g. `google-payment-resume-token-ttl-minutes`) with value `90` and reference it like your other payment secrets (you must add it to `cloudbuild.yaml` `--set-secrets` yourself if you use Cloud Build).

**6. Verify**

After a deploy, `POST /api/ticket-payment-proofs/resume` with a valid JSON body should return **200** and a `resumeToken`. If the secret is missing or not mounted, you get **503** “Resume token is not configured”.

---

### Sheets first (no bucket / no Drive yet)

You only need:

| Config key | Env var (example) |
|------------|-------------------|
| `Google:Payment:ServiceAccountJson` | `Google__Payment__ServiceAccountJson` |
| `Google:Payment:SheetsSpreadsheetId` | `Google__Payment__SheetsSpreadsheetId` |
| `Google:Payment:SheetsTabName` | `Google__Payment__SheetsTabName` (default `Sheet1`) |

1. Create a Google Sheet with row 1: `Timestamp`, `Full Name`, `Phone`, `Email`, `TicketType`, `Quantity`, `PaymentProof` (column **G** — left empty when no file is stored).
2. **Share** the spreadsheet with the service account email (**Editor**).
3. Either **do not** set `GcsBucketName` / `DrivePaymentRootFolderId`, **or** set **`DisableFileStorage` to `true`** if those IDs are still in user secrets from earlier tests.

**Important:** If `GcsBucketName` is set, uploads go to **GCS** (Drive is not used). If you only use **Drive** and the folder is under **personal My Drive**, you can get **403 storage quota**. For sheets-only, run:

```bash
dotnet user-secrets set "Google:Payment:DisableFileStorage" "true"
```

(or remove `DrivePaymentRootFolderId` entirely).

Then each successful POST **appends one row** (A–G). Column **G** (`PaymentProof`) shows **`{phone}_{ticketCode}_{qty}`** (e.g. `0903822904_LTHD-GA2D_4`) as a **real Sheets link** (via API `TextFormat.Link`, not a `=HYPERLINK` formula), so it works for **all locales** (no `,` vs `;` formula errors). It stays empty for sheets-only mode. The **`file`** field is **optional** when file storage is off; if the client sends an image, it is read and discarded (not stored) until you enable storage.

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

4. **Configuration** (local or Cloud Run env vars)

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

### Google Cloud Storage (GCS) — receipt images

1. **Create a bucket** (same GCP project as the service account, or grant cross-project IAM).
2. **IAM on the bucket:** add the service account (`…@….iam.gserviceaccount.com`) with **Storage Object Admin** (or **Storage Admin**).
3. **Enable** the **Cloud Storage API** for the project if prompted.
4. **Configuration**

   | Key | Value |
   |-----|--------|
   | `Google__Payment__GcsBucketName` | Bucket name (e.g. `my-project-payment-proofs`) |
   | `Google__Payment__GcsObjectPrefix` | Optional; default `payment-proofs` — objects are `{prefix}/{yyyy-MM-dd}/{phone}_{ticket}_{qty}.{ext}` |
   | `Google__Payment__DisableFileStorage` | `false` or **unset** |
   | `Google__Payment__DrivePaymentRootFolderId` | **Optional** — if `GcsBucketName` is set, uploads use **GCS only**; you can remove the Drive folder id to avoid confusion |

5. **PaymentProof URL** — The sheet stores `https://storage.googleapis.com/...` (works in the browser if the object is **publicly readable**; private buckets return 403 unless you add fine-grained access or use signed URLs in a future change).

---

## Local secrets (development)

```bash
cd backend/FestivalApi
dotnet user-secrets init   # once
dotnet user-secrets set "Google:Payment:ServiceAccountJson" "$(cat /path/to/key.json)"
dotnet user-secrets set "Google:Payment:SheetsSpreadsheetId" "YOUR_SHEET_ID"
dotnet user-secrets set "Google:Payment:GcsBucketName" "your-bucket-name"
dotnet user-secrets set "Google:Payment:DisableFileStorage" "false"
# Mobile receipt QR / link (generate a strong secret; example)
dotnet user-secrets set "Google:Payment:ResumeTokenSecret" "$(openssl rand -base64 32)"
# Optional: token lifetime in minutes (5–240; default 60)
dotnet user-secrets set "Google:Payment:ResumeTokenTtlMinutes" "60"
```

Use **`dotnet user-secrets`** (with an **s** in `user-secrets`).

### Troubleshooting

| Symptom | Check |
|---------|--------|
| **503** | Service account JSON + spreadsheet id set? Sheet shared with SA? |
| **503** “Resume token is not configured” | Set `Google__Payment__ResumeTokenSecret` (see **Mobile receipt upload**). |
| **400** “Invalid resume token” | Token expired, tampered, or wrong secret (e.g. secret rotated without reissuing links). |
| **502** on append | Sheet id, tab name, and APIs (Sheets API enabled in GCP). |
| **403** / Drive / `storageQuotaExceeded` | The folder ID is still under **personal My Drive**, or the service account is **not** a member of the **Shared drive** that contains that folder. Create a **new** folder **inside** the Shared drive, copy its URL id, update `DrivePaymentRootFolderId`, and remove any old personal-Drive folder id. |
| **403** / GCS `AccessDenied` | Bucket name correct? Service account has **Storage Object Admin** on **this** bucket? Cloud Storage API enabled? |
| **`PKCS8 data must be contained within BEGIN PRIVATE KEY`** (env var) | The `Google__Payment__ServiceAccountJson` value is mangled. Use the **full** JSON from GCP. In Secret Manager, prefer **one line**: `jq -c . your-key.json` then paste the output. Do not strip `private_key` or break newlines inside the JSON string. |

---

## GCP Cloud Run deployment (staging)

Use `cloudbuild.yaml` in this folder for build + deploy.

### Required Secret Manager secrets

- `google-payment-sa-json` -> maps to `Google__Payment__ServiceAccountJson`
- `google-payment-sheets-spreadsheet-id` -> maps to `Google__Payment__SheetsSpreadsheetId`
- `google-payment-sheets-tab-name` -> maps to `Google__Payment__SheetsTabName`
- `google-payment-disable-file-storage` -> maps to `Google__Payment__DisableFileStorage`
- `google-payment-resume-token-secret` -> maps to `Google__Payment__ResumeTokenSecret` (mobile receipt upload)
- `google-payment-resume-token-ttl-minutes` -> optional; maps to `Google__Payment__ResumeTokenTtlMinutes`
- `cors-allowed-origins` -> maps to `CORS_ALLOWED_ORIGINS`

### Health endpoint

- `GET /healthz` returns `200` with `{ "status": "ok" }`

### Runtime baseline

- `min-instances=1`
- `concurrency=30`
- `cpu=1`
- `memory=512Mi`
- `timeout=30s`

These values are defaults in `cloudbuild.yaml` and can be overridden with Cloud Build substitutions.

