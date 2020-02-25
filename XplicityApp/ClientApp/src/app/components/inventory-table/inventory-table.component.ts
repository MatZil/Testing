import { Component, OnInit, Input} from '@angular/core';
import { InventoryItem } from 'src/app/models/inventory-item';
import { InventoryService } from 'src/app/services/inventory.service';
import { InventoryCategory } from 'src/app/models/inventory-category';
import { InventoryCategoryService } from 'src/app/services/inventory-category.service';
import { TableRowUserModel } from 'src/app/models/table-row-user-model';
import { UserService } from 'src/app/services/user.service';
import { MatDialog } from '@angular/material';
import { AddInventoryFormComponent } from '../inventory-add-form/inventory-add-form.component';
import { EditInventoryFormComponent } from '../inventory-edit-form/inventory-edit-form.component';
import { NewInventoryItem } from '../../models/new-inventory-item';

@Component({
  selector: 'app-inventory-table',
  templateUrl: './inventory-table.component.html',
  styleUrls: ['./inventory-table.component.scss']
})
export class InventoryTableComponent implements OnInit {
  equipment: InventoryItem[] = [];
  @Input() employeeId: number;

  inventoryItemToUpdate: InventoryItem

  categories: InventoryCategory[] = [];
  employees: TableRowUserModel[] = [];

  searchCategoryValue = '';
  searchOwnerValue = '';
  searchDateValue: Date;
  sortName: string | null = null;
  sortValue: string | null = null;
  listOfData: InventoryItem[] = [];

  constructor(
    private inventoryService: InventoryService,
    private categoryService: InventoryCategoryService,
    public dialog: MatDialog,
    private userService: UserService
  ) {
  }

  ngOnInit() {
    this.getAllUsers();
    this.getCategoriesList();
    this.refreshTable();
  }

  refreshTable() {
    this.inventoryService.getAllInventoryItems().subscribe(inventoryItems => {
      this.equipment = inventoryItems;
      this.listOfData = [...this.equipment];
    });
  }

  openAddForm(): void {
    const dialogRef = this.dialog.open(AddInventoryFormComponent, {
      width: '550px',
      data: {
        employees: this.employees,
        categories: this.categories
      }
    });

    dialogRef.afterClosed().subscribe(newInventoryItem => {
      if (newInventoryItem) {
        this.addInventoryItem(newInventoryItem);
      }
    });
  }

  openEditForm(inventoryItem: InventoryItem): void {
    this.inventoryItemToUpdate = Object.assign(inventoryItem);
    const dialogRef = this.dialog.open(EditInventoryFormComponent, {
      width: '550px',
      data: {
        inventoryItemToUpdate: this.inventoryItemToUpdate,
        employees: this.employees,
        categories: this.categories,
        tags: this.inventoryItemToUpdate.tags
      }
    });
    dialogRef.afterClosed().subscribe(inventoryItemToUpdate => {
      if (inventoryItemToUpdate) {
        this.saveInventoryItem(inventoryItemToUpdate, inventoryItem.id);
      }
    });
  }

  saveInventoryItem(updateInventoryItem: InventoryItem, id: number) {
    this.inventoryService.updateInventoryItem(id, updateInventoryItem).subscribe(() => {
      this.refreshTable();
    });
  }

  addInventoryItem(newInventoryItem: NewInventoryItem) {
    this.inventoryService.createNewInventoryItem(newInventoryItem).subscribe(() => {
      this.refreshTable();

    });
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
      if (!this.searchDateValue) {
        return (
          item.category.name.toUpperCase().indexOf(this.searchCategoryValue.toUpperCase()) !== -1 &&
          item.assignedTo.toUpperCase().indexOf(this.searchOwnerValue.toUpperCase()) !== -1
        );
      }
      else {
        if (!item.expiryDate)
          return false;
        else {
          var targetDate = Date.parse(this.searchDateValue.toDateString());
          var tempDate = new Date(item.expiryDate);
          var itemDate = Date.parse(tempDate.toDateString());
          return (
            item.category.name.toUpperCase().indexOf(this.searchCategoryValue.toUpperCase()) !== -1 &&
            item.assignedTo.toUpperCase().indexOf(this.searchOwnerValue.toUpperCase()) !== -1 &&
            targetDate == itemDate
          );
        }
      }
    }
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

