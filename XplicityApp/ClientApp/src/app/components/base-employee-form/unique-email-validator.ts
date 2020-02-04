import { AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { debounceTime, map } from 'rxjs/operators';
import { Observable } from 'rxjs';

export function uniqueEmailValidator(userService: UserService): AsyncValidatorFn {
    return (emailControl: AbstractControl): Observable<ValidationErrors | null> => {
        return userService.emailExists(emailControl.value).pipe(
            debounceTime(500),
            map(exists => {
                return exists ? { 'emailExists': true } : null;
            })
        );
    };
}


