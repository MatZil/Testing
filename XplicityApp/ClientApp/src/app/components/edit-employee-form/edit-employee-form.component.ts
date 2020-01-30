import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { EditModalData } from '../employees-table/employees-table.component';
import { EmployeeStatus } from 'src/app/models/employee-status.enum';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';

@Component({
  selector: 'app-edit-employee-form',
  templateUrl: './edit-employee-form.component.html',
  styleUrls: ['./edit-employee-form.component.css']
})
export class EditEmployeeFormComponent {
  initialStatusValue: EmployeeStatus;
  controlGroup: FormGroup;
  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<EditEmployeeFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EditModalData) {
    this.adjustClientId();
    this.initialStatusValue = this.data.userToUpdate.status;
    this.initializeFormGroup();
  }

  closeModal(returnValue: any): void {
    this.dialogRef.close(returnValue);
  }

  confirmEdit(): void {
    const userToUpdate = Object.assign(this.controlGroup.value);
    if (this.initialStatusValue !== userToUpdate.status) {
      if (confirm('Do you really want to change this employee\'s status?')) {
        this.closeModal(userToUpdate);
      }
    } else {
      this.closeModal(userToUpdate);
    }
  }

  adjustClientId(): void {
    if (this.data.userToUpdate.clientId === null) {
      this.data.userToUpdate.clientId = 0;
    }
  }

  initializeFormGroup(): void {
    this.controlGroup = this.formBuilder.group({
      name: [this.data.userToUpdate.name, [
        Validators.required,
        Validators.pattern('[a-zA-ZąčęėįšųūžĄČĘĖĮŠŲŪŽ]*')
      ]],
      surname: [this.data.userToUpdate.surname, [
        Validators.required,
        Validators.pattern('[a-zA-ZąčęėįšųūžĄČĘĖĮŠŲŪŽ]*')
      ]],
      position: [this.data.userToUpdate.position, [
        Validators.required
      ]],
      email: [this.data.userToUpdate.email, [
        Validators.required,
        Validators.email
      ]],
      worksFromDate: [this.data.userToUpdate.worksFromDate, [
        Validators.required
      ]],
      birthdayDate: [this.data.userToUpdate.birthdayDate, [
        Validators.required
      ]],
      healthCheckDate: [this.data.userToUpdate.healthCheckDate, [
        Validators.required
      ]],
      freeWorkDays: [this.data.userToUpdate.freeWorkDays, [
        Validators.required
      ]],
      overtimeHours: [this.data.userToUpdate.overtimeHours, [
        Validators.required
      ]],
      clientId: [this.data.userToUpdate.clientId],
      role: [this.data.userToUpdate.role],
      daysOfVacation: [this.data.userToUpdate.daysOfVacation],
      parentalLeaveLimit: [this.data.userToUpdate.parentalLeaveLimit],
      status: [{value: this.data.userToUpdate.status, disabled: this.data.isEditingSelf}]
    });
  }
}
