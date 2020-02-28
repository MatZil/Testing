import { AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { map, switchMap } from 'rxjs/operators';
import { Observable, of, timer } from 'rxjs';
import { HolidaysService } from 'src/app/services/holidays.service';

export function freeWorkdayValidatorAsync(holidaysService: HolidaysService): AsyncValidatorFn {
    return (dateControl: AbstractControl): Observable<ValidationErrors | null> => {
        const date = dateControl.value;
        if (date) {
            return timer(500).pipe(
                switchMap(() => {
                    return holidaysService.isFreeWorkday(date).pipe(
                        map(isFreeWorkday => {
                            if (isFreeWorkday) {
                                return { 'isFreeWorkday': true };
                            }
                        })
                    );
                })
            );
        }
        return of(null);
    };
}


