import { Component, OnInit, Input } from '@angular/core';
import { InventoryItem } from 'src/app/models/inventory-item';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { InventoryService } from 'src/app/services/inventory.service';
import { InventoryCategory } from 'src/app/models/inventory-category';
import { InventoryCategoryService } from 'src/app/services/inventory-category.service';
import { TableRowUserModel } from 'src/app/models/table-row-user-model';
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
  employees: TableRowUserModel[] = [];
  selectedEmployee;
  isModifying = false;

  searchCategoryValue = '';
  searchOwnerValue = '';
  searchDateValue : Date;
  sortName: string | null = null;
  sortValue: string | null = null;
  listOfData: InventoryItem[] = [];

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
      expiryDate: [null, [Validators.required]],
      category: [null],
      assignedTo: [null],
      comment: [null],
      archived: [false],
      inventoryCategoryId: [null, [Validators.required]],
      employeeId: [null]
    });
    this.getAllUsers();
    this.getCategoriesList();
    this.refreshTable(this.employeeId);
    this.onCategoryChange();
  }
  onCategoryChange() {
    this.input.get('inventoryCategoryId').valueChanges.subscribe(selectedCategoryId => {
      const licenseCategoryId = 4;
      if (selectedCategoryId !== licenseCategoryId) {
        this.input.get('expiryDate').reset();
        this.input.get('expiryDate').disable();
      } else {
        this.input.get('expiryDate').enable();
      }
    });
  }
  refreshTable(id: number) {
    if (id) {
      this.inventoryService.getEquipmentByEmployeeId(id).subscribe(inventoryItems => {
        this.equipment = inventoryItems;
        this.listOfData = [...this.equipment];
      });
    } else {
      this.inventoryService.getAllInventoryItems().subscribe(inventoryItems => {
        this.equipment = inventoryItems;
        this.listOfData = [...this.equipment];
      });
    }
  }
  showNewItemModal() {
    this.isVisibleNewItemModal = true;
    this.selectedEmployee = this.employeeId;
  }

  closeNewItemModal() {
    this.isVisibleNewItemModal = false;
    this.isModifying = false;
    this.input.reset();
  }

  saveInventoryItem() {
    if (!this.isModifying) {
      this.inventoryService.createNewInventoryItem(this.input.value).subscribe(() => {
        this.refreshTable(this.employeeId);

      });
    } else {
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
    this.selectedEmployee = this.employeeId;
    this.isModifying = true;
    this.isVisibleNewItemModal = true;
  }
  archiveItem() {
    this.input.controls.archived.setValue(true);
    this.saveInventoryItem();
  }

  resetCategory(): void {
    this.searchCategoryValue = '';
    this.search();
  }
  resetOwner(): void {
    this.searchOwnerValue = '';
    this.search();
  }
  search(): void {
    const filterFunc = (item: InventoryItem) => {
      if(!this.searchDateValue){
        return (
          item.category.name.toUpperCase().indexOf(this.searchCategoryValue.toUpperCase()) !== -1 &&
          item.assignedTo.toUpperCase().indexOf(this.searchOwnerValue.toUpperCase()) !== -1
        );
      }
      else {
        if(!item.expiryDate)
          return false;
        else {
          var targetDate = this.searchDateValue.getFullYear() + 
            '/' + ((this.searchDateValue.getMonth().toString().length > 1) ? (this.searchDateValue.getMonth() + 1) : ('0' + (this.searchDateValue.getMonth() + 1))) + 
            '/' + ((this.searchDateValue.getDate().toString().length > 1) ? this.searchDateValue.getDate() : ('0' + this.searchDateValue.getDate()));//result: YYYY/MM/DD
          var tempDate = new Date(item.expiryDate);
          var itemDate = tempDate.getFullYear() + 
          '/' + ((tempDate.getMonth().toString().length > 1) ? (tempDate.getMonth() + 1) : ('0' + (tempDate.getMonth() + 1))) + 
          '/' + ((tempDate.getDate().toString().length > 1) ? tempDate.getDate() : ('0' + tempDate.getDate()));//result: YYYY/MM/DD
          return (
            item.category.name.toUpperCase().indexOf(this.searchCategoryValue.toUpperCase()) !== -1 &&
            item.assignedTo.toUpperCase().indexOf(this.searchOwnerValue.toUpperCase()) !== -1 &&
            targetDate.indexOf(itemDate) !== -1
          );
        }
      }

    };
    const data = this.listOfData.filter((item: InventoryItem) => filterFunc(item));
    this.equipment = data.sort((a, b) =>
      this.sortValue === 'ascend'
        ? a[this.sortName!] > b[this.sortName!]
          ? 1
          : -1
        : b[this.sortName!] > a[this.sortName!]
          ? 1
          : -1
    );
  }

}
