import { Component, OnInit} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { PasswordChangeModel } from 'src/app/models/password-change-model';
import { passwordMatcherValidatorFn } from '../../helpers/password-match-validator';
import { AuthenticationService } from '../../services/authentication.service';
import { UserService } from '../../services/user.service';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-user-password-form',
  templateUrl: './user-password-form.component.html',
  styleUrls: ['./user-password-form.component.scss']
})
export class UserPasswordFormComponent implements OnInit {
  changePasswordForm: FormGroup;
  hideOldPassword = true;
  hideFirstPassword = true;
  hideSecondPassword = true;

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<UserPasswordFormComponent>,
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private alertService: AlertService
  ) { }

  ngOnInit() {
    this.initializeFormGroup();
  }

  initializeFormGroup() {
    this.changePasswordForm = this.formBuilder.group({
      passwords: this.formBuilder.group({
        oldPassword: ['', Validators.required],
        newPassword: ['', [Validators.required, Validators.minLength(6)]],
        passwordConfirm: ['', Validators.required]
      }, { validator: passwordMatcherValidatorFn })
    });
  }

  closePasswordModal(){
    this.dialogRef.close();
  }

  onChangeClicked() {
    const currentPassword = this.changePasswordForm.get('passwords.oldPassword').value;
    const newPassword = this.changePasswordForm.get('passwords.newPassword').value;
    const id = this.authenticationService.getUserId();

    if (this.changePasswordForm.valid && currentPassword && newPassword) {
      const passwordChangeModel = new PasswordChangeModel();
      passwordChangeModel.currentPassword = currentPassword;
      passwordChangeModel.newPassword = newPassword;

      this.userService.changePassword(id, passwordChangeModel)
        .subscribe(
          () => {
            this.alertService.displayMessage('You have successfuly changed your password');
            this.changePasswordForm.reset();
          },
          error => {
            this.alertService.displayMessage('There was an error while changing password');
            console.log(error);
          }
        );
    }
    this.dialogRef.close();
  }

}
