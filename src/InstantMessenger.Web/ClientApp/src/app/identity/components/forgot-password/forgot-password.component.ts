import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {isSubmittingSelector} from 'src/app/identity/store/selectors';
import {forgotPasswordAction} from '../../store/actions/forgotPassword.actions';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss'],
})
export class ForgotPasswordComponent implements OnInit {
  form: FormGroup;
  isSubmitting$: Observable<boolean>;
  constructor(private fb: FormBuilder, private store: Store) {}
  ngOnInit(): void {
    this.initializeForm();
    this.initializeValues();
  }
  initializeValues() {
    this.isSubmitting$ = this.store.pipe(select(isSubmittingSelector));
  }
  initializeForm() {
    this.form = this.fb.group({
      email: ['', Validators.compose([Validators.required, Validators.email])],
    });
  }
  get email() {
    return this.form.get('email');
  }
  onSubmit() {
    const data = this.form.value;
    this.store.dispatch(forgotPasswordAction({request: {email: data.email}}));
  }
}
