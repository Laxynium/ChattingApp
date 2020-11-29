import {
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';

export const passwordMismatch: ValidatorFn = (
  form: FormGroup
): ValidationErrors | null => {
  const password = form.get('password');
  const passwordConfirmation = form.get('passwordConfirmation');
  return password.value === passwordConfirmation.value
    ? null
    : {passwordMismatch: true};
};

export const validPassword: ValidatorFn = (
  control: FormControl
): ValidationErrors | null => {
  const passwordRegex = /^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,}$/;
  return Validators.pattern(passwordRegex)(control);
};
