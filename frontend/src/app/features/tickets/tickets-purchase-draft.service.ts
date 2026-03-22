import { Injectable, signal } from '@angular/core';

/** In-memory buyer + line item context between info and confirm steps (not in URL). */
export interface TicketsPurchaseDraft {
  purchaseType: string;
  qty: number;
  fullName: string;
  phone: string;
  email: string;
}

@Injectable({ providedIn: 'root' })
export class TicketsPurchaseDraftService {
  private readonly _draft = signal<TicketsPurchaseDraft | null>(null);

  /** Read-only draft for templates / guards */
  readonly draft = this._draft.asReadonly();

  setDraft(value: TicketsPurchaseDraft): void {
    this._draft.set(value);
  }

  clearDraft(): void {
    this._draft.set(null);
  }
}
