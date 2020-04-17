import { Component, OnInit, ViewChild, AfterViewInit, ChangeDetectorRef, AfterViewChecked } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { HolidaysService } from 'src/app/services/holidays.service';
import { Holiday } from 'src/app/models/holiday';
import { Birthday } from 'src/app/models/birthday';
import { UserService } from '../../services/user.service';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import { HolidayType } from 'src/app/enums/holidayType';
import { BehaviorSubject } from 'rxjs';
@Component({
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit, AfterViewInit {
  viewDate: Date = new Date();
  events: any[];
  calendarTitle = new BehaviorSubject<string>(null);
  options: any;
  currentUserId: number;
  dataHolidays = new MatTableDataSource<Holiday>();
  dataBirthdays = new MatTableDataSource<Birthday>();
  holidayTypes = [['#B7CEF5', 'Annual unpaid'], ['#88B0F5', 'Annual paid'], ['#547EC8', 'Annual paid, with overtime'],
      ['#DBC7FC', 'Science'], ['#BDA1EA', 'Day for children'], ['#FF4D4D', 'Birthday']];

  @ViewChild('fullCalendar') fullCalendar: any;

  constructor(
    private holidayService: HolidaysService,
    private userService: UserService) {
  }

  ngOnInit() {
    this.setCalendarOptions();
    this.getUserAndCurrentMonthHolidays();
  }

  getUserAndCurrentMonthHolidays() {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUserId = user.id;
      this.addMontsToCurrentDate(0);
      this.getHolidays();
      this.getBirthdays();
    });
  }

  setCalendarOptions() {
    this.options = {
      plugins: [dayGridPlugin, interactionPlugin],
      defaultDate: new Date(),
      header: false,
      titleFormat: {
        month: '2-digit',
        year: 'numeric'
      },
      editable: true,
      firstDay: 1,
      displayEventTime: false
    };
  }

  addMontsToCurrentDate(monthsToAdd): void {
    this.viewDate = new Date(this.viewDate.setMonth(this.viewDate.getMonth() + monthsToAdd));
  }

  getHolidays(): void {
    this.holidayService.getConfirmedHolidaysBySelectedMonth(this.viewDate, this.currentUserId).subscribe(holidays => {
      this.dataHolidays.data = holidays;
      this.getHolidayEvents();
    });
  }

  getHolidayEvents(): void {
    this.dataHolidays.data.forEach(holiday => {
      const color = this.getColor(holiday);
      const endDate = this.addDayToEndDate(holiday.toInclusive);
      const event = { 'title': holiday.employeeFullName, 'start': holiday.fromInclusive, 'end': endDate, 'color': color };
      this.events.push(event);
    });
  }

  getBirthdays(): void {
    this.userService.getBirthdaysBySelectedMonth(this.viewDate, this.currentUserId).subscribe(birthdays => {
      this.dataBirthdays.data = birthdays;
      this.getBirthdayEvents();
    });
  }

  getBirthdayEvents(): void {
    this.events = [];
    this.dataBirthdays.data.forEach(birthday => {
      const color = this.holidayTypes[5][0];
      const endDate = this.addDayToEndDate(birthday.birthdayDate);
      let title = birthday.fullName + ' (year ' + birthday.birthdayYear + ')';
      //let icon = 

      if (!birthday.isPublic) {
        title = "(private) " + title;
      }

      const event = { 'title': title, 'start': birthday.birthdayDate, 'end': endDate, 'color': color };

      // this.fullCalendar.calendar.ad .n ( 'renderEvent', event );
      this.events.push(event);
    });
  }

  getColor(holiday: Holiday) {
    let holidayType: string;
    if (holiday.type === HolidayType.Annual && holiday.overtimeDays > 0) {
      holidayType = this.holidayTypes[2][0];
    } else if (holiday.type === HolidayType.Unpaid) {
      holidayType = this.holidayTypes[0][0];
    } else if (holiday.type === HolidayType.Annual) {
      holidayType = this.holidayTypes[1][0];
    } else if (holiday.type === HolidayType.Science) {
      holidayType = this.holidayTypes[3][0];
    } else if (holiday.type === HolidayType.DayForChildren) {
      holidayType = this.holidayTypes[4][0];
    }
    return holidayType;
  }

  addDayToEndDate(toInclusive) {
    const startDate = new Date(toInclusive);
    const day = 60 * 60 * 24 * 1000;
    return new Date(startDate.getTime() + day);
  }

  previousMonthButtonClick() {
    this.fullCalendar.calendar.prev();
    this.updateCalendarTitle();
    this.addMontsToCurrentDate(-1);
    this.getHolidays();
    this.getBirthdays();
  }

  nextMonthButtonClick() {
    this.fullCalendar.calendar.next();
    this.updateCalendarTitle();
    this.addMontsToCurrentDate(1);
    this.getHolidays();
    this.getBirthdays();
  }

  ngAfterViewInit() {
    setTimeout(() => this.updateCalendarTitle(), 100);
  }

  updateCalendarTitle() {
    this.calendarTitle.next(this.fullCalendar?.calendar?.view?.title);
  }
}