import { Component, OnInit, Output, EventEmitter } from '@angular/core';

import { Client } from '../../models/client';
import { Newclient } from '../../models/newclient';
import { ClientService } from '../../services/client.service';
import { NgForm } from '@angular/forms';
import { NzModalRef, NzModalService } from 'ng-zorro-antd';

@Component({
  selector: 'app-client-table',
  templateUrl: './client-table.component.html',
  styleUrls: ['./client-table.component.scss']
})
export class ClientTableComponent implements OnInit {
  client: Client[];
  formData: Client = new Client();
  formDataNoId: Newclient;
  errorMessage: string;
  newClient: Newclient = new Newclient();
  isVisibleCreator = false;
  isConfirmLoadingCreator = false;
  isVisibleEditor = false;
  isConfirmLoadingEditor = false;

  confirmDeleteModal: NzModalRef;

  constructor(
    private clientService: ClientService,
    private modal: NzModalService
  ) { }

  ngOnInit() {
    this.clientService.getClient().subscribe(client => {
      this.client = client;
    }, error => {
      console.error(error);
      this.errorMessage = error.message;
    });
  }

  onAddButtonClick(clien: Client) {
    this.clientService.addClient(clien)
    .subscribe(savedClient => {
      console.log(savedClient);
      this.client.push(savedClient);
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

  handleOkEditor(): void {
    this.isConfirmLoadingEditor = true;
    setTimeout(() => {
      this.isVisibleEditor = false;
      this.isConfirmLoadingEditor = false;
    }, 3000);
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
    this.clientService.deleteClient(id).subscribe();
  }

  onEditButtonClick(clien: Client) {
    this.clientService.getClientById(clien.id).subscribe();
  }

  onEditConfirmButtonClick(client: Newclient, id: number) {
    this.clientService.editClient(client, id).subscribe();
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
      nzTitle: 'Do you want to delete this client?',
      nzContent: 'When clicked the OK button this client will be deleted',
      nzOnOk: () => this.deleteClientOnModalClose(id)
    });
  }
}
