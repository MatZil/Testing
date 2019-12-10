import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FileType } from '../enums/fileType';
@Injectable({
  providedIn: 'root'
})
export class FilesService {
  private readonly filesApi = `${this.baseUrl}api/File`;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getFilePathByType(fileType: FileType): Observable<string> {
    return this.http.get<string>(`${this.filesApi}/GetByType?fileType=${fileType}`, { responseType: 'text' as 'json' });
  }

  upload(formData: FormData) {
    return this.http.post(`${this.filesApi}/upload`, formData, { reportProgress: true, observe: 'events' });
  }
}
