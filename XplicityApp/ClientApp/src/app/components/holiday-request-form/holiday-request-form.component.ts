import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { EditEmployeeFormComponent } from '../edit-employee-form/edit-employee-form.component';
import { NewHoliday } from 'src/app/models/new-holiday';
import { HolidayType } from 'src/app/enums/holidayType';
import { RequestHolidayData } from './request-holiday-data';
import { DatePipe } from '@angular/common';
import { fromInclusiveValidator, dateRangeValidator, weekendValidator } from './holiday-request-validators';
import { freeWorkdayValidatorAsync } from './free-workday-validator';
import { HolidaysService } from 'src/app/services/holidays.service';

@Component({
  selector: 'app-holiday-request-form',
  templateUrl: './holiday-request-form.component.html',
  styleUrls: ['./holiday-request-form.component.css']
})
export class HolidayRequestFormComponent implements OnInit {
  requestHolidayForm: FormGroup;
  newHoliday: NewHoliday = new NewHoliday();

  constructor(
    private holidaysService: HolidaysService,
    private formBuilder: FormBuilder,
    private datePipe: DatePipe,
    public dialogRef: MatDialogRef<EditEmployeeFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RequestHolidayData) { }

  ngOnInit() {
    this.setDefaultValues();
    this.initializeFormGroup();
  }

  setDefaultValues() {
    this.newHoliday.paid = false;
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
      overtimeDays: [],
      paid: [this.newHoliday.paid]
    }, { validators: dateRangeValidator() });
    this.requestHolidayForm.controls.type.valueChanges.subscribe(type => {
      if (type === HolidayType.Annual) {
        this.requestHolidayForm.controls.paid.setValue(true);
      } else {
        this.requestHolidayForm.controls.paid.setValue(false);
      }
    });
  }

  canBePaid(): boolean {
    return this.requestHolidayForm.controls.type.value === HolidayType.Annual;
  }

  isPaid(): boolean {
    return this.requestHolidayForm.controls.paid.value;
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
}
