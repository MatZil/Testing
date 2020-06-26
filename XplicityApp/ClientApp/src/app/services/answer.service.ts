import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Answer } from '../models/answer';

@Injectable({
  providedIn: 'root'
})
export class AnswerService {
  private readonly answersApi = `${this.baseUrl}api/Answers`;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) { }

  createAnswers(answers: Answer[]) {
    return this.http.post(this.answersApi, answers);
  }
}
