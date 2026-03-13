import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import type { Band } from '../models/band.model';

@Injectable({ providedIn: 'root' })
export class BandService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  getBands(featured?: boolean): Observable<Band[]> {
    const options =
      featured !== undefined
        ? { params: { featured: featured.toString() } }
        : {};
    return this.http.get<Band[]>(`${this.apiUrl}/api/bands`, options);
  }

  getBand(id: number): Observable<Band> {
    return this.http.get<Band>(`${this.apiUrl}/api/bands/${id}`);
  }
}
