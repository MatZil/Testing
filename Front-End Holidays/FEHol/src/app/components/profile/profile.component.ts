import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user';
import { AuthenticationService } from '../../services/authentication-service.service';
import { UserService } from '../../services/user.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { passwordMatcherValidatorFn } from '../../helpers/password-match-validator';
import { PasswordChangeModel } from '../../models/password-change-model';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  private readonly snackbarConfiguration: MatSnackBarConfig = { duration: 5000 };
  currentUser: User;
  hideOldPassword = true;
  hideFirstPassword = true;
  hideSecondPassword = true;
  registerForm: FormGroup;
  constructor(
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar
  ) {
  }
  get oldPassword() {
    return this.registerForm.get('passwords.oldPassword');
  }

  get newPassword() {
    return this.registerForm.get('passwords.newPassword');
  }

  get passwordConfirm() {
    return this.registerForm.get('passwords.passwordConfirm');
  }
  ngOnInit() {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
    });
    this.createFormGroup();
  }

  createFormGroup() {
    this.registerForm = this.formBuilder.group({
      passwords: this.formBuilder.group({
        oldPassword: ['', Validators.required],
        newPassword: ['', [Validators.required, Validators.minLength(6)]],
        passwordConfirm: ['', Validators.required]
      }, { validator: passwordMatcherValidatorFn })
    });
  }

  onChangeClicked() {
    const currentPassword = this.registerForm.get('passwords.oldPassword').value;
    const newPassword = this.registerForm.get('passwords.newPassword').value;
    const id = this.authenticationService.getUserId();

    if (this.registerForm.valid && currentPassword && newPassword) {
      const passwordChangeModel = new PasswordChangeModel();
      passwordChangeModel.currentPassword = currentPassword;
      passwordChangeModel.newPassword = newPassword;

      this.userService.changePassword(id, passwordChangeModel)
        .subscribe(
          () => {
            this.snackBar.open('You have successfuly changed your password', 'OK', this.snackbarConfiguration);
            this.registerForm.reset();
          },
          error => {
            this.snackBar.open('There was an error while changing password', 'OK', this.snackbarConfiguration);
            console.log(error);
          }
        );
    }
  }
}
