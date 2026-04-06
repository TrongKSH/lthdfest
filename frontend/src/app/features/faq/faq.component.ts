import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { TranslocoPipe } from '@ngneat/transloco';

import { FAQ_ITEM_IDS } from './faq-content';

@Component({
  selector: 'app-faq',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [TranslocoPipe],
  templateUrl: './faq.component.html',
  styleUrl: './faq.component.scss',
})
export class FaqComponent {
  readonly itemIds = FAQ_ITEM_IDS;

  /** One panel open at a time; null = all closed */
  readonly openId = signal<number | null>(null);

  toggle(id: number): void {
    this.openId.update((current) => (current === id ? null : id));
  }
}
