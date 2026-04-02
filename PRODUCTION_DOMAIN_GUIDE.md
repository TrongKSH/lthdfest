# Production Domain Guide — longtranhhodau.com

This guide covers everything needed to go live on `longtranhhodau.com`, replacing the old `longtranhhodau.xyz` domain entirely.

## Target architecture

```
longtranhhodau.com (Cloudflare Registrar + DNS)
        │
   ┌────┴────────────────────────────┐
   │                                 │
   ▼                                 ▼
Firebase Hosting               Cloud Run
(Angular frontend)          (.NET 8 API)
   │                                 │
   ├─ www.longtranhhodau.com         ├─ api.longtranhhodau.com
   ├─ longtranhhodau.com (apex)      └─ api-staging.longtranhhodau.com
   └─ staging.longtranhhodau.com
```

## Domain plan

| Environment | Frontend | API |
|---|---|---|
| **Production** | `www.longtranhhodau.com` + `longtranhhodau.com` | `api.longtranhhodau.com` |
| **Staging** | `staging.longtranhhodau.com` | `api-staging.longtranhhodau.com` |

---

## ~~Step 1 — Buy the domain~~ DONE

Purchased `longtranhhodau.com` on **Cloudflare Registrar**.

---

## ~~Step 2 — Set up Cloudflare DNS~~ DONE

Domain was bought directly on Cloudflare — DNS is already active, no nameserver swap needed.

**One thing to verify:** In Cloudflare dashboard → **SSL/TLS** → set encryption mode to **Full (strict)** (GCP provides valid certs on their side). This prevents mixed-content or redirect-loop issues.

---

## Step 3 — Map frontend domains (Firebase Hosting)

### 3a. Create or reuse Firebase project

If you already have a Firebase project from staging, reuse it. Otherwise:

