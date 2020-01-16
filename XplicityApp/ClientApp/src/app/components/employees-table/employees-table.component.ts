import { Component, OnInit } from '@angular/core';

import { User } from '../../models/user';
import { Newuser } from '../../models/newuser';
import { Updateuser } from '../../models/updateuser';
import { UserService } from '../../services/user.service';
import { AuthenticationService } from '../../services/authentication.service';

import { Client } from '../../models/client';
import { ClientService } from '../../services/client.service';

import { NgForm } from '@angular/forms';
import { NzModalRef, NzModalService } from 'ng-zorro-antd';
import { NzNotificationService } from 'ng-zorro-antd';
import { Role } from '../../models/role';
import { EmployeeStatus } from '../../models/employee-status.enum';

@Component({
  selector: 'app-employees-table',
  templateUrl: './employees-table.component.html',
  styleUrls: ['./employees-table.component.scss']
})
export class EmployeesTableComponent implements OnInit {
  users: User[];
  roles: Role[];
  roleName: string;
  formDataUsers: User;
  formDataUsersNoId: Updateuser;
  newUser: Newuser = new Newuser();
  employeeStatus = EmployeeStatus;
  selected: any;
  employeeIdForEquipment: number;

  clients: Client[] = [];
  oneClient: Client;

  isVisibleCreator = false;
  isConfirmLoadingCreator = false;
  isVisibleEditor = false;
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
    private authenticationService: AuthenticationService
  ) {}

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

  onAddButtonClick(newUser: Newuser) {
    newUser.status = this.employeeStatus.Current;
    this.userService.registerUser(newUser).subscribe(() => {
      this.refreshTable();
      this.handleOkCreator();
    }, error => {
      this.showUnexpectedError();
    });
  }

  getCurrentUserId() {
    return this.authenticationService.getUserId();
  }
  onDeleteButtonClick(id: number) {
    this.userService.deleteUser(id).subscribe(() => {
      this.refreshTable();
    });
  }

  onEditButtonClick(user: User) {
    this.userService.getUser(user.id).subscribe();
  }

  onEditConfirmButtonClick(user: Updateuser, id: number) {
    this.userService.editUser(user, id).subscribe(() => {
      this.refreshTable();
      this.handleCancelEditor();
    }, error => {
      this.showUnexpectedError();
    });
  }

  deleteEmployeeOnModalClose(id: number) {
    this.onDeleteButtonClick(id);
    this.handleCancelEditor();
  }

  showDeleteConfirm(id: number): void {
    this.confirmDeleteModal = this.modal.confirm({
      nzTitle: 'Do you want to delete this section?',
      nzContent: 'When clicked the OK button this section will be deleted',
      nzOnOk: () => this.deleteEmployeeOnModalClose(id)
    });
  }
  showStatusConfirm() {
    this.confirmDeleteModal = this.modal.confirm({
      nzTitle: 'Do you want to change status of this user?',
      nzContent: 'When clicked the OK button the status will be changed',
      nzOnOk: () => this.onEditConfirmButtonClick(this.formDataUsersNoId, this.formDataUsers.id)
    });
  }
  onSubmit(form: NgForm) {
    form.resetForm();
  }

  populateUserForm(user: User) {
    this.formDataUsers = Object.assign({}, user);
  }

  populateUserFormNoId(user: Newuser) {
    this.formDataUsersNoId = Object.assign({}, user);
  }

  showModalCreator(): void {
    this.isVisibleCreator = true;
  }

  handleOkCreator(): void {
    this.isConfirmLoadingCreator = true;
    setTimeout(() => {
      this.isVisibleCreator = false;
      this.isConfirmLoadingCreator = false;
    }, 3000);
  }

  handleCancelCreator(): void {
    this.isVisibleCreator = false;
  }

  showModalEditor(): void {
    this.isVisibleEditor = true;
  }

  handleCancelEditor(): void {
    this.isVisibleEditor = false;
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
