import { Component, ChangeDetectionStrategy, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TranslocoPipe } from '@ngneat/transloco';
import type { Band } from '../../models/band.model';

@Component({
  selector: 'app-band-card',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, TranslocoPipe],
  templateUrl: './band-card.component.html',
  styleUrl: './band-card.component.scss',
})
export class BandCardComponent {
  band = input.required<Band>();
  isPlaceholder = input<boolean>(false);
  boostLogo = input<boolean>(false);
}