1. Go to [Firebase Console](https://console.firebase.google.com/).
2. Create project (or open existing one).
3. **Hosting** → **Get started** (if not already set up).
4. Update `frontend/.firebaserc`:

```json
{
  "projects": {
    "default": "your-actual-firebase-project-id"
  }
}
```

### 3b. Add custom domains in Firebase Hosting

In Firebase Console → **Hosting** → **Custom domains** → **Add custom domain**:

1. Add `longtranhhodau.com` (apex).
   - Firebase shows a **TXT** record for verification + **A** records (typically `151.101.1.195`, `151.101.65.195`).
2. Add `www.longtranhhodau.com`.
   - Firebase shows a **CNAME** record (e.g. pointing to your Firebase hosting URL).
3. Add `staging.longtranhhodau.com`.
   - Same process — separate **CNAME** record.

### 3c. Add DNS records in Cloudflare

For each domain above, add the records Firebase gave you in **Cloudflare DNS → Records**:

| Type | Name | Value | Proxy |
|---|---|---|---|
| TXT | `@` | Firebase verification string | N/A |
| A | `@` | `151.101.1.195` | **DNS only** (grey cloud) |
| A | `@` | `151.101.65.195` | **DNS only** (grey cloud) |
| CNAME | `www` | `your-project.web.app` | **DNS only** (grey cloud) |
| CNAME | `staging` | `your-project.web.app` | **DNS only** (grey cloud) |

**Important:** Use **DNS only** mode (grey cloud icon), not Proxied (orange cloud). GCP/Firebase needs to terminate TLS and verify domain ownership directly.

Wait for Firebase to show **Connected** status (can take up to 24h but usually under 1h).

---

## Step 4 — Map API domains (Cloud Run)

### 4a. Add custom domain mappings in Cloud Run

In [GCP Console](https://console.cloud.google.com/) → **Cloud Run**:

**Production API:**
1. Open the production Cloud Run service (or create `festival-api-prod` when ready).
2. **Integrations** or **Manage custom domains** → **Add mapping**.
3. Enter `api.longtranhhodau.com`.
4. Cloud Run gives you a **CNAME** record value (e.g. `ghs.googlehosted.com`).

**Staging API:**
1. Open the staging service `festival-api-staging`.
2. Same process for `api-staging.longtranhhodau.com`.

### 4b. Add DNS records in Cloudflare

| Type | Name | Value | Proxy |
|---|---|---|---|
| CNAME | `api` | `ghs.googlehosted.com` | **DNS only** (grey cloud) |
| CNAME | `api-staging` | `ghs.googlehosted.com` | **DNS only** (grey cloud) |

Wait for Cloud Run to show domain as **Active** with a managed SSL certificate.

---

## Step 5 — Update GCP Secret Manager

Update the `cors-allowed-origins` secret to include the new domains.

### Production

```
https://longtranhhodau.com,https://www.longtranhhodau.com
```

### Staging

```
https://staging.longtranhhodau.com
```

If both environments share one Cloud Run service for now, combine them:

```
https://longtranhhodau.com,https://www.longtranhhodau.com,https://staging.longtranhhodau.com
```

In Secret Manager → open `cors-allowed-origins` → **New version** → paste → **Add**. Cloud Run picks it up on next deploy (or restart).

---

## Step 6 — Code changes in the repo

### 6a. `backend/FestivalApi/appsettings.json`

Update `AllowedHosts` to allow the new domain:

```json
"AllowedHosts": "longtranhhodau.com;*.longtranhhodau.com;*.run.app;localhost"
```

### 6b. `frontend/src/environments/environment.prod.ts`

Update the fallback API URL:

```typescript
export const environment = {
  production: true,
  apiUrl:
    (globalThis as any).__LT_HD_API_URL__ ??
    'https://api.longtranhhodau.com',
  paymentProofImageOptional:
    (globalThis as any).__LT_HD_PAYMENT_PROOF_IMAGE_OPTIONAL__ ?? false,
};
```

### 6c. `frontend/public/runtime-config.js`

Update the default API URL:

```javascript
globalThis.__LT_HD_API_URL__ =
  globalThis.__LT_HD_API_URL__ ??
  "https://api.longtranhhodau.com";
globalThis.__LT_HD_PAYMENT_PROOF_IMAGE_OPTIONAL__ =
  globalThis.__LT_HD_PAYMENT_PROOF_IMAGE_OPTIONAL__ ?? false;
```

For staging deploys, override this value before deploying frontend hosting (the Cloud Build trigger can substitute it, or you deploy a staging-specific `runtime-config.js` with `https://api-staging.longtranhhodau.com`).

### 6d. `frontend/firebase.json` — CSP header

Update the `connect-src` directive to allow the new API domain:

```
connect-src 'self' https://*.longtranhhodau.com https://*.run.app https://*.googleapis.com
```

This covers both `api.longtranhhodau.com` and `api-staging.longtranhhodau.com` via the wildcard.

### 6e. Delete obsolete files

- `frontend/vercel.json` — Vercel rewrite rules (Firebase Hosting handles this)
- `backend/FestivalApi/RENDER-DOCKER.md` — Render deployment guide

### 6f. Update comments referencing Render

These files have comments mentioning "Render" — update to "Cloud Run":

- `backend/FestivalApi/Dockerfile` — header comments
- `backend/FestivalApi/Program.cs` — CORS comments
- `backend/FestivalApi/Options/GooglePaymentOptions.cs` — XML doc
- `backend/FestivalApi/Services/GoogleServiceAccountJsonNormalizer.cs` — XML doc
- `backend/FestivalApi/README.md` — troubleshooting table

---

## Step 7 — Deploy and verify

### Backend (Cloud Run)

Deploy the API with the updated `appsettings.json`:

```bash
# Trigger via Cloud Build, or manually:
gcloud builds submit --config=backend/FestivalApi/cloudbuild.yaml \
  --substitutions=_SERVICE_NAME=festival-api-prod,_ENV=prod
```

Verify:

```bash
curl https://api.longtranhhodau.com/healthz
# Expected: {"status":"ok"}
```

### Frontend (Firebase Hosting)

Build and deploy:

```bash
cd frontend
npm run build:prod
firebase deploy --only hosting
```

Verify:

- Open `https://longtranhhodau.com` — Angular app loads
- Open `https://www.longtranhhodau.com` — same
- Open `https://staging.longtranhhodau.com` — staging version

### CORS check

Open browser DevTools on `https://longtranhhodau.com`, navigate to a page that calls the API, and confirm no CORS errors in the console.

### Full payment flow test

1. Submit a test payment proof from the frontend.
2. Confirm the row appears in Google Sheets.
3. Test the mobile receipt QR transfer flow.

---

## Step 8 — (Optional) Redirect old domain

If `longtranhhodau.xyz` stays active for a while and you want to redirect visitors:

1. Keep `longtranhhodau.xyz` DNS in Vercel (or move to Cloudflare too).
2. Set up a **Cloudflare redirect rule** or a simple Firebase Hosting redirect to `https://longtranhhodau.com`.
3. Once traffic drops to zero, let the old domain expire.

If you don't care about the old domain, just let it expire — no action needed.

---

## Checklist summary

- [x] Buy `longtranhhodau.com` (Cloudflare Registrar)
- [x] Set up Cloudflare DNS (automatic — bought on Cloudflare)
- [ ] Verify Cloudflare SSL/TLS mode is **Full (strict)**
- [x] Add Firebase Hosting custom domains (apex, www, staging)
- [x] Add Cloud Run custom domain mappings (api-staging)
- [x] Add all DNS records in Cloudflare (DNS-only mode)
- [ ] Wait for SSL certs to provision (Firebase + Cloud Run)
- [ ] Update `cors-allowed-origins` in Secret Manager
- [ ] Update code: `appsettings.json`, `environment.prod.ts`, `runtime-config.js`, `firebase.json` CSP
- [x] Delete `vercel.json` and `RENDER-DOCKER.md` (already done)
- [x] Update Render references in comments to Cloud Run (already done)
- [x] Rewrite `DEPLOYMENT_GCP.md` for Cloudflare + new domain
- [ ] Deploy backend to Cloud Run
- [ ] Deploy frontend to Firebase Hosting
- [ ] Verify health endpoint, frontend loads, CORS, payment flow
- [ ] (Optional) Set up redirect from old domain
