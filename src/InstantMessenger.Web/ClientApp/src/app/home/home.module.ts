import {CommonModule} from '@angular/common';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {RouterModule} from '@angular/router';
import {AuthGuard} from './home/guards/auth.guard';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {ProfileComponent} from './profile/components/profile/profile.component';
import {HomeComponent} from 'src/app/home/home/components/home/home.component';
import {ProfilesModule} from 'src/app/home/profile/profiles.module';
import {FriendsComponent} from './friends/components/friends/friends.component';
import {FriendsModule} from 'src/app/home/friends/friends.module';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {HubService} from 'src/app/shared/services/hub.service';
import {ConversationsModule} from 'src/app/home/conversations/privateMessages.module';
import {ConversationsComponent} from 'src/app/home/conversations/components/conversations/conversations.component';
import {ConversationComponent} from 'src/app/home/conversations/components/conversation/conversation.component';
import {GroupsComponent} from './groups/components/groups/groups.component';
import {GroupsModule} from 'src/app/home/groups/groups.module';
import {GroupComponent} from 'src/app/home/groups/components/group/group.component';
import {ChannelComponent} from 'src/app/home/groups/components/channel/channel.component';

const routes = [
  {
    path: '',
    component: HomeComponent,
    canActivate: [AuthGuard],
    children: [
      {path: '', component: ProfileComponent, canActivate: [AuthGuard]},
      {path: 'profile', component: ProfileComponent, canActivate: [AuthGuard]},
      {path: 'friends', component: FriendsComponent, canActivate: [AuthGuard]},
      {
        path: 'conversations/:id',
        component: ConversationComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'conversations',
        component: ConversationsComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'groups/:id',
        component: GroupComponent,
        canActivate: [AuthGuard],
        children: [
          {
            path: 'channels/:channelId',
            component: ChannelComponent,
            canActivate: [AuthGuard],
          },
        ],
      },
      {
        path: 'groups',
        component: GroupsComponent,
        canActivate: [AuthGuard],
      },
    ],
  },
  {path: '**', redirectTo: '/'},
];

@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    NgbModule,
    ReactiveFormsModule,
    FormsModule,
    FontAwesomeModule,

    ProfilesModule,
    FriendsModule,
    ConversationsModule,
    GroupsModule,
  ],
  providers: [HubService],
})
export class HomeModule {}
