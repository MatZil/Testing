import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { EmailTemplate } from '../models/email-template';
import { NewEmailTemplate } from '../models/new-email-template';

@Injectable({
  providedIn: 'root'
})

export class EmailTemplatesService {
  private readonly emailTemplateApi = `${environment.webApiUrl}/EmailTemplates`;

  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  getEmailTemplates(): Observable<EmailTemplate[]> {
    return this.http.get<EmailTemplate[]>(this.emailTemplateApi);
  }

  getEmailTemplate(id: number): Observable<EmailTemplate> {
    return this.http.get<EmailTemplate>(`${this.emailTemplateApi}/${id}`);
  }

  editEmailTemplate(emailTemplate: NewEmailTemplate, id: number): Observable<NewEmailTemplate> {
    return this.http.put<NewEmailTemplate>(`${this.emailTemplateApi}/${id}`, emailTemplate);
  }
}
