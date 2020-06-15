import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EditEmployeeFormComponent } from '../edit-employee-form/edit-employee-form.component';
import { NewHoliday } from 'src/app/models/new-holiday';
import { HolidayType } from 'src/app/enums/holidayType';
import { RequestHolidayData } from './request-holiday-data';
import { DatePipe } from '@angular/common';
import { fromInclusiveValidator, dateRangeValidator, weekendValidator,
         negativeOvertimeValidator, exceededOvertimeValidatorAsync } from './holiday-request-validators';
import { freeWorkdayValidatorAsync } from './free-workday-validator';
import { HolidaysService } from 'src/app/services/holidays.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-holiday-request-form',
  templateUrl: './holiday-request-form.component.html',
  styleUrls: ['./holiday-request-form.component.css']
})
export class HolidayRequestFormComponent implements OnInit {
  requestHolidayForm: FormGroup;
  newHoliday: NewHoliday = new NewHoliday();
  currentUserOvertimeDays: number;

  constructor(
    private holidaysService: HolidaysService,
    private userService: UserService,
    private formBuilder: FormBuilder,
    private datePipe: DatePipe,
    public dialogRef: MatDialogRef<EditEmployeeFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RequestHolidayData) { }

  ngOnInit() {
    this.getCurrentUserOvertimeDays();
    this.setDefaultValues();
    this.initializeFormGroup();
  }

  getCurrentUserOvertimeDays(): void {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUserOvertimeDays = user.overtimeDays;
    });
  }

  setDefaultValues() {
    this.newHoliday.type = HolidayType.Annual;
  }

  initializeFormGroup() {
    this.requestHolidayForm = this.formBuilder.group({
      type: [this.newHoliday.type],
      fromInclusive: ['', [
        Validators.required,
        fromInclusiveValidator(),
        weekendValidator()
      ], freeWorkdayValidatorAsync(this.holidaysService)],
      toInclusive: ['', [
        Validators.required,
        weekendValidator()
      ], freeWorkdayValidatorAsync(this.holidaysService)],
      overtimeDays: ['', [
         negativeOvertimeValidator()
        ], exceededOvertimeValidatorAsync(this.userService)],
      paid: [this.newHoliday.paid]
    }, { validators: dateRangeValidator() });
  }

  canBePaid(): boolean {
    return this.requestHolidayForm.controls.type.value === HolidayType.Annual;
  }

  onSubmit() {
    const newHoliday = this.getFormHoliday();

    this.dialogRef.close(newHoliday);
  }

  getFormHoliday(): NewHoliday {
    const formHoliday = Object.assign(this.requestHolidayForm.value);
    formHoliday.employeeId = this.data.employeeId;
    formHoliday.fromInclusive = this.datePipe.transform(formHoliday.fromInclusive, 'yyyy-MM-dd');
    formHoliday.toInclusive = this.datePipe.transform(formHoliday.toInclusive, 'yyyy-MM-dd');

    if (!formHoliday.overtimeDays) {
      formHoliday.overtimeDays = 0;
    }

    return formHoliday;
  }

  getOvertimeDaysErrorMessage(): string {
    return this.requestHolidayForm.controls.overtimeDays.errors?.isNegative ? 'Cannot be a negative number.' :
      this.requestHolidayForm.controls.overtimeDays.errors?.isExceeding ? 'Cannot exceed the available amount.' :
        '';
  }

  getFromInclusiveErrorMessage(): string {
    return this.requestHolidayForm.controls.fromInclusive.errors?.required ? ' Date cannot be empty.' :
      this.requestHolidayForm.controls.fromInclusive.errors?.isEarlierThanToday ? 'From date cannot be earlier than today.' :
        this.requestHolidayForm.controls.fromInclusive.errors?.isWeekend ? 'From date can not be a weekend.' :
          this.requestHolidayForm.controls.fromInclusive.errors?.isFreeWorkday ? 'From date can not be a public holiday.' :
            '';
  }

  getToInclusiveErrorMessage(): string {
    return this.requestHolidayForm.controls.toInclusive.errors?.required ? ' Date cannot be empty.' :
      this.requestHolidayForm.controls.toInclusive.errors?.isWeekend ? 'To date can not be a weekend.' :
        this.requestHolidayForm.controls.toInclusive.errors?.isFreeWorkday ? 'To date can not be a public holiday.' :
          '';
  }
}
