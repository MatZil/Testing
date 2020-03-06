import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { EditModalData } from './edit-modal-data';

@Component({
  selector: 'app-emailtemplates-edit-form',
  templateUrl: './emailtemplates-edit-form.component.html',
  styleUrls: ['./emailtemplates-edit-form.component.scss']
})
export class EmailTemplatesFormComponent implements OnInit {
  editEmailTemplateForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<EmailTemplatesFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EditModalData
  ) { }

  ngOnInit() {
    this.initializeFormGroup();
  }

  initializeFormGroup() {
    this.data.emailTemplatesFormData.purpose
    this.editEmailTemplateForm = this.formBuilder.group({
      purpose: ['', [
        Validators.required,
        Validators.pattern('[a-zA-ZąčęėįšųūžĄČĘĖĮŠŲŪŽ]*')
      ]],
      subject: ['', [
        Validators.required,
        Validators.pattern('[a-zA-ZąčęėįšųūžĄČĘĖĮŠŲŪŽ]*')
      ]],
      template: ['', [
        Validators.required
      ]],
      instructions: ['', [
        Validators.required
      ]],
    });
  }
  isFormInvalid(baseForm: FormGroup): boolean {
    return !(baseForm.valid && this.editEmailTemplateForm.valid);
  }
  closeModal(returnValue: any) {
    this.dialogRef.close(returnValue);
  }
}
