import {ActionReducerMap} from '@ngrx/store';
import {groupReducer, GroupsState} from 'src/app/home/groups/store/groups/group.reducer';
import {channelReducer, ChannelsState} from 'src/app/home/groups/store/channels/channel.reducer';
import {messageReducer, MessagesState} from 'src/app/home/groups/store/messages/message.reducer';
import {
  invitationReducer,
  InvitationsState,
} from "src/app/home/groups/store/invitations/invitation.reducer";
import {
  reducer as roleOverrideReducer,
  RolePermissionOverridesState,
} from 'src/app/home/groups/store/channels/channel.override.role.reducer';
import {
  reducer as memberOverrideReducer,
  MemberPermissionOverridesState,
} from 'src/app/home/groups/store/channels/channel.override.member.reducer';
import {roleReducer, RolesState} from 'src/app/home/groups/store/roles/role.redcuer';
import {
  rolePermissionReducer,
  RolePermissionsState,
} from 'src/app/home/groups/store/roles/role.permission.reducer';
import {membersReducer, MembersState} from 'src/app/home/groups/store/members/member.reducer';
import {
  memberRolesReducer,
  MemberRolesState,
} from 'src/app/home/groups/store/members/member.role.reducer';
import {
  allowedActionsReducer,
  AllowedActionsState,
} from 'src/app/home/groups/store/access-control/reducer';

export interface GroupsModuleState {
  groups: GroupsState;
  channels: ChannelsState;
  messages: MessagesState;
  invitations: InvitationsState;
  roles: RolesState;
  rolePermissions: RolePermissionsState;
  members: MembersState;
  memberRoles: MemberRolesState;
  roleOverrides: RolePermissionOverridesState;
  memberOverrides: MemberPermissionOverridesState;
  allowedActions: AllowedActionsState;
}

export const reducers: ActionReducerMap<GroupsModuleState> = {
  groups: groupReducer,
  channels: channelReducer,
  invitations: invitationReducer,
  messages: messageReducer,
  roles: roleReducer,
  rolePermissions: rolePermissionReducer,
  members: membersReducer,
  memberRoles: memberRolesReducer,
  roleOverrides: roleOverrideReducer,
  memberOverrides: memberOverrideReducer,
  allowedActions: allowedActionsReducer,
};
