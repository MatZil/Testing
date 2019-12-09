import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { InventoryCategory } from '../models/inventory-category';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InventoryCategoryService {
  private readonly itemCategoryApi = `${environment.webApiUrl}/InventoryCategories`;

  constructor(private http: HttpClient) { }

  getAllCategories(): Observable<InventoryCategory[]> {
    return this.http.get<InventoryCategory[]>(this.itemCategoryApi);
  }
}
