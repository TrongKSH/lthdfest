import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin, shareReplay } from 'rxjs';
import { environment } from '../../environments/environment';
import type { Festival } from '../models/festival.model';
import type { CountdownDto } from '../models/countdown.model';

export interface HomePageData {
  festival: Festival;
  countdown: CountdownDto;
}

@Injectable({ providedIn: 'root' })
export class FestivalService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  /** One in-flight / cached GET; refCount clears when no subscribers remain. */
  private readonly festival$ = this.http.get<Festival>(`${this.apiUrl}/api/festival`).pipe(
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  private readonly countdown$ = this.http.get<CountdownDto>(`${this.apiUrl}/api/countdown`).pipe(
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  getFestival(): Observable<Festival> {
    return this.festival$;
  }

  getCountdown(): Observable<CountdownDto> {
    return this.countdown$;
  }

  /** Single parallel pair of requests; reuses cached streams when already loaded. */
  getHomePageData(): Observable<HomePageData> {
    return forkJoin({
      festival: this.festival$,
      countdown: this.countdown$,
    });
  }
}
