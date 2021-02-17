import {ActionReducerMap} from '@ngrx/store';
import {groupReducer, GroupsState} from './group.reducer';
import {channelReducer, ChannelsState} from './channel.reducer';
import {messageReducer, MessagesState} from './message.reducer';
import {invitationReducer, InvitationsState} from './invitation.reducer';
import {
  reducer as roleOverrideReducer,
  RolePermissionOverridesState,
} from './channel.override.role.reducer';
import {
  reducer as memberOverrideReducer,
  MemberPermissionOverridesState,
} from './channel.override.member.reducer';
import {roleReducer, RolesState} from './role.redcuer';
import {
  rolePermissionReducer,
  RolePermissionsState,
} from './role.permission.reducer';
import {membersReducer, MembersState} from './member.reducer';
import {memberRolesReducer, MemberRolesState} from './member.role.reducer';
import {allowedActionsReducer, AllowedActionsState} from "./access-control/reducer";


export interface GroupsModuleState {
  groups: GroupsState;
  channels: ChannelsState;
  message: MessagesState;
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
  message: messageReducer,
  roles: roleReducer,
  rolePermissions: rolePermissionReducer,
  members: membersReducer,
  memberRoles: memberRolesReducer,
  roleOverrides: roleOverrideReducer,
  memberOverrides: memberOverrideReducer,
  allowedActions: allowedActionsReducer,
};
