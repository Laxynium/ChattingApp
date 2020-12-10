import {Action, createReducer, on} from '@ngrx/store';
import {
  ChannelDto,
  GroupDto,
} from 'src/app/home/groups/services/responses/group.dto';
import {
  changeCurrentGroupAction,
  changeCurrentGroupFailureAction,
  changeCurrentGroupSuccessAction,
  createChannelAction,
  createChannelFailureAction,
  createChannelSuccessAction,
  createGroupAction,
  createGroupFailureAction,
  createGroupSuccessAction,
  generateInvitationAction,
  generateInvitationFailureAction,
  generateInvitationSuccessAction,
  getChannelsAction,
  getChannelsFailureAction,
  getChannelsSuccessAction,
  getGroupsAction,
  getGroupsFailureAction,
  getGroupsSuccessAction,
  getInvitationsAction,
  getInvitationsFailureAction,
  getInvitationsSuccessAction,
  joinGroupAction,
  joinGroupFailureAction,
  joinGroupSuccessAction,
  removeChannelAction,
  removeChannelFailureAction,
  removeChannelSuccessAction,
  removeGroupAction,
  removeGroupFailureAction,
  removeGroupSuccessAction,
  revokeInvitationAction,
  revokeInvitationFailureAction,
  revokeInvitationSuccessAction,
} from 'src/app/home/groups/store/actions';
import {
  addRoleToMemberSuccessAction,
  getMemberRolesAction,
  getMemberRolesFailureAction,
  getMemberRolesSuccessAction,
  getMembersAction,
  getMembersFailureAction,
  getMembersSuccessAction,
  kickMemberAction,
  kickMemberFailureAction,
  kickMemberSuccessAction,
  removeRoleFromMemberAction,
} from 'src/app/home/groups/store/members/actions';
import {
  createRoleAction,
  createRoleFailureAction,
  createRoleSuccessAction,
  getRolePermissionsAction,
  getRolePermissionsFailureAction,
  getRolePermissionsSuccessAction,
  getRolesAction,
  getRolesFailureAction,
  getRolesSuccessAction,
  removeRoleAction,
  removeRoleFailureAction,
  removeRoleSuccessAction,
} from 'src/app/home/groups/store/roles/actions';
import {
  CurrentGroup,
  EmptyCurrentGroup,
  ICurrentGroup,
} from 'src/app/home/groups/store/types/currentGroup';
import {InvitationDto} from 'src/app/home/groups/store/types/invitation';
import {MemberDto} from 'src/app/home/groups/store/types/member';
import {MemberRoleDto} from 'src/app/home/groups/store/types/member-role';
import {PermissionDto} from 'src/app/home/groups/store/types/permission';
import {RoleDto} from 'src/app/home/groups/store/types/role';

export interface GroupsStateInterface {
  groups: GroupDto[];
  groupsLoading: boolean;
  currentGroup: ICurrentGroup;
  channels: ChannelDto[];
  generatedInvitation: {
    groupId: string;
    invitationId: string;
    code: string;
    isBeingGenerated: boolean;
  };
  invitations: InvitationDto[];
  roles: RoleDto[];
  creatingRole: boolean;
  rolePermissions: PermissionDto[];
  rolePermissionsLoading: boolean;

  members: MemberDto[];
  membersLoading: boolean;

  memberRoles: RoleDto[];
}

const initialState: GroupsStateInterface = {
  groups: [],
  groupsLoading: false,
  currentGroup: new EmptyCurrentGroup(),
  channels: [],
  generatedInvitation: {
    groupId: null,
    invitationId: null,
    code: null,
    isBeingGenerated: false,
  },
  invitations: [],
  roles: [],
  creatingRole: false,
  rolePermissions: [],
  rolePermissionsLoading: false,

  members: [],
  membersLoading: false,

  memberRoles: [],
};

