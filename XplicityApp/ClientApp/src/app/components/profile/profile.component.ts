import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user';
import { AuthenticationService } from '../../services/authentication.service';
import { UserService } from '../../services/user.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { passwordMatcherValidatorFn } from '../../helpers/password-match-validator';
import { PasswordChangeModel } from '../../models/password-change-model';
import { AlertService } from 'src/app/services/alert.service';
import { FileType } from '../../enums/fileType';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  currentUser: User;
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
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
      this.currentUser.role = this.userService.getRole();
    });
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

  isAdmin() {
    if (this.userService.getRole() === 'Admin') {
      return true;
    }
    else {
      return false;
    }
  }
}
