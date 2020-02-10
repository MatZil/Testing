import { Injectable, Inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UrlService {
  root: string;
  constructor(@Inject('BASE_URL') baseUrl: string) {
    this.root = baseUrl;
  }

  getAbsolutePath(relativePath: string): string {
    return this.root + relativePath;
  }
}
