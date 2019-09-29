import { Component, OnInit } from '@angular/core';
import { EmailTemplate } from '../../models/email-template';
import { NewEmailTemplate } from '../../models/new-email-template';
import { NgForm } from '@angular/forms';
import { NzModalRef, NzModalService, NzNotificationService } from 'ng-zorro-antd';
import { EmailTemplatesService } from 'src/app/services/email-templates.service';

@Component({
  selector: 'app-emailtemplates-table',
  templateUrl: './emailtemplates-table.component.html',
  styleUrls: ['./emailtemplates-table.component.scss']
})
export class EmailtemplatesTableComponent implements OnInit {
  emailTemplates: EmailTemplate[] = [];
  formData: EmailTemplate;
  formDataNoId: NewEmailTemplate;
  newEmailTemplate: NewEmailTemplate = new NewEmailTemplate();

  isVisibleCreator = false;
  isConfirmLoadingCreator = false;
  isVisibleEditor = false;

  confirmDeleteModal: NzModalRef;

  searchValue = '';
  listOfSearchAddress: string[] = [];
  sortName: string | null = null;
  sortValue: string | null = null;
  listOfData: EmailTemplate[] = [];

  constructor(
    private emailTemplatesService: EmailTemplatesService,
    private modal: NzModalService,
    private notification: NzNotificationService
  ) { }

  ngOnInit() {
    this.refreshTable();
  }

  refreshTable() {
    this.emailTemplatesService.getEmailTemplates().subscribe(templates => {
      this.emailTemplates = templates;
      this.listOfData = [...this.emailTemplates];
    });
  }
}
