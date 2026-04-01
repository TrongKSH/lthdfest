import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { TranslocoPipe } from '@ngneat/transloco';
import { toSignal } from '@angular/core/rxjs-interop';
import type { LineupBand } from '../../models/band.model';
import { BandService } from '../../services/band.service';

const TWO_HOURS_MS = 2 * 60 * 60 * 1000;

type LineupFilter = 'all' | 'longtranh' | 'hodau';

@Component({
  selector: 'app-lineup',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, TranslocoPipe],
  templateUrl: './lineup.component.html',
  styleUrl: './lineup.component.scss',
})
export class LineupComponent {
  private readonly bandService = inject(BandService);
  private readonly route = inject(ActivatedRoute);
  private readonly destroyRef = inject(DestroyRef);
  private bucketBoundaryTimeoutId: ReturnType<typeof setTimeout> | undefined;
  private readonly compactLogoBands = new Set([
    'blackindustry',
    'cutlon',
    'empathize',
    'elbowdrop',
    'underpressure'
  ]);
  private static parseFilter(raw: string | null): LineupFilter {
    if (raw === 'longtranh' || raw === 'hodau') return raw;
    return 'all';
  }
  protected readonly activeFilter = signal<LineupFilter>(
    LineupComponent.parseFilter(this.route.snapshot.queryParamMap.get('filter')),
  );
  protected readonly activeFilterIndex = computed(() => {
    const f = this.activeFilter();
    if (f === 'all') return 0;
    if (f === 'longtranh') return 1;
    return 2; // hodau
  });
  protected readonly bands = toSignal(this.bandService.getLineupBands(), {
    initialValue: [] as LineupBand[],
  });
  private readonly timeBucket = signal(this.getCurrentTwoHourBucket());
  private readonly randomizedBands = computed(() =>
    this.shuffleWithSeed(this.bands(), this.timeBucket() + 101)
  );
  protected readonly filteredBands = computed(() => {
    const allBands = this.randomizedBands();
    const filter = this.activeFilter();
    if (filter === 'all') return allBands;
    return allBands.filter((band) => this.belongsToFilter(band, filter));
  });

  protected readonly boostLogoMap = computed(() => {
    const m = new Map<number, boolean>();
    for (const band of this.bands()) {
      m.set(band.id, this.normalizeThenCheck(band.name));
    }
    return m;
  });

  protected setFilter(filter: LineupFilter): void {
    this.activeFilter.set(filter);
  }

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

  private belongsToFilter(band: LineupBand, filter: Exclude<LineupFilter, 'all'>): boolean {
    return filter === 'longtranh' ? band.lineupDay === 'LongTranh' : band.lineupDay === 'HoDau';
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

  private normalizeThenCheck(bandName: string): boolean {
    const key = bandName
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '')
      .replace(/[đĐ]/g, 'd')
      .toLowerCase()
      .replace(/[^a-z0-9]/g, '');
    return this.compactLogoBands.has(key);
  }
}

