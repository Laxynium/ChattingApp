import {CommonModule} from '@angular/common';
import {NgModule} from '@angular/core';
import {ReactiveFormsModule} from '@angular/forms';
import {RouterModule} from '@angular/router';
import {AuthGuard} from '../identity/guards/auth.guard';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {ProfileComponent} from './profile/components/profile/profile.component';
import {HomeComponent} from 'src/app/home/home/components/home/home.component';
import {ProfilesModule} from 'src/app/home/profile/profiles.module';
import {FriendsComponent} from './friends/components/friends/friends.component';
import {FriendsModule} from 'src/app/home/friends/friends.module';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {HubService} from 'src/app/shared/services/hub.service';

const routes = [
  {
    path: '',
    component: HomeComponent,
    canActivate: [AuthGuard],
    children: [
      {path: '', component: ProfileComponent, canActivate: [AuthGuard]},
      {path: 'profile', component: ProfileComponent, canActivate: [AuthGuard]},
      {path: 'friends', component: FriendsComponent, canActivate: [AuthGuard]},
    ],
  },
  {path: '**', redirectTo: '/'},
];

@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
    NgbModule,
    FontAwesomeModule,

    ProfilesModule,
    FriendsModule,
  ],
  providers: [HubService],
})
export class HomeModule {}
