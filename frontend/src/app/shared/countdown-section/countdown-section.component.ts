import { Component, ChangeDetectionStrategy, inject, signal, computed, effect } from '@angular/core';
import { FestivalService } from '../../services/festival.service';
import { toSignal } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-countdown-section',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './countdown-section.component.html',
  styleUrl: './countdown-section.component.scss',
})
export class CountdownSectionComponent {
  private readonly festivalService = inject(FestivalService);
  readonly countdownDto = toSignal(this.festivalService.getCountdown(), { initialValue: null });
  readonly eventDate = computed(() => {
    const dto = this.countdownDto();
    if (!dto?.eventDate) return null;
    const date = new Date(dto.eventDate);
    return isNaN(date.getTime()) ? null : date;
  });
  readonly eventName = computed(() => this.countdownDto()?.eventName ?? '');
  readonly daysLeft = signal<number | null>(null);
  readonly formattedDate = computed(() => {
    const d = this.eventDate();
    if (!d) return '';
    const day = d.getDate();
    const month = d.getMonth() + 1;
    const year = d.getFullYear();
    return `${day}/${String(month).padStart(2, '0')}/${year}`;
  });

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
