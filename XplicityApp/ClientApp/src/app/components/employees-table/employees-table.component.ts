import { Component, OnInit } from '@angular/core';

import { User } from '../../models/user';
import { Newuser } from '../../models/newuser';
import { Updateuser } from '../../models/updateuser';
import { UserService } from '../../services/user.service';
import { AuthenticationService } from '../../services/authentication.service';

import { Client } from '../../models/client';
import { ClientService } from '../../services/client.service';

import { NzModalRef, NzModalService } from 'ng-zorro-antd';
import { NzNotificationService } from 'ng-zorro-antd';
import { Role } from '../../models/role';
import { EmployeeStatus } from '../../models/employee-status.enum';
import { MatDialog } from '@angular/material';
import { AddEmployeeFormComponent } from '../add-employee-form/add-employee-form.component';
import { EditEmployeeFormComponent } from '../edit-employee-form/edit-employee-form.component';

export interface AddModalData {
  newUser: Newuser;
  roles: Role[];
  clients: Client[];
}

export interface EditModalData {
  userToUpdate: Updateuser;
  roles: Role[];
  clients: Client[];
}

@Component({
  selector: 'app-employees-table',
  templateUrl: './employees-table.component.html',
  styleUrls: ['./employees-table.component.scss']
})
export class EmployeesTableComponent implements OnInit {
  users: User[];
  roles: Role[];

  userToUpdate: Updateuser;
  newUser: Newuser = new Newuser();

  employeeStatus = EmployeeStatus;

  employeeIdForEquipment: number;

  clients: Client[] = [];

  isVisibleEquipmentModal = false;

  confirmDeleteModal: NzModalRef;

  searchValue = '';
  listOfSearchAddress: string[] = [];
  sortName: string | null = null;
  sortValue: string | null = null;
  listOfData: User[] = [];

  constructor(
    private userService: UserService,
    private clientService: ClientService,
    private modal: NzModalService,
    private notification: NzNotificationService,
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
    });
  }

  getAllRoles() {
    this.authenticationService.getRoles().subscribe(roles => {
      this.roles = roles;
    });
  }

  registerUser(newUser: Newuser) {
    if (newUser.clientId === 0) {
      newUser.clientId = null;
    }
    newUser.status = EmployeeStatus.Current;
    this.userService.registerUser(newUser).subscribe(() => {
      this.refreshTable();
    }, error => {
      this.showUnexpectedError();
    });
  }

  deleteUserById(id: number) {
    this.userService.deleteUser(id).subscribe(() => {
      this.refreshTable();
    });
  }

  editUser(user: Updateuser, id: number) {
    if (user.clientId === 0) {
      user.clientId = null;
    }
    this.userService.editUser(user, id).subscribe(() => {
      this.refreshTable();
    }, error => {
      this.showUnexpectedError();
    });
  }

  showDeleteConfirm(userToDelete: User): void {
    this.confirmDeleteModal = this.modal.confirm({
      nzTitle: 'Are you sure?',
      nzContent: `If you confirm, ${userToDelete.name} ${userToDelete.surname} will be permanently deleted.`,
      nzOnOk: () => this.deleteUserById(userToDelete.id)
    });
  }

  openAddForm(): void {
    const dialogRef = this.dialog.open(AddEmployeeFormComponent, {
      width: '550px',
      data: {
        newUser: this.newUser,
        roles: this.roles,
        clients: this.clients
      }
    });

    dialogRef.afterClosed().subscribe(newUser => {
      if (newUser) {
        this.registerUser(newUser);
        this.newUser = new Newuser();
      }
    });
  }

  openEditForm(user: User): void {
    this.userToUpdate = Object.assign({}, user);
    const dialogRef = this.dialog.open(EditEmployeeFormComponent, {
      width: '550px',
      data: {
        userToUpdate: this.userToUpdate,
        roles: this.roles,
        clients: this.clients
      }
    });

    dialogRef.afterClosed().subscribe(userToUpdate => {
      if (userToUpdate) {
        this.editUser(userToUpdate, user.id);
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
    this.notification.blank(
      'Form error',
      'Unexpected error occurred'
    );
  }

  showEquipmentModal(employeeId: number) {
    this.employeeIdForEquipment = employeeId;
    this.isVisibleEquipmentModal = true;

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

  reset(): void {
    this.searchValue = '';
    this.search();
  }

  search(): void {
    const filterFunc = (item: {
      name: string;
      surname: string;
      clientId: number;
      worksFromDate: Date;
      birthdayDate: Date;
      daysOfVacation: number;
      overtimeHours: number;
      email: string;
      role: string;
      position: string;
    }) => {
      return (
        (this.listOfSearchAddress.length
          ? this.listOfSearchAddress.some(name => item.name.indexOf(name) !== -1)
          : true) && item.surname.indexOf(this.searchValue) !== -1
      );
    };
    const data = this.listOfData.filter((item: {
      name: string;
      surname: string;
      clientId: number;
      worksFromDate: Date;
      birthdayDate: Date;
      daysOfVacation: number;
      overtimeHours: number;
      email: string;
      role: string;
      position: string;
    }) => filterFunc(item));
    this.users = data.sort((a, b) =>
      this.sortValue === 'ascend'
        // tslint:disable-next-line:no-non-null-assertion
        ? a[this.sortName!] > b[this.sortName!]
          ? 1
          : -1
        // tslint:disable-next-line:no-non-null-assertion
        : b[this.sortName!] > a[this.sortName!]
          ? 1
          : -1
    );
  }
}
