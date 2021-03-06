import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Holiday } from '../models/holiday';
import { NewHoliday } from '../models/new-holiday';
import { EmployeeStatus } from '../models/employee-status.enum';

@Injectable({
  providedIn: 'root'
})
export class HolidaysService {

  private readonly holidaysApiBase = `${this.baseUrl}api/Holidays`;
  private readonly holidaysApiRequest = `${this.baseUrl}api/HolidayConfirm`;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getHolidays(): Observable<Holiday[]> {
    return this.http.get<Holiday[]>(this.holidaysApiBase);
  }

  getHolidayById(id: number): Observable<Holiday> {
    return this.http.get<Holiday>(`${this.holidaysApiBase}/${id}`);
  }

  getHolidaysByStatus(status: EmployeeStatus): Observable<Holiday[]> {
    let parameters = new HttpParams();
    parameters = parameters.append('employeeStatus', status.toString());
    return this.http.get<Holiday[]>(`${this.holidaysApiBase}/GetByStatus`, { params: parameters });
  }

  addHoliday(holiday: NewHoliday): Observable<any> {
    return this.http.post(this.holidaysApiRequest, holiday);
  }

  updateHoliday(id: number, holiday: Holiday): Observable<Holiday> {
    return this.http.put<Holiday>(`${this.holidaysApiBase}/${id}`, holiday);
  }

  isFreeWorkday(date: Date): Observable<boolean> {
    const dateString = date.toLocaleDateString('lt-LT');
    return this.http.get<boolean>(`${this.holidaysApiBase}/is-free-workday?date=${dateString}`);
  }
  getFilteredConfirmedHolidaysBySelectedMonth(date: Date, currentUserId: number, filter: number): Observable<Holiday[]> {
    let parameters = new HttpParams();
    parameters = parameters.append('currentUserId', currentUserId.toString());
    const dateString = date.toLocaleDateString('lt-LT');
    parameters = parameters.append('selectedDate', dateString);
    parameters = parameters.append('filter', filter.toString());

    return this.http.get<Holiday[]>(`${this.holidaysApiBase}/GetFilteredConfirmedByMonth`, { params: parameters });
  }
}
