import { Injectable, Inject } from '@angular/core';
import { InventoryItem } from '../models/inventory-item';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { FormGroup } from '@angular/forms';
import { NewInventoryItem } from '../models/new-inventory-item';

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

  createNewInventoryItem(newInventoryItem: NewInventoryItem) {
    console.log(newInventoryItem);
    return this.http.post(this.inventoryItemApi, newInventoryItem);
  }

  updateInventoryItem(id: number, inventoryItem: InventoryItem) {
    return this.http.put(`${this.inventoryItemApi}/${id}`, inventoryItem);
  }
}
