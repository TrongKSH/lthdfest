export const environment = {
  production: true,
  // For static hosts (Cloudflare Pages / Netlify), this can be overridden at runtime:
  // set `globalThis.__LT_HD_API_URL__ = "https://<service>.koyeb.app"` before the app bootstraps.
  apiUrl:
    (globalThis as any).__LT_HD_API_URL__ ??
    'https://<your-service>.koyeb.app',
};
