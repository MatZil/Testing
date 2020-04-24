import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AuditLog } from '../models/audit-log';

@Injectable({
  providedIn: 'root'
})
export class AuditLogService {
  private readonly auditLogsApi = `${this.baseUrl}api/AuditLogs`;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getAuditLogsByType(entityType: string): Observable<AuditLog[]> {
    let parameters = new HttpParams();
    parameters = parameters.append('AuditLogs', entityType);
    return this.http.get<AuditLog[]>(`${this.auditLogsApi}`, { params: parameters });
  }

  getAuditLogsPage(page: number, size: number): Observable<AuditLog[]> {
    let parameters = new HttpParams();
    parameters = parameters.append('page', page.toString());
    parameters = parameters.append('pageSize', size.toString());
    console.log(`${this.auditLogsApi}/page` + { params: parameters });
    console.log(parameters);
    console.log(`${this.auditLogsApi}/page?page=5&pagesize=5`);
    return this.http.get<AuditLog[]>(`${this.auditLogsApi}/page?page=5&pagesize=5`);
  }
}