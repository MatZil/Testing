import { ValidatorFn, AbstractControl, ValidationErrors, FormGroup, AsyncValidatorFn } from '@angular/forms';
import { WeekDay } from '@angular/common';
import { timer, Observable, of } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { UserService } from 'src/app/services/user.service';

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

export function negativeOvertimeValidator(): ValidatorFn {
  return (overtimeDaysControl: AbstractControl): ValidationErrors | null => {
    const overtimeDays = overtimeDaysControl.value;
    const isNegative = overtimeDays < 0;
    return isNegative ? { 'isNegative': true } : null;
  };
}

export function exceededOvertimeValidatorAsync(userService: UserService): AsyncValidatorFn {
  return (overtimeDaysControl: AbstractControl): Observable<ValidationErrors | null> => {
    const overtimeDays = overtimeDaysControl.value;

      if (overtimeDays) {
          return timer(500).pipe(
              switchMap(() => {
                  return userService.getCurrentUser().pipe(
                      map(user => {
                          if (overtimeDays > user.overtimeDays) {
                              return { 'isExceeding': true };
                          }
                      })
                  );
              })
          );
      }
      return of(null);
  };
}
