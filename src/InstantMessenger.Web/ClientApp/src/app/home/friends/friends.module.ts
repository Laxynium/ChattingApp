import {CommonModule} from '@angular/common';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {StoreModule} from '@ngrx/store';
import {EffectsModule} from '@ngrx/effects';
import {FriendsComponent} from 'src/app/home/friends/components/friends/friends.component';
import {AllFriendsComponent} from './components/all-friends/all-friends.component';
import {PendingInvitationsComponent} from './components/pending-invitations/pending-invitations.component';
import {InviteFriendComponent} from './components/invite-friend/invite-friend.component';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {reducers} from 'src/app/home/friends/store/reducers';
import {
  AcceptInvitationEffect,
  CancelInvitationEffect,
  GetFriendsData,
  RejectInvitationEffect,
  RemoveFriendEffect,
  SendInvitationEffect,
} from 'src/app/home/friends/store/effects/effects';
import {FriendsService} from 'src/app/home/friends/services/friends.service';

@NgModule({
  declarations: [
    FriendsComponent,
    AllFriendsComponent,
    PendingInvitationsComponent,
    InviteFriendComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbModule,
    FormsModule,
    FontAwesomeModule,
    StoreModule.forFeature('friends', reducers),
    EffectsModule.forFeature([
      GetFriendsData,
      SendInvitationEffect,
      AcceptInvitationEffect,
      RejectInvitationEffect,
      CancelInvitationEffect,
      RemoveFriendEffect,
    ]),
  ],
  providers: [FriendsService],
})
export class FriendsModule {}
