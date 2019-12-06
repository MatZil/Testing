import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { FileType } from '../enums/fileType';
@Injectable({
  providedIn: 'root'
})
export class FilesService {
  private readonly filesApi = `${environment.webApiUrl}/File`;
  constructor(private http: HttpClient) { }

  getFilePathByType(fileType: FileType): Observable<string> {
    return this.http.get<string>(`${this.filesApi}/GetByType?fileType=${fileType}`, { responseType: 'text' as 'json' });
  }

  upload(formData: FormData) {
    return this.http.post(`${this.filesApi}/upload`, formData, { reportProgress: true, observe: 'events' });
  }
}
