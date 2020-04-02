import { Component, OnInit, ViewChild } from '@angular/core';
import { Holiday } from '../../models/holiday';
import { HolidaysService } from '../../services/holidays.service';
import { TableRowUserModel } from '../../models/table-row-user-model';
import { UserService } from '../../services/user.service';
import { EnumToStringConverterService } from 'src/app/services/enum-to-string-converter.service';
import { AuthenticationService } from '../../services/authentication.service';
import { MatDialog } from '@angular/material/dialog';
import { HolidayStatus } from 'src/app/enums/holidayStatus';
import { EmployeeStatus } from 'src/app/models/employee-status.enum';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

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

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(
    private userService: UserService,
    private holidayService: HolidaysService,
    private authenticationService: AuthenticationService,
    public enumConverter: EnumToStringConverterService
  ) { }

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
    });
    this.refreshTable(this.selectedEmployeeStatus);
    this.dataSource.paginator = this.paginator;
    if (this.isAdmin()) {
      this.displayedColumns = [
        'employee',
        'holidaysType',
        'dateFrom',
        'dateTo',
        'overtimeHours',
        'status',
        'rejectedConfirmed',
        'creationDate',
        'action'];
    }
    else {
      this.displayedColumns = [
        'holidaysType',
        'dateFrom',
        'dateTo',
        'overtimeDays',
        'status',
        'rejectedConfirmed',
        'creationDate',
        'action'];
    }
  }

  refreshTable(status: EmployeeStatus) {
    this.holidayService.getHolidaysByStatus(status).subscribe(holidays => {
      this.holidays = holidays;
      this.dataSource = new MatTableDataSource<Holiday>(this.getHolidaysByRole());
      this.dataSource.paginator = this.paginator;
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

  abandonHoliday(holiday: Holiday): void {
    holiday.status = HolidayStatus.Abandoned;
    this.holidayService.updateHoliday(holiday.id, holiday).subscribe(() => {
      this.refreshTable(this.selectedEmployeeStatus);
    });
  }

  allowedToAbandon(holiday: Holiday): boolean {
    if (holiday.employeeId == this.authenticationService.getUserId()) {
      if (holiday.status == HolidayStatus.Pending) {
        return false;
      }
      else if (holiday.status == HolidayStatus.Abandoned) {
        return true;
      }
      else {
        return true;
      }
    }
    else {
      return true;
    }
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

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  updateHolidaysTable() {
    this.refreshTable(this.selectedEmployeeStatus);
  }
}
