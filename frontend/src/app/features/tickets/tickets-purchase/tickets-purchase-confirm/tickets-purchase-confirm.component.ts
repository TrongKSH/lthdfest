import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  untracked,
} from '@angular/core';
import { ActivatedRoute, Router, type ParamMap } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { map } from 'rxjs';
import { TranslocoPipe } from '@ngneat/transloco';

import { AppLocaleService } from '../../../../i18n/locale.service';
import { getPurchaseHeaderMetaKeys, getTicketPricing, purchaseTierTitleKey } from '../../tickets-content';
import { TicketsPurchaseDraftService } from '../../tickets-purchase-draft.service';

@Component({
  selector: 'app-tickets-purchase-confirm',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [TranslocoPipe],
  templateUrl: './tickets-purchase-confirm.component.html',
  styleUrl: './tickets-purchase-confirm.component.scss',
})
export class TicketsPurchaseConfirmComponent {
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly draftService = inject(TicketsPurchaseDraftService);
  private readonly locale = inject(AppLocaleService);

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
    this.route.queryParamMap.pipe(map((p) => TicketsPurchaseConfirmComponent.qtyFromParams(p))),
    { initialValue: TicketsPurchaseConfirmComponent.qtyFromParams(this.route.snapshot.queryParamMap) },
  );

  readonly draft = this.draftService.draft;

  readonly tierTitleKey = computed(
    () => purchaseTierTitleKey(this.purchaseType() ?? 'presale') ?? 'tickets.packs.presale.title',
  );

  readonly metaKeys = computed(() => getPurchaseHeaderMetaKeys(this.purchaseType() ?? 'presale'));

  readonly pricing = computed(() => {
    const t = this.purchaseType();
    return t ? getTicketPricing(t) : null;
  });

  readonly ticketSummaryKey = computed(() => this.pricing()?.summaryKey ?? '');

  readonly unitPriceVnd = computed(() => this.pricing()?.unitPriceVnd ?? 0);

  readonly subtotalVnd = computed(() => this.qty() * this.unitPriceVnd());

  readonly qtyPadded = computed(() => String(this.qty()).padStart(2, '0'));

  readonly emailForTickets = computed(() => this.draft()?.email?.trim() ?? '');

  constructor() {
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
            queryParams: { purchase: t, step: 'info', qty: q },
          }),
        );
      }
    });
  }

  formatVnd(n: number): string {
    return this.locale.formatVnd(n);
  }

  onClose(): void {
    this.draftService.clearDraft();
    void this.router.navigate(['/tickets'], { queryParams: {} });
  }

  onReselectTickets(): void {
    this.draftService.clearDraft();
    void this.router.navigate(['/tickets'], { queryParams: {} });
  }

  onPay(): void {
    const t = this.purchaseType();
    const q = this.qty();
    if (!t || q <= 0) return;
    void this.router.navigate(['/tickets'], {
      queryParams: { purchase: t, step: 'transfer', qty: q },
    });
  }
}
