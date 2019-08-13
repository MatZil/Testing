import { Component, OnInit } from '@angular/core';

import { User } from '../../models/user';
import { Newuser } from '../../models/newuser';
import { Updateuser } from '../../models/updateuser';
import { UserService } from '../../services/user.service';

import { Client } from '../../models/client';
import { ClientService } from '../../services/client.service';

import { NgForm } from '@angular/forms';
import { NzModalRef, NzModalService } from 'ng-zorro-antd';
import { NzNotificationService } from 'ng-zorro-antd';

@Component({
  selector: 'app-employees-table',
  templateUrl: './employees-table.component.html',
  styleUrls: ['./employees-table.component.scss']
})
export class EmployeesTableComponent implements OnInit {
  users: User[];
  formDataUsers: User;
  formDataUsersNoId: Updateuser;
  newUser: Newuser = new Newuser();

  clients: Client[] = [];
  oneClient: Client;

  isVisibleCreator = false;
  isConfirmLoadingCreator = false;
  isVisibleEditor = false;

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
    private notification: NzNotificationService
  ) { }

  ngOnInit() {
    this.refreshTable();

    this.clientService.getClient().subscribe(clients => {
      this.clients = clients;
    });
  }

  refreshTable() {
    this.userService.getAllUsers().subscribe(users => {
      this.users = users;
      this.listOfData = [...this.users]; });
  }

  onAddButtonClick(user: User) {
    this.userService.registerUser(user).subscribe(() => {
      this.refreshTable();
      this.handleOkCreator(); }, error => {
        this.createBasicNotification();
      });
  }

  onDeleteButtonClick(id: number) {
    this.userService.deleteUser(id).subscribe(() => {
      this.refreshTable(); });
  }

  onEditButtonClick(user: User) {
    this.userService.getUser(user.id).subscribe();
  }

  onEditConfirmButtonClick(user: Updateuser, id: number) {
    this.userService.editUser(user, id).subscribe(() => {
      this.refreshTable();
      this.handleCancelEditor(); }, error => {
        this.createBasicNotification();
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

  createBasicNotification(): void {
    this.notification.blank(
      'Form error',
      'An employee with this email already exists'
    );
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
      email: string;
      role: string;
      position: string; }) => {
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
      email: string;
      role: string;
      position: string; }) => filterFunc(item));
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
