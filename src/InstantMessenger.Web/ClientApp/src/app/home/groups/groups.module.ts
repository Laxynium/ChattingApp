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
import {ManageRolePermissionsModal} from 'src/app/home/groups/components/manage-role-permissions/manage-role-permissions.modal';
import {MembersComponent} from './components/members/members.component';
import {MembersModal} from 'src/app/home/groups/components/members/members.modal';
import {MembersEffects} from 'src/app/home/groups/store/members/effects';
import {MembersService} from 'src/app/home/groups/services/members.service';
import {ManageMemberRolesComponent} from './components/manage-member-roles/manage-member-roles.component';
import {ManageMemberRolesModal} from 'src/app/home/groups/components/manage-member-roles/manage-member-roles.modal';
import {MessagesComponent} from './components/messages/messages.component';
import {MesssagesService} from './services/messages.service';
import {MessagesEffects} from './store/messages/effects';
import {SharedModule} from '../../shared/shared.module';
import {AccessControlDirective} from 'src/app/home/groups/directives/access-control.directive';
import {AccessControlEffects} from 'src/app/home/groups/store/access-control/effects';
import {ManageChannelPermissionsModal} from 'src/app/home/groups/components/channel/manage-channel-permissions.modal';
import {ChannelComponent} from 'src/app/home/groups/components/channel/channel.component';
import {RouterModule} from '@angular/router';
import {RolePermissionOverridesComponent} from './components/channel/role-permission-overrides/role-permission-overrides.component';
import {RolePermissionOverridesModal} from 'src/app/home/groups/components/channel/role-permission-overrides/role-permission-overrides.modal';
import {ChannelsEffects} from 'src/app/home/groups/store/channels/effects';
import {ChannelsService} from 'src/app/home/groups/services/channels.service';
import {MemberPermissionOverridesComponent} from 'src/app/home/groups/components/channel/member-permission-overrides/member-permission-overrides.component';
import {MemberPermissionOverridesModal} from 'src/app/home/groups/components/channel/member-permission-overrides/member-permission-overrides.modal';
import {RenameRoleComponent} from './components/rename-role/rename-role.component';
import {RenameChannelComponent} from './components/rename-channel/rename-channel.component';
import {RenameGroupComponent} from './components/rename-group/rename-group.component';
import {RenameRoleModal} from 'src/app/home/groups/components/rename-role/rename-role.modal';
import {RenameChannelModal} from 'src/app/home/groups/components/rename-channel/rename-channel.modal';
import {RenameGroupModal} from 'src/app/home/groups/components/rename-group/rename-group.modal';

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
    ManageRolePermissionsModal,
    MembersComponent,
    MembersModal,
    ManageMemberRolesComponent,
    ManageMemberRolesModal,
    MessagesComponent,
    AccessControlDirective,
    ManageChannelPermissionsModal,
    ChannelComponent,
    RolePermissionOverridesComponent,
    RolePermissionOverridesModal,
    MemberPermissionOverridesComponent,
    MemberPermissionOverridesModal,
    RenameRoleComponent,
    RenameRoleModal,
    RenameChannelComponent,
    RenameChannelModal,
    RenameGroupComponent,
    RenameGroupModal,
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
    EffectsModule.forFeature([
      GroupsEffects,
      RolesEffects,
      MembersEffects,
      MessagesEffects,
      AccessControlEffects,
      ChannelsEffects,
    ]),
    SharedModule,
    RouterModule,
  ],
  providers: [
    GroupsService,
    RolesService,
    MembersService,
    MesssagesService,
    ChannelsService,
  ],
})
export class GroupsModule {}
