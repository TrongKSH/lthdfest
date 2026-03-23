export const environment = {
  production: true,
  // Optional override before bootstrap: globalThis.__LT_HD_API_URL__
  apiUrl:
    (globalThis as any).__LT_HD_API_URL__ ??
    'https://api-staging.longtranhhodau.xyz',
  /**
   * Optional override before bootstrap: globalThis.__LT_HD_PAYMENT_PROOF_IMAGE_OPTIONAL__
   * Keep true for sheets-only mode (no file storage).
   */
  paymentProofImageOptional:
    (globalThis as any).__LT_HD_PAYMENT_PROOF_IMAGE_OPTIONAL__ ?? true,
};
