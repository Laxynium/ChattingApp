import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {Store, select} from '@ngrx/store';
import {FormGroup, FormBuilder, Validators} from '@angular/forms';
import {Observable} from 'rxjs';
import {isSubmittingSelector} from '../../store/selectors';
import {ActivateRequestInterface} from 'src/app/identity/types/activateRequest.interface';
import {activateAction} from 'src/app/identity/store/actions/activate.actions';

@Component({
  selector: 'app-login',
  templateUrl: './activation.component.html',
  styleUrls: ['./activation.component.scss'],
})
export class ActivationComponent implements OnInit {
  activation: ActivateRequestInterface;
  form: FormGroup;
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
    this.form = this.fb.group({
      nickname: ['', Validators.required],
    });
  }
  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.activation = {
        userId: params.userId,
        token: params.token,
        nickname: null,
      };
    });
  }
  onActivate() {
    const nickname = this.form.value.nickname;
    const payload: any = {...this.activation, nickname};
    this.store.dispatch(activateAction({request: payload}));
  }

  get nickname() {
    return this.form.get('nickname');
  }
}
