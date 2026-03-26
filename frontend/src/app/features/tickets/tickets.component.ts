import { Component, ChangeDetectionStrategy, inject, signal, computed, effect } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { map } from 'rxjs';
import { TranslocoPipe } from '@ngneat/transloco';
import { PRESALE_DEF, TICKET_PACK_DEFS, type TicketPackDef } from './tickets-content';
import { TicketsPurchaseComponent } from './tickets-purchase/tickets-purchase.component';
import { TicketsPurchaseInfoComponent } from './tickets-purchase/tickets-purchase-info/tickets-purchase-info.component';
import { TicketsPurchaseConfirmComponent } from './tickets-purchase/tickets-purchase-confirm/tickets-purchase-confirm.component';
import { TicketsPurchaseTransferComponent } from './tickets-purchase/tickets-purchase-transfer/tickets-purchase-transfer.component';

@Component({
  selector: 'app-tickets',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    TranslocoPipe,
    TicketsPurchaseComponent,
    TicketsPurchaseInfoComponent,
    TicketsPurchaseConfirmComponent,
    TicketsPurchaseTransferComponent,
  ],
  templateUrl: './tickets.component.html',
  styleUrl: './tickets.component.scss',
})
export class TicketsComponent {
  readonly presale = PRESALE_DEF;
  readonly packs = TICKET_PACK_DEFS;
  /** For template: packs that show a secondary peek line */
  protected readonly hasPeekSub = (p: TicketPackDef): boolean => p.id === 'brotherhood';
  protected readonly hasHoverSubtitle = (p: TicketPackDef): boolean => p.id === 'brotherhood';

  /** For touch devices: one expanded card at a time. */
  readonly activeCardId = signal<string | null>(null);

  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly document = inject(DOCUMENT);

  readonly purchaseType = toSignal(
    this.route.queryParamMap.pipe(map((p) => p.get('purchase'))),
    { initialValue: null },
  );

  readonly purchaseStep = toSignal(
    this.route.queryParamMap.pipe(map((p) => p.get('step') ?? 'qty')),
    { initialValue: 'qty' },
  );

  readonly purchaseQty = toSignal(
    this.route.queryParamMap.pipe(
      map((p) => {
        const raw = p.get('qty');
        const n = raw ? Number(raw) : 0;
        return Number.isFinite(n) && n > 0 ? Math.floor(n) : 0;
      }),
    ),
    { initialValue: 0 },
  );

  readonly hasPurchase = computed(() => this.purchaseType() !== null);
  readonly hasPurchaseFlow = computed(
    () => this.purchaseType() !== null || this.purchaseStep() === 'receipt',
  );

  constructor() {
    effect((onCleanup) => {
      const lock = this.hasPurchaseFlow();
      const body = this.document.body;
      const prevOverflow = body.style.overflow;
      const prevTouchAction = body.style.touchAction;

      if (lock) {
        body.style.overflow = 'hidden';
        body.style.touchAction = 'none';
      } else {
        body.style.overflow = '';
        body.style.touchAction = '';
      }

      onCleanup(() => {
        body.style.overflow = prevOverflow;
        body.style.touchAction = prevTouchAction;
      });
    });
  }

  toggleCard(id: string): void {
    this.activeCardId.update((current) => (current === id ? null : id));
  }

  onPresaleBuy(): void {
    this.activeCardId.set(null);
    this.router.navigate(['/tickets'], { queryParams: { purchase: 'presale' } });
  }

  onPackBuy(pack: TicketPackDef): void {
    this.activeCardId.set(null);
    this.router.navigate(['/tickets'], { queryParams: { purchase: pack.id } });
  }
}
