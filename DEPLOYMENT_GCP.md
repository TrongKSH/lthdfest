# GCP Deployment Guide

This guide matches the current project setup:

- Frontend: Angular static build -> Firebase Hosting
- Backend: .NET 8 API container -> Cloud Run
- Data persistence: Google Sheets/Drive via service account
- DNS: Cloudflare (`longtranhhodau.com`)
- Firebase project: `longtranhhodau-fest`

## 1) Architecture and domains

| Environment | Frontend | API |
|---|---|---|
| **Production** | `www.longtranhhodau.com` + `longtranhhodau.com` | `api.longtranhhodau.com` |
| **Staging** | `staging.longtranhhodau.com` | `api-staging.longtranhhodau.com` |

## 2) Prerequisites

In GCP Console for your project:

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
- `google-payment-gcs-bucket-name` -> GCS bucket name (when file storage is enabled)
- `google-payment-resume-token-secret` -> random string for mobile receipt resume tokens
- `cors-allowed-origins` -> `https://longtranhhodau.com,https://www.longtranhhodau.com,https://staging.longtranhhodau.com`

Notes:

- If you later store files in GCS/Drive, set `google-payment-disable-file-storage=false`.
- Keep secrets per environment (staging and production values should be separate).

## 4) Backend deployment (Cloud Run)

The backend pipeline file is:

- `backend/FestivalApi/cloudbuild.yaml`

It builds/pushes image and deploys Cloud Run with baseline runtime settings:

- Service: `festival-api-staging` (staging) or `festival-api-prod` (production)
- Region: `asia-southeast1`
- Min instances: `1`
- Max instances: `10`
- Concurrency: `30`
- CPU: `1`
- Memory: `512Mi`
- Timeout: `30s`

For production, use the same `cloudbuild.yaml` with different Cloud Build trigger substitutions:

- `_SERVICE_NAME=festival-api-prod`
- `_ENV=prod`

### Cloud Run environment mapping (already expected by code)

- `CORS_ALLOWED_ORIGINS` <- `cors-allowed-origins`
- `Google__Payment__ServiceAccountJson` <- `google-payment-sa-json`
- `Google__Payment__SheetsSpreadsheetId` <- `google-payment-sheets-spreadsheet-id`
- `Google__Payment__SheetsTabName` <- `google-payment-sheets-tab-name`
- `Google__Payment__DisableFileStorage` <- `google-payment-disable-file-storage`
- `Google__Payment__GcsBucketName` <- `google-payment-gcs-bucket-name`
- `Google__Payment__ResumeTokenSecret` <- `google-payment-resume-token-secret`

### Health check

- `GET /healthz` should return HTTP 200

## 5) Frontend deployment (Firebase Hosting)

Frontend deployment files:

- `frontend/firebase.json`
- `frontend/.firebaserc` (project: `longtranhhodau-fest`)
- `frontend/cloudbuild.yaml`

### Runtime API config

`frontend/public/runtime-config.js` is loaded before Angular app boot.

Set per environment:

- **Staging:** `globalThis.__LT_HD_API_URL__ = "https://api-staging.longtranhhodau.com"`
- **Production:** `globalThis.__LT_HD_API_URL__ = "https://api.longtranhhodau.com"`
- `globalThis.__LT_HD_PAYMENT_PROOF_IMAGE_OPTIONAL__ = true` (sheets-only mode)

Important caching behavior:

- `runtime-config.js` is served with `Cache-Control: no-store`.
- Static hashed assets are long-cached (`immutable`) for fast repeat loads.

### Build output

- Angular output is served from `dist/frontend/browser`.

## 6) Custom domain mapping with Cloudflare DNS

Domain: `longtranhhodau.com` (purchased on Cloudflare Registrar — DNS is managed directly).

### Firebase Hosting domains

1. In Firebase Hosting console, add custom domains:
   - `longtranhhodau.com` (apex)
   - `www.longtranhhodau.com`
   - `staging.longtranhhodau.com`
2. Firebase provides DNS records (TXT for verification, A for apex, CNAME for subdomains).
3. Add those records in Cloudflare DNS with **DNS only** mode (grey cloud, not proxied).

### Cloud Run API domains

1. In Cloud Run, add custom domain mappings:
   - `api.longtranhhodau.com` -> production service
   - `api-staging.longtranhhodau.com` -> staging service
2. Cloud Run provides a CNAME record (typically `ghs.googlehosted.com`).
3. Add the CNAME records in Cloudflare DNS with **DNS only** mode.

### Cloudflare settings

- **SSL/TLS encryption mode:** Full (strict) — GCP provides valid certificates.
- **Proxy status:** DNS only (grey cloud) for all GCP-pointed records so Firebase/Cloud Run manage TLS.

## 7) Validation checklist

After deploy and DNS propagation:

1. Frontend loads:
  - `https://longtranhhodau.com`
  - `https://www.longtranhhodau.com`
  - `https://staging.longtranhhodau.com`
2. API reachable:
  - `https://api.longtranhhodau.com/healthz`
  - `https://api-staging.longtranhhodau.com/healthz`
3. Browser CORS works from frontend to API.
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

## 9) Troubleshooting quick reference

- **403/502 from Google APIs**
  - Check service account JSON secret is valid.
  - Check spreadsheet is shared with service account email.
  - Check Sheets API/Drive API enabled as needed.
- **CORS blocked in browser**
  - Verify `cors-allowed-origins` secret matches frontend domain exactly (including `https://`).
- **Frontend points to wrong API**
  - Update `frontend/public/runtime-config.js` and redeploy Hosting.
- **Slow first request**
  - Confirm Cloud Run min instances is `1`.
- **SSL certificate not provisioning**
  - Ensure Cloudflare proxy status is **DNS only** (grey cloud) for GCP records.
  - Wait up to 24h for propagation (usually under 1h).
