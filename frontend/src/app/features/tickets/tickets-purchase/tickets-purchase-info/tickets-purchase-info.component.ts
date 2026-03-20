import { ChangeDetectionStrategy, Component, computed, effect, inject, input, signal } from '@angular/core';
import { Router } from '@angular/router';
import { FESTIVAL_EVENT_NAME, FESTIVAL_WHERE, FESTIVAL_WHEN, PRESALE } from '../../tickets-content';

type RadioValue = 'yes' | 'no';

const PRE_SALE_UNIT_PRICE_VND = 549_000;

@Component({
  selector: 'app-tickets-purchase-info',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './tickets-purchase-info.component.html',
  styleUrl: './tickets-purchase-info.component.scss',
})
export class TicketsPurchaseInfoComponent {
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

  readonly subtotalVnd = computed(() => this.quantity() * PRE_SALE_UNIT_PRICE_VND);

  readonly headerTitle = computed(() => `${FESTIVAL_EVENT_NAME} - ${PRESALE.title}`);
  readonly headerWhen = FESTIVAL_WHEN;
  readonly headerWhere = FESTIVAL_WHERE;

  readonly continueEnabled = computed(() => {
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

  readonly continueError = signal<string | null>(null);

  onContinue(): void {
    if (!this.continueEnabled()) {
      this.continueError.set('Vui lòng trả lời hết tất cả câu hỏi để tiếp tục.');
      return;
    }

    // TODO: replace with payment/checkout page.
    this.router.navigate(['/tickets']);
  }

  onClose(): void {
    this.router.navigate(['/tickets']);
  }

  private formatVnd(n: number): string {
    return `${n.toLocaleString('vi-VN')} vnđ`;
  }

  private readonly router = inject(Router);
}

