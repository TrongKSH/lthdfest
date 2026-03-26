import { ChangeDetectionStrategy, Component, computed, effect, inject, input, signal } from '@angular/core';
import { Router } from '@angular/router';
import { TranslocoPipe, TranslocoService } from '@ngneat/transloco';

import { AppLocaleService } from '../../../../i18n/locale.service';
import {
  getPurchaseHeaderMetaKeys,
  getTicketPricing,
  purchaseTierTitleKey,
} from '../../tickets-content';
import { TicketsPurchaseDraftService } from '../../tickets-purchase-draft.service';

type RadioValue = 'yes' | 'no';

@Component({
  selector: 'app-tickets-purchase-info',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [TranslocoPipe],
  templateUrl: './tickets-purchase-info.component.html',
  styleUrl: './tickets-purchase-info.component.scss',
})
export class TicketsPurchaseInfoComponent {
  private readonly router = inject(Router);
  private readonly draftService = inject(TicketsPurchaseDraftService);
  private readonly transloco = inject(TranslocoService);
  private readonly locale = inject(AppLocaleService);

  readonly type = input<string>('presale');
  readonly qty = input<number>(0);

  readonly quantity = signal(0);

  readonly termsAccepted = signal<RadioValue>('no');
  readonly consentProgram = signal<RadioValue>('no');

  readonly fullName = signal('');
  readonly phone = signal('');
  readonly email = signal('');

  readonly termIndices = [1, 2, 3, 4, 5, 6, 7] as const;

  constructor() {
    effect(() => {
      const q = this.qty();
      this.quantity.set(Number.isFinite(q) ? Math.max(0, Math.floor(q)) : 0);
    });
  }

  readonly qtyLabel = computed(() => String(this.quantity()).padStart(2, '0'));

  readonly pricing = computed(() => getTicketPricing(this.type()));

  readonly unitPriceVnd = computed(() => this.pricing()?.unitPriceVnd ?? 0);

  readonly tierTitleKey = computed(
    () => purchaseTierTitleKey(this.type()) ?? 'tickets.packs.presale.title',
  );

  readonly metaKeys = computed(() => getPurchaseHeaderMetaKeys(this.type()));

  readonly subtotalVnd = computed(() => this.quantity() * this.unitPriceVnd());

  readonly continueEnabled = computed(() => {
    if (!this.pricing()) return false;
    if (this.quantity() <= 0) return false;
    if (this.termsAccepted() !== 'yes') return false;
    if (this.consentProgram() !== 'yes') return false;
    if (!this.fullName().trim()) return false;
    if (!this.phone().trim()) return false;
    if (!this.email().trim() || !this.email().includes('@')) return false;
    return true;
  });

  readonly continueImgSrc = computed(() =>
    this.continueEnabled()
      ? '/assets/images/continue-enabled.png'
      : '/assets/images/continue-disbaled.png',
  );

  readonly formattedSubtotal = computed(() => this.locale.formatVnd(this.subtotalVnd()));

  readonly formattedUnitPrice = computed(() => this.locale.formatVnd(this.unitPriceVnd()));

  readonly continueError = signal<string | null>(null);

  onContinue(): void {
    if (!this.continueEnabled()) {
      this.continueError.set(this.transloco.translate('tickets.info.continueError'));
      return;
    }

    const purchaseType = this.type();
    const qty = this.quantity();
    this.draftService.setDraft({
      purchaseType,
      qty,
      fullName: this.fullName().trim(),
      phone: this.phone().trim(),
      email: this.email().trim(),
    });

    void this.router.navigate(['/tickets'], {
      queryParams: { purchase: purchaseType, step: 'confirm', qty },
    });
  }

  onClose(): void {
    this.draftService.clearDraft();
    void this.router.navigate(['/tickets'], { queryParams: {} });
  }
}
