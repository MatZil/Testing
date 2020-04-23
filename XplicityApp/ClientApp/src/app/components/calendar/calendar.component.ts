import {Component, OnInit, ViewChild, AfterViewInit, ChangeDetectorRef, AfterViewChecked} from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';
import {HolidaysService} from 'src/app/services/holidays.service';
import {Holiday} from 'src/app/models/holiday';
import {UserService} from '../../services/user.service';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import {HolidayType} from 'src/app/enums/holidayType';
import {BehaviorSubject} from 'rxjs';
import {Client} from 'src/app/models/client';
import {ClientService} from 'src/app/services/client.service';

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
  dataSource = new MatTableDataSource<Holiday>();
  holidayTypes = [['#99ccff', 'Annual unpaid'], ['#0099ff', 'Annual paid'],
    ['#006699', 'Annual paid, with overtime'], ['#9933ff', 'Science'], ['#cc99ff', 'Day for children']];

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
      this.currentUsersClientId = user.clientId;
      this.getHolidays(0,filter);
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
  getHolidays(monthsToAdd, filter): void {
    this.viewDate = new Date(this.viewDate.setMonth(this.viewDate.getMonth() + monthsToAdd));
      this.holidayService.getFilteredConfirmedHolidaysBySelectedMonth(this.viewDate, this.currentUsersId, filter).subscribe(holidays => {
        this.dataSource.data = holidays;
        this.getEvents();
      });

  }

  getEvents(): void {
    this.events = [];
    this.dataSource.data.forEach(holiday => {
      const color = this.getColor(holiday);
      const endDate = this.addDayToEndDate(holiday.toInclusive);
      const event = {'title': holiday.employeeFullName, 'start': holiday.fromInclusive, 'end': endDate, 'color': color};
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

  previousMonthButtonClick(filter) {
    this.fullCalendar.calendar.prev();
    this.updateCalendarTitle();
    this.getHolidays(-1, filter);
  }

  nextMonthButtonClick(filter) {
    this.fullCalendar.calendar.next();
    this.updateCalendarTitle();
    this.getHolidays(1, filter);
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
  isEmployee(): boolean {
    return !this.userService.isAdmin();
  }
}