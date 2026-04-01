import {
  ChangeDetectionStrategy,
  Component,
  computed,
  DestroyRef,
  effect,
  inject,
  signal,
  untracked,
} from '@angular/core';
import { ActivatedRoute, Router, type ParamMap } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { map } from 'rxjs';
import { TranslocoPipe, TranslocoService } from '@ngneat/transloco';
import { getPurchaseHeaderMetaKeys, getTicketPricing, purchaseTierTitleKey } from '../../tickets-content';
import { TicketsPurchaseDraftService } from '../../tickets-purchase-draft.service';
import { TicketPaymentProofService } from '../../../../services/ticket-payment-proof.service';
import { environment } from '../../../../../environments/environment';
import { HttpErrorResponse } from '@angular/common/http';
import { catchError, EMPTY, firstValueFrom, switchMap, timer } from 'rxjs';

@Component({
  selector: 'app-tickets-purchase-transfer',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [TranslocoPipe],
  templateUrl: './tickets-purchase-transfer.component.html',
  styleUrl: './tickets-purchase-transfer.component.scss',
})
export class TicketsPurchaseTransferComponent {
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly draftService = inject(TicketsPurchaseDraftService);
  private readonly paymentProofService = inject(TicketPaymentProofService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly transloco = inject(TranslocoService);

  private static qtyFromParams(p: ParamMap): number {
    const raw = p.get('qty');
    const n = raw ? Number(raw) : 0;
    return Number.isFinite(n) && n > 0 ? Math.floor(n) : 0;
  }

  readonly purchaseType = toSignal(
    this.route.queryParamMap.pipe(map((p) => p.get('purchase'))),
    { initialValue: this.route.snapshot.queryParamMap.get('purchase') },
  );

  readonly qty = toSignal(
    this.route.queryParamMap.pipe(map((p) => TicketsPurchaseTransferComponent.qtyFromParams(p))),
    {
      initialValue: TicketsPurchaseTransferComponent.qtyFromParams(this.route.snapshot.queryParamMap),
    },
  );

  readonly purchaseStep = toSignal(
    this.route.queryParamMap.pipe(map((p) => p.get('step') ?? 'qty')),
    { initialValue: this.route.snapshot.queryParamMap.get('step') ?? 'qty' },
  );

  readonly resumeToken = toSignal(
    this.route.queryParamMap.pipe(map((p) => p.get('token')?.trim() ?? '')),
    { initialValue: this.route.snapshot.queryParamMap.get('token')?.trim() ?? '' },
  );

  readonly draft = this.draftService.draft;
  readonly isReceiptMode = computed(() => this.purchaseStep() === 'receipt');

  readonly tierTitleKey = computed(
    () => purchaseTierTitleKey(this.purchaseType() ?? 'presale') ?? 'tickets.packs.presale.title',
  );

  readonly metaKeys = computed(() =>
    getPurchaseHeaderMetaKeys(this.purchaseType() ?? 'presale'),
  );

  /** When true, user may submit without an image (API sheets-only). */
  readonly paymentProofImageOptional = environment.paymentProofImageOptional;

  readonly pricing = computed(() => {
    const t = this.purchaseType();
    return t ? getTicketPricing(t) : null;
  });

  /** Selected proof image (client-only until upload API exists). */
  readonly selectedFile = signal<File | null>(null);

  /** Object URL for preview — revoked on replace and on destroy. */
  readonly previewUrl = signal<string | null>(null);

  readonly submitting = signal(false);
  readonly submitError = signal<string | null>(null);
  readonly submitSuccess = signal(false);
  readonly generatingMobileLink = signal(false);
  readonly mobileUploadError = signal<string | null>(null);
  readonly mobileUploadQrDataUrl = signal<string | null>(null);

  /** Raw token for polling: desktop learns when phone upload completes. */
  readonly desktopPollToken = signal<string | null>(null);

  private lastMobileContextKey: string | null = null;

  readonly canSubmit = computed(() => {
    if (this.submitting() || this.submitSuccess()) return false;
    const file = this.selectedFile();
    const imageOk = file !== null || this.paymentProofImageOptional;
    if (this.isReceiptMode()) {
      return imageOk && this.resumeToken().length > 0;
    }
    return imageOk;
  });

  constructor() {
    this.destroyRef.onDestroy(() => {
      const url = this.previewUrl();
      if (url) URL.revokeObjectURL(url);
    });

    effect(() => {
      if (this.isReceiptMode()) {
        return;
      }
      const t = this.purchaseType();
      const q = this.qty();
      const d = this.draft();

      if (t === null || t === '') {
        return;
      }
      if (q <= 0 || !this.pricing()) {
        untracked(() => void this.router.navigate(['/tickets'], { queryParams: {} }));
        return;
      }
      if (!d || d.purchaseType !== t || d.qty !== q) {
        untracked(() =>
          void this.router.navigate(['/tickets'], {
            queryParams: { purchase: t, step: 'confirm', qty: q },
          }),
        );
      }
    });

    effect(() => {
      if (this.isReceiptMode()) {
        return;
      }
      const t = this.purchaseType();
      const q = this.qty();
      const d = this.draft();
      if (!t || q <= 0 || !d || d.purchaseType !== t || d.qty !== q) {
        this.lastMobileContextKey = null;
        this.mobileUploadQrDataUrl.set(null);
        this.mobileUploadError.set(null);
        this.desktopPollToken.set(null);
        return;
      }

      const key = `${d.fullName}|${d.phone}|${d.email}|${d.merchSize}|${t}|${q}`;
      if (this.lastMobileContextKey === key) {
        return;
      }
      this.lastMobileContextKey = key;
      void this.prepareMobileUploadLink();
    });

    effect((onCleanup) => {
      if (this.isReceiptMode()) return;
      if (this.submitSuccess()) return;
      const token = this.desktopPollToken();
      if (!token) return;

      const sub = timer(0, 2500)
        .pipe(
          switchMap(() =>
            this.paymentProofService.getResumeSubmissionStatus(token).pipe(
              catchError(() => EMPTY),
            ),
          ),
        )
        .subscribe((status) => {
          if (status.submitted) {
            this.submitSuccess.set(true);
          }
        });

      onCleanup(() => sub.unsubscribe());
    });
  }

  onClose(): void {
    this.revokePreview();
    this.selectedFile.set(null);
    this.draftService.clearDraft();
    void this.router.navigate(['/tickets'], { queryParams: {} });
  }

  /** After successful upload: close modal state and go to site homepage. */
  finishAndGoHome(): void {
    this.revokePreview();
    this.selectedFile.set(null);
    this.submitSuccess.set(false);
    this.draftService.clearDraft();
    void this.router.navigate(['/']);
  }

  onBackToConfirm(): void {
    if (this.isReceiptMode()) {
      void this.router.navigate(['/tickets'], { queryParams: {} });
      return;
    }
    this.revokePreview();
    this.selectedFile.set(null);
    const t = this.purchaseType();
    const q = this.qty();
    if (!t || q <= 0) return;
    void this.router.navigate(['/tickets'], {
      queryParams: { purchase: t, step: 'confirm', qty: q },
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0] ?? null;

    this.revokePreview();
    this.selectedFile.set(null);
    this.previewUrl.set(null);

    if (!file || !file.type.startsWith('image/')) {
      input.value = '';
      return;
    }

    this.selectedFile.set(file);
    const url = URL.createObjectURL(file);
    this.previewUrl.set(url);
  }

  async onSubmitProof(): Promise<void> {
    if (!this.canSubmit()) return;

    const file = this.selectedFile();
    const purchaseType = this.purchaseType();
    const q = this.qty();
    const token = this.resumeToken();
    const receiptMode = this.isReceiptMode();
    const draft = this.draft();

    if (!file && !this.paymentProofImageOptional) return;
    if (receiptMode && !token) return;
    if (!receiptMode && (!draft || !purchaseType || q <= 0)) return;
    if (!receiptMode && draft && (draft.purchaseType !== purchaseType || draft.qty !== q)) return;

    this.submitting.set(true);
    this.submitError.set(null);

    try {
      if (receiptMode) {
        await firstValueFrom(this.paymentProofService.submitProofWithResumeToken(file ?? null, token));
      } else {
        await firstValueFrom(
          this.paymentProofService.submitProof(file ?? null, {
            fullName: draft!.fullName,
            phone: draft!.phone,
            email: draft!.email,
            purchaseType: purchaseType!,
            qty: q,
            merchSize: draft!.merchSize,
          }),
        );
      }
      this.submitSuccess.set(true);
    } catch (err: unknown) {
      const message = this.errorMessageFromHttp(err);
      this.submitError.set(message);
    } finally {
      this.submitting.set(false);
    }
  }

  private errorMessageFromHttp(err: unknown): string {
    if (err instanceof HttpErrorResponse) {
      const body = err.error;
      if (typeof body === 'string' && body.trim()) return body.trim();
      if (body && typeof body === 'object') {
        const o = body as Record<string, unknown>;
        const detail = o['detail'];
        if (typeof detail === 'string' && detail.trim()) return detail.trim();

        const fromErrors = this.messageFromAspNetValidationErrors(o['errors']);
        if (fromErrors) return this.withUploadHintVi(fromErrors);

        const message = o['message'];
        if (typeof message === 'string' && message.trim()) return message.trim();

        const title = o['title'];
        if (typeof title === 'string' && title.trim()) {
          const t = title.trim();
          if (t.toLowerCase() !== 'one or more validation errors occurred.') return t;
        }
      }
      if (err.status === 0) {
        return this.transloco.translate('tickets.transfer.errors.network');
      }
      if (err.status === 413) {
        return this.transloco.translate('tickets.transfer.errors.payloadTooLarge');
      }
      if (err.status === 400) {
        return this.transloco.translate('tickets.transfer.errors.badRequest');
      }
      if (err.status === 503) {
        return this.transloco.translate('tickets.transfer.errors.serviceUnavailable');
      }
      return this.transloco.translate('tickets.transfer.errors.genericStatus', { status: err.status });
    }
    return this.transloco.translate('tickets.transfer.errors.unknown');
  }

  /** Flattens ASP.NET ValidationProblemDetails `errors` (per-field string arrays). */
  private messageFromAspNetValidationErrors(errors: unknown): string | null {
    if (!errors || typeof errors !== 'object') return null;
    const parts: string[] = [];
    for (const v of Object.values(errors as Record<string, unknown>)) {
      if (Array.isArray(v)) {
        for (const item of v) {
          if (typeof item === 'string' && item.trim()) parts.push(item.trim());
        }
      } else if (typeof v === 'string' && v.trim()) {
        parts.push(v.trim());
      }
    }
    if (parts.length === 0) return null;
    return [...new Set(parts)].join(' ');
  }

  private withUploadHintVi(message: string): string {
    if (/boundary|request form|multipart/i.test(message)) {
      return this.transloco.translate('tickets.transfer.errors.multipartHint', { message });
    }
    return message;
  }

  private revokePreview(): void {
    const url = this.previewUrl();
    if (url) {
      URL.revokeObjectURL(url);
      this.previewUrl.set(null);
    }
  }

  private async prepareMobileUploadLink(): Promise<void> {
    const draft = this.draft();
    const purchaseType = this.purchaseType();
    const qty = this.qty();
    if (!draft || !purchaseType || qty <= 0) return;

    this.generatingMobileLink.set(true);
    this.mobileUploadError.set(null);
    this.mobileUploadQrDataUrl.set(null);
    this.desktopPollToken.set(null);
    try {
      const response = await firstValueFrom(
        this.paymentProofService.createResumeToken({
          fullName: draft.fullName,
          phone: draft.phone,
          email: draft.email,
          purchaseType,
          qty,
          merchSize: draft.merchSize,
        }),
      );
      const url = `${window.location.origin}/tickets?step=receipt&token=${encodeURIComponent(response.resumeToken)}`;
      const QRCode = (await import('qrcode')).default;
      const qrDataUrl = await QRCode.toDataURL(url, {
        width: 260,
        margin: 1,
      });
      this.desktopPollToken.set(response.resumeToken);
      this.mobileUploadQrDataUrl.set(qrDataUrl);
    } catch (err: unknown) {
      this.mobileUploadError.set(this.errorMessageFromHttp(err));
    } finally {
      this.generatingMobileLink.set(false);
    }
  }
}
