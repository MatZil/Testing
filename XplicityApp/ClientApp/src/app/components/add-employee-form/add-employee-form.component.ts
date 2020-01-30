import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AddModalData } from '../employees-table/employees-table.component';
import { Newuser } from 'src/app/models/newuser';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-employee-form',
  templateUrl: './add-employee-form.component.html',
  styleUrls: ['./add-employee-form.component.css']
})
export class AddEmployeeFormComponent {
  controlGroup: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<AddEmployeeFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AddModalData) {
    this.setDefaultValues();
    this.initializeFormGroup();
  }

  setDefaultValues(): void {
    this.data.newUser.clientId = 0;
    this.data.newUser.parentalLeaveLimit = 0;
    this.data.newUser.role = 'Employee';
    this.data.newUser.daysOfVacation = 20;
    this.data.newUser.isManualHolidaysInput = false;
  }

  initializeFormGroup(): void {
    this.controlGroup = this.formBuilder.group({
      name: [this.data.newUser.name, [
        Validators.required,
        Validators.pattern('[a-zA-ZąčęėįšųūžĄČĘĖĮŠŲŪŽ]*')
      ]],
      surname: [this.data.newUser.surname, [
        Validators.required,
        Validators.pattern('[a-zA-ZąčęėįšųūžĄČĘĖĮŠŲŪŽ]*')
      ]],
      position: [this.data.newUser.position, [
        Validators.required
      ]],
      email: [this.data.newUser.email, [
        Validators.required,
        Validators.email
      ]],
      password: [this.data.newUser.password, [
        Validators.required,
        Validators.minLength(6)
      ]],
      worksFromDate: [this.data.newUser.worksFromDate, [
        Validators.required
      ]],
      birthdayDate: [this.data.newUser.birthdayDate, [
        Validators.required
      ]],
      healthCheckDate: [this.data.newUser.healthCheckDate, [
        Validators.required
      ]],
      freeWorkDays: [this.data.newUser.freeWorkDays],
      clientId: [this.data.newUser.clientId],
      role: [this.data.newUser.role],
      daysOfVacation: [this.data.newUser.daysOfVacation],
      parentalLeaveLimit: [this.data.newUser.parentalLeaveLimit],
      isManualHolidaysInput: [this.data.newUser.isManualHolidaysInput]
    });
  }

  onSubmit(): void {
    const newUser = Object.assign(this.controlGroup.value);
    if (!newUser.freeWorkDays) {
      newUser.freeWorkDays = 0;
    }
    this.dialogRef.close(newUser);
  }
}
