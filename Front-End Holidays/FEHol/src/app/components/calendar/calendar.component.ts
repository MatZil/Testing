import { Component } from '@angular/core';

import { ScheduleComponent, WeekService, MonthService, AgendaService, TimelineViewsService,
         TimelineMonthService, EventSettingsModel, GroupModel } from '@syncfusion/ej2-angular-schedule';

@Component({ templateUrl: 'calendar.component.html', styleUrls: ['calendar.component.scss'] })

export class CalendarComponent {
  date = new Date();
  mode = 'month';

  constructor() { }

  panelChange(change: { date: Date; mode: string }): void {
    console.log(change.date, change.mode);
  }
}
