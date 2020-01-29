import { Component, OnInit } from '@angular/core';

import { Client } from '../../models/client';
import { Newclient } from '../../models/newclient';
import { ClientService } from '../../services/client.service';
import { NzModalRef, NzModalService } from 'ng-zorro-antd';
import { NzNotificationService } from 'ng-zorro-antd';
import { MatDialog } from '@angular/material';
import { EditClientFormComponent } from '../edit-client-form/edit-client-form.component';
import { AddClientFormComponent } from '../add-client-form/add-client-form.component';

export interface AddModalClient {
  newClient: Newclient;
}

export interface EditModalClient {
  clientToEdit : Newclient
}

@Component({
  selector: "app-client-table",
  templateUrl: "./client-table.component.html",
  styleUrls: ["./client-table.component.scss"]
})

export class ClientTableComponent implements OnInit {
  clients: Client[] = [];
  formDataNoId: Newclient;
  newClient: Newclient = new Newclient();

  confirmDeleteModal: NzModalRef;

  searchValue = '';
  listOfSearchAddress: string[] = [];
  sortName: string | null = null;
  sortValue: string | null = null;
  listOfData: Client[] = [];

  constructor(
    private clientService: ClientService,
    private modal: NzModalService,
    private notification: NzNotificationService,
    public dialog: MatDialog
  ) { }

  ngOnInit() {
    this.refreshTable();
  }

  refreshTable(): void {
    this.clientService.getClient().subscribe(clients => {
      this.clients = clients;
      this.listOfData = [...this.clients];
    });
  }

  updateField() {
    this.clientService.getClient().subscribe(client => {
      this.clients = client;
    });
  }

  onDeleteButtonClick(id: number) {
    this.clientService.deleteClient(id).subscribe(() => {
      this.refreshTable();
    });
  }

  deleteClientOnModalClose(id: number) {
    this.onDeleteButtonClick(id);
  }

  showDeleteConfirm(id: number): void {
    this.confirmDeleteModal = this.modal.confirm({
      nzTitle: "Do you want to delete this section?",
      nzContent: "When clicked the OK button this section will be deleted",
      nzOnOk: () => this.deleteClientOnModalClose(id)
    });
  }

  createBasicNotification(): void {
    this.notification.blank(
      "Form error",
      "A client with this company name already exists"
    );
  }

  reset(): void {
    this.searchValue = '';
    this.search();
  }

  search(): void {
    const filterFunc = (item: {
      companyName: string;
      ownerName: string;
      ownerSurname: string;
      ownerEmail: string;
      ownerPhone: string;
    }) => {
      return (
        (this.listOfSearchAddress.length
          ? this.listOfSearchAddress.some(
            ownerName => item.ownerName.indexOf(ownerName) !== -1
          )
          : true) && item.companyName.indexOf(this.searchValue) !== -1
      );
    };
    const data = this.listOfData.filter(
      (item: {
        companyName: string;
        ownerName: string;
        ownerSurname: string;
        ownerEmail: string;
        ownerPhone: string;
      }) => filterFunc(item)
    );
    this.clients = data.sort((a, b) =>
      this.sortValue === "ascend"
        ? // tslint:disable-next-line:no-non-null-assertion
        a[this.sortName!] > b[this.sortName!]
          ? 1
          : -1
        : // tslint:disable-next-line:no-non-null-assertion
        b[this.sortName!] > a[this.sortName!]
          ? 1
          : -1
    );
  }

  openEditForm(client: Client): void {
    this.formDataNoId = Object.assign({}, client);
    const dialogRef = this.dialog.open(EditClientFormComponent, {
      width: '550px',
      data: {
        clientToEdit : this.formDataNoId
      }

    });
    dialogRef.afterClosed().subscribe(editClient => {
      if (typeof (editClient) === "object") {
        this.editClient(editClient, client.id);
      } else if (typeof (editClient) === "boolean") {
        this.showDeleteConfirm(client.id);
      }
    })
  }

  openAddForm() {
    const dialogRef = this.dialog.open(AddClientFormComponent, {
      width: '550px',
      data: {
        newClient: this.newClient
      }
    });

    dialogRef.afterClosed().subscribe(newClient => {
      if (newClient) {
        this.addNewClient(newClient);
        this.newClient = new Newclient();
      }
    });
  }


  editClient(client: Newclient, id: number) {
    this.clientService.editClient(client, id).subscribe(() => this.refreshTable()
    );
  }

  addNewClient(client: Newclient) {
    this.clientService.addClient(client).subscribe(
      () => {
        this.refreshTable();
      },
      error => {
        this.createBasicNotification();
      }
    );
  }
}

