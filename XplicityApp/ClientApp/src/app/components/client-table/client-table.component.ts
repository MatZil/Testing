import { Component, OnInit, ViewChild } from '@angular/core';

import { Client } from '../../models/client';
import { Newclient } from '../../models/newclient';
import { ClientService } from '../../services/client.service';
import { NzModalRef, NzModalService } from 'ng-zorro-antd';
import { NzNotificationService } from 'ng-zorro-antd';
import { MatDialog } from '@angular/material';
import { ClientFormComponent } from '../client-form/client-form.component';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';

@Component({
  selector: 'app-client-table',
  templateUrl: './client-table.component.html',
  styleUrls: ['./client-table.component.scss']
})

export class ClientTableComponent implements OnInit {
  clients: Client[] = [];
  newClient: Newclient = new Newclient();
  newClientFormData: Newclient;

  displayedColumns: string[] = [
    'companyName', 
    'ownerName', 
    'ownerSurname', 
    'ownerEmail', 
    'ownerPhone', 
    'buttonEdit', 
    'buttonDelete'];
  dataSource = new MatTableDataSource<Client>(this.clients);

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

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
    this.dataSource.paginator = this.paginator;
  }

  refreshTable(): void {
    this.clientService.getClient().subscribe(clients => {
      this.clients = clients;
      this.listOfData = [...this.clients];
      this.dataSource = new MatTableDataSource<Client>(this.clients);
    });
  }

  onDeleteButtonClick(id: number) {
    this.clientService.deleteClient(id).subscribe(() => {
      this.refreshTable();
    });
  }

  showDeleteConfirm(id: number): void {
    this.confirmDeleteModal = this.modal.confirm({
      nzTitle: 'Do you want to delete this section?',
      nzContent: 'When clicked the OK button this section will be deleted',
      nzOnOk: () => this.onDeleteButtonClick(id)
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
      this.sortValue === 'ascend'
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
    this.newClientFormData = Object.assign({}, client);
    const dialogRef = this.dialog.open(ClientFormComponent, {
      width: '550px',
      data: {
        clientFormData: this.newClientFormData,
        formTitle: 'Edit client',
        formConfirmationButtonName: 'Confirm edit'
      }
    });

    dialogRef.afterClosed().subscribe(editClient => {
      if (editClient) {
        this.editClient(editClient, client.id);
      }
    })
  }

  openAddForm() {
    const dialogRef = this.dialog.open(ClientFormComponent, {
      width: '550px',
      data: {
        clientFormData: this.newClient,
        formTitle: 'Add client',
        formConfirmationButtonName: 'Add client'
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
    this.clientService.editClient(client, id).subscribe(() => {
      this.refreshTable();
    }
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

