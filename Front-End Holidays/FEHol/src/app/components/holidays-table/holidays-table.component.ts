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
  requestHolidays: Requestholidays = new Requestholidays();

  isVisibleCreator = false;
  isConfirmLoadingCreator = false;
  isVisibleEditor = false;

  confirmDeleteModal: NzModalRef;

  users: User[];
  currentUser: User;
  currentUserId: number;

  holidaysType: string;
  holidaysStatus: string;

  constructor(
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private holidayService: HolidaysService,
    private modal: NzModalService
  ) {
    this.currentUser = this.authenticationService.currentUserValue;
    this.currentUserId = this.currentUser.id;
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

  onEditConfirmButtonClick(holidays: Newholidays, id: number) {
    this.holidayService.editHolidays(holidays, id).subscribe(() => {
      this.refreshTable();
    });
  }

  isAdmin() {
    if (this.currentUser.role === 'admin') {
      return true;
    }
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

  assignStatusString(status: number) {
    if (status === 0) {
      this.holidaysStatus = 'Unconfirmed';
    }

    if (status === 1) {
      this.holidaysStatus = 'Declined';
    }

    if (status === 2) {
      this.holidaysStatus = 'Confirmed';
    }

    return this.holidaysStatus;
  }

  getUserNameById(id: number) {
    for (const user of this.users) {
      if (user.id === id) {
        return user.name + ' ' + user.surname;
      }
    }
  }

  changePaid() {
    this.requestHolidays.paid = !this.requestHolidays.paid;
  }

  setPaid() {
    if (this.requestHolidays.type === 1) {
      this.requestHolidays.paid = true;
    } else if (this.requestHolidays.type === 2) {
      this.requestHolidays.paid = false;
    }
  }
}
