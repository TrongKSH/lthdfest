export const environment = {
  production: false,
  apiUrl: 'http://localhost:5018',
  /**
   * When true, transfer step allows sending without an image (API sheets-only: no GCS/Drive).
   * Default false: submit stays disabled until the user chooses an image (normal upload flow).
   */
  paymentProofImageOptional: false,
};
