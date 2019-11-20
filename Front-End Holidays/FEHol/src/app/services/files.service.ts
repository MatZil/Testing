import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class FilesService {
  private readonly filesApi = `${environment.webApiUrl}/File`;
  constructor(private http: HttpClient) { }

  getFilePathByType(fileType: string): Observable<string> {
    return this.http.get<string>(`${this.filesApi}/GetByType?fileType=${fileType}`, { responseType: 'text' as 'json' });
  }
}
