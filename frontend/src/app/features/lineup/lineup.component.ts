import { ChangeDetectionStrategy, Component, computed, signal } from '@angular/core';
import type { Band } from '../../models/band.model';
type LineupFilter = 'all' | 'longtranh' | 'hodau';

const CLIENT_BANDS: Band[] = Array.from({ length: 21 }, (_, index) => {
  const n = index + 1;
  return {
    id: n,
    name: `Band ${n.toString().padStart(2, '0')}`,
    shortBio: '',
    lineupPosition: n,
    genre: n <= 11 ? 'Long Tranh' : 'Ho Dau',
  };
});

@Component({
  selector: 'app-lineup',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [],
  templateUrl: './lineup.component.html',
  styleUrl: './lineup.component.scss',
})
export class LineupComponent {
  protected readonly activeFilter = signal<LineupFilter>('all');
  protected readonly bands = computed(() => CLIENT_BANDS);
  protected readonly filteredBands = computed(() => {
    const allBands = this.bands();
    const filter = this.activeFilter();
    if (filter === 'all') return allBands;
    return allBands.filter((band, index) => this.belongsToFilter(band, filter, index));
  });

  protected setFilter(filter: LineupFilter): void {
    this.activeFilter.set(filter);
  }

  private belongsToFilter(band: Band, filter: Exclude<LineupFilter, 'all'>, index: number): boolean {
    const haystack = this.normalize(`${band.name} ${band.genre ?? ''}`);

    if (haystack.includes('long tranh')) return filter === 'longtranh';
    if (haystack.includes('ho dau') || haystack.includes('hổ đấu')) return filter === 'hodau';
    if (haystack.includes('longtranh')) return filter === 'longtranh';
    if (haystack.includes('hodau')) return filter === 'hodau';

    // Fallback split when API data has no explicit group marker.
    return filter === 'longtranh' ? index % 2 === 0 : index % 2 === 1;
  }

  private normalize(value: string): string {
    return value
      .normalize('NFD')
      .replace(/\p{Diacritic}/gu, '')
      .toLowerCase()
      .trim();
  }
}

