# GCP Deployment Guide (Manual Console First)

This guide matches the current project setup:

- Frontend: Angular static build -> Firebase Hosting
- Backend: .NET 8 API container -> Cloud Run
- Data persistence: Google Sheets/Drive via service account
- DNS manager: Vercel (`longtranhhodau.xyz`)

This flow is staging-first, then production on a new domain.

## 1) Architecture and domains

### Staging

- Frontend: `staging.longtranhhodau.xyz`
- API: `api-staging.longtranhhodau.xyz`

### Production (later)

- Frontend: `www.<new-domain>`
- API: `api.<new-domain>`

## 2) Prerequisites

In GCP Console for your staging project:

1. Enable billing.
2. Enable APIs:
   - Cloud Run API
   - Cloud Build API
   - Artifact Registry API
   - Secret Manager API
3. Create Artifact Registry Docker repo:
   - Name: `lthdfest-repo`
   - Region: `asia-southeast1`

## 3) Secret Manager setup

Create these secrets (string values):

- `google-payment-sa-json` -> full service account JSON (single-line JSON is safest)
- `google-payment-sheets-spreadsheet-id` -> target spreadsheet ID
- `google-payment-sheets-tab-name` -> usually `Sheet1`
- `google-payment-disable-file-storage` -> `true` for sheets-only mode
- `cors-allowed-origins` -> `https://staging.longtranhhodau.xyz`

Notes:

- If you later store files in GCS/Drive, set `google-payment-disable-file-storage=false`.
- Keep secrets per environment (staging and production values should be separate).

## 4) Backend deployment (Cloud Run)

The backend pipeline file is:

- `backend/FestivalApi/cloudbuild.yaml`

It builds/pushes image and deploys Cloud Run with baseline runtime settings:

- Service: `festival-api-staging`
- Region: `asia-southeast1`
- Min instances: `1`
- Max instances: `10`
- Concurrency: `30`
- CPU: `1`
- Memory: `512Mi`
- Timeout: `30s`

### Cloud Run environment mapping (already expected by code)

- `CORS_ALLOWED_ORIGINS` <- `cors-allowed-origins`
- `Google__Payment__ServiceAccountJson` <- `google-payment-sa-json`
- `Google__Payment__SheetsSpreadsheetId` <- `google-payment-sheets-spreadsheet-id`
- `Google__Payment__SheetsTabName` <- `google-payment-sheets-tab-name`
- `Google__Payment__DisableFileStorage` <- `google-payment-disable-file-storage`

### Health check

- `GET /healthz` should return HTTP 200

## 5) Frontend deployment (Firebase Hosting)

Frontend deployment files:

- `frontend/firebase.json`
- `frontend/.firebaserc`
- `frontend/cloudbuild.yaml`

### Runtime API config

`frontend/public/runtime-config.js` is loaded before Angular app boot.

Set:

- `globalThis.__LT_HD_API_URL__ = "https://api-staging.longtranhhodau.xyz"`
- `globalThis.__LT_HD_PAYMENT_PROOF_IMAGE_OPTIONAL__ = true` (sheets-only mode)

Important caching behavior:

- `runtime-config.js` is served with `Cache-Control: no-store`.
- Static hashed assets are long-cached (`immutable`) for fast repeat loads.

### Build output

- Angular output is served from `dist/frontend/browser`.

## 6) Custom domain mapping with Vercel DNS

1. In Firebase Hosting, add custom domain: `staging.longtranhhodau.xyz`.
2. In Cloud Run service, add custom domain: `api-staging.longtranhhodau.xyz`.
3. Copy DNS records shown by Firebase/Cloud Run (TXT/CNAME/A/AAAA).
4. Add those exact records in Vercel DNS for `longtranhhodau.xyz`.
5. Wait for domain verification and managed SSL certificate issuance.

## 7) Validation checklist

After deploy and DNS propagation:

1. Frontend loads:
   - `https://staging.longtranhhodau.xyz`
2. API reachable:
   - `https://api-staging.longtranhhodau.xyz/healthz`
3. Browser CORS works from staging frontend to staging API.
4. Submit one test payment proof flow.
5. Confirm new row appears in Google Sheet.

## 8) Performance baseline

Start with:

- Cloud Run `min instances = 1` (reduces cold starts)
- Cloud Run `concurrency = 30`
- Cloud Run `cpu = 1`, `memory = 512Mi`
- Keep Firebase cache headers from `frontend/firebase.json`

Target expectations:

- First visit (cold): around 3-5s
- Repeat visit (warm/cache): around 1.5-2.5s
- API read p95 (warm): around 500-800ms

## 9) Production rollout later

Repeat same setup in production:

1. Create production Cloud Run service and production Firebase Hosting target/site.
2. Create production secrets in Secret Manager.
3. Set production runtime config values.
4. Map new domain:
   - `www.<new-domain>` -> Firebase Hosting
   - `api.<new-domain>` -> Cloud Run
5. Keep staging domain as rollback during initial launch window.

## 10) Troubleshooting quick reference

- **403/502 from Google APIs**
  - Check service account JSON secret is valid.
  - Check spreadsheet is shared with service account email.
  - Check Sheets API/Drive API enabled as needed.
- **CORS blocked in browser**
  - Verify `cors-allowed-origins` secret matches frontend domain exactly.
- **Frontend points to wrong API**
  - Update `frontend/public/runtime-config.js` and redeploy Hosting.
- **Slow first request**
  - Confirm Cloud Run min instances is `1`.
