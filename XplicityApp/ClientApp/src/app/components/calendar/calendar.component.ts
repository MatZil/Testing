import { Component } from '@angular/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';

@Component({
  templateUrl: 'calendar.component.html',
  styleUrls: ['calendar.component.scss']
})
export class CalendarComponent {
  viewDate: Date = new Date();

  events: object[];
  options: any;

  constructor() { }

  ngOnInit() {
    this.events = [
      {
          "title": "Intern day off",
          "start": "2020-02-28"
      },
      {
          "title": "Worker vacation",
          "start": "2020-03-02",
          "end": "2020-03-04"
      },
      {
          "title": "Intern day off",
          "start": "2020-03-12"
      },
      {
          "title": "Intern day off",
          "start": "2020-03-18"
      },
      {
          "title": "Worker vacation",
          "start": "2020-03-23",
          "end": "2020-03-28"
      },
      {
          "title": "Worker vacation",
          "start": "2020-03-30",
          "end": "2020-04-05"
      }
      ,
      {
          "title": "Worker vacation",
          "start": "2020-03-17",
          "end": "2020-03-20"
      }
  ];
      
      this.options = {
          plugins: [dayGridPlugin, interactionPlugin],
          defaultDate: this.viewDate,
          header: {
              left: 'prev,next',
              center: 'title',
              right: 'today',
          },
          firstDay: 1,
          editable: true
      };
  }
}

