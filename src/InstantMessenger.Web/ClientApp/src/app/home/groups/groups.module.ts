import {CommonModule} from '@angular/common';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {GroupsComponent} from 'src/app/home/groups/components/groups/groups.component';
import {BrowserModule} from '@angular/platform-browser';
import {CreateGroupModal} from 'src/app/home/groups/components/groups/create-group.modal';
import {JoinGroupModal} from 'src/app/home/groups/components/groups/join-group.modal';
import {StoreModule} from '@ngrx/store';
import {reducers} from 'src/app/home/groups/store/reducers';
import {EffectsModule} from '@ngrx/effects';
import {GroupsEffects} from 'src/app/home/groups/store/effects';
import {GroupsService} from 'src/app/home/groups/services/groups.service';
import {GroupComponent} from './components/group/group.component';
import {CreateChannelModal} from 'src/app/home/groups/components/group/create-channel.modal';

@NgModule({
  declarations: [
    GroupsComponent,
    CreateGroupModal,
    JoinGroupModal,
    GroupComponent,
    CreateChannelModal,
  ],
  imports: [
    CommonModule,
    BrowserModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    StoreModule.forFeature('groups', reducers),
    EffectsModule.forFeature([GroupsEffects]),
  ],
  providers: [GroupsService],
})
export class GroupsModule {}
