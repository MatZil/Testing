import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AddModalData } from '../employees-table/employees-table.component';
import { Newuser } from 'src/app/models/newuser';

@Component({
  selector: 'app-add-employee-form',
  templateUrl: './add-employee-form.component.html',
  styleUrls: ['./add-employee-form.component.css']
})
export class AddEmployeeFormComponent {

  readonly minDate = new Date(1900, 1, 1);
  readonly maxDate = new Date(2100, 1, 1);

  constructor(
    public dialogRef: MatDialogRef<AddEmployeeFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AddModalData) {
    this.setDefaultValues();
  }

  setDefaultValues(): void {
    this.data.newUser.clientId = 0;
    this.data.newUser.parentalLeaveLimit = 0;
    this.data.newUser.role = 'Employee';
    this.data.newUser.daysOfVacation = 20;
  }
}
