import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { HolidaysService } from 'src/app/services/holidays.service';
import { Holiday } from 'src/app/models/holiday';
import { UserService } from '../../services/user.service';

import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import 'rxjs/add/operator/map';
import { HolidayType } from 'src/app/enums/holidayType';

@Component({
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})

export class CalendarComponent implements OnInit, AfterViewInit {
  viewDate: Date = new Date();
  events: any[];
  prevButton: any;
  nextButton: any;
  options: any;
  currentUserId: number;
  dataSource = new MatTableDataSource<Holiday>();
  colors = [['#B7CEF5', 'Annual unpaid'], ['#88B0F5', 'Annual paid'], ['#547EC8', 'Annual paid, with overtime'], ['#BDA1EA', 'Science'], ['#DBC7FC', 'Day for children']];
  
      @ViewChild('fullCalendar') fullCalendar: any;
              
      constructor(
        private holidayService: HolidaysService,
        private userService: UserService
      ) {}

      ngOnInit() {
        this.setCalendarOptions();
        this.getUserAndCurrentMonthHolidays();
      }

      getUserAndCurrentMonthHolidays() {
        this.userService.getCurrentUser().subscribe(user => {
          let self = this;
          setTimeout(function() {
            self.currentUserId = user.id;
            self.getHolidays(0);
          }, 500);
        });
      }

      setCalendarOptions() {
        this.options = {
          plugins: [dayGridPlugin, interactionPlugin],
          defaultDate: new Date(),
          header: {
              left: 'prev',
              center: 'title',
              right: 'next'
          },
          titleFormat: {
            month: '2-digit',
            year: 'numeric'
          },
          editable: true,
          firstDay: 1,
          displayEventTime: false,
          eventClick: function(info) {
            console.log(info.event.start);
          }
        };
      }

      getHolidays(monthsToAdd): void {
        this.viewDate = new Date(this.viewDate.setMonth(this.viewDate.getMonth() + monthsToAdd));
    
          this.holidayService.getConfirmedHolidaysBySelectedMonth(this.viewDate, this.currentUserId).subscribe(holidays => {
            this.dataSource.data = holidays;
            this.getEvents();
            });
      }

      getEvents(): void {
        this.events = [];
        this.dataSource.data.forEach(holiday => {
          let color = this.getColor(holiday);
          let endDate = this.addDayToEndDate(holiday.toInclusive);
          
         let event = {"title" : holiday.employeeFullName, "start": holiday.fromInclusive, "end": endDate, "color": color};
         this.events.push(event);
       });
     }

     getColor(holiday)
     {
        let color : string;

        if (holiday.type  == HolidayType.Annual && holiday.paid && holiday.overtimeDays > 0)
        {
            color = this.colors[2][0];
        }
        else if (holiday.type == HolidayType.Annual && holiday.paid)
        {
            color = this.colors[1][0];
        }
        else if (holiday.type == HolidayType.Unpaid)
        {
            color = this.colors[0][0];
        }
        else if (holiday.type == HolidayType.Science)
        {
            color = this.colors[3][0];
        }
        else if (holiday.type == HolidayType.DayForChildren)
        {
            color = this.colors[4][0];
        }

        return color;
    }

     addDayToEndDate(toInclusive)
     {
      var startDate = new Date(toInclusive);
      var day = 60 * 60 * 24 * 1000;
      return new Date(startDate.getTime() + day);
     }
  
    defineEventFunctions()
    {
          this.nextButton[0].addEventListener('click', ()=>{
              this.defineEventFunctions();
              this.getHolidays(1);
          });
  
          this.prevButton[0].addEventListener('click', ()=>{
            this.defineEventFunctions();
            this.getHolidays(-1);
        });
    }
  
      ngAfterViewInit() {
          this.bindEvents();
      }
  
      bindEvents() {
        console.log(this.viewDate);
        this.nextButton = this.fullCalendar.el.nativeElement.getElementsByClassName("fc-next-button");
        this.prevButton = this.fullCalendar.el.nativeElement.getElementsByClassName("fc-prev-button");
   
        setTimeout(() => {
         this.defineEventFunctions();
       }, 100);
      }
  }