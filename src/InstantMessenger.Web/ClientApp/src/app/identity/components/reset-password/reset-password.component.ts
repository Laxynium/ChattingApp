import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {resetPasswordAction} from 'src/app/identity/store/actions/forgotPassword.actions';
import {isSubmittingSelector} from 'src/app/identity/store/selectors';
import {ResetPasswordRequestInterface} from '../../store/types/resetPasswordRequest.interface';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {
  validPassword,
  passwordMismatch,
} from 'src/app/identity/validators/password.validators';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent implements OnInit {
  form: FormGroup;
  resetPassword: ResetPasswordRequestInterface;
  isSubmitting$: Observable<boolean>;
  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private store: Store
  ) {
    this.initializeForm();
    this.initializeValues();
  }

  initializeValues() {
    this.isSubmitting$ = this.store.pipe(select(isSubmittingSelector));
  }

  initializeForm() {
    this.form = this.fb.group(
      {
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
  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.resetPassword = {
        userId: params.userId,
        token: params.token,
        password: null,
      };
    });
  }
  onSubmit() {
    const password = this.form.value.password;
    const payload = {...this.resetPassword, password: password};
    this.store.dispatch(resetPasswordAction({request: payload}));
  }
  get password() {
    return this.form.get('password');
  }
  get passwordConfirmation() {
    return this.form.get('passwordConfirmation');
  }
}
