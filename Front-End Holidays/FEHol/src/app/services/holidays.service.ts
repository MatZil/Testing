import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

import { Holidays } from '../models/holidays';
import { Requestholidays } from '../models/requestholidays';
import { Newholidays } from '../models/newholidays';
import { EmployeeStatus } from '../models/employee-status.enum';

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

  getHolidaysByStatus(status: EmployeeStatus): Observable<Holidays[]> {
    let parameters = new HttpParams();
    parameters = parameters.append('employeeStatus', status.toString());
    return this.http.get<Holidays[]>(`${this.holidaysApiBase}/GetByStatus`, { params: parameters });
  }

  addHolidays(holidays: Requestholidays): Observable<any> {
    return this.http.post(this.holidaysApiRequest, holidays);
  }

  editHolidays(holidays: Newholidays, id: number): Observable<Newholidays> {
    return this.http.put<Newholidays>(`${this.holidaysApiBase}/${id}`, holidays);
  }

  deleteHolidays(id: number): Observable<Holidays> {
    return this.http.delete<Holidays>(`${this.holidaysApiBase}/${id}`);
  }
}
