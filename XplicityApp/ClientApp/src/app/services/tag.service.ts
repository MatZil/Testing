import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Tag } from '../models/tag';
import { Observable } from 'rxjs';
import { NewTag } from '../models/new-tag';

@Injectable({
  providedIn: 'root'
})
export class TagService {
  private readonly tagApi = `${this.baseUrl}api/Tags`;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getAllByFilter(tagTitle: string): Observable<Tag[]> {
    return this.http.get<Tag[]>(this.tagApi +`/`+ tagTitle);
  }

  createNewTag(newTag: NewTag) {
    console.log(newTag);
    return this.http.post(this.tagApi, newTag);
  }
}
