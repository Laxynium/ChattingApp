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
import {ForgotPasswordComponent} from './components/forgot-password/forgot-password.component';
import {ForgotPasswordEffect} from 'src/app/identity/store/effects/forgotPassword.effect';
import {ResetPasswordComponent} from './components/reset-password/reset-password.component';
import {getMetaReducers} from 'src/app/identity/store/meta.reducers';
import {LogoutEffect} from 'src/app/identity/store/effects/logout.effect';
import {ChangeNicknameEffect} from 'src/app/identity/store/effects/changeNickname.effect';
import {UploadAvatarEffect} from 'src/app/identity/store/effects/uploadAvatar.effect';
import {GetProfileEffect} from 'src/app/identity/store/effects/getCurrentUser.effect';

const routes = [
  {path: 'sign-in', component: LoginComponent},
  {path: 'sign-up', component: RegisterComponent},
  {path: 'identity/activate', component: ActivationComponent},
  {path: 'forgot-password', component: ForgotPasswordComponent},
  {path: 'identity/reset-password', component: ResetPasswordComponent},
  // {path: '**', redirectTo: '/sign-in'},
];

@NgModule({
  declarations: [
    RegisterComponent,
    LoginComponent,
    ActivationComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
    StoreModule.forFeature('identity', reducers, {
      metaReducers: getMetaReducers(),
    }),
    EffectsModule.forFeature([
      SignUpEffect,
      ActivateEffect,
      SignInEffect,
      ForgotPasswordEffect,
      LogoutEffect,
      ChangeNicknameEffect,
      UploadAvatarEffect,
      GetProfileEffect,
    ]),
  ],
  providers: [IdentityService, PersistanceService],
})
export class IdentityModule {}
