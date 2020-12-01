import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {sendInvitationAction} from 'src/app/home/friends/store/actions';
import {isSubmittingSelector} from 'src/app/home/friends/store/selectors';

@Component({
  selector: 'app-invite-friend',
  templateUrl: './invite-friend.component.html',
  styleUrls: ['./invite-friend.component.scss'],
})
export class InviteFriendComponent implements OnInit {
  form: FormGroup;
  $isSubmitting: Observable<boolean>;
  constructor(private store: Store, private fb: FormBuilder) {
    this.$isSubmitting = this.store.pipe(select(isSubmittingSelector));
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      nickname: ['', Validators.required],
    });
  }

  sendInvitation() {
    const nickname = this.form.value.nickname;
    this.store.dispatch(sendInvitationAction({nickname: nickname}));
  }
}
