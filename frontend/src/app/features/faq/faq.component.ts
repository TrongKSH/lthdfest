import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { FAQ_ITEMS, type FaqItem } from './faq-content';

@Component({
  selector: 'app-faq',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [],
  templateUrl: './faq.component.html',
  styleUrl: './faq.component.scss',
})
export class FaqComponent {
  /** Static copy from faq-content.ts — no HTTP, no database */
  readonly items: readonly FaqItem[] = FAQ_ITEMS;

  /** One panel open at a time; null = all closed */
  readonly openId = signal<number | null>(null);

  toggle(id: number): void {
    this.openId.update((current) => (current === id ? null : id));
  }

  isOpen(id: number): boolean {
    return this.openId() === id;
  }
}
