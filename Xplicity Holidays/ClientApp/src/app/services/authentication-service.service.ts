import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { JwtHelperService } from '@auth0/angular-jwt';
import { Role } from '../models/role';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

  private thisUserId: number;

  constructor(private http: HttpClient, public jwtHelper: JwtHelperService, 
    @Inject('BASE_URL') private baseUrl: string) {

  }

  public isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    // Check whether the token is expired and return
    // true or false
    return !this.jwtHelper.isTokenExpired(token);
  }

  login(email, password) {
    return this.http.post<any>(`${this.baseUrl}api/Auth/login`, { email, password })
      .pipe(map(it => {
        localStorage.setItem('userId', JSON.stringify(it.employeeId));
        localStorage.setItem('token', it.token);
      }));

  }

  logout() {
    // remove user from local storage and set current user to null
    localStorage.removeItem('userId');
    localStorage.removeItem('token');
  }

  getUserId() {
    this.thisUserId = JSON.parse(localStorage.getItem('userId'));
    return this.thisUserId;
  }

  getRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(`${this.baseUrl}/api/Auth/roles`);
  }
}
