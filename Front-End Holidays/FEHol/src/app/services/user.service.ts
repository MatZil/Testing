import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { User } from '../models/user';

@Injectable({ providedIn: 'root' })

export class UserService {

    private readonly userApi = `${environment.webApiUrl}/employee`;

    private readonly httpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      };

    constructor(private http: HttpClient) { }

    getAll(): Observable<User[]> {
        return this.http.get<User[]>(this.userApi);
    }

    register(user: User): Observable<User[]> {
        return this.http.post<User[]>(this.userApi, user);
    }

    delete(id: number) {
        return this.http.delete(this.userApi);
    }
}
