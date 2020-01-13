import { Injectable } from '@angular/core';
import { HttpResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HeadersService {

  constructor() { }

  getFileName(response: HttpResponse<Blob>): string {
    const contentDisposition = response.headers.get('content-disposition');
    const fileName = contentDisposition.split(';')[1].split('filename')[1].split('=')[1].trim();
    return fileName;
  }
}
