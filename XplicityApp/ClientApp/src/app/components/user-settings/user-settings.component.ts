import { Component, OnInit } from '@angular/core';
import { TableRowUserModel } from '../../models/table-row-user-model';
import { AuthenticationService } from '../../services/authentication.service';
import { UserService } from '../../services/user.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { passwordMatcherValidatorFn } from '../../helpers/password-match-validator';
import { PasswordChangeModel } from '../../models/password-change-model';
import { AlertService } from 'src/app/services/alert.service';
import { FileType } from '../../enums/fileType';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss']
})
export class UserSettings implements OnInit {
  currentUser: TableRowUserModel;
  hideOldPassword = true;
  hideFirstPassword = true;
  hideSecondPassword = true;
  registerForm: FormGroup;
  isVisibleUploadModal = false;
  isVisiblePasswordModal = false;
  fileTypes = FileType;

  constructor(
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private formBuilder: FormBuilder,
    private alertService: AlertService
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
    this.createFormGroup();
  }

  showUploadModal() {
    this.isVisibleUploadModal = true;
  }

  closeUploadModal() {
    this.isVisibleUploadModal = false;
  }
  closePasswordModal() {
    this.isVisiblePasswordModal = false;
  }
  showPasswordModal() {
    this.isVisiblePasswordModal = true;
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
            this.alertService.displayMessage('You have successfuly changed your password');
            this.registerForm.reset();
          },
          error => {
            this.alertService.displayMessage('There was an error while changing password');
            console.log(error);
          }
        );
    }
  }

  isAdmin(): boolean {
    return this.userService.isAdmin();
  }
}
