import { Component, OnInit, ViewChild } from '@angular/core';
import { EmailTemplate } from '../../models/email-template';
import { NewEmailTemplate } from '../../models/new-email-template';
import { NgForm } from '@angular/forms';
import { EmailTemplatesService } from 'src/app/services/email-templates.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material';
import { EmailTemplatesFormComponent } from '../emailtemplates-edit-form/emailtemplates-edit-form.component';

@Component({
  selector: 'app-emailtemplates-table',
  templateUrl: './emailtemplates-table.component.html',
  styleUrls: ['./emailtemplates-table.component.scss']
})

export class EmailtemplatesTableComponent implements OnInit {
  emailTemplates: EmailTemplate[] = [];
  formObject: NewEmailTemplate;
  newEmailTemplateFormData: NewEmailTemplate;
  editId: number;

  displayedColumns: string[] = [
    'purpose',
    'subject',
    'template',
    'buttonEdit'
  ];
  dataSource = new MatTableDataSource<EmailTemplate>();

  isVisibleEditor = false;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(
    private emailTemplatesService: EmailTemplatesService,
    public dialog: MatDialog
  ) { }

  ngOnInit() {
    this.refreshTable();
    this.dataSource.paginator = this.paginator;
  }

  refreshTable() {
    this.emailTemplatesService.getEmailTemplates().subscribe(templates => {
      this.emailTemplates = templates;//delete after
      this.dataSource.data = templates;
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

  setEditId(id: number) {
    this.editId = id;
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
  showEmailTemplatesModal(emailTemplate: EmailTemplate) {
    this.newEmailTemplateFormData = Object.assign({}, emailTemplate);
    const dialogRef = this.dialog.open(EmailTemplatesFormComponent, {
      width: '1000px',
      data: {
        emailTemplatesFormData: this.newEmailTemplateFormData,
        formTitle: 'Edit 111'
      }
    });
    dialogRef.afterClosed().subscribe(editEmailTemplate => {
      if (editEmailTemplate) {
        this.editEmailTemplate(editEmailTemplate, emailTemplate.id);
      }
    })
  }
  editEmailTemplate(emailTemplate: EmailTemplate, id: number) {
    this.emailTemplatesService.editEmailTemplate(emailTemplate, id).subscribe(() => {
      this.refreshTable();
    }
    );
  }
  closePasswordModal() {
    this.dialog.closeAll();
  }
}
