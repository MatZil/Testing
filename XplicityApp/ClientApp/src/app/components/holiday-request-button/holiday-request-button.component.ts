import { Component, OnInit } from '@angular/core';
import { HolidayRequestFormComponent } from '../holiday-request-form/holiday-request-form.component';
import { MatDialog } from '@angular/material/dialog';
import { TableRowUserModel } from 'src/app/models/table-row-user-model';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-holiday-request-button',
  templateUrl: './holiday-request-button.component.html'
})
export class HolidayRequestButtonComponent implements OnInit {

  currentUser: TableRowUserModel;

  constructor(
    public dialog: MatDialog,
    private userService: UserService
  ) { }

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
    });
  }

  openRequestHolidayModal() {
    const dialogRef = this.dialog.open(HolidayRequestFormComponent, {
      width: '350px',
      data: {
        employeeId: this.currentUser.id,
        isParentalAvailable: this.currentUser.currentAvailableLeaves > 0 || this.currentUser.nextMonthAvailableLeaves > 0
      }
    });
  }
}
