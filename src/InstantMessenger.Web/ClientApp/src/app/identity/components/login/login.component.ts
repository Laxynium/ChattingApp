import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ToastService} from '../../../shared/toasts/toast.service';
import {select, Store} from '@ngrx/store';
import {signInAction} from '../../store/actions/signIn.actions';
import {Observable} from 'rxjs';
import {isSubmittingSelector, currentUser} from '../../store/selectors';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  isSubmitting$: Observable<boolean>;
  constructor(
    private fb: FormBuilder,
    private store: Store,
    private route: ActivatedRoute,
    private router: Router
  ) {
    store.pipe(select(currentUser)).subscribe((x) => {
      if (x) {
        this.router.navigateByUrl('/');
      }
    });
  }
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
      password: ['', Validators.required],
    });
  }
  onSubmit() {
    const data = this.form.value;
    this.store.dispatch(
      signInAction({
        request: {
          email: data.email,
          password: data.password,
        },
      })
    );
  }
  get email() {
    return this.form.get('email');
  }
  get password() {
    return this.form.get('password');
  }
}
