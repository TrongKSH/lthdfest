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
  FESTIVAL_WHERE,
  FESTIVAL_WHEN,
  getPurchaseHeaderTitle,
  getTicketPricing,
} from '../../tickets-content';
import { TicketsPurchaseDraftService } from '../../tickets-purchase-draft.service';

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

  readonly draft = this.draftService.draft;

  readonly headerTitle = computed(() => {
    const t = this.purchaseType();
    return t ? getPurchaseHeaderTitle(t) : '';
  });

  readonly headerWhen = FESTIVAL_WHEN;
  readonly headerWhere = FESTIVAL_WHERE;

  readonly pricing = computed(() => {
    const t = this.purchaseType();
    return t ? getTicketPricing(t) : null;
  });

  /** Selected proof image (client-only until upload API exists). */
  readonly selectedFile = signal<File | null>(null);

  /** Object URL for preview — revoked on replace and on destroy. */
  readonly previewUrl = signal<string | null>(null);

  readonly canSubmit = computed(() => this.selectedFile() !== null);

  constructor() {
    this.destroyRef.onDestroy(() => {
      const url = this.previewUrl();
      if (url) URL.revokeObjectURL(url);
    });

    effect(() => {
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
  }

  onClose(): void {
    this.revokePreview();
    this.selectedFile.set(null);
    this.draftService.clearDraft();
    void this.router.navigate(['/tickets'], { queryParams: {} });
  }

  onBackToConfirm(): void {
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

  /** TODO: POST multipart to backend when API exists */
  onSubmitProof(): void {
    if (!this.canSubmit()) return;
    /* placeholder — integrate HttpClient upload + order id */
  }

  private revokePreview(): void {
    const url = this.previewUrl();
    if (url) {
      URL.revokeObjectURL(url);
      this.previewUrl.set(null);
    }
  }
}
