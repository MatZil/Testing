import { Component, OnInit } from '@angular/core';
import { TableRowUserModel } from '../../models/table-row-user-model';
import { Newuser } from '../../models/newuser';
import { Updateuser } from '../../models/updateuser';
import { UserService } from '../../services/user.service';
import { AuthenticationService } from '../../services/authentication.service';
import { Client } from '../../models/client';
import { ClientService } from '../../services/client.service';
import { Role } from '../../models/role';
import { EmployeeStatus } from '../../models/employee-status.enum';
import { MatDialog } from '@angular/material/dialog';
import { AddEmployeeFormComponent } from '../add-employee-form/add-employee-form.component';
import { EditEmployeeFormComponent } from '../edit-employee-form/edit-employee-form.component';
import { InventoryTableComponent } from '../inventory-table/inventory-table.component';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-employees-table',
  templateUrl: './employees-table.component.html',
  styleUrls: ['./employees-table.component.scss']
})
export class EmployeesTableComponent implements OnInit {
  users: TableRowUserModel[];
  roles: Role[];

  userToUpdate: Updateuser;

  employeeStatus = EmployeeStatus;

  employeeIdForEquipment: number;

  clients: Client[] = [];

  isVisibleEquipmentModal = false;

  searchValue = '';
  listOfSearchAddress: string[] = [];
  sortName: string | null = null;
  sortValue: string | null = null;
  listOfData: TableRowUserModel[] = [];

  displayedColumns: string[] = ['name', 'surname', 'client', 'worksFromDate',
    'birthdayDate', 'daysOfVacation', 'freeWorkDays', 'overtimeHours',
    'email', 'position', 'parentalLeaveLimit', 'currentAvailableLeaves',
    'nextMonthAvailableLeaves', 'healthCheckDate', 'employeeStatus',
    'actions'];
  employeeDataSource = new MatTableDataSource(this.listOfData);

  constructor(
    private userService: UserService,
    private clientService: ClientService,
    private authenticationService: AuthenticationService,
    public dialog: MatDialog
  ) { }

  ngOnInit() {
    this.refreshTable();
    this.getAllRoles();
    this.clientService.getClient().subscribe(clients => {
      this.clients = clients;
    });
  }

  refreshTable() {
    this.userService.getAllUsers().subscribe(users => {
      this.users = users;
      this.listOfData = [...this.users];
      this.employeeDataSource = new MatTableDataSource(this.listOfData);
    });
  }

  getAllRoles() {
    this.authenticationService.getRoles().subscribe(roles => {
      this.roles = roles;
    });
  }

  registerUser(newUser: Newuser) {
    this.userService.registerUser(newUser).subscribe(() => {
      this.refreshTable();
    }, error => {
      this.showUnexpectedError();
    });
  }

  editUser(user: Updateuser, id: number) {
    this.userService.editUser(user, id).subscribe(() => {
      this.refreshTable();
    }, error => {
      this.showUnexpectedError();
    });
  }

  closeModal() {
    this.dialog.closeAll();
  }

  openAddForm(): void {
    const dialogRef = this.dialog.open(AddEmployeeFormComponent, {
      width: '550px',
      data: {
        roles: this.roles,
        clients: this.clients
      }
    });

    dialogRef.afterClosed().subscribe(newUser => {
      if (newUser) {
        this.registerUser(newUser);
      }
    });
  }

  openEditForm(user: TableRowUserModel): void {
    this.userToUpdate = Object.assign(user);
    const dialogRef = this.dialog.open(EditEmployeeFormComponent, {
      width: '550px',
      data: {
        userToUpdate: this.userToUpdate,
        roles: this.roles,
        clients: this.clients,
        isEditingSelf: user.id === this.getCurrentUserId()
      }
    });

    dialogRef.afterClosed().subscribe(userToUpdate => {
      if (userToUpdate) {
        this.editUser(userToUpdate, user.id);
        this.refreshTable();
      }
    });
  }

  getCurrentUserId() {
    return this.authenticationService.getUserId();
  }

  getClientName(id: number) {
    for (const client of this.clients) {
      if (id != null && client.id === id) {
        return client.companyName;
      }
    }

    return 'No client';
  }

  showUnexpectedError(): void {
    error => {
      this.showUnexpectedError();
    }
  }

  showEquipmentModal(employeeId: number) {
    const dialogRef = this.dialog.open(InventoryTableComponent, {
      width: '1000px',
      data: { id: employeeId }
    });
  }

  closeEquipmentModal() {
    this.isVisibleEquipmentModal = false;
  }

  formatDate(date: Date) {
    const d = new Date(date);
    let month = '' + (d.getMonth() + 1);
    let day = '' + d.getDate();
    const year = d.getFullYear();

    if (month.length < 2) {
      month = '0' + month;
    }
    if (day.length < 2) {
      day = '0' + day;
    }

    return [year, month, day].join('-');
  }

  applyFilter(filterValue: string) {
    this.employeeDataSource.filter = filterValue.trim().toLowerCase();
  }
}
