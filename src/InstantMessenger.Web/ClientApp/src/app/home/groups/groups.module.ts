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
import {InvitationsModal} from 'src/app/home/groups/components/group/invitations.modal';
import {InvitationsComponent} from './components/invitations/invitations.component';
import {GenerateInvitationComponent} from './components/generate-invitation/generate-invitation.component';
import {TimepsanPipe} from 'src/app/home/groups/components/invitations/timespan.pipe';
import {ManageRolesModal} from 'src/app/home/groups/components/group/manage-roles.modal';
import {RolesComponent} from './components/roles/roles.component';
import {AddRoleComponent} from './components/add-role/add-role.component';
import {ManageRolePermissionsComponent} from './components/manage-role-permissions/manage-role-permissions.component';
import {UiSwitchModule} from 'ngx-ui-switch';
import {RolesService} from 'src/app/home/groups/services/roles.service';
import {RolesEffects} from 'src/app/home/groups/store/roles/effects';

@NgModule({
  declarations: [
    GroupsComponent,
    CreateGroupModal,
    JoinGroupModal,
    GroupComponent,
    CreateChannelModal,
    InvitationsModal,
    ManageRolesModal,
    InvitationsComponent,
    GenerateInvitationComponent,
    TimepsanPipe,
    RolesComponent,
    AddRoleComponent,
    ManageRolePermissionsComponent,
  ],
  imports: [
    CommonModule,
    BrowserModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    UiSwitchModule,
    StoreModule.forFeature('groups', reducers),
    EffectsModule.forFeature([GroupsEffects, RolesEffects]),
  ],
  providers: [GroupsService, RolesService],
})
export class GroupsModule {}
