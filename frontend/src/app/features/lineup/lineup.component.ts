import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import type { Band } from '../../models/band.model';
import { BandService } from '../../services/band.service';
type LineupFilter = 'all' | 'longtranh' | 'hodau';

@Component({
  selector: 'app-lineup',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink],
  templateUrl: './lineup.component.html',
  styleUrl: './lineup.component.scss',
})
export class LineupComponent {
  private readonly bandService = inject(BandService);
  protected readonly activeFilter = signal<LineupFilter>('all');
  protected readonly activeFilterIndex = computed(() => {
    const f = this.activeFilter();
    if (f === 'all') return 0;
    if (f === 'longtranh') return 1;
    return 2; // hodau
  });
  protected readonly bands = toSignal(this.bandService.getBands(), { initialValue: [] as Band[] });
  protected readonly filteredBands = computed(() => {
    const allBands = this.bands().slice().sort((a, b) => a.lineupPosition - b.lineupPosition);
    const filter = this.activeFilter();
    if (filter === 'all') return allBands;
    return allBands.filter((band) => this.belongsToFilter(band, filter));
  });

  protected setFilter(filter: LineupFilter): void {
    this.activeFilter.set(filter);
  }

  private belongsToFilter(band: Band, filter: Exclude<LineupFilter, 'all'>): boolean {
    return filter === 'longtranh' ? band.lineupDay === 'LongTranh' : band.lineupDay === 'HoDau';
  }
}

