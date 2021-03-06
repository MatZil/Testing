import { Component, OnInit, ViewChild } from '@angular/core';
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
import { EmployeeEquipmentComponent } from '../employee-equipment/employee-equipment.component';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';

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
  selectedEmployeeStatus: EmployeeStatus = EmployeeStatus.Current;
  clients: Client[] = [];
  isVisibleEquipmentModal = false;
  listOfData: TableRowUserModel[] = [];

  displayedColumns: string[] = [
    'name',
    'surname',
    'client',
    'worksFromDate',
    'daysOfVacation',
    'freeWorkDays',
    'overtimeHours',
    'email',
    'position',
    'healthCheckDate',
    'actions'
  ];
  employeeDataSource = new MatTableDataSource(this.listOfData);

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(
    private userService: UserService,
    private clientService: ClientService,
    private authenticationService: AuthenticationService,
    public dialog: MatDialog
  ) { }

  ngOnInit() {
    this.refreshTable(this.selectedEmployeeStatus);
    this.getAllRoles();
    this.clientService.getClient().subscribe(clients => {
      this.clients = clients;
    });
  }

  refreshTable(status: EmployeeStatus) {
    this.userService.getUsersByStatus(status).subscribe(users => {
      this.users = users;
      this.listOfData = [...this.users];
      this.employeeDataSource = new MatTableDataSource(this.listOfData);
      this.employeeDataSource.paginator = this.paginator;
      this.employeeDataSource.filterPredicate = this.filterTable;
    });
  }

  getAllRoles() {
    this.authenticationService.getRoles().subscribe(roles => {
      this.roles = roles;
    });
  }

  registerUser(newUser: Newuser) {
    this.userService.registerUser(newUser).subscribe(() => {
      this.refreshTable(this.selectedEmployeeStatus);
    }, error => {
      this.showUnexpectedError();
    });
  }

  editUser(user: Updateuser, id: number) {
    this.userService.editUser(user, id).subscribe(() => {
      this.refreshTable(this.selectedEmployeeStatus);
    }, error => {
      this.showUnexpectedError();
    });
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
        isEditingSelf: user.id === this.getCurrentUserId(),
        employeeId: user.id
      }
    });

    dialogRef.afterClosed().subscribe(userToUpdate => {
      if (userToUpdate) {
        this.editUser(userToUpdate, user.id);
        this.refreshTable(this.selectedEmployeeStatus);
      }
    });
  }

  getCurrentUserId() {
    return this.authenticationService.getUserId();
  }

  getClientName(id: number): string {
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
    this.dialog.open(EmployeeEquipmentComponent, {
      width: '1300px',
      data: { id: employeeId }
    });
  }

  closeEquipmentModal() {
    this.isVisibleEquipmentModal = false;
  }

  applyFilter(filterValue: string) {
    this.employeeDataSource.filter = filterValue.trim().toLowerCase();
  }

  private filterTable(user: TableRowUserModel, filterText: string): boolean {
    return (user.name && user.name.toLowerCase().indexOf(filterText) >= 0) ||
      (user.surname && user.surname.toLowerCase().indexOf(filterText) >= 0) ||
      (user.email && user.email.toLowerCase().indexOf(filterText) >= 0) ||
      (user.position && user.position.toLowerCase().indexOf(filterText) >= 0);
  }
}
