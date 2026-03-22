export const environment = {
  production: true,
  // Optional override before bootstrap: globalThis.__LT_HD_API_URL__
  apiUrl:
    (globalThis as any).__LT_HD_API_URL__ ?? 'https://lthd-api.onrender.com',
  /** Require receipt image unless API runs sheets-only without storage. */
  paymentProofImageOptional: false,
};
