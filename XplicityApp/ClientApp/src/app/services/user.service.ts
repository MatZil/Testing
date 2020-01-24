import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { User } from '../models/user';
import { Updateuser } from '../models/updateuser';
import { PasswordChangeModel } from '../models/password-change-model';
import decode from 'jwt-decode';
import { Newuser } from '../models/newuser';

@Injectable({ providedIn: 'root' })

export class UserService {

    private readonly userApi = `${this.baseUrl}api/Employees`;

    private readonly httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json'
        })
    };

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

    getAllUsers(): Observable<User[]> {
        return this.http.get<User[]>(this.userApi);
    }

    getUser(id: number): Observable<User> {
        return this.http.get<User>(`${this.userApi}/${id}`);
    }

    registerUser(user: Newuser): Observable<User> {
        return this.http.post<User>(this.userApi, user);
    }

    deleteUser(id: number) {
        return this.http.delete(`${this.userApi}/${id}`);
    }

    editUser(user: Updateuser, id: number) {
        return this.http.put(`${this.userApi}/${id}`, user);
    }

    changePassword(id: number, passwordChangeModel: PasswordChangeModel) {
        return this.http.post(`${this.userApi}/${id}/ChangePassword`, passwordChangeModel);
    }

    getCurrentUser(): Observable<User> {
        return this.http.get<User>(`${this.userApi}/self`);
    }

    getRole(): string {
        const token = localStorage.getItem('token');
        // decode the token to get its payload
        const tokenPayload = decode(token);
        return tokenPayload.role;
    }
}