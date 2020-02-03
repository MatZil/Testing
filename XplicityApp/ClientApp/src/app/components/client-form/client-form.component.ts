import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { ClientModal } from './client-modal';

@Component({
  selector: 'app-client-form',
  templateUrl: './client-form.component.html'
})

export class ClientFormComponent{

  constructor(
    public dialogRef: MatDialogRef<ClientModal>,
    @Inject(MAT_DIALOG_DATA) public data: ClientModal
  ) { }

  closeModal(returnValue: any) {
    this.dialogRef.close(returnValue);
  }
}
