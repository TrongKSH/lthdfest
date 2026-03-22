import { Component, ChangeDetectionStrategy, signal, computed, inject, input, effect } from '@angular/core';
import { Router } from '@angular/router';
import { getTicketPricing, PRESALE, TICKET_PACKS, type TicketPack } from '../tickets-content';

const PERKS = ['vé 2 ngày', 'vòng tay', 'LTHD passport'] as const;

@Component({
  selector: 'app-tickets-purchase',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './tickets-purchase.component.html',
  styleUrl: './tickets-purchase.component.scss',
})
export class TicketsPurchaseComponent {
  private readonly router = inject(Router);

  readonly type = input<string>('presale');

  readonly qty = input<number>(0);

  readonly quantity = signal(0);

  readonly perks = PERKS;

  readonly continueImgSrc = computed(() =>
    this.quantity() > 0
      ? '/assets/images/continue-enabled.png'
      : '/assets/images/continue-disbaled.png',
  );

  readonly selectedPack = computed<TicketPack | null>(() => {
    const t = this.type();
    if (t === 'presale') return null;
    return TICKET_PACKS.find((p) => p.id === t) ?? null;
  });

  readonly heading = computed(() => {
    const t = this.type();
    if (t === 'presale') return `Hạng vé ${PRESALE.title}`;
    const pack = this.selectedPack();
    return pack ? `Hạng vé ${pack.peekTitle}` : 'Hạng vé';
  });

  increment(): void {
    this.quantity.update((q) => q + 1);
  }

  decrement(): void {
    this.quantity.update((q) => Math.max(0, q - 1));
  }

  onQuantityInput(event: Event): void {
    const target = event.target as HTMLInputElement | null;
    const raw = target?.value ?? '0';
    const parsed = Number(raw);
    this.quantity.set(Number.isFinite(parsed) && parsed >= 0 ? Math.floor(parsed) : 0);
  }

  constructor() {
    effect(() => {
      const q = this.qty();
      if (Number.isFinite(q) && q > 0) this.quantity.set(q);
    });
  }

  onClose(): void {
    void this.router.navigate(['/tickets'], { queryParams: {} });
  }

  /** Back to ticket grid (same tier rule: pick another pack / presale). */
  onChooseArea(): void {
    void this.router.navigate(['/tickets'], { queryParams: {} });
  }

  onContinue(): void {
    const qty = this.quantity();
    if (qty <= 0) return;

    const purchase = this.type();
    if (!getTicketPricing(purchase)) return;

    void this.router
      .navigate(['/tickets'], {
        queryParams: { purchase, step: 'info', qty },
      })
      .catch(() => {
        window.location.href = `/tickets?purchase=${encodeURIComponent(purchase)}&step=info&qty=${encodeURIComponent(String(qty))}`;
      });
  }
}

