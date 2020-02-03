import { Component, Input, forwardRef, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators, NG_VALUE_ACCESSOR, NG_VALIDATORS, ControlValueAccessor } from '@angular/forms';
import { Client } from 'src/app/models/client';
import { Role } from 'src/app/models/role';

@Component({
  selector: 'app-base-employee-form',
  templateUrl: './base-employee-form.component.html',
  styleUrls: ['./base-employee-form.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => BaseEmployeeFormComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => BaseEmployeeFormComponent),
      multi: true
    }
  ]
})

export class BaseEmployeeFormComponent implements OnInit, ControlValueAccessor, Validators {
  baseForm: FormGroup;
  @Input() clients: Client[];
  @Input() roles: Role[];

  readonly minDate = new Date(1900, 1, 1);
  readonly maxDate = new Date(2100, 1, 1);

  constructor(private formBuilder: FormBuilder) { }

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
      ]],
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

  validate(_: FormControl) {
    return this.baseForm.valid ? null : { invalidForm: { valid: false, message: 'BaseEmployeeForm fields are invalid.' } };
  }
}
