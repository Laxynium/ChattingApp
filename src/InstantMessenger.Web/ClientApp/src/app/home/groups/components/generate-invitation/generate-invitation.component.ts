import {Component, OnInit} from '@angular/core';
import {FormGroup, FormBuilder} from '@angular/forms';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {Store, select} from '@ngrx/store';
import {Observable} from 'rxjs';
import {first} from 'rxjs/operators';
import {generateInvitationAction} from 'src/app/home/groups/store/actions';
import {
  currentInvitationSelector,
  currentGroupSelector,
} from 'src/app/home/groups/store/selectors';
import {
  ExpirationTimeType,
  UsageCounterType,
} from 'src/app/home/groups/store/types/invitation';

@Component({
  selector: 'app-generate-invitation',
  templateUrl: './generate-invitation.component.html',
  styleUrls: ['./generate-invitation.component.scss'],
})
export class GenerateInvitationComponent implements OnInit {
  ExpirationTimeType = ExpirationTimeType;
  UsageCounterType = UsageCounterType;
  form: FormGroup;
  expirationTime = {hour: 1, minute: 0};
  $invitation: Observable<{code: string; isBeingGenerated: boolean}>;
  constructor(
    public modal: NgbActiveModal,
    private fb: FormBuilder,
    private store: Store
  ) {
    this.form = this.fb.group({
      expirationTimeType: [ExpirationTimeType.INFINITE],
      expirationTime: [this.expirationTime],
      usageType: [UsageCounterType.INFINITE],
      usage: [1],
    });
    this.$invitation = this.store.select(currentInvitationSelector);
  }
  ngOnInit(): void {}

  generateInvitation() {
    const formValue = this.form.value;
    console.log(formValue);
    const expirationTime =
      formValue.expirationTimeType == ExpirationTimeType.INFINITE
        ? {type: formValue.expirationTimeType, period: null}
        : {
            type: formValue.expirationTimeType,
            period: `${formValue.expirationTime.hour}:${formValue.expirationTime.minute}`,
          };
    const usageCounter =
      formValue.usageType == UsageCounterType.INFINITE
        ? {type: formValue.usageType, times: null}
        : {
            type: formValue.usageType,
            times: formValue.usage,
          };
    this.store.pipe(select(currentGroupSelector), first()).subscribe((g) => {
      this.store.dispatch(
        generateInvitationAction({
          groupId: g.groupId,
          expirationTime: expirationTime,
          usageCounter: usageCounter,
        })
      );
    });
  }

  get expirationTimeType() {
    return this.form.get('expirationTimeType');
  }
  get usageType() {
    return this.form.get('usageType');
  }
}
