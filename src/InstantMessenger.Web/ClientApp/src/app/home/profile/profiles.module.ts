import {CommonModule} from '@angular/common';
import {NgModule} from '@angular/core';
import {ReactiveFormsModule} from '@angular/forms';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {ProfileService} from 'src/app/home/profile/services/profile.service';
import {StoreModule} from '@ngrx/store';
import {reducers} from 'src/app/home/profile/store/reducers';
import {EffectsModule} from '@ngrx/effects';
import {UploadAvatarEffect} from 'src/app/home/profile/store/effects/uploadAvatar.effect';
import {ProfileComponent} from 'src/app/home/profile/components/profile/profile.component';
import {GetProfileEffect} from 'src/app/home/profile/store/effects/getProfile.effect';

@NgModule({
  declarations: [ProfileComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbModule,
    StoreModule.forFeature('profiles', reducers),
    EffectsModule.forFeature([UploadAvatarEffect, GetProfileEffect]),
  ],
  providers: [ProfileService],
})
export class ProfilesModule {}
