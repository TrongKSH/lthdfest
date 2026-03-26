import { Component, ChangeDetectionStrategy, signal, computed, inject, input, effect } from '@angular/core';
import { Router } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { startWith } from 'rxjs';
import { TranslocoPipe, TranslocoService } from '@ngneat/transloco';
import {
  getTicketPricing,
  perksGroupKey,
  TICKET_PACK_DEFS,
  type TicketPackDef,
} from '../tickets-content';

@Component({
  selector: 'app-tickets-purchase',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [TranslocoPipe],
  templateUrl: './tickets-purchase.component.html',
  styleUrl: './tickets-purchase.component.scss',
})
export class TicketsPurchaseComponent {
  private readonly router = inject(Router);
  private readonly transloco = inject(TranslocoService);
  private readonly activeLang = toSignal(
    this.transloco.langChanges$.pipe(startWith(this.transloco.getActiveLang())),
    { initialValue: this.transloco.getActiveLang() },
  );

  readonly type = input<string>('presale');

  readonly qty = input<number>(0);

  readonly quantity = signal(0);

  readonly perks = computed(() => {
    this.activeLang();
    const g = perksGroupKey(this.type());
    const v = this.transloco.translateObject(`tickets.perks.${g}`) as unknown;
    return Array.isArray(v) ? (v as string[]) : [];
  });

  readonly selectedPack = computed<TicketPackDef | null>(() => {
    const t = this.type();
    if (t === 'presale') return null;
    return TICKET_PACK_DEFS.find((p) => p.id === t) ?? null;
  });

  readonly heading = computed(() => {
    this.activeLang();
    const t = this.type();
    const tierKey =
      t === 'presale'
        ? 'tickets.packs.presale.title'
        : this.selectedPack()
          ? `tickets.packs.${this.selectedPack()!.id}.peekTitle`
          : 'tickets.packs.presale.title';
    const tier = this.transloco.translate(tierKey);
    return this.transloco.translate('tickets.purchase.headingTier', { tier });
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
