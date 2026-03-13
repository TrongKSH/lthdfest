import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { BandCardComponent } from '../band-card/band-card.component';
import { BandService } from '../../services/band.service';
import { toSignal } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-bands-section',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [BandCardComponent, RouterLink],
  templateUrl: './bands-section.component.html',
  styleUrl: './bands-section.component.scss',
})
export class BandsSectionComponent {
  private readonly bandService = inject(BandService);
  protected readonly bands = toSignal(this.bandService.getBands(), { initialValue: [] as import('../../models/band.model').Band[] });
}
