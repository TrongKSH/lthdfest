import { ChangeDetectionStrategy, Component, computed, effect, inject, input, signal } from '@angular/core';
import { Router } from '@angular/router';
import {
  getPurchaseHeaderMeta,
  getPurchaseHeaderTitle,
  getTicketPricing,
} from '../../tickets-content';
import { TicketsPurchaseDraftService } from '../../tickets-purchase-draft.service';

type RadioValue = 'yes' | 'no';

@Component({
  selector: 'app-tickets-purchase-info',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './tickets-purchase-info.component.html',
  styleUrl: './tickets-purchase-info.component.scss',
})
export class TicketsPurchaseInfoComponent {
  private readonly router = inject(Router);
  private readonly draftService = inject(TicketsPurchaseDraftService);

  readonly type = input<string>('presale');
  readonly qty = input<number>(0);

  readonly quantity = signal(0);

  readonly termsAccepted = signal<RadioValue>('no');
  readonly consentProgram = signal<RadioValue>('no');

  readonly fullName = signal('');
  readonly phone = signal('');
  readonly email = signal('');
  readonly taxCode = signal('');

  constructor() {
    effect(() => {
      const q = this.qty();
      this.quantity.set(Number.isFinite(q) ? Math.max(0, Math.floor(q)) : 0);
    });
  }

  readonly qtyLabel = computed(() => String(this.quantity()).padStart(2, '0'));

  readonly pricing = computed(() => getTicketPricing(this.type()));

  readonly unitPriceVnd = computed(() => this.pricing()?.unitPriceVnd ?? 0);

  readonly summaryLineLabel = computed(() => this.pricing()?.summaryDisplayName ?? '—');

  readonly subtotalVnd = computed(() => this.quantity() * this.unitPriceVnd());

  readonly headerTitle = computed(() => getPurchaseHeaderTitle(this.type()));
  readonly headerMeta = computed(() => getPurchaseHeaderMeta(this.type()));
  readonly headerWhen = computed(() => this.headerMeta().when);
  readonly headerWhere = computed(() => this.headerMeta().where);

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

  readonly formattedSubtotal = computed(() => this.formatVnd(this.subtotalVnd()));

  readonly formattedUnitPrice = computed(() => this.formatVnd(this.unitPriceVnd()));

  readonly continueError = signal<string | null>(null);

  onContinue(): void {
    if (!this.continueEnabled()) {
      this.continueError.set('Vui lòng trả lời hết tất cả câu hỏi để tiếp tục.');
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

  private formatVnd(n: number): string {
    return `${n.toLocaleString('vi-VN')} vnđ`;
  }
}

