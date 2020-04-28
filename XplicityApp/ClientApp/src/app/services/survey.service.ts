import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Survey } from '../models/survey';

@Injectable({
  providedIn: 'root'
})
export class SurveyService {

  private readonly surveyApi = `${this.baseUrl}api/Surveys`;

  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getAllSurveys(): Observable<Survey[]> {
    return this.http.get<Survey[]>(this.surveyApi);
  }

  getSurveyById(id: number): Observable<Survey> {
    return this.http.get<Survey>(`${this.surveyApi}/${id}`);
  }

  deleteSurvey(surveyId: number): Observable<Survey> {
    return this.http.delete<Survey>(`${this.surveyApi}/${surveyId}`);
  }
}
