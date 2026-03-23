import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, of, shareReplay, switchMap } from 'rxjs';
import { environment } from '../../environments/environment';
import type { Band, LineupBand } from '../models/band.model';

@Injectable({ providedIn: 'root' })
export class BandService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;
  private readonly allBands$ = this.http
    .get<Band[]>(`${this.apiUrl}/api/bands`)
    .pipe(shareReplay({ bufferSize: 1, refCount: true }));
  private readonly lineupBands$ = this.http
    .get<LineupBand[]>(`${this.apiUrl}/api/bands/lineup`)
    .pipe(shareReplay({ bufferSize: 1, refCount: true }));
  private readonly featuredBands$ = this.http
    .get<Band[]>(`${this.apiUrl}/api/bands`, { params: { featured: 'true' } })
    .pipe(shareReplay({ bufferSize: 1, refCount: true }));

  getBands(featured?: boolean): Observable<Band[]> {
    if (featured === true) return this.featuredBands$;
    return this.allBands$;
  }

  getLineupBands(): Observable<LineupBand[]> {
    return this.lineupBands$;
  }

  getBand(id: number): Observable<Band> {
    return this.allBands$.pipe(
      map((bands) => bands.find((band) => band.id === id) ?? null),
      switchMap((band) =>
        band ? of(band) : this.http.get<Band>(`${this.apiUrl}/api/bands/${id}`)
      )
    );
  }
}
