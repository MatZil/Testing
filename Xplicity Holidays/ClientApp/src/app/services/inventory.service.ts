import { Injectable } from '@angular/core';
import { InventoryItem } from '../models/inventory-item';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { FormGroup } from '@angular/forms';


@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  private readonly itemApi = `${environment.webApiUrl}/InventoryItems`;

  constructor(private http: HttpClient) { }

  getEquipmentByEmployeeId(employeeId: number): Observable<InventoryItem[]> {
    return this.http.get<InventoryItem[]>(`${this.itemApi}/employee/${employeeId}`);
  }

  getAllInventoryItems(): Observable<InventoryItem[]> {
    return this.http.get<InventoryItem[]>(this.itemApi);
  }

  createNewInventoryItem(newInventoryItem: FormGroup) {
    return this.http.post(this.itemApi, newInventoryItem);
  }
}
