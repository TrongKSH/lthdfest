import { Component, ChangeDetectionStrategy } from '@angular/core';
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
