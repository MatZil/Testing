import { Component, OnInit, ViewChild } from '@angular/core';
import { Holiday } from '../../models/holiday';
import { NewHoliday } from '../../models/new-holiday';
import { HolidaysService } from '../../services/holidays.service';
import { TableRowUserModel } from '../../models/table-row-user-model';
import { UserService } from '../../services/user.service';
import { EnumToStringConverterService } from 'src/app/services/enum-to-string-converter.service';
import { MatDialog } from '@angular/material';
import { HolidayRequestFormComponent } from '../holiday-request-form/holiday-request-form.component';
import { HolidayStatus } from 'src/app/enums/holidayStatus';
import { EmployeeStatus } from 'src/app/models/employee-status.enum';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';

@Component({
  selector: 'app-holidays-table',
  templateUrl: './holidays-table.component.html',
  styleUrls: ['./holidays-table.component.scss']
})
export class HolidaysTableComponent implements OnInit {
  holidays: Holiday[] = [];
  selectedEmployeeStatus: EmployeeStatus = EmployeeStatus.Current;
  displayedColumns: string[];
  currentUser: TableRowUserModel;
  dataSource = new MatTableDataSource<Holiday>(this.getHolidaysByRole());

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  constructor(
    private userService: UserService,
    private holidayService: HolidaysService,
    public enumConverter: EnumToStringConverterService,
    public dialog: MatDialog) { }

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
    });
    this.refreshTable(this.selectedEmployeeStatus);
    this.dataSource.paginator = this.paginator;
    if(this.isAdmin()){
      this.displayedColumns = [
        'employee', 
        'holidaysType', 
        'paid', 
        'dateFrom', 
        'dateTo', 
        'overtimeHours', 
        'status', 
        'rejectedConfirmed', 
        'creationDate'
      ];
    }
    else {
      this.displayedColumns = [
        'holidaysType', 
        'paid', 
        'dateFrom', 
        'dateTo', 
        'overtimeDays', 
        'status', 
        'rejectedConfirmed', 
        'creationDate'
      ];
    }
  }


  

  refreshTable(status: EmployeeStatus) {
    this.holidayService.getHolidaysByStatus(status).subscribe(holidays => {
      this.holidays = holidays;
      this.dataSource = new MatTableDataSource<Holiday>(this.getHolidaysByRole());
      this.dataSource.paginator = this.paginator;
    });
  }

  addHoliday(newHoliday: NewHoliday) {
    this.holidayService.addHoliday(newHoliday).subscribe(() => {
      this.refreshTable(this.selectedEmployeeStatus);
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
    dialogRef.afterClosed().subscribe(newHoliday => {
      if (newHoliday) {
        this.addHoliday(newHoliday);
      }
    });
  }

  getHolidaysByRole(): Holiday[] {
    const currentEmployeeHolidays = [];
    if (!this.isAdmin()) {
      this.holidays.forEach(holiday => {
        if (this.belongsToCurrentUser(holiday)) {
          currentEmployeeHolidays.push(holiday);
        }
      });

      return currentEmployeeHolidays;
    }
    return this.holidays;
  }

  belongsToCurrentUser(holiday: Holiday) {
    if (this.currentUser.id === holiday.employeeId) {
      return true;
    }
  }

  getStatusName(status: HolidayStatus): string {
    return this.enumConverter.determineHolidayStatus(status);
  }

  getStatusColor(status: HolidayStatus): string {
    return this.enumConverter.determineHolidayStatusColor(status);
  }

  getOvertime(holiday: Holiday): number | string {
    if (!holiday.paid) {
      return '-';
    } else if (this.isAdmin()) {
      return holiday.overtimeHours;
    }
    return holiday.overtimeDays;
  }

  paidToString(paid: boolean): string {
    if (paid) {
      return 'Yes';
    } else {
      return 'No';
    }
  }

  isAdmin(): boolean {
    return this.userService.isAdmin();
  }
}
