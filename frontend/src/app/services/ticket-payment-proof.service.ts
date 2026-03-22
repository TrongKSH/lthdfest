import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface TicketPaymentProofPayload {
  fullName: string;
  phone: string;
  email: string;
  /** Route/query purchase key, e.g. presale or pack id */
  purchaseType: string;
  qty: number;
}

export interface TicketPaymentProofResponse {
  driveFileId?: string;
  driveWebViewLink?: string;
  /** Present when the API stores files in Google Cloud Storage (personal setups). */
  gcsObjectUri?: string;
}

@Injectable({ providedIn: 'root' })
export class TicketPaymentProofService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  /**
   * POST multipart: text fields; optional `file` when API is sheets-only.
   */
  submitProof(
    file: File | null,
    payload: TicketPaymentProofPayload,
  ): Observable<TicketPaymentProofResponse> {
    const body = new FormData();
    if (file) {
      body.append('file', file, file.name);
    }
    body.append('fullName', payload.fullName.trim());
    body.append('phone', payload.phone.trim());
    body.append('email', payload.email.trim());
    body.append('purchaseType', payload.purchaseType.trim());
    body.append('qty', String(payload.qty));

    return this.http.post<TicketPaymentProofResponse>(
      `${this.apiUrl}/api/ticket-payment-proofs`,
      body,
    );
  }
}
