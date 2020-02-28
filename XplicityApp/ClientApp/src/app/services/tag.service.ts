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

  getAllByTitle(tagTitle: string): Observable<Tag[]> {
    return this.http.get<Tag[]>(`${this.tagApi}/find/` + tagTitle);
  }

  getAll(): Observable<Tag[]> {
    return this.http.get<Tag[]>(this.tagApi);
  }

  createNewTag(newTag: NewTag) {
    return this.http.post(this.tagApi, newTag);
  }

  isTagNameValid(tagTitle: string): boolean {
    const tagTitleValidationRegExp = new RegExp('^[a-zA-Z0-9#-]*$');

    if (tagTitle.length > 10 || tagTitle.length < 3 || !tagTitleValidationRegExp.test(tagTitle)) {
      return false;
    }

    return true;
  }
}
