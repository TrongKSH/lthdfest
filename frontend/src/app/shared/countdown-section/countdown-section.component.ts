import { Component, ChangeDetectionStrategy, input, signal, computed, effect } from '@angular/core';
import { TranslocoPipe } from '@ngneat/transloco';
import type { CountdownDto } from '../../models/countdown.model';

const STATIC_EVENT_DATE = new Date(2026, 4, 8); // 08 May 2026 (month 0-indexed)

@Component({
  selector: 'app-countdown-section',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [TranslocoPipe],
  templateUrl: './countdown-section.component.html',
  styleUrl: './countdown-section.component.scss',
})
export class CountdownSectionComponent {
  /** When set (e.g. from Home), avoids a second countdown subscription. */
  readonly countdownDto = input<CountdownDto | null>(null);
  readonly eventDate = computed(() => {
    const dto = this.countdownDto();
    if (dto?.eventDate) {
      const date = new Date(dto.eventDate);
      if (!isNaN(date.getTime())) return date;
    }
    return STATIC_EVENT_DATE;
  });
  readonly eventName = computed(() => this.countdownDto()?.eventName ?? '');
  readonly daysLeft = signal<number | null>(null);
  readonly formattedDate = computed(() => {
    const d = this.eventDate();
    const day = d.getDate();
    const month = d.getMonth() + 1;
    const year = d.getFullYear();
    return `${String(day).padStart(2, '0')}/${String(month).padStart(2, '0')}/${year}`;
  });
  readonly cautionDateRange = '08-09/05/2026';

  constructor() {
    effect(() => {
      const target = this.eventDate();
      if (!target) return;
      const update = () => {
        const now = new Date();
        if (now >= target) {
          this.daysLeft.set(0);
          return;
        }
        const diff = target.getTime() - now.getTime();
        this.daysLeft.set(Math.max(0, Math.ceil(diff / (1000 * 60 * 60 * 24))));
      };
      update();
      const id = setInterval(update, 60_000);
      return () => clearInterval(id);
    });
  }
}
