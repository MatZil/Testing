import { Component, OnInit, ViewChild } from '@angular/core';
import { Client } from '../../models/client';
import { Newclient } from '../../models/newclient';
import { ClientService } from '../../services/client.service';
import { MatDialog } from '@angular/material/dialog';
import { ClientFormComponent } from '../client-form/client-form.component';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { AlertService } from '../../services/alert.service';

@Component({
  selector: 'app-client-table',
  templateUrl: './client-table.component.html',
  styleUrls: ['./client-table.component.scss']
})

export class ClientTableComponent implements OnInit {

  newClient: Newclient = new Newclient();
  newClientFormData: Newclient;

  displayedColumns: string[] = [
    'companyName',
    'ownerName',
    'ownerSurname',
    'ownerEmail',
    'ownerPhone',
    'actions'];
  dataSource = new MatTableDataSource<Client>();


  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(
    private clientService: ClientService,
    private alertService: AlertService,
    public dialog: MatDialog
  ) { }

  ngOnInit() {
    this.refreshTable();
    this.dataSource.paginator = this.paginator;
  }

  refreshTable(): void {
    this.clientService.getClient().subscribe(clients => {
      this.dataSource.data = clients;
    });
  }

  onDeleteButtonClick(id: number) {
    this.clientService.deleteClient(id).subscribe(() => {
      this.refreshTable();
    });
  }

  showDeleteConfirm(id: number): void {
    if (confirm('When clicked the OK button this section will be deleted')) {
      this.onDeleteButtonClick(id);
      this.closeModal();
    }
  }

  closeModal() {
    this.dialog.closeAll();
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
        this.alertService.displayMessage('A client with this company name already exists');
      }
    );
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

}

