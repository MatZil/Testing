import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FileType } from '../enums/fileType';
import * as mime from 'mime-types';

@Injectable({
  providedIn: 'root'
})
export class FilesService {
  private readonly fileApi = `${this.baseUrl}api/File`;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getPolicyRelativePath(): Observable<string> {
    return this.http.get<string>(`${this.fileApi}/Policy`, { responseType: 'text' as 'json' });
  }

  upload(formData: FormData) {
    return this.http.post(`${this.fileApi}/Upload`, formData, { reportProgress: true, observe: 'events' });
  }

  getValidationErrorMessage(fileToUpload: File, requiredType: FileType): string {
    if (fileToUpload.size > 1048576) {
      return 'The file is too large. File size limit is 10mb';
    }
    if (requiredType === FileType.HolidayPolicy) {
      if (fileToUpload.type !== mime.lookup('pdf')) {
        return 'You can only upload pdf files for holiday policy';
      }
    } else if (requiredType === FileType.Document) {
      if (fileToUpload.type !== mime.lookup('docx') && fileToUpload.type !== mime.lookup('doc')) {
        return 'You can only upload doc and docx files for this option';
      }
    } else if (requiredType === FileType.Image) {
      if (fileToUpload.type !== mime.lookup('jpg') && fileToUpload.type !== mime.lookup('png')) {
        return 'You can only upload jpg and png files for this option';
      }
    }
    return '';
  }
}
