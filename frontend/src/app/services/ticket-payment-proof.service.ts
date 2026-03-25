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

export interface TicketPaymentProofResumeResponse {
  resumeToken: string;
  expiresAtUtc: string;
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

  createResumeToken(payload: TicketPaymentProofPayload): Observable<TicketPaymentProofResumeResponse> {
    return this.http.post<TicketPaymentProofResumeResponse>(
      `${this.apiUrl}/api/ticket-payment-proofs/resume`,
      {
        fullName: payload.fullName.trim(),
        phone: payload.phone.trim(),
        email: payload.email.trim(),
        purchaseType: payload.purchaseType.trim(),
        qty: payload.qty,
      },
    );
  }

  /**
   * POST multipart: text fields; optional `file` when API is sheets-only.
   */
  submitProof(
    file: File | null,
    payload: TicketPaymentProofPayload,
  ): Observable<TicketPaymentProofResponse> {
    return this.submitProofInternal(file, payload, null);
  }

  submitProofWithResumeToken(
    file: File | null,
    resumeToken: string,
  ): Observable<TicketPaymentProofResponse> {
    return this.submitProofInternal(file, null, resumeToken);
  }

  private submitProofInternal(
    file: File | null,
    payload: TicketPaymentProofPayload | null,
    resumeToken: string | null,
  ): Observable<TicketPaymentProofResponse> {
    const body = new FormData();
    if (file) {
      body.append('file', file, file.name);
    }
    if (resumeToken && resumeToken.trim()) {
      body.append('resumeToken', resumeToken.trim());
    }
    if (payload) {
      body.append('fullName', payload.fullName.trim());
      body.append('phone', payload.phone.trim());
      body.append('email', payload.email.trim());
      body.append('purchaseType', payload.purchaseType.trim());
      body.append('qty', String(payload.qty));
    }

    return this.http.post<TicketPaymentProofResponse>(
      `${this.apiUrl}/api/ticket-payment-proofs`,
      body,
    );
  }
}
