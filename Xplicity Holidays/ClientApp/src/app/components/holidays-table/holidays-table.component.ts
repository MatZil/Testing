import { Component, OnInit } from '@angular/core';

import { Holidays } from '../../models/holidays';
import { Newholidays } from '../../models/newholidays';
import { Requestholidays } from '../../models/requestholidays';
import { HolidaysService } from '../../services/holidays.service';

import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import { AuthenticationService } from '../../services/authentication.service';

import { NgForm } from '@angular/forms';
import { NzModalRef, NzModalService } from 'ng-zorro-antd';

import { saveAs } from 'file-saver';
import { EnumToStringConverterService } from 'src/app/services/enum-to-string-converter.service';
import { HolidayType } from 'src/app/enums/holidayType';
import { EmployeeStatus } from 'src/app/models/employee-status.enum';
import { HttpResponse } from '@angular/common/http';
import { HeadersService } from 'src/app/services/headers.service';

@Component({
  selector: 'app-holidays-table',
  templateUrl: './holidays-table.component.html',
  styleUrls: ['./holidays-table.component.scss']
})
export class HolidaysTableComponent implements OnInit {
  holidays: Holidays[];
  requestHolidays: Requestholidays = new Requestholidays();
  selected = 1;
  isVisibleCreator = false;
  isConfirmLoadingCreator = false;
  isVisibleEditor = false;

  confirmDeleteModal: NzModalRef;

  users: User[];
  currentUser: User;
  currentUserId: number;
  holidaysType: string;
  holidaysStatus: string;
  role: string;
  overtimeDays: number;

  constructor(
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private holidayService: HolidaysService,
    private headersService: HeadersService = new HeadersService(),
    private modal: NzModalService,
    private enumConverter: EnumToStringConverterService
  ) {
    this.currentUserId = this.authenticationService.getUserId();
    this.requestHolidays.employeeId = this.currentUserId;
  }

  ngOnInit() {
    this.refreshTable(this.selected);

    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
    });

    this.userService.getAllUsers().subscribe(users => {
      this.users = users;
    });
    this.role = this.userService.getRole();
  }

  refreshTable(status: number) {
    this.holidayService.getHolidaysByStatus(status).subscribe(holidays => {
      this.holidays = holidays;
    });
  }

  onAddButtonClick() {
    this.requestHolidays.employeeId = this.currentUser.id;
    this.holidayService.addHolidays(this.requestHolidays).subscribe(() => {
      this.refreshTable(this.selected);
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
      this.refreshTable(this.selected);
    });
  }

  isTheRightId(holidays: Holidays) {
    if (this.currentUserId === holidays.employeeId) {
      return true;
    }
  }

  getUserNameById(id: number) {
    if (this.users) {
      for (const userIndex of Object.keys(this.users)) {
        const user = this.users[userIndex];
        if (user.id === id) {
          return user.name + ' ' + user.surname;
        }
      }
    }
  }

  changePaid() {
    this.requestHolidays.paid = !this.requestHolidays.paid;
  }

  setPaid() {
    if (this.requestHolidays.type === HolidayType.Parental) {
      this.requestHolidays.paid = true;
    } else if (this.requestHolidays.type === HolidayType.Science) {
      this.requestHolidays.paid = false;
    }
  }

  changeStatus(data) {
    this.refreshTable(this.selected);
  }

  getStatusName(status) {
    return this.enumConverter.determineHolidayStatus(status);
  }

  getStatusColor(status) {
    return this.enumConverter.determineHolidayStatusColor(status);
  }

  getOvertime(paid, overtimeDays) {
    if (paid) { return overtimeDays}
    else { return "-"}
  }

  isAdmin() {
    if (this.role === 'Admin') {
      return true;
    } else {
      return false;
    }
  }
}
