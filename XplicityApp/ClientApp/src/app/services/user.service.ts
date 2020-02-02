import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { TableRowUserModel } from '../models/table-row-user-model';
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

    getAllUsers(): Observable<TableRowUserModel[]> {
        return this.http.get<TableRowUserModel[]>(this.userApi);
    }

    getUser(id: number): Observable<TableRowUserModel> {
        return this.http.get<TableRowUserModel>(`${this.userApi}/${id}`);
    }

    registerUser(user: Newuser): Observable<TableRowUserModel> {
      return this.http.post<TableRowUserModel>(this.userApi, user);
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

    getCurrentUser(): Observable<TableRowUserModel> {
      return this.http.get<TableRowUserModel>(`${this.userApi}/self`);
    }

    getRole(): string {
        const token = localStorage.getItem('token');
        // decode the token to get its payload
        const tokenPayload = decode(token);
        return tokenPayload.role;
    }
}
