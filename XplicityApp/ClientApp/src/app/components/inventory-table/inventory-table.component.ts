import { Component, OnInit, Input } from '@angular/core';
import { InventoryItem } from 'src/app/models/inventory-item';
import { InventoryService } from 'src/app/services/inventory.service';
import { InventoryCategory } from 'src/app/models/inventory-category';
import { InventoryCategoryService } from 'src/app/services/inventory-category.service';
import { TableRowUserModel } from 'src/app/models/table-row-user-model';
import { UserService } from 'src/app/services/user.service';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { AddInventoryFormComponent } from '../inventory-add-form/inventory-add-form.component';
import { EditInventoryFormComponent } from '../inventory-edit-form/inventory-edit-form.component';
import { NewInventoryItem } from '../../models/new-inventory-item';
import { TableRowInventoryItem } from 'src/app/models/table-row-inventory-item';

@Component({
  selector: 'app-inventory-table',
  templateUrl: './inventory-table.component.html',
  styleUrls: ['./inventory-table.component.scss']
})
export class InventoryTableComponent implements OnInit {
  equipmentDataSource: MatTableDataSource<TableRowInventoryItem>;
  @Input() employeeId: number;

  showArchivedInventory = false;
  displayedColumns = [
    'category',
    'name',
    'serial number',
    'purchase date',
    'originalPrice',
    'currentPrice',
    'assigned to',
    'expiry date',
    'comment',
    'tags',
    'edit'];
  inventoryItemToUpdate: InventoryItem;

  categories: InventoryCategory[] = [];
  employees: TableRowUserModel[] = [];
  currentUser: TableRowUserModel;

  constructor(
    private inventoryService: InventoryService,
    private categoryService: InventoryCategoryService,
    public dialog: MatDialog,
    private userService: UserService) { }

  ngOnInit() {
    this.getCategoriesList();
    this.getAllUsers();
    this.refreshTable(this.showArchivedInventory);
  }

  refreshTable(showArchivedInventory: boolean) {
    this.inventoryService.getInventoryByStatus(showArchivedInventory).subscribe(inventoryItems => {
      this.equipmentDataSource = new MatTableDataSource(inventoryItems as TableRowInventoryItem[]);
      this.equipmentDataSource.data.forEach(row => {
        row.categoryName = row.category.name;
        row.tagTitles = [];
        row.tags.forEach(tag => {
          row.tagTitles.push(tag.title);
        });
      });
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
      this.refreshTable(this.showArchivedInventory);
    });
  }

  addInventoryItem(newInventoryItem: NewInventoryItem) {
    this.inventoryService.createNewInventoryItem(newInventoryItem).subscribe(() => {
      this.refreshTable(this.showArchivedInventory);

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

  isAdmin(): boolean {
    return this.userService.isAdmin();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.equipmentDataSource.filter = filterValue.trim().toLowerCase();
  }
}

