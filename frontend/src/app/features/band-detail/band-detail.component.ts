import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { BandService } from '../../services/band.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-band-detail',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink],
  templateUrl: './band-detail.component.html',
  styleUrl: './band-detail.component.scss',
})
export class BandDetailComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly bandService = inject(BandService);
  readonly band = toSignal<import('../../models/band.model').Band | null>(
    this.route.paramMap.pipe(
      switchMap((params) => {
        const id = params.get('id');
        return this.bandService.getBand(id ? +id : 0);
      })
    ),
    { initialValue: null }
  );
}
