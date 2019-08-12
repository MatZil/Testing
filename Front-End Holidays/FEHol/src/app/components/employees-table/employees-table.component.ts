import { Component, OnInit } from '@angular/core';

import { User } from '../../models/user';
import { Newuser } from '../../models/newuser';
import { Updateuser } from '../../models/updateuser';
import { UserService } from '../../services/user.service';

import { Client } from '../../models/client';
import { ClientService } from '../../services/client.service';

import { NgForm } from '@angular/forms';
import { NzModalRef, NzModalService } from 'ng-zorro-antd';
import { DateAdapter } from '@angular/material/core';

@Component({
  selector: 'app-employees-table',
  templateUrl: './employees-table.component.html',
  styleUrls: ['./employees-table.component.scss']
})
export class EmployeesTableComponent implements OnInit {
  users: User[];
  formDataUsers: User = new User();
  formDataUsersNoId: Updateuser;
  newUser: Newuser = new Newuser();

  clients: Client[];
  oneClient: Client;

  isVisibleCreator = false;
  isConfirmLoadingCreator = false;
  isVisibleEditor = false;

  confirmDeleteModal: NzModalRef;

  constructor(
    private userService: UserService,
    private clientService: ClientService,
    private modal: NzModalService
  ) { }

  ngOnInit() {
    this.refreshTable();
    this.getClientsDetails();
  }

  refreshTable() {
    this.userService.getAllUsers().subscribe(users => {
      this.users = users; });
  }

  onAddButtonClick(user: User) {
    this.userService.registerUser(user).subscribe(() => {
      this.refreshTable(); });
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
      this.refreshTable(); });
  }

  deleteEmployeeOnModalClose(id: number) {
    this.onDeleteButtonClick(id);
    this.handleCancelEditor();
  }

  showDeleteConfirm(id: number): void {
    this.confirmDeleteModal = this.modal.confirm({
      nzTitle: 'Do you want to delete this client?',
      nzContent: 'When clicked the OK button this client will be deleted',
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

  getClientsDetails() {
    this.clientService.getClient().subscribe(clients => {
      this.clients = clients;
    });
  }

  getClientByTheId(id: number) {
    this.clientService.getClientById(id).subscribe(client => {
      this.oneClient = client;
    });
  }

  getClientName(id: number) {
    this.getClientByTheId(id);
    return this.oneClient.companyName;
  }
}
