import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterLink } from '@angular/router';
import { BandCardComponent } from '../band-card/band-card.component';
import type { Band } from '../../models/band.model';

const SLOTS_COUNT = 11; // 4 + 3 + 4 rows

const PLACEHOLDER_BAND: Band = {
  id: 0,
  name: '',
  shortBio: '',
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
  /** Row 1: 4 bands */
  protected readonly slotsRow1 = PLACEHOLDER_SLOTS.slice(0, 4);
  /** Row 2: 3 bands (centered) */
  protected readonly slotsRow2 = PLACEHOLDER_SLOTS.slice(4, 7);
  /** Row 3: 4 bands */
  protected readonly slotsRow3 = PLACEHOLDER_SLOTS.slice(7, 11);
}
