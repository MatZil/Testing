import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { PasswordChangeModel } from 'src/app/models/password-change-model';
import { passwordMatcherValidatorFn } from '../../helpers/password-match-validator';
import { AuthenticationService } from '../../services/authentication.service';
import { UserService } from '../../services/user.service';
import { AlertService } from 'src/app/services/alert.service';
import { EmailTemplatesService } from '../../services/email-templates.service';
import { NewEmailTemplate } from '../../models/new-email-template';
import { EditModalData } from './edit-modal-data';

@Component({
  selector: 'app-emailtemplates-edit-form',
  templateUrl: './emailtemplates-edit-form.component.html',
  styleUrls: ['./emailtemplates-edit-form.component.scss']
})
export class EmailTemplatesFormComponent implements OnInit {
  changePasswordForm: FormGroup;
  hideOldPassword = true;
  hideFirstPassword = true;
  hideSecondPassword = true;

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<EmailTemplatesFormComponent>,
    private authenticationService: AuthenticationService,
    private emailTemplateService: EmailTemplatesService,
    private userService: UserService,
    private alertService: AlertService,
    @Inject(MAT_DIALOG_DATA) public data: EditModalData
  ) { }

  ngOnInit() {
    this.initializeFormGroup();
  }

  initializeFormGroup() {
    this.data.emailTemplatesFormData.purpose
    this.changePasswordForm = this.formBuilder.group({
      purpose: [],
      subject: [],
      template: [],
      instructions: []
    });
  }

  closePasswordModal(){
    this.dialogRef.close();
  }

  onChangeClicked() {

    this.dialogRef.close();
  }
}
