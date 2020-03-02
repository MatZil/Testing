import { ValidatorFn, AbstractControl, ValidationErrors, FormGroup } from '@angular/forms';
import { WeekDay } from '@angular/common';

export function fromInclusiveValidator(): ValidatorFn {
  return (fromInclusiveControl: AbstractControl): ValidationErrors | null => {
    var today = new Date();
    var yesterday = addDays(today, -1);
    const isEarlierThanToday = fromInclusiveControl.value && fromInclusiveControl.value <= yesterday;
    return isEarlierThanToday ? { 'isEarlierThanToday': true } : null;
  };
}

var addDays = function (date, days) {
  var daysInMilliseconds = days * 24 * 60 * 60 * 1000;
  return new Date(date.getTime() + daysInMilliseconds);
};

export function dateRangeValidator(): ValidatorFn {
  return (holidayFormGroup: FormGroup): ValidationErrors | null => {
    const fromInclusive = holidayFormGroup.controls.fromInclusive.value;
    const toInclusive = holidayFormGroup.controls.toInclusive.value;
    const isFromLaterThanTo = fromInclusive && toInclusive && fromInclusive > toInclusive;
    return isFromLaterThanTo ? { 'isFromLaterThanTo': true } : null;
  };
}

export function weekendValidator(): ValidatorFn {
  return (dateControl: AbstractControl): ValidationErrors | null => {
    if (!dateControl.value) {
      return null;
    }
    const day = dateControl.value.getDay();
    const isWeekend = day === WeekDay.Saturday || day === WeekDay.Sunday;
    return isWeekend ? { 'isWeekend': true } : null;
  };
}
