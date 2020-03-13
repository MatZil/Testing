import { Component } from '@angular/core';
import { CalendarView } from 'angular-calendar';

@Component({
  templateUrl: 'calendar.component.html',
  styleUrls: ['calendar.component.scss']
})
export class CalendarComponent {
  view: CalendarView = CalendarView.Month;
  CalendarView = CalendarView;
  viewDate: Date = new Date();

  constructor() {}
}
