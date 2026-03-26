import { Component, ChangeDetectionStrategy, DestroyRef, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TranslocoPipe } from '@ngneat/transloco';
import { toSignal } from '@angular/core/rxjs-interop';
import { BandCardComponent } from '../band-card/band-card.component';
import { BandService } from '../../services/band.service';
import type { Band } from '../../models/band.model';

const TWO_HOURS_MS = 2 * 60 * 60 * 1000;

const SLOTS_COUNT = 11; // 4 + 3 + 4 rows

const PLACEHOLDER_BAND: Band = {
  id: 0,
  name: '',
  bio: '',
  lineupDay: 'LongTranh',
  lineupPosition: 0,
};

export type BandSlot = { band: Band; isPlaceholder: boolean };

const PLACEHOLDER_SLOTS: BandSlot[] = Array.from({ length: SLOTS_COUNT }, () => ({
  band: PLACEHOLDER_BAND,
  isPlaceholder: true,
}));

@Component({
  selector: 'app-bands-section',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [BandCardComponent, RouterLink, TranslocoPipe],
  templateUrl: './bands-section.component.html',
  styleUrl: './bands-section.component.scss',
})
export class BandsSectionComponent {
  private readonly bandService = inject(BandService);
  private readonly destroyRef = inject(DestroyRef);
  private bucketBoundaryTimeoutId: ReturnType<typeof setTimeout> | undefined;
  private readonly compactLogoBands = new Set([
    'blackindustry',
    'cutlon',
    'empathize',
    'elbowdrop',
    'underpressure',
  ]);
  private readonly featuredBands = toSignal(this.bandService.getBands(true), {
    initialValue: [] as Band[],
  });
  private readonly timeBucket = signal(this.getCurrentTwoHourBucket());

  private readonly slots = computed<BandSlot[]>(() => {
    const featured = this
      .shuffleWithSeed(this.featuredBands(), this.timeBucket() + 202)
      .slice(0, SLOTS_COUNT);
    const filled: BandSlot[] = featured.map((band) => ({ band, isPlaceholder: false }));
    if (filled.length >= SLOTS_COUNT) return filled;
    return filled.concat(PLACEHOLDER_SLOTS.slice(0, SLOTS_COUNT - filled.length));
  });

  constructor() {
    this.scheduleNextTwoHourBucketTick();
    this.destroyRef.onDestroy(() => {
      if (this.bucketBoundaryTimeoutId !== undefined) {
        clearTimeout(this.bucketBoundaryTimeoutId);
      }
    });
  }

  private scheduleNextTwoHourBucketTick(): void {
    const now = Date.now();
    const nextBoundaryMs = (Math.floor(now / TWO_HOURS_MS) + 1) * TWO_HOURS_MS;
    const delayMs = nextBoundaryMs - now;
    this.bucketBoundaryTimeoutId = setTimeout(() => {
      this.timeBucket.set(this.getCurrentTwoHourBucket());
      this.scheduleNextTwoHourBucketTick();
    }, delayMs);
  }

  /** Row 1: 4 bands */
  protected readonly slotsRow1 = computed(() => this.slots().slice(0, 4));
  /** Row 2: 3 bands (centered) */
  protected readonly slotsRow2 = computed(() => this.slots().slice(4, 7));
  /** Row 3: 4 bands */
  protected readonly slotsRow3 = computed(() => this.slots().slice(7, 11));

  protected shouldBoostLogo(bandName: string): boolean {
    const key = bandName
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '')
      .replace(/[đĐ]/g, 'd')
      .toLowerCase()
      .replace(/[^a-z0-9]/g, '');
    return this.compactLogoBands.has(key);
  }

  private getCurrentTwoHourBucket(): number {
    return Math.floor(Date.now() / TWO_HOURS_MS);
  }

  private seededRandom(seed: number): () => number {
    let state = seed >>> 0;
    return () => {
      state = (state + 0x6d2b79f5) >>> 0;
      let t = Math.imul(state ^ (state >>> 15), 1 | state);
      t ^= t + Math.imul(t ^ (t >>> 7), 61 | t);
      return ((t ^ (t >>> 14)) >>> 0) / 4294967296;
    };
  }

  private shuffleWithSeed<T>(items: readonly T[], seed: number): T[] {
    const shuffled = [...items];
    const random = this.seededRandom(seed);
    for (let i = shuffled.length - 1; i > 0; i--) {
      const j = Math.floor(random() * (i + 1));
      [shuffled[i], shuffled[j]] = [shuffled[j], shuffled[i]];
    }
    return shuffled;
  }
}
