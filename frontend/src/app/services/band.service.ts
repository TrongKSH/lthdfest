import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, of, shareReplay } from 'rxjs';
import { environment } from '../../environments/environment';
import type { Band, BandListItem, LineupBand } from '../models/band.model';

@Injectable({ providedIn: 'root' })
export class BandService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;
  private readonly allBands$ = this.http
    .get<BandListItem[]>(`${this.apiUrl}/api/bands`)
    .pipe(shareReplay({ bufferSize: 1, refCount: true }));
  private readonly lineupBands$ = this.http
    .get<LineupBand[]>(`${this.apiUrl}/api/bands/lineup`)
    .pipe(shareReplay({ bufferSize: 1, refCount: true }));
  private readonly featuredBands$ = this.http
    .get<BandListItem[]>(`${this.apiUrl}/api/bands`, { params: { featured: 'true' } })
    .pipe(shareReplay({ bufferSize: 1, refCount: true }));

  private readonly bandCache = new Map<number, Observable<Band | null>>();

  getBands(featured?: boolean): Observable<BandListItem[]> {
    if (featured === true) return this.featuredBands$;
    return this.allBands$;
  }

  getLineupBands(): Observable<LineupBand[]> {
    return this.lineupBands$;
  }

  getBand(id: number): Observable<Band | null> {
    let cached = this.bandCache.get(id);
    if (!cached) {
      cached = this.http.get<Band>(`${this.apiUrl}/api/bands/${id}`).pipe(
        catchError(() => of(null)),
        shareReplay({ bufferSize: 1, refCount: true }),
      );
      this.bandCache.set(id, cached);
    }
    return cached;
  }
}
