import { Component, OnInit, Input } from '@angular/core';
import { InventoryItem } from 'src/app/models/inventory-item';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { InventoryService } from 'src/app/services/inventory.service';
import { InventoryCategory } from 'src/app/models/inventory-category';
import { InventoryCategoryService } from 'src/app/services/inventory-category.service';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user.service';

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
  employees: User[] = [];
  selectedEmployee;
  isModifying = false;

  constructor(
    private inventoryService: InventoryService,
    private formBuilder: FormBuilder,
    private categoryService: InventoryCategoryService,
    private userService: UserService
  ) { }

  ngOnInit() {
    this.input = this.formBuilder.group({
      id: [null],
      name: [null, [Validators.required]],
      serialNumber: [null, [Validators.required]],
      price: [null, [Validators.required]],
      purchaseDate: [null, [Validators.required]],
      expiryDate: [null],
      category: [null],
      comment: [null],
      inventoryCategoryId: [null, [Validators.required]],
      employeeId: [null]
    });
    this.getAllUsers();
    this.getCategoriesList();
    this.refreshTable(this.employeeId);
  }
  refreshTable(id: number) {
    if (id) {
      this.inventoryService.getEquipmentByEmployeeId(id).subscribe(inventoryItems => {
        this.equipment = inventoryItems;
      });
      this.selectedEmployee = id;
    } else {
      this.inventoryService.getAllInventoryItems().subscribe(inventoryItems => {
        this.equipment = inventoryItems;
      });
    }
  }
  showNewItemModal() {
    this.isVisibleNewItemModal = true;
  }

  closeNewItemModal() {
    this.isVisibleNewItemModal = false;
    this.isModifying = false;
  }

  saveInventoryItem() {
    if (!this.isModifying) {
      console.log(this.input.value);
      this.inventoryService.createNewInventoryItem(this.input.value).subscribe(() => {
        this.refreshTable(this.employeeId);
      });
    } else {
      console.log(this.input.value);
      this.inventoryService.updateInventoryItem(this.input.value.id, this.input.value).subscribe(() => {
        this.refreshTable(this.employeeId);
      });
    }
    this.isVisibleNewItemModal = false;
    this.isModifying = false;
    this.input.reset();
  }

  getCategoriesList() {
    this.categoryService.getAllCategories().subscribe(categories => {
      this.categories = categories;
    });
  }

  getAllUsers() {
    this.userService.getAllUsers().subscribe(users => {
      this.employees = users;
    });
  }

  modify(data) {
    this.input.setValue(data);
    this.isModifying = true;
    this.isVisibleNewItemModal = true;
  }
}