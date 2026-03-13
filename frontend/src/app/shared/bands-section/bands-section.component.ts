import { Component, ChangeDetectionStrategy, inject, computed } from '@angular/core';
import { RouterLink } from '@angular/router';
import { startWith } from 'rxjs/operators';
import { BandCardComponent } from '../band-card/band-card.component';
import { BandService } from '../../services/band.service';
import { toSignal } from '@angular/core/rxjs-interop';
import type { Band } from '../../models/band.model';

@Component({
  selector: 'app-bands-section',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [BandCardComponent, RouterLink],
  templateUrl: './bands-section.component.html',
  styleUrl: './bands-section.component.scss',
})
export class BandsSectionComponent {
  private readonly bandService = inject(BandService);
  protected readonly bands = toSignal(
    this.bandService.getBands().pipe(startWith<Band[] | null>(null)),
    { initialValue: null as Band[] | null }
  );
  protected readonly loading = computed(() => this.bands() === null);
}
