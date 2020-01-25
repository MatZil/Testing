import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { InventoryCategory } from '../models/inventory-category';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InventoryCategoryService {
  private readonly inventoryCategoryApi = `${this.baseUrl}api/InventoryCategories`;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getAllCategories(): Observable<InventoryCategory[]> {
    return this.http.get<InventoryCategory[]>(this.inventoryCategoryApi);
  }
}
