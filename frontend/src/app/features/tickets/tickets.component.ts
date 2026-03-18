import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { PRESALE, TICKET_PACKS, type TicketPack } from './tickets-content';

@Component({
  selector: 'app-tickets',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [],
  templateUrl: './tickets.component.html',
  styleUrl: './tickets.component.scss',
})
export class TicketsComponent {
  readonly presale = PRESALE;
  readonly packs = TICKET_PACKS;
  /** For touch devices: one expanded card at a time. */
  readonly activeCardId = signal<string | null>(null);

  toggleCard(id: string): void {
    this.activeCardId.update((current) => (current === id ? null : id));
  }

  onPresaleBuy(event: MouseEvent): void {
    const url = (this.presale.buyUrl as string).trim();
    if (!url || url === '#') {
      event.preventDefault();
    }
  }

  onPackBuy(event: MouseEvent, pack: TicketPack): void {
    const url = (pack.buyUrl as string).trim();
    if (!url || url === '#') {
      event.preventDefault();
    }
  }
}
