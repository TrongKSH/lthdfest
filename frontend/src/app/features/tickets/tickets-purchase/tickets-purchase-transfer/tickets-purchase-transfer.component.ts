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
import {
  getPurchaseHeaderMeta,
  getPurchaseHeaderTitle,
  getTicketPricing,
} from '../../tickets-content';
import { TicketsPurchaseDraftService } from '../../tickets-purchase-draft.service';
import { TicketPaymentProofService } from '../../../../services/ticket-payment-proof.service';
import { environment } from '../../../../../environments/environment';
import { HttpErrorResponse } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import QRCode from 'qrcode';

@Component({
  selector: 'app-tickets-purchase-transfer',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './tickets-purchase-transfer.component.html',
  styleUrl: './tickets-purchase-transfer.component.scss',
})
export class TicketsPurchaseTransferComponent {
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly draftService = inject(TicketsPurchaseDraftService);
  private readonly paymentProofService = inject(TicketPaymentProofService);
  private readonly destroyRef = inject(DestroyRef);

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

  readonly headerTitle = computed(() => {
    const t = this.purchaseType();
    return t ? getPurchaseHeaderTitle(t) : '';
  });

  readonly headerMeta = computed(() => {
    const t = this.purchaseType();
    return t ? getPurchaseHeaderMeta(t) : getPurchaseHeaderMeta('presale');
  });
  readonly headerWhen = computed(() => this.headerMeta().when);
  readonly headerWhere = computed(() => this.headerMeta().where);

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
  readonly mobileUploadUrl = signal<string | null>(null);
  readonly mobileUploadQrDataUrl = signal<string | null>(null);
  readonly copyingLink = signal(false);
  readonly copyFeedback = signal<string | null>(null);

  private lastMobileContextKey: string | null = null;

  readonly canSubmit = computed(() => {
    if (this.submitting() || this.submitSuccess()) return false;
    const file = this.selectedFile();
    const imageOk = file !== null || environment.paymentProofImageOptional;
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
        this.mobileUploadUrl.set(null);
        this.mobileUploadQrDataUrl.set(null);
        this.mobileUploadError.set(null);
        return;
      }

      const key = `${d.fullName}|${d.phone}|${d.email}|${t}|${q}`;
      if (this.lastMobileContextKey === key) {
        return;
      }
      this.lastMobileContextKey = key;
      void this.prepareMobileUploadLink();
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

    if (!file && !environment.paymentProofImageOptional) return;
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
      if (typeof body === 'string' && body.trim()) return body;
      if (body && typeof body === 'object') {
        const title = (body as { title?: string }).title;
        const detail = (body as { detail?: string }).detail;
        const msg = (body as { message?: string }).message;
        if (typeof detail === 'string' && detail.trim()) return detail;
        if (typeof title === 'string' && title.trim()) return title;
        if (typeof msg === 'string' && msg.trim()) return msg;
      }
      if (err.status === 0) {
        return 'Không kết nối được máy chủ. Kiểm tra mạng hoặc thử lại sau.';
      }
      if (err.status === 413) {
        return 'Ảnh quá lớn. Vui lòng chọn file nhỏ hơn.';
      }
      if (err.status === 400) {
        return 'Thông tin tải biên lai không hợp lệ hoặc đã hết hạn. Vui lòng quay lại bước thanh toán.';
      }
      if (err.status === 503) {
        return 'Chưa cấu hình Google Sheet (hoặc máy chủ). Kiểm tra API và Google:Payment.';
      }
      return `Gửi không thành công (${err.status}).`;
    }
    return 'Đã xảy ra lỗi. Vui lòng thử lại.';
  }

  private revokePreview(): void {
    const url = this.previewUrl();
    if (url) {
      URL.revokeObjectURL(url);
      this.previewUrl.set(null);
    }
  }

  async copyMobileUploadLink(): Promise<void> {
    const url = this.mobileUploadUrl();
    if (!url) return;

    this.copyingLink.set(true);
    this.copyFeedback.set(null);
    try {
      if (navigator?.clipboard?.writeText) {
        await navigator.clipboard.writeText(url);
      } else {
        throw new Error('Clipboard API unavailable');
      }
      this.copyFeedback.set('Đã sao chép liên kết tải biên lai.');
    } catch {
      this.copyFeedback.set('Không thể sao chép tự động. Vui lòng nhấn giữ để copy liên kết.');
    } finally {
      this.copyingLink.set(false);
    }
  }

  private async prepareMobileUploadLink(): Promise<void> {
    const draft = this.draft();
    const purchaseType = this.purchaseType();
    const qty = this.qty();
    if (!draft || !purchaseType || qty <= 0) return;

    this.generatingMobileLink.set(true);
    this.mobileUploadError.set(null);
    this.mobileUploadUrl.set(null);
    this.mobileUploadQrDataUrl.set(null);
    this.copyFeedback.set(null);
    try {
      const response = await firstValueFrom(
        this.paymentProofService.createResumeToken({
          fullName: draft.fullName,
          phone: draft.phone,
          email: draft.email,
          purchaseType,
          qty,
        }),
      );
      const url = `${window.location.origin}/tickets?step=receipt&token=${encodeURIComponent(response.resumeToken)}`;
      const qrDataUrl = await QRCode.toDataURL(url, {
        width: 260,
        margin: 1,
      });
      this.mobileUploadUrl.set(url);
      this.mobileUploadQrDataUrl.set(qrDataUrl);
    } catch (err: unknown) {
      this.mobileUploadError.set(this.errorMessageFromHttp(err));
    } finally {
      this.generatingMobileLink.set(false);
    }
  }
}
