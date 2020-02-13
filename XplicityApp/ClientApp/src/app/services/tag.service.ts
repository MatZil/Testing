import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Tag } from '../models/tag';
import { NewTag } from '../models/new-tag';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TagService {
  private readonly tagApi = `${this.baseUrl}api/Tags`;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getAllByFilter(tagTitle: string): Observable<Tag[]> {
    return this.http.get<Tag[]>(`${this.tagApi}/find/` + tagTitle);
  }

  getAll(): Observable<Tag[]> {
    return this.http.get<Tag[]>(`${this.tagApi}`);
  }

  createNewTag(newTag: NewTag) {
    return this.http.post(this.tagApi, newTag);
  }
}
