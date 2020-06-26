import { Component, OnInit, ViewChild } from '@angular/core';
import { Survey } from '../../models/survey';
import { SurveyService } from '../../services/survey.service';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { EnumToStringConverterService } from 'src/app/services/enum-to-string-converter.service';
import { SurveysFormComponent } from '../surveys-form/surveys-form.component';
import { TableRowUserModel } from 'src/app/models/table-row-user-model';
import { UserService } from 'src/app/services/user.service';
import { EmployeeStatus } from 'src/app/models/employee-status.enum';

@Component({
  selector: 'app-surveys-table',
  templateUrl: './surveys-table.component.html',
  styleUrls: ['./surveys-table.component.scss']
})
export class SurveysTableComponent implements OnInit {

  survey: Survey = new Survey();
  url: string;
  users: TableRowUserModel[] = [];

  displayedColumns: string[] = [
    'title',
    'author',
    'creationDate',
    'actions'
  ];

  dataSource = new MatTableDataSource<Survey>();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(
    private surveyService: SurveyService,
    public dialog: MatDialog,
    public enumConverter: EnumToStringConverterService,
    private userService: UserService
  ) { }

  ngOnInit() {
    this.refreshTable();
    this.dataSource.paginator = this.paginator;
    this.getUsers();
  }

  refreshTable(): void {
    this.surveyService.getAllSurveys().subscribe(surveys => {
      this.dataSource.data = surveys;
    });
  }

  onDeleteButtonClick(id: number) {
    this.surveyService.deleteSurvey(id).subscribe(() => {
      this.refreshTable();
    });
  }

  onApproveButtonClick(id: string) {
    this.url = this.surveyService.createUrl(id);
  }

  showDeleteConfirm(id: number): void {
    if (confirm('Click OK to delete.')) {
      this.onDeleteButtonClick(id);
      this.closeModal();
    }
  }

  showApproveConfirm(id: string): void {
    if (confirm('Click OK to approve and get a shareable link. Editing of this survey will no longer be allowed.')) {
      this.onApproveButtonClick(id);
      this.closeModal();
    }
    alert(this.url);
  }

  closeModal() {
    this.dialog.closeAll();
  }

  openAddForm() {
    const dialogRef = this.dialog.open(SurveysFormComponent, {
      width: '550px',
      data: {
        formTitle: 'Add survey',
        formConfirmationButtonName: 'Add survey'
      }
    });

    dialogRef.afterClosed().subscribe(() => {
      this.refreshTable();
    });
  }

  openEditForm() {
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  getAuthorsName(id: number) {
    for (const user of this.users) {
      if (user.id === id) {
        return user.name;
      }
    }
  }

  getUsers() {
    this.userService.getAllUsers().subscribe(users => {
      this.users = users;
    });
  }
}
