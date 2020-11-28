import {CommonModule} from '@angular/common';
import {NgModule} from '@angular/core';
import {ReactiveFormsModule} from '@angular/forms';
import {RouterModule} from '@angular/router';
import {EffectsModule} from '@ngrx/effects';
import {StoreModule} from '@ngrx/store';

import {LoginComponent} from './components/login/login.component';
import {RegisterComponent} from './components/register/register.component';
import {IdentityService} from './services/identity.service';
import {reducers} from './store/reducers';
import {SignUpEffect} from 'src/app/identity/store/effects/signUp.effect';

const routes = [
  {path: 'sign-in', component: LoginComponent},
  {path: 'sign-up', component: RegisterComponent},
  {path: '**', redirectTo: '/sign-in'},
];

@NgModule({
  declarations: [RegisterComponent, LoginComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
    StoreModule.forFeature('identity', reducers),
    EffectsModule.forFeature([SignUpEffect]),
  ],
  providers: [IdentityService],
})
export class IdentityModule {}
