import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Newuser } from 'src/app/models/newuser';
import { AddModalData } from './add-modal-data';
import { EmployeeStatus } from 'src/app/models/employee-status.enum';

@Component({
  selector: 'app-add-employee-form',
  templateUrl: './add-employee-form.component.html',
  styleUrls: ['./add-employee-form.component.css']
})
export class AddEmployeeFormComponent implements OnInit {
  addEmployeeForm: FormGroup;
  newUser = new Newuser();

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<AddEmployeeFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AddModalData) {}

  ngOnInit() {
    this.setDefaultValues();
    this.initializeFormGroup();
  }

  setDefaultValues(): void {
    this.newUser.clientId = 0;
    this.newUser.parentalLeaveLimit = 0;
    this.newUser.role = 'Employee';
    this.newUser.daysOfVacation = 20;
    this.newUser.isManualHolidaysInput = false;
  }

  initializeFormGroup(): void {
    this.addEmployeeForm = this.formBuilder.group({
      baseForm: [this.newUser],
      password: [this.newUser.password, [
        Validators.required,
        Validators.minLength(6)
      ]],
      freeWorkDays: [this.newUser.freeWorkDays],
      isManualHolidaysInput: [this.newUser.isManualHolidaysInput]
    });
  }

  onSubmit(): void {
    const newUser = this.getFormUser();

    this.dialogRef.close(newUser);
  }

  getFormUser(): Newuser {
    const formUser = Object.assign({}, this.addEmployeeForm.value, this.addEmployeeForm.controls.baseForm.value);

    if (!formUser.freeWorkDays) {
      formUser.freeWorkDays = 0;
    }

    if (formUser.clientId === 0) {
      formUser.clientId = null;
    }

    formUser.status = EmployeeStatus.Current;

    return formUser;
  }
}
