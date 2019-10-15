import { Component, OnInit } from '@angular/core';

import { Holidays } from '../../models/holidays';
import { Newholidays } from '../../models/newholidays';
import { Requestholidays } from '../../models/requestholidays';
import { HolidaysService } from '../../services/holidays.service';

import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import { AuthenticationService } from '../../services/authentication-service.service';

import { NgForm } from '@angular/forms';
import { NzModalRef, NzModalService } from 'ng-zorro-antd';

import { saveAs } from 'file-saver';

@Component({
  selector: 'app-holidays-table',
  templateUrl: './holidays-table.component.html',
  styleUrls: ['./holidays-table.component.scss']
})
export class HolidaysTableComponent implements OnInit {
  holidays: Holidays[];
  formData: Holidays;
  formDataNoId: Newholidays;
  requestHolidays: Requestholidays = new Requestholidays();

  isVisibleCreator = false;
  isConfirmLoadingCreator = false;
  isVisibleEditor = false;

  confirmDeleteModal: NzModalRef;

  users: User[];
  currentUser: User;
  currentUserId: number;

  holidaysType: string;

  constructor(
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private holidayService: HolidaysService,
    private modal: NzModalService
  ) {
    this.currentUserId = this.authenticationService.getUserId();
    this.requestHolidays.employeeId = this.currentUserId;
  }

  ngOnInit() {
    this.refreshTable();

    this.userService.getUser(this.authenticationService.getUserId()).subscribe(user => {
      this.currentUser = user;
    });

    this.userService.getAllUsers().subscribe(users => {
      this.users = users;
    });
  }

  refreshTable() {
    this.holidayService.getHolidays().subscribe(holidays => {
      this.holidays = holidays;
    });
  }

  onAddButtonClick() {
    this.requestHolidays.employeeId = this.currentUser.id;
    this.holidayService.addHolidays(this.requestHolidays).subscribe(response => {
      saveAs(response, 'Holidays_Request');
      this.refreshTable();
    });
  }

  showModalCreator(): void {
    this.isVisibleCreator = true;
  }

  handleOkCreator(): void {
    this.isConfirmLoadingCreator = true;
    setTimeout(() => {
      this.isVisibleCreator = false;
      this.isConfirmLoadingCreator = false;
    }, 3000);
  }

  handleCancelCreator(): void {
    this.isVisibleCreator = false;
  }

  showModalEditor(): void {
    this.isVisibleEditor = true;
  }

  handleCancelEditor(): void {
    this.isVisibleEditor = false;
  }

  onSubmit(form: NgForm) {
    form.resetForm();
  }

  onDeleteButtonClick(id: number) {
    this.holidayService.deleteHolidays(id).subscribe(() => {
      this.refreshTable();
    });
  }

  onEditConfirmButtonClick(holidays: Newholidays, id: number) {
    this.holidayService.editHolidays(holidays, id).subscribe(() => {
      this.refreshTable();
    });
  }

  populateForm(holidays: Holidays) {
    this.formData = Object.assign({}, holidays);
  }

  populateFormNoId(holidays: Newholidays) {
    this.formDataNoId = Object.assign({}, holidays);
  }

  deleteClientOnModalClose(id: number) {
    this.onDeleteButtonClick(id);
    this.handleCancelEditor();
  }

  showDeleteConfirm(id: number): void {
    this.confirmDeleteModal = this.modal.confirm({
      nzTitle: 'Do you want to delete this section?',
      nzContent: 'When clicked the OK button this section will be deleted',
      nzOnOk: () => this.deleteClientOnModalClose(id)
    });
  }



  isTheRightId(holidays: Holidays) {
    if (this.currentUserId === holidays.employeeId) {
      return true;
    }
  }

  checkWhatTypeOfHoliday(type: number) {
    if (type === 0) {
      this.holidaysType = 'Annual';
    }

    if (type === 1) {
      this.holidaysType = 'Parental';
    }

    if (type === 2) {
      this.holidaysType = 'Science';
    }

    return this.holidaysType;
  }

  getUserNameById(id: number) {
    for (const user of this.users) {
      if (user.id === id) {
        return user.name + ' ' + user.surname;
      }
    }
  }
}
