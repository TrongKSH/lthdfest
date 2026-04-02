export const environment = {
  production: true,
  // Optional override before bootstrap: globalThis.__LT_HD_API_URL__
  apiUrl:
    (globalThis as any).__LT_HD_API_URL__ ??
    'https://api.longtranhhodau.com',
  /**
   * Optional override before bootstrap: globalThis.__LT_HD_PAYMENT_PROOF_IMAGE_OPTIONAL__
   * Default false (require receipt image). Set true only for sheets-only APIs with no file storage.
   */
  paymentProofImageOptional:
    (globalThis as any).__LT_HD_PAYMENT_PROOF_IMAGE_OPTIONAL__ ?? false,
};
