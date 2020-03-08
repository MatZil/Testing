import { Component, OnInit, ViewChild } from '@angular/core';
import { EmailTemplate } from '../../models/email-template';
import { NewEmailTemplate } from '../../models/new-email-template';
import { EmailTemplatesService } from 'src/app/services/email-templates.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material';
import { EmailTemplatesFormComponent } from '../edit-email-templates-form/edit-email-templates-form.component';

@Component({
  selector: 'app-email-templates-table',
  templateUrl: './email-templates-table.component.html',
  styleUrls: ['./email-templates-table.component.scss']
})

export class EmailtemplatesTableComponent implements OnInit {
  newEmailTemplateFormData: NewEmailTemplate;
  displayedColumns: string[] = [
    'purpose',
    'subject',
    'template',
    'buttonEdit'
  ];
  dataSource = new MatTableDataSource<EmailTemplate>();

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
      this.dataSource.data = templates;
    });
  }

  showEmailTemplatesModal(emailTemplate: EmailTemplate) {
    this.newEmailTemplateFormData = Object.assign({}, emailTemplate);
    const dialogRef = this.dialog.open(EmailTemplatesFormComponent, {
      width: '1000px',
      data: {
        emailTemplatesFormData: this.newEmailTemplateFormData
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
}
