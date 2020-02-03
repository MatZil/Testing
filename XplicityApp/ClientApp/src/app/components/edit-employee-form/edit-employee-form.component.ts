import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { EmployeeStatus } from 'src/app/models/employee-status.enum';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Updateuser } from 'src/app/models/updateuser';
import { EditModalData } from './edit-modal-data';

@Component({
  selector: 'app-edit-employee-form',
  templateUrl: './edit-employee-form.component.html',
  styleUrls: ['./edit-employee-form.component.css']
})
export class EditEmployeeFormComponent implements OnInit {
  initialStatusValue: EmployeeStatus;
  editEmployeeForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<EditEmployeeFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EditModalData) {}

  ngOnInit() {
    this.initialStatusValue = this.data.userToUpdate.status;
    this.adjustClientId();
    this.initializeFormGroup();
  }

  closeModal(returnValue: any): void {
    this.dialogRef.close(returnValue);
  }

  confirmEdit(): void {
    const userToUpdate = this.getFormUser();

    if (this.initialStatusValue !== userToUpdate.status) {
      if (confirm('Do you really want to change this employee\'s status?')) {
        this.closeModal(userToUpdate);
      }
    } else {
      this.closeModal(userToUpdate);
    }
  }

  getFormUser(): Updateuser {
    const formUser = Object.assign({}, this.editEmployeeForm.value, this.editEmployeeForm.controls.baseForm.value);

    if (formUser.clientId === 0) {
      formUser.clientId = null;
    }

    return formUser;
  }

  adjustClientId(): void {
    if (this.data.userToUpdate.clientId === null) {
      this.data.userToUpdate.clientId = 0;
    }
  }

  initializeFormGroup(): void {
    this.editEmployeeForm = this.formBuilder.group({
      baseForm: [this.data.userToUpdate],
      freeWorkDays: [this.data.userToUpdate.freeWorkDays, [
        Validators.required
      ]],
      overtimeHours: [this.data.userToUpdate.overtimeHours, [
        Validators.required
      ]],
      status: [{value: this.data.userToUpdate.status, disabled: this.data.isEditingSelf}]
    });
  }
}