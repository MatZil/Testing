import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { EditModalData } from '../employees-table/employees-table.component';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { EmployeeStatus } from 'src/app/models/employee-status.enum';

@Component({
  selector: 'app-edit-employee-form',
  templateUrl: './edit-employee-form.component.html',
  styleUrls: ['./edit-employee-form.component.css']
})
export class EditEmployeeFormComponent {
  initialStatusValue: EmployeeStatus;

  constructor(
    private authenticationService: AuthenticationService,
    public dialogRef: MatDialogRef<EditEmployeeFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EditModalData) {
    this.adjustClientId();
    this.initialStatusValue = this.data.userToUpdate.status;
  }

  closeModal(returnValue: any): void {
    this.dialogRef.close(returnValue);
  }

  confirmEdit(): void {
    if (this.initialStatusValue !== this.data.userToUpdate.status) {
      if (confirm('Do you really want to change this employee\'s status?')) {
        this.closeModal(this.data.userToUpdate);
      }
    } else {
      this.closeModal(this.data.userToUpdate);
    }
  }

  adjustClientId(): void {
    if (this.data.userToUpdate.clientId === null) {
      this.data.userToUpdate.clientId = 0;
    }
  }

  getCurrentUserId() {
    return this.authenticationService.getUserId();
  }
}
