import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { TableRowUserModel } from 'src/app/models/table-row-user-model';
import { UserService } from 'src/app/services/user.service';
import { HolidaysService } from 'src/app/services/holidays.service';
import { MatDialog } from '@angular/material';
import { NewHoliday } from 'src/app/models/new-holiday';
import { HolidayRequestFormComponent } from '../holiday-request-form/holiday-request-form.component';

@Component({
  selector: 'app-holiday-request-button',
  templateUrl: './holiday-request-button.component.html'
})

export class HolidayRequestButtonComponent implements OnInit {

  currentUser: TableRowUserModel;
  @Output() holidayRequested: EventEmitter<any> = new EventEmitter();

  constructor(
    private userService: UserService,
    private holidayService: HolidaysService,
    public dialog: MatDialog
  ) { }

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
    });
  }

  addHoliday(newHoliday: NewHoliday) {
    this.holidayService.addHoliday(newHoliday).subscribe(
      () => {
        this.holidayRequested.emit();
      }
    );
  }

  openRequestHolidayModal() {
    const dialogRef = this.dialog.open(HolidayRequestFormComponent, {
      width: '350px',
      data: {
        employeeId: this.currentUser.id,
        isParentalAvailable: this.currentUser.currentAvailableLeaves > 0 || this.currentUser.nextMonthAvailableLeaves > 0
      }
    });
    dialogRef.afterClosed().subscribe(newHoliday => {
      if (newHoliday) {
        this.addHoliday(newHoliday);
      }
    });
  }
}
