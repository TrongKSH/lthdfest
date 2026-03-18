# Deploy Festival API on Render (Docker)

Render does not list .NET as a native runtime. Use a **Web Service** with **Docker**.

## Steps

1. **New +** → **Web Service** → connect your Git repo.
2. **Runtime**: **Docker** (not Native).
3. **Root Directory**: `backend/FestivalApi`  
   (so Render finds the `Dockerfile` there.)
4. **Dockerfile Path**: `Dockerfile` (default if file is in root directory).
5. **Environment variables** (example):
   - `Cors__AllowedOrigins__0` = `https://your-app.vercel.app`  
     (use your real Vercel URL; add more indices `__1`, `2` if needed.)
   - Or a single JSON array in some setups; Render often uses `Cors__AllowedOrigins` as repeated keys — simplest is one origin:  
     In `appsettings`, array maps to `Cors__AllowedOrigins__0` in env.

   For ASP.NET Core, multiple origins:
   - `Cors__AllowedOrigins__0` = `https://a.vercel.app`
   - `Cors__AllowedOrigins__1` = `https://b.vercel.app`

6. Deploy. Your API URL will look like `https://<service>.onrender.com`.

7. Set `frontend` production `apiUrl` to that URL (and redeploy Vercel).

## Local Docker test

```bash
cd backend/FestivalApi
docker build -t festival-api .
docker run --rm -p 8080:8080 -e PORT=8080 festival-api
```

Then open `http://localhost:8080/swagger` if Development, or hit your controllers.
