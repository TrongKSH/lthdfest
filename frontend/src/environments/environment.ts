export const environment = {
  production: false,
  apiUrl: 'http://localhost:5018',
  /**
   * When true, transfer step allows sending without an image (API must be in sheets-only mode: no GCS/Drive).
   * Set false when the API stores files and requires `file`.
   */
  paymentProofImageOptional: true,
};
