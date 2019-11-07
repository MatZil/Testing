import { AbstractControl } from '@angular/forms';

export function passwordMatcherValidatorFn(control: AbstractControl): { [key: string]: boolean } {
    const password = control.get('newPassword');
    const confirm = control.get('passwordConfirm');
    if (!password || !confirm) { return null; }
    return password.value === confirm.value ? null : { nomatch: true };
}
