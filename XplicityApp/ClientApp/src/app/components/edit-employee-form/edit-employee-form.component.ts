import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EmployeeStatus } from 'src/app/models/employee-status.enum';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Updateuser } from 'src/app/models/updateuser';
import { EditModalData } from './edit-modal-data';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-edit-employee-form',
  templateUrl: './edit-employee-form.component.html',
  styleUrls: ['./edit-employee-form.component.css']
})
export class EditEmployeeFormComponent implements OnInit {
  editEmployeeForm: FormGroup;

  constructor(
    private userService: UserService,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<EditEmployeeFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EditModalData) { }

  ngOnInit() {
    this.adjustClientId();
    this.initializeFormGroup();
  }

  closeModal(returnValue: any): void {
    this.dialogRef.close(returnValue);
  }

  confirmEdit(): void {
    const userToUpdate = this.getFormUser();

    if (this.data.userToUpdate.status !== this.editEmployeeForm.value.status) {
      if (confirm('Do you really want to change this employee\'s status?')) {
        userToUpdate.status = this.editEmployeeForm.value.status;
        this.closeModal(userToUpdate);
      }
    } else {
      this.closeModal(userToUpdate);
    }
  }

  getFormUser(): Updateuser {
    this.editEmployeeForm.controls.baseForm.value.overtimeHours = this.editEmployeeForm.value.overtimeHours;
    this.editEmployeeForm.controls.baseForm.value.freeWorkDays = this.editEmployeeForm.value.freeWorkDays;
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
      password: [this.data.userToUpdate.password, [
        Validators.minLength(6)
      ]],
      status: [{ value: this.data.userToUpdate.status, disabled: this.data.isEditingSelf }]
    });
  }

  isFormInvalid(baseForm: FormGroup): boolean {
    return !(baseForm.valid && this.editEmployeeForm.valid);
  }

  deleteUserById(id: number) {
    this.userService.deleteUser(id).subscribe(() => {
    });
  }

  showDeleteConfirm(userToDelete: Updateuser, employeeId: number): void {
    const userToUpdate = this.getFormUser();
    if (confirm('If you confirm,' + userToDelete.name + ' ' + userToDelete.surname + ' will be permanently deleted.')) {
      this.deleteUserById(employeeId);
      this.closeModal(userToUpdate);
    }
  }

  isStatusCurrent(): boolean {
    return this.editEmployeeForm.controls.status.value === EmployeeStatus.Current;
  }
}
