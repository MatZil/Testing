import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { Client } from '../models/client';

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  private readonly clientApi = `${environment.webApiUrl}/clients`;

  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  getClient(): Observable<Client[]> {
    return this.http.get<Client[]>(this.clientApi);
  }

  getClientById(clientId: number): Observable<Client> {
    return this.http.get<Client>(`${this.clientApi}/${clientId}`);
  }

  addClient(client: Client): Observable<Client> {
    return this.http.post<Client>(this.clientApi, client, this.httpOptions);
  }

  deleteClient(clientId: number): Observable<Client> {
    return this.http.delete<Client>(`${this.clientApi}/${clientId}`);
  }
}
