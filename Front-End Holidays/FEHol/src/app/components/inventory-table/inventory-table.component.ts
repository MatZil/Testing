import { Component, OnInit, Input } from '@angular/core';
import { InventoryItem } from 'src/app/models/inventory-item';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { InventoryService } from 'src/app/services/inventory.service';
import { InventoryCategory } from 'src/app/models/inventory-category';
import { InventoryCategoryService } from 'src/app/services/inventory-category.service';
import { NzTreeHigherOrderServiceToken } from 'ng-zorro-antd';

@Component({
  selector: 'app-inventory-table',
  templateUrl: './inventory-table.component.html',
  styleUrls: ['./inventory-table.component.scss']
})
export class InventoryTableComponent implements OnInit {
  equipment: InventoryItem[] = [];
  @Input()
  employeeId: number;
  isVisibleNewItemModal = false;
  input: FormGroup;
  categories: InventoryCategory[] = [];
  constructor(private inventoryService: InventoryService, private fb: FormBuilder, private categoryService: InventoryCategoryService) { }

  ngOnInit() {
    this.input = this.fb.group({
      name: [null, [Validators.required]],
      serialNumber: [null, [Validators.required]],
      price: [null, [Validators.required]],
      purchaseDate: [null, [Validators.required]],
      comment: [null],
      inventoryCategoryId: [null, [Validators.required]]
    });

    this.refreshTable(this.employeeId);
  }
  refreshTable(id: number) {
    if (!isNaN(id)) {
      this.inventoryService.getEquipmentByEmployeeId(id).subscribe(data => {
        this.equipment = data;
      });
    } else {
      this.inventoryService.getAllInventoryItems().subscribe(data => {
        this.equipment = data;
      });
    }
  }
  showNewItemModal() {
    this.isVisibleNewItemModal = true;
    this.getCategoriesList();
  }

  closeNewItemModal() {
    this.isVisibleNewItemModal = false;
  }

  save() {
    this.inventoryService.create(this.input.value).subscribe(() => {
      this.isVisibleNewItemModal = false;
      this.refreshTable(this.employeeId);
    });
    this.input.reset();

  }

  getCategoriesList() {
    this.categoryService.getAllCategories().subscribe(data => {
      this.categories = data;
    });
  }
}
