import { Component, OnInit, Output, EventEmitter } from '@angular/core';

import { Client } from '../../models/client';
import { ClientService } from '../../services/client.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-client-table',
  templateUrl: './client-table.component.html',
  styleUrls: ['./client-table.component.scss']
})
export class ClientTableComponent implements OnInit {
  @Output()
  addButtonClick = new EventEmitter<Client>();

  client: Client[];
  errorMessage: string;
  newClient: Client = new Client();
  isVisibleCreator = false;
  isConfirmLoadingCreator = false;
  isVisibleEditor = false;
  isConfirmLoadingEditor = false;

  constructor(
    private clientService: ClientService
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

  onDeleteButtonClick(clien: Client) {
    this.clientService.deleteClient(clien.clientId)
    .subscribe();
  }

  onEditButtonClick(clien: Client) {
    this.clientService.getClientById(clien.clientId).subscribe();
  }
}