const groupsReducer = createReducer(
  initialState,
  on(getGroupsAction, (s, a) => ({
    ...s,
    groupsLoading: true,
  })),
  on(getGroupsSuccessAction, (s, a) => ({
    ...s,
    groups: a.groups,
    groupsLoading: false,
  })),
  on(getGroupsFailureAction, (s, a) => ({
    ...s,
    groupsLoading: false,
  })),

  on(getChannelsAction, (s, a) => ({
    ...s,
  })),
  on(getChannelsSuccessAction, (s, a) => ({
    ...s,
    channels: [...a.channels],
  })),
  on(getChannelsFailureAction, (s, a) => ({
    ...s,
  })),

  on(createGroupAction, (s, a) => ({
    ...s,
  })),
  on(createGroupSuccessAction, (s, a) => ({
    ...s,
    groups: [...s.groups, a.group],
  })),
  on(createGroupFailureAction, (s, a) => ({
    ...s,
  })),

  on(removeGroupAction, (s, a) => ({
    ...s,
  })),
  on(removeGroupSuccessAction, (s, a) => ({
    ...s,
    groups: [...s.groups.filter((g) => g.groupId != a.groupId)],
  })),
  on(removeGroupFailureAction, (s, a) => ({
    ...s,
  })),

  on(generateInvitationAction, (s, a) => ({
    ...s,
    generatedInvitation: {
      ...s.generatedInvitation,
      isBeingGenerated: true,
    },
  })),
  on(generateInvitationSuccessAction, (s, a) => ({
    ...s,
    generatedInvitation: {
      ...s.generatedInvitation,
      isBeingGenerated: false,
      code: a.code,
      groupId: a.groupId,
      invitationId: a.invitationId,
    },
  })),
  on(generateInvitationFailureAction, (s, a) => ({
    ...s,
    generatedInvitation: {
      ...s.generatedInvitation,
      isBeingGenerated: false,
      code: null,
      groupId: null,
      invitationId: null,
    },
  })),

  on(revokeInvitationAction, (s, a) => ({
    ...s,
  })),
  on(revokeInvitationSuccessAction, (s, a) => ({
    ...s,
    invitations: [
      ...s.invitations.filter((i) => i.invitationId != a.invitationId),
    ],
  })),
  on(revokeInvitationFailureAction, (s, a) => ({
    ...s,
  })),

  on(getInvitationsAction, (s, a) => ({
    ...s,
  })),
  on(getInvitationsSuccessAction, (s, a) => ({
    ...s,
    invitations: [...a.invitations],
  })),
  on(getInvitationsFailureAction, (s, a) => ({
    ...s,
  })),

  on(joinGroupAction, (s, a) => ({
    ...s,
  })),
  on(joinGroupSuccessAction, (s, a) => ({
    ...s,
  })),
  on(joinGroupFailureAction, (s, a) => ({
    ...s,
  })),

  on(changeCurrentGroupAction, (s, a) => ({
    ...s,
  })),
  on(changeCurrentGroupSuccessAction, (s, a) => ({
    ...s,
    currentGroup: new CurrentGroup(a.groupId),
  })),
  on(changeCurrentGroupFailureAction, (s, a) => ({
    ...s,
  })),

  on(createChannelAction, (s, a) => ({
    ...s,
  })),
  on(createChannelSuccessAction, (s, a) => ({
    ...s,
    channels: [...s.channels, a.channel],
  })),
  on(createChannelFailureAction, (s, a) => ({
    ...s,
  })),

  on(removeChannelAction, (s, a) => ({
    ...s,
  })),
  on(removeChannelSuccessAction, (s, a) => ({
    ...s,
    channels: [...s.channels.filter((c) => c.channelId != a.channelId)],
  })),
  on(removeChannelFailureAction, (s, a) => ({
    ...s,
  })),

  on(getRolesAction, (s, a) => ({
    ...s,
  })),
  on(getRolesSuccessAction, (s, a) => ({
    ...s,
    roles: [...a.roles],
  })),
  on(getRolesFailureAction, (s, a) => ({
    ...s,
  })),

  on(createRoleAction, (s, a) => ({
    ...s,
    creatingRole: true,
  })),
  on(createRoleSuccessAction, (s, a) => ({
    ...s,
    roles: [...s.roles, a.role],
    creatingRole: false,
  })),
  on(createRoleFailureAction, (s, a) => ({
    ...s,
    creatingRole: false,
  })),

  on(removeRoleAction, (s, a) => ({
    ...s,
  })),
  on(removeRoleSuccessAction, (s, a) => ({
    ...s,
    roles: [...s.roles.filter((r) => r.roleId != a.roleId)],
  })),
  on(removeRoleFailureAction, (s, a) => ({
    ...s,
  })),

  on(getRolePermissionsAction, (s, a) => ({
    ...s,
    rolePermissionsLoading: true,
  })),
  on(getRolePermissionsSuccessAction, (s, a) => ({
    ...s,
    rolePermissionsLoading: false,
    rolePermissions: [...a.roles],
  })),
  on(getRolePermissionsFailureAction, (s, a) => ({
    ...s,
    rolePermissionsLoading: false,
  })),

  on(getMembersAction, (s, a) => ({
    ...s,
    membersLoading: true,
  })),
  on(getMembersSuccessAction, (s, a) => ({
    ...s,
    members: [...a.members],
    membersLoading: false,
  })),
  on(getMembersFailureAction, (s, a) => ({
    ...s,
    membersLoading: false,
  })),

  on(kickMemberAction, (s, a) => ({
    ...s,
  })),
  on(kickMemberSuccessAction, (s, a) => ({
    ...s,
    members: [
      ...s.members.filter(
        (m) => m.userId != a.userId || m.groupId != a.groupId
      ),
    ],
  })),
  on(kickMemberFailureAction, (s, a) => ({
    ...s,
    membersLoading: false,
  })),

  on(getMemberRolesAction, (s, a) => ({
    ...s,
  })),
  on(getMemberRolesSuccessAction, (s, a) => ({
    ...s,
    memberRoles: [...a.roles],
  })),
  on(getMemberRolesFailureAction, (s, a) => ({
    ...s,
  })),

  on(addRoleToMemberSuccessAction, (s, a) => ({
    ...s,
    memberRoles: [...s.memberRoles, a.memberRole.role],
  })),

  on(removeRoleFromMemberAction, (s, a) => ({
    ...s,
    memberRoles: [
      ...s.memberRoles.filter((r) => r.roleId != a.memberRole.role.roleId),
    ],
  }))
);

export function reducers(state: GroupsStateInterface, action: Action) {
  return groupsReducer(state, action);
}
