import {Component, OnInit} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import {select, Store} from '@ngrx/store';
import * as R from 'ramda';
import {Observable} from 'rxjs';
import {signUpAction} from 'src/app/identity/store/actions/signUp.actions';
import {isSubmittingSelector} from 'src/app/identity/store/selectors';
import {passwordMismatch} from 'src/app/identity/validators/password.validators';
import {validPassword} from '../../validators/password.validators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.sass'],
})
export class RegisterComponent implements OnInit {
  form: FormGroup;
  isSubmitting$: Observable<boolean>;
  constructor(private fb: FormBuilder, private store: Store) {}

  ngOnInit(): void {
    this.initializeForm();
    this.initializeValues();
  }
  initializeValues(): void {
    this.isSubmitting$ = this.store.pipe(select(isSubmittingSelector));
  }
  initializeForm(): void {
    this.form = this.fb.group(
      {
        email: [
          '',
          Validators.compose([Validators.required, Validators.email]),
        ],
        password: [
          '',
          Validators.compose([Validators.required, validPassword]),
        ],
        passwordConfirmation: [
          '',
          Validators.compose([Validators.required, validPassword]),
        ],
      },
      {validators: passwordMismatch}
    );
  }
  onSubmit(): void {
    const payload: any = R.omit(['passwordConfirmation'], this.form.value);
    this.store.dispatch(signUpAction({request: payload}));
  }
  get email() {
    return this.form.get('email');
  }
  get password() {
    return this.form.get('password');
  }
  get passwordConfirmation() {
    return this.form.get('passwordConfirmation');
  }
}
