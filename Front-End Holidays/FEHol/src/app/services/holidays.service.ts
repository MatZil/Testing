import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';

import { Holidays } from '../models/holidays';
import { Requestholidays } from '../models/requestholidays';
import { Newholidays } from '../models/newholidays';

@Injectable({
  providedIn: 'root'
})
export class HolidaysService {

  private readonly holidaysApiBase = `${environment.webApiUrl}/Holidays`;
  private readonly holidaysApiRequest = `${environment.webApiUrl}/HolidayConfirm`;

  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  getHolidays(): Observable<Holidays[]> {
    return this.http.get<Holidays[]>(this.holidaysApiBase);
  }

  getHolidaysById(id: number): Observable<Holidays> {
    return this.http.get<Holidays>(`${this.holidaysApiBase}/${id}`);
  }

  addHolidays(holidays: Requestholidays): Observable<Blob> {
    const headersType = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.post<Blob>(this.holidaysApiRequest, holidays, { headers: headersType, responseType: 'blob' as 'json' });
  }

  editHolidays(holidays: Newholidays, id: number): Observable<Newholidays> {
    return this.http.put<Newholidays>(`${this.holidaysApiBase}/${id}`, holidays);
  }

  deleteHolidays(id: number): Observable<Holidays> {
    return this.http.delete<Holidays>(`${this.holidaysApiBase}/${id}`);
  }
}
