import { Component, OnInit, Inject} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AddModalClient } from '../client-table/client-table.component';

@Component({
  selector: "app-add-client-form",
  templateUrl: "./add-client-form.component.html",
  styleUrls: ["./add-client-form.component.scss"]
})

export class AddClientFormComponent {

  constructor(
    public dialogRef: MatDialogRef<AddModalClient>,
    @Inject(MAT_DIALOG_DATA) public data: AddModalClient
  ) {
  }

}
