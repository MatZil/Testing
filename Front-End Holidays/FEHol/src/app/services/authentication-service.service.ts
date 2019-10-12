import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Role } from '../models/role';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

  private thisUser: number;
  public token: string;


  constructor(private http: HttpClient, public jwtHelper: JwtHelperService) {

  }



  public isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    // Check whether the token is expired and return
    // true or false
    return !this.jwtHelper.isTokenExpired(token);
  }

  login(email, password) {
    return this.http.post<any>(`${environment.webApiUrl}/Auth/login`, { email, password })
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
    this.thisUser = JSON.parse(localStorage.getItem('userId'));
    return this.thisUser;
  }

  getRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(`${environment.webApiUrl}/Auth/roles`);
  }
}
