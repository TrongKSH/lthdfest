# Deploy Festival API on Render (Docker)

Render does not list .NET as a native runtime. Use a **Web Service** with **Docker**.

## Steps

1. **New +** → **Web Service** → connect your Git repo.
2. **Runtime**: **Docker** (not Native).
3. **Root Directory**: `backend/FestivalApi`  
   (so Render finds the `Dockerfile` there.)
4. **Dockerfile Path**: `Dockerfile` (default if file is in root directory).
5. **CORS (required for browser → API)**  
   Browsers block calls from Vercel unless the API allows that origin. Set **one** env var (comma-separated, no spaces after commas is fine):

   **`CORS_ALLOWED_ORIGINS`** =  
   `https://lthdfest.vercel.app,https://longtranhhodau.xyz,https://www.longtranhhodau.xyz`

   Include every URL users open in the browser (Vercel default domain + custom domain + `www` if you use it). After saving, **redeploy** the service (or wait for auto-deploy).

   Alternative: `Cors__AllowedOrigins__0`, `Cors__AllowedOrigins__1`, … per origin.

6. Deploy. Your API URL will look like `https://<service>.onrender.com`.

7. Set `frontend` production `apiUrl` to that URL (and redeploy Vercel).

## Local Docker test

```bash
cd backend/FestivalApi
docker build -t festival-api .
docker run --rm -p 8080:8080 -e PORT=8080 festival-api
```

Then open `http://localhost:8080/swagger` if Development, or hit your controllers.
