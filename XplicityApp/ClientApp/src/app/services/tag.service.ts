import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Tag } from '../models/tag';
import { NewTag } from '../models/new-tag';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TagService {
  private readonly tagApi = `${this.baseUrl}api/Tags`;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getAllByFilter(tagTitle: string) {
    return this.http.get<Tag[]>(`${this.tagApi}/find/` + tagTitle);
  }

  createNewTag(newTag: NewTag): Observable<any> {
    return this.http.post(this.tagApi, newTag);
  }
}
