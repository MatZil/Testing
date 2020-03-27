import { Component, OnInit, Inject } from '@angular/core';
import { InventoryItem } from 'src/app/models/inventory-item';
import { InventoryService } from 'src/app/services/inventory.service';
import { TableRowUserModel } from 'src/app/models/table-row-user-model';
import { UserService } from 'src/app/services/user.service';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { TableRowInventoryItem } from 'src/app/models/table-row-inventory-item';

@Component({
  selector: 'app-employee-equipment',
  templateUrl: './employee-equipment.component.html',
  styleUrls: ['./employee-equipment.component.scss']
})
export class EmployeeEquipmentComponent implements OnInit {
  assignedEquipment: MatTableDataSource<TableRowInventoryItem>;
  unassignedEquipment: MatTableDataSource<TableRowInventoryItem>;
  
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
    'action'];
  selectedEmployee: TableRowUserModel = new TableRowUserModel;

  constructor(
    private inventoryService: InventoryService,
    public dialog: MatDialog,
    private userService: UserService,
    @Inject(MAT_DIALOG_DATA) public data: number) { }

  ngOnInit() {
    this.refreshTables();
    this.getEmployee(this.data['id']);
  }

  refreshTables() {
      this.inventoryService.getEquipmentByEmployeeId(this.data['id']).subscribe(inventoryItems => {
        this.assignedEquipment = new MatTableDataSource(inventoryItems as TableRowInventoryItem[]);
        this.assignedEquipment.data.forEach(row => {
          row.categoryName = row.category.name;
          row.tagTitles = [];
          row.tags.forEach(tag => {
            row.tagTitles.push(tag.title);
          });
        });
      });
      this.inventoryService.getAllInventoryItems().subscribe(inventoryItems => {
        this.unassignedEquipment = new MatTableDataSource(
          inventoryItems.filter(x => x.assignedTo == "Office") as TableRowInventoryItem[]);
        this.unassignedEquipment.data.forEach(row => {
          row.categoryName = row.category.name;
          row.tagTitles = [];
          row.tags.forEach(tag => {
            row.tagTitles.push(tag.title);
          });
        });
      });
  }

  applyFilter(filterValue: string) {
    this.unassignedEquipment.filter = filterValue.trim().toLowerCase();
  }

  getEmployee(userId: number){
    this.userService.getUser(userId).subscribe(user => {
      this.selectedEmployee = user;
    });
  }

  unassignItem(inventoryItem: InventoryItem){
    inventoryItem.employeeId = null;
    this.inventoryService.updateInventoryItem(inventoryItem.id , inventoryItem).subscribe(() => {
      this.refreshTables();
    });
  }
  assignItem(inventoryItem: InventoryItem){
    inventoryItem.employeeId = this.data['id'];
    this.inventoryService.updateInventoryItem(inventoryItem.id , inventoryItem).subscribe(() => {
      this.refreshTables();
    });
  }

}