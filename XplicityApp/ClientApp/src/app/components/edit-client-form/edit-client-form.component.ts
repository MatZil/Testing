import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { EditModalClient } from '../client-table/client-table.component';

@Component({
  selector: "app-edit-client-form",
  templateUrl: "./edit-client-form.component.html",
  styleUrls: ["./edit-client-form.component.scss"]
})
export class EditClientFormComponent{

  constructor(
    public dialogRef: MatDialogRef<EditModalClient>,
    @Inject(MAT_DIALOG_DATA) public data: EditModalClient
  ) { }

  closeModal(returnValue: any) {
    this.dialogRef.close(returnValue);
  }
}
