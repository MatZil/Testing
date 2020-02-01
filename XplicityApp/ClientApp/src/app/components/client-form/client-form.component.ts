import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { ClientModal } from '../client-table/client-table.component';
import { ModalType } from '../../enums/modal-type.enum';

@Component({
  selector: 'app-client-form',
  templateUrl: './client-form.component.html',
  styleUrls: ['./client-form.component.scss']
})
export class ClientFormComponent{

  constructor(
    public dialogRef: MatDialogRef<ClientModal>,
    @Inject(MAT_DIALOG_DATA) public data: ClientModal
  ) { }

  closeModal(returnValue: any) {
    this.dialogRef.close(returnValue);
  }

  checkModalType() {
    if (this.data.modalType === ModalType.Add) {
      return true;
    }
    return false;
  }
}
