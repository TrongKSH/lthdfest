import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import type { Festival } from '../models/festival.model';
import type { CountdownDto } from '../models/countdown.model';

@Injectable({ providedIn: 'root' })
export class FestivalService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  getFestival(): Observable<Festival> {
    return this.http.get<Festival>(`${this.apiUrl}/api/festival`);
  }

  getCountdown(): Observable<CountdownDto> {
    return this.http.get<CountdownDto>(`${this.apiUrl}/api/countdown`);
  }
}
