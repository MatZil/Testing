import { Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import { HolidaysService } from 'src/app/services/holidays.service';
import { Holiday } from 'src/app/models/holiday';
import { Birthday } from 'src/app/models/birthday';
import { UserService } from '../../services/user.service';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import { HolidayType } from 'src/app/enums/holidayType';
import {BehaviorSubject, of, zip} from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Client } from 'src/app/models/client';
import { ClientService } from 'src/app/services/client.service';

@Component({
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit, AfterViewInit {
  clients: Client[];
  viewDate: Date = new Date();
  events: any[];
  calendarTitle = new BehaviorSubject<string>(null);
  options: any;
  filter: number = 0;
  currentUsersId: number;
  currentUsersClientId: number;
    holidayTypes = [['#B7CEF5', 'Annual unpaid'], ['#88B0F5', 'Annual paid'], ['#547EC8', 'Annual paid, with overtime'],
    ['#DBC7FC', 'Science'], ['#BDA1EA', 'Day for children'], ['#FF93AC', 'Birthday']];

  @ViewChild('fullCalendar') fullCalendar: any;

  constructor(
    private holidayService: HolidaysService,
    private clientService: ClientService,
    private userService: UserService) {
  }

  ngOnInit() {
    this.clientService.getClient().subscribe(clients => {
      this.clients = clients;
    });
    this.setCalendarOptions();
    this.getUserAndFilteredCurrentMonthHolidays(0);
  }

    getUserAndFilteredCurrentMonthHolidays(filter) {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUsersId = user.id;
      this.addMonthsToCurrentDate(0);
      this.getEvents(filter);
    });
  }

  private setCalendarOptions() {
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

  private addMonthsToCurrentDate(monthsToAdd): void {
    this.viewDate = new Date(this.viewDate.setMonth(this.viewDate.getMonth() + monthsToAdd));
  }

  private getEvents(filter) {
    return zip(this.holidayService.getFilteredConfirmedHolidaysBySelectedMonth(this.viewDate, this.currentUsersId, filter),
      this.userService.getBirthdaysBySelectedMonth(this.viewDate, this.currentUsersId)).pipe(switchMap(val => {
        const holidayEvents = this.getHolidayEvents(val[0]);
        const birthdayEvents = this.getBirthdayEvents(val[1]);
        const allEvents = holidayEvents.concat(birthdayEvents);
        return of(allEvents);
    })).subscribe(events => {
      this.events = events;
    });
  }

  private getHolidayEvents(holidays: Holiday[]): any[] {
    const holidayEvents = [];
    holidays.forEach(holiday => {
      const color = this.getColor(holiday);
      const endDate = this.addDayToEndDate(holiday.toInclusive);
      const event = { 'title': holiday.employeeFullName, 'start': holiday.fromInclusive, 'end': endDate, 'color': color };
      holidayEvents.push(event);
    });
    return holidayEvents;
  }

  private getBirthdayEvents(birthdays: Birthday[]): any[] {
    const birthdayEvents = [];
    birthdays.forEach(birthday => {
      const color = this.holidayTypes[5][0];
      const endDate = this.addDayToEndDate(birthday.birthdayDate);
      let title = birthday.fullName;

      if (!birthday.isPublic) {
        title = `(private) ${title}`;
      }

      const event = { 'title': title, 'start': birthday.birthdayDate, 'end': endDate, 'color': color };

      birthdayEvents.push(event);
    });
    return birthdayEvents;
  }

  private getColor(holiday: Holiday) {
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

  private addDayToEndDate(toInclusive) {
    const startDate = new Date(toInclusive);
    const day = 60 * 60 * 24 * 1000;
    return new Date(startDate.getTime() + day);
  }

  previousMonthButtonClick(filter) {
    this.fullCalendar.calendar.prev();
    this.updateCalendarTitle();
    this.addMonthsToCurrentDate(-1);
    this.getEvents(filter);
  }

  nextMonthButtonClick(filter) {
    this.fullCalendar.calendar.next();
    this.updateCalendarTitle();
    this.addMonthsToCurrentDate(1);
    this.getEvents(filter);
  }

  ngAfterViewInit() {
    setTimeout(() => this.updateCalendarTitle(), 100);
  }

  updateCalendarTitle() {
    this.calendarTitle.next(this.fullCalendar?.calendar?.view?.title);
  }
  isAdmin(): boolean {
    return this.userService.isAdmin();
  }
}