import { Component, OnInit } from '@angular/core';

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
  formData: Client;
  formDataNoId: Newclient;
  newClient: Newclient = new Newclient();

  isVisibleCreator = false;
  isConfirmLoadingCreator = false;
  isVisibleEditor = false;

  confirmDeleteModal: NzModalRef;

  constructor(
    private clientService: ClientService,
    private modal: NzModalService
  ) { }

  ngOnInit() {
    this.refreshTable();
  }

  refreshTable() {
    this.clientService.getClient().subscribe(clients => {
      this.client = clients; });
  }

  onAddButtonClick(clien: Client) {
    this.clientService.addClient(clien).subscribe(() => {
      this.refreshTable(); });
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
      this.refreshTable(); });
  }

  onEditButtonClick(clien: Client) {
    this.clientService.getClientById(clien.id).subscribe();
  }

  onEditConfirmButtonClick(client: Newclient, id: number) {
    this.clientService.editClient(client, id).subscribe(() => {
      this.refreshTable(); });
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
}
