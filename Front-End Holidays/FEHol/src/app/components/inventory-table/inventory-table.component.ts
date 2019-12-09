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
  constructor(
    private inventoryService: InventoryService,
    private formBuilder: FormBuilder,
    private categoryService: InventoryCategoryService
  ) { }

  ngOnInit() {
    this.input = this.formBuilder.group({
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
    console.log(id);
    if (id) {

      this.inventoryService.getEquipmentByEmployeeId(id).subscribe(inventoryItems => {
        this.equipment = inventoryItems;
      });
    } else {
      this.inventoryService.getAllInventoryItems().subscribe(inventoryItems => {
        this.equipment = inventoryItems;
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

  createNewItem() {
    this.inventoryService.createNewInventoryItem(this.input.value).subscribe(() => {
      this.isVisibleNewItemModal = false;
      this.refreshTable(this.employeeId);
    });
    this.input.reset();

  }

  getCategoriesList() {
    this.categoryService.getAllCategories().subscribe(categories => {
      this.categories = categories;
    });
  }
}
