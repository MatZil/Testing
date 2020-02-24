import { Injectable, Inject } from '@angular/core';
import { InventoryItem } from '../models/inventory-item';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FormGroup } from '@angular/forms';
import { NewInventoryItem } from '../models/new-inventory-item';
import { InventoryStatus } from '../models/inventory-status.enum';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  private readonly inventoryItemApi = `${this.baseUrl}api/InventoryItems`;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getEquipmentByEmployeeId(employeeId: number): Observable<InventoryItem[]> {
    return this.http.get<InventoryItem[]>(`${this.inventoryItemApi}/employee/${employeeId}`);
  }

  getAllInventoryItems(): Observable<InventoryItem[]> {
    return this.http.get<InventoryItem[]>(this.inventoryItemApi);
  }

  getIventoryByStatus(status: InventoryStatus): Observable<InventoryItem[]> {
    let parameters = new HttpParams();
    parameters = parameters.append('inventoryItemStatus', status.toString());
    return this.http.get<InventoryItem[]>(`${this.inventoryItemApi}/GetByStatus`, { params: parameters });
  }

  createNewInventoryItem(newInventoryItem: NewInventoryItem) {
    return this.http.post(this.inventoryItemApi, newInventoryItem);
  }

  updateInventoryItem(id: number, inventoryItem: InventoryItem) {
    return this.http.put(`${this.inventoryItemApi}/${id}`, inventoryItem);
  }
}
