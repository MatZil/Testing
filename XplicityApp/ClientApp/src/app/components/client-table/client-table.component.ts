import { Component, OnInit } from '@angular/core';

import { Client } from '../../models/client';
import { Newclient } from '../../models/newclient';
import { ClientService } from '../../services/client.service';
import { NgForm } from '@angular/forms';
import { NzModalRef, NzModalService } from 'ng-zorro-antd';
import { NzNotificationService } from 'ng-zorro-antd';

@Component({
  selector: 'app-client-table',
  templateUrl: './client-table.component.html',
  styleUrls: ['./client-table.component.scss']
})
export class ClientTableComponent implements OnInit {
  client: Client[] = [];
  formData: Client;
  formDataNoId: Newclient;
  newClient: Newclient = new Newclient();

  isVisibleCreator = false;
  isConfirmLoadingCreator = false;
  isVisibleEditor = false;

  confirmDeleteModal: NzModalRef;

  searchValue = '';
  listOfSearchAddress: string[] = [];
  sortName: string | null = null;
  sortValue: string | null = null;
  listOfData: Client[] = [];

  constructor(
    private clientService: ClientService,
    private modal: NzModalService,
    private notification: NzNotificationService
  ) { }

  ngOnInit() {
    this.refreshTable();
  }

  refreshTable() {
    this.clientService.getClient().subscribe(clients => {
      this.client = clients;
      this.listOfData = [...this.client];
    });
  }

  onAddButtonClick(client: Newclient) {
    this.clientService.addClient(client).subscribe(() => {
      this.refreshTable();
      this.handleOkCreator();
    }, error => {
      this.createBasicNotification();
    });
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

  onSubmit(form: NgForm) {
    form.resetForm();
  }

  updateField() {
    this.clientService.getClient().subscribe(client => {
      this.client = client;
    });
  }

  onDeleteButtonClick(id: number) {
    this.clientService.deleteClient(id).subscribe(() => {
      this.refreshTable();
    });
  }

  onEditButtonClick(clien: Client) {
    this.clientService.getClientById(clien.id).subscribe();
  }

  onEditConfirmButtonClick(client: Newclient, id: number) {
    this.clientService.editClient(client, id).subscribe(() => {
      this.refreshTable();
      this.handleCancelEditor();
    }, error => {
      this.createBasicNotification();
    });
  }

  populateForm(clien: Client) {
    this.formData = Object.assign({}, clien);
  }

  populateFormNoId(client: Newclient) {
    this.formDataNoId = Object.assign({}, client);
  }

  deleteClientOnModalClose(id: number) {
    this.onDeleteButtonClick(id);
    this.handleCancelEditor();
  }

  showDeleteConfirm(id: number): void {
    this.confirmDeleteModal = this.modal.confirm({
      nzTitle: 'Do you want to delete this section?',
      nzContent: 'When clicked the OK button this section will be deleted',
      nzOnOk: () => this.deleteClientOnModalClose(id)
    });
  }

  createBasicNotification(): void {
    this.notification.blank(
      'Form error',
      'A client with this company name already exists'
    );
  }

  reset(): void {
    this.searchValue = '';
    this.search();
  }

  search(): void {
    const filterFunc = (item: {
      companyName: string; ownerName: string; ownerSurname: string;
      ownerEmail: string; ownerPhone: string;
    }) => {
      return (
        (this.listOfSearchAddress.length
          ? this.listOfSearchAddress.some(ownerName => item.ownerName.indexOf(ownerName) !== -1)
          : true) && item.companyName.indexOf(this.searchValue) !== -1
      );
    };
    const data = this.listOfData.filter((item: {
      companyName: string; ownerName: string; ownerSurname: string;
      ownerEmail: string; ownerPhone: string;
    }) => filterFunc(item));
    this.client = data.sort((a, b) =>
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
