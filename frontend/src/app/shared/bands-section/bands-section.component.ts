import { Component, ChangeDetectionStrategy, computed, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { BandCardComponent } from '../band-card/band-card.component';
import { BandService } from '../../services/band.service';
import type { Band } from '../../models/band.model';

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
  imports: [BandCardComponent, RouterLink],
  templateUrl: './bands-section.component.html',
  styleUrl: './bands-section.component.scss',
})
export class BandsSectionComponent {
  private readonly bandService = inject(BandService);
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

  private readonly slots = computed<BandSlot[]>(() => {
    const featured = this.featuredBands().slice(0, SLOTS_COUNT);
    const filled: BandSlot[] = featured.map((band) => ({ band, isPlaceholder: false }));
    if (filled.length >= SLOTS_COUNT) return filled;
    return filled.concat(PLACEHOLDER_SLOTS.slice(0, SLOTS_COUNT - filled.length));
  });

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
}
