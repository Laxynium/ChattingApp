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
import {ActivationComponent} from 'src/app/identity/components/activation/activation.component';
import {ActivateEffect} from 'src/app/identity/store/effects/activate.effect';
import {SignInEffect} from 'src/app/identity/store/effects/signIn.effect';
import {PersistanceService} from '../shared/services/persistance.service';

const routes = [
  {path: 'sign-in', component: LoginComponent},
  {path: 'sign-up', component: RegisterComponent},
  {path: 'identity/activate', component: ActivationComponent},
  {path: '**', redirectTo: '/sign-in'},
];

@NgModule({
  declarations: [RegisterComponent, LoginComponent, ActivationComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
    StoreModule.forFeature('identity', reducers),
    EffectsModule.forFeature([SignUpEffect, ActivateEffect, SignInEffect]),
  ],
  providers: [IdentityService, PersistanceService],
})
export class IdentityModule {}
