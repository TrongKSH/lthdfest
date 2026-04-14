import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  signal,
  untracked,
} from '@angular/core';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { filter, interval, map } from 'rxjs';
import { TranslocoPipe, TranslocoService } from '@ngneat/transloco';
import { LucideAngularModule } from 'lucide-angular';
import {
  ANNOUNCEMENT_FACEBOOK_URLS,
  ANNOUNCEMENT_IMAGE_SRCS,
  type AnnouncementSlideI18n,
} from './announcement-slides.config';

const ANNOUNCEMENT_AUTO_ADVANCE_MS = 20000;

@Component({
  selector: 'app-announcement-section',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [TranslocoPipe, LucideAngularModule],
  templateUrl: './announcement-section.component.html',
  styleUrl: './announcement-section.component.scss',
})
export class AnnouncementSectionComponent {
  readonly iconSize = 30;
  readonly imageSrcs = ANNOUNCEMENT_IMAGE_SRCS;
  readonly facebookUrls = ANNOUNCEMENT_FACEBOOK_URLS;

  private readonly transloco = inject(TranslocoService);

  readonly currentIndex = signal(0);

  /** From i18n; reactive to translation load (unlike synchronous `translateObject`). */
  readonly items = toSignal(
    this.transloco.selectTranslateObject<unknown>('announcement.items').pipe(
      map((raw) => (Array.isArray(raw) ? (raw as AnnouncementSlideI18n[]) : [])),
    ),
    { initialValue: [] as AnnouncementSlideI18n[] },
  );

  readonly activeItem = computed(() => {
    const list = this.items();
    const i = this.currentIndex();
    if (!list.length) return null;
    return list[i] ?? null;
  });

  readonly activeParagraphs = computed(() => {
    const p = this.activeItem()?.paragraphs;
    return Array.isArray(p) ? p : [];
  });

  readonly activeImageSrc = computed(() => {
    const i = this.currentIndex();
    const srcs = this.imageSrcs;
    if (!srcs.length) return '';
    return srcs[i % srcs.length];
  });

  readonly activeFacebookUrl = computed(() => {
    const i = this.currentIndex();
    const urls = this.facebookUrls;
    if (!urls.length) return '';
    return urls[i % urls.length];
  });

  readonly slideTotal = computed(() => this.items().length);

  /** Drives slide-in direction for CSS animation (next = from right, prev = from left). */
  readonly lastNav = signal<'next' | 'prev'>('next');

  constructor() {
    effect(() => {
      const list = this.items();
      const n = list.length;
      if (n === 0) return;
      const idx = this.currentIndex();
      if (idx >= n) {
        untracked(() => this.currentIndex.set(n - 1));
      }
    });

    interval(ANNOUNCEMENT_AUTO_ADVANCE_MS)
      .pipe(
        takeUntilDestroyed(),
        filter(() => this.slideTotal() > 1),
      )
      .subscribe(() => this.next());
  }

  next(): void {
    const n = this.items().length;
    if (n < 2) return;
    this.lastNav.set('next');
    this.currentIndex.update((i) => (i + 1) % n);
  }

  previous(): void {
    const n = this.items().length;
    if (n < 2) return;
    this.lastNav.set('prev');
    this.currentIndex.update((i) => (i - 1 + n) % n);
  }
}
