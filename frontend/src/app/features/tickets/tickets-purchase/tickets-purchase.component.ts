import { Component, ChangeDetectionStrategy, signal, computed, inject, input, effect } from '@angular/core';
import { Router } from '@angular/router';
import { getTicketPricing, PRESALE, TICKET_PACKS, type TicketPack } from '../tickets-content';

const DEFAULT_PERKS = ['Vé 2 ngày', 'Vòng tay', 'LTHĐ passport'] as const;
const LONG_TRANH_PERKS = ['Vé 1 ngày 08/05/2026', 'Vòng tay', 'LTHĐ passport'] as const;
const HO_DAU_PERKS = ['Vé 1 ngày 09/05/2026', 'Vòng tay', 'LTHĐ passport'] as const;
const BROTHERHOOD_PERKS = ['2 vé 2 ngày', '2 vòng tay', '2 LTHĐ passport'] as const;
const METALHEAD_PERKS = ['Vé 2 ngày', 'Áo thun Official Merch 350.000đ', '2 phần nước'] as const;
const VIP_PERKS = [
  'Vé 2 ngày',
  'Áo thun Merch (phiên bản đặc biệt)',
  'Uống bia free-flow (không giới hạn) trong 2 khung giờ vàng',
  'Lối đi fast-track không xếp hàng',
  'Khu vực phòng VIP',
] as const;

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

  readonly perks = computed<readonly string[]>(() => {
    const t = this.type();
    if (t === 'longtranh') return LONG_TRANH_PERKS;
    if (t === 'hodau') return HO_DAU_PERKS;
    if (t === 'brotherhood') return BROTHERHOOD_PERKS;
    if (t === 'metalhead') return METALHEAD_PERKS;
    if (t === 'vip') return VIP_PERKS;
    return DEFAULT_PERKS;
  });

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

