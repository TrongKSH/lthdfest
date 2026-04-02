// Runtime config loaded before Angular bootstraps.
// Picks API URL based on hostname: staging subdomain -> staging API, everything else -> prod API.
if (!globalThis.__LT_HD_API_URL__) {
  const host = globalThis.location?.hostname ?? "";
  globalThis.__LT_HD_API_URL__ =
    host === "staging.longtranhhodau.com"
      ? "https://api-staging.longtranhhodau.com"
      : "https://api.longtranhhodau.com";
}
globalThis.__LT_HD_PAYMENT_PROOF_IMAGE_OPTIONAL__ =
  globalThis.__LT_HD_PAYMENT_PROOF_IMAGE_OPTIONAL__ ?? false;
