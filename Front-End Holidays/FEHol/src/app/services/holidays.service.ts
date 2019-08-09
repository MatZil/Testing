import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { Holidays } from '../models/holidays';

@Injectable({
  providedIn: 'root'
})
export class HolidaysService {

  private readonly holidaysApi = `${environment.webApiUrl}/holidays`;

  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  getHolidays(): Observable<Holidays[]> {
    return this.http.get<Holidays[]>(this.holidaysApi);
  }

  addHolidays(holidays: Holidays): Observable<Holidays> {
    return this.http.post<Holidays>(this.holidaysApi, holidays, this.httpOptions);
  }

  deleteHolidays(): Observable<Holidays> {
    return this.http.delete<Holidays>(this.holidaysApi, this.httpOptions);
  }
}
