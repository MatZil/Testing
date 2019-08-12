import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PolicyService {

  private readonly holidayInfoApi = `${environment.webApiUrl}/HolidayInfo`;

  constructor(private http: HttpClient) { }

  getHolidaysCount(userId: number): Observable<number> {
    return this.http.get<number>(`${this.holidayInfoApi}/${userId}`);
  }
}

