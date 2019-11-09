import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { User } from '../models/user';
import { Newuser } from '../models/newuser';
import { Updateuser } from '../models/updateuser';
import { NgForm } from '@angular/forms';
import { PasswordChangeModel } from '../models/password-change-model';
@Injectable({ providedIn: 'root' })

export class UserService {

    private readonly userApi = `${environment.webApiUrl}/Employees`;

    private readonly httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json'
        })
    };

    constructor(private http: HttpClient) { }

    getAllUsers(): Observable<User[]> {
        return this.http.get<User[]>(this.userApi);
    }

    getUser(id: number): Observable<User> {
        return this.http.get<User>(`${this.userApi}/${id}`);
    }

    registerUser(user: User): Observable<User[]> {
        return this.http.post<User[]>(this.userApi, user);
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
}
