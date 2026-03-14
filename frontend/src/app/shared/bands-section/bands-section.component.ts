import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterLink } from '@angular/router';
import { BandCardComponent } from '../band-card/band-card.component';
import type { Band } from '../../models/band.model';

const SLOTS_COUNT = 15;

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
  protected readonly slots = PLACEHOLDER_SLOTS;
}
