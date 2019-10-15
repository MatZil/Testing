import { Component, OnInit } from '@angular/core';
import { EmailTemplate } from '../../models/email-template';
import { NewEmailTemplate } from '../../models/new-email-template';
import { NgForm } from '@angular/forms';
import { EmailTemplatesService } from 'src/app/services/email-templates.service';

@Component({
  selector: 'app-emailtemplates-table',
  templateUrl: './emailtemplates-table.component.html',
  styleUrls: ['./emailtemplates-table.component.scss']
})

export class EmailtemplatesTableComponent implements OnInit {
  emailTemplates: EmailTemplate[] = [];
  formObject: NewEmailTemplate;

  isVisibleEditor = false;

  constructor(
    private emailTemplatesService: EmailTemplatesService
  ) { }

  ngOnInit() {
    this.refreshTable();
  }

  refreshTable() {
    this.emailTemplatesService.getEmailTemplates().subscribe(templates => {
      this.emailTemplates = templates;
    });
  }

  showModalEditor(): void {
    this.isVisibleEditor = true;
  }

  handleCancelEditor(): void {
    this.isVisibleEditor = false;
  }

  createFormObject(emailTemplate: NewEmailTemplate) {
    this.formObject = Object.assign({}, emailTemplate);
  }

  onSubmit(form: NgForm) {
    form.resetForm();
  }

  onEditConfirmButtonClick(emailTemplate: NewEmailTemplate, id: number) {
    this.emailTemplatesService.editEmailTemplate(emailTemplate, id).subscribe(() => {
      this.refreshTable();
      this.handleCancelEditor();
    });
  }
}
