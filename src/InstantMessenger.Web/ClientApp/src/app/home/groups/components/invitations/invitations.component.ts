import {Component, OnInit} from '@angular/core';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {first} from 'rxjs/operators';
import {} from 'src/app/home/groups/store/groups/actions';
import {
  getInvitationsAction,
  revokeInvitationAction,
} from 'src/app/home/groups/store/invitations/actions';
import {invitationsSelector} from 'src/app/home/groups/store/invitations/selectors';
import {currentGroupSelector} from 'src/app/home/groups/store/groups/selectors';
import {
  ExpirationTimeType,
  InvitationDto,
  UsageCounterType,
} from 'src/app/home/groups/store/types/invitation';

@Component({
  selector: 'app-invitations',
  templateUrl: './invitations.component.html',
  styleUrls: ['./invitations.component.scss'],
})
export class InvitationsComponent implements OnInit {
  ExpirationType = ExpirationTimeType;
  CounterType = UsageCounterType;
  $invitations: Observable<InvitationDto[]>;
  constructor(private store: Store) {
    this.$invitations = this.store.pipe(select(invitationsSelector));
  }

  ngOnInit(): void {
    this.store.pipe(select(currentGroupSelector), first()).subscribe((g) => {
      if (g) {
        this.store.dispatch(getInvitationsAction({groupId: g.groupId}));
      }
    });
  }
  revokeInvitation(groupId: string, invitationId: string) {
    this.store.dispatch(revokeInvitationAction({groupId, invitationId}));
  }
}
