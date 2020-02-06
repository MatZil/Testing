import { AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { map, switchMap } from 'rxjs/operators';
import { Observable, of, timer } from 'rxjs';

export function uniqueEmailValidator(userService: UserService, initialEmail: string): AsyncValidatorFn {
    return (emailControl: AbstractControl): Observable<ValidationErrors | null> => {
        if (!initialEmail || initialEmail !== emailControl.value) {
            return timer(500).pipe(
                switchMap(() => {
                    return userService.emailExists(emailControl.value).pipe(
                        map(exists => {
                            if (exists) {
                                return { 'emailExists': true };
                            }
                        })
                    );
                })
            );
        }
        return of(null);
    };
}


