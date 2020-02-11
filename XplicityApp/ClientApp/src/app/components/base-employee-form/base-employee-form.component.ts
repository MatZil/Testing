import { Component, Input, forwardRef, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { Client } from 'src/app/models/client';
import { Role } from 'src/app/models/role';
import { UserService } from 'src/app/services/user.service';
import { uniqueEmailValidator } from './unique-email-validator';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-base-employee-form',
  templateUrl: './base-employee-form.component.html',
  styleUrls: ['./base-employee-form.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => BaseEmployeeFormComponent),
      multi: true
    }
  ],
  exportAs: 'baseEmployeeComponent'
})

export class BaseEmployeeFormComponent implements OnInit, ControlValueAccessor {
  baseForm: FormGroup;
  @Input() clients: Client[];
  @Input() roles: Role[];
  @Input() initialEmail?: string;

  readonly minDate = new Date(1900, 1, 1);
  readonly maxDate = new Date(2100, 1, 1);

  constructor(
    private formBuilder: FormBuilder,
    private datePipe: DatePipe,
    private userService: UserService) { }

  ngOnInit() {
    this.initializeFormGroup();
  }

  initializeFormGroup() {
    this.baseForm = this.formBuilder.group({
      name: ['', [
        Validators.required,
        Validators.pattern('[a-zA-ZąčęėįšųūžĄČĘĖĮŠŲŪŽ]*')
      ]],
      surname: ['', [
        Validators.required,
        Validators.pattern('[a-zA-ZąčęėįšųūžĄČĘĖĮŠŲŪŽ]*')
      ]],
      email: ['', [
        Validators.required,
        Validators.email
      ], uniqueEmailValidator(this.userService, this.initialEmail)],
      position: ['', [
        Validators.required
      ]],
      worksFromDate: ['', [
        Validators.required
      ]],
      birthdayDate: ['', [
        Validators.required
      ]],
      healthCheckDate: ['', [
        Validators.required
      ]],
      clientId: [],
      role: [],
      daysOfVacation: [],
      parentalLeaveLimit: []
    });
    this.stripTimeFromDates();
  }

  stripTimeFromDates() {
    this.baseForm.controls.worksFromDate.valueChanges.subscribe(value => {
      this.baseForm.controls.worksFromDate.setValue(this.datePipe.transform(value, 'yyyy-MM-dd'), { emitEvent: false });
    });
    this.baseForm.controls.birthdayDate.valueChanges.subscribe(value => {
      this.baseForm.controls.birthdayDate.setValue(this.datePipe.transform(value, 'yyyy-MM-dd'), { emitEvent: false });
    });
    this.baseForm.controls.healthCheckDate.valueChanges.subscribe(value => {
      this.baseForm.controls.healthCheckDate.setValue(this.datePipe.transform(value, 'yyyy-MM-dd'), { emitEvent: false });
    });
  }

  writeValue(value: any) {
    if (value) {
      this.baseForm.patchValue(value);
    }
  }

  registerOnChange(fn: any) {
    this.baseForm.valueChanges.subscribe(fn);
  }

  registerOnTouched(fn: any) { }
}
