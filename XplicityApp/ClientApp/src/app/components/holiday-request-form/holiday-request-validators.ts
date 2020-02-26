import { ValidatorFn, AbstractControl, ValidationErrors, FormGroup } from '@angular/forms';
import { WeekDay } from '@angular/common';

export function fromInclusiveValidator(): ValidatorFn {
  return (fromInclusiveControl: AbstractControl): ValidationErrors | null => {
    const isTodayOrEarlier = fromInclusiveControl.value && fromInclusiveControl.value <= new Date();
    return isTodayOrEarlier ? { 'isTodayOrEarlier': true } : null;
  };
}

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
