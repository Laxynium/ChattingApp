import {CommonModule} from '@angular/common';
import {NgModule} from '@angular/core';
import {ReactiveFormsModule} from '@angular/forms';
import {RouterModule} from '@angular/router';
import {AuthGuard} from '../identity/guards/auth.guard';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {ProfileComponent} from './profile/components/profile/profile.component';
import {HomeComponent} from 'src/app/home/home/components/home/home.component';
import {ProfilesModule} from 'src/app/home/profile/profiles.module';

const routes = [
  {
    path: '',
    component: HomeComponent,
    canActivate: [AuthGuard],
    children: [
      {path: 'profile', component: ProfileComponent, canActivate: [AuthGuard]},
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

    ProfilesModule,
  ],
  providers: [],
})
export class HomeModule {}
