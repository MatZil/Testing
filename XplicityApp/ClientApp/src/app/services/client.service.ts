import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Client } from '../models/client';
import { Newclient } from '../models/newclient';

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  private readonly clientApi = `${this.baseUrl}api/Clients`;

  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getClient(): Observable<Client[]> {
    return this.http.get<Client[]>(this.clientApi);
  }

  getClientById(id: number): Observable<Client> {
    return this.http.get<Client>(`${this.clientApi}/${id}`);
  }

  editClient(client: Newclient, id: number): Observable<Newclient> {
    return this.http.put<Newclient>(`${this.clientApi}/${id}`, client);
  }

  addClient(newClient: Newclient): Observable<Client> {
    return this.http.post<Client>(this.clientApi, newClient, this.httpOptions);
  }

  deleteClient(clientId: number): Observable<Client> {
    return this.http.delete<Client>(`${this.clientApi}/${clientId}`);
  }
}
