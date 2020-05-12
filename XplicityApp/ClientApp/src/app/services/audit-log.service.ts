import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { AuditLogs } from '../models/audit-logs';

@Injectable({
  providedIn: 'root'
})
export class AuditLogService {
  private readonly auditLogsApi = `${this.baseUrl}api/AuditLogs`;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getAuditLogsByType(entity: string , page: number, size: number): Observable<AuditLogs> {
    return this.http.get<AuditLogs>(`${this.auditLogsApi}/page?entityType=${entity}&page=${page}&pagesize=${size}`);
  }
  
}