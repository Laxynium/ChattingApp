import {MessageDto} from '../store/types/message';
import {
  getChannelsAction,
  getInvitationsAction,
  removeGroupSuccessAction,
} from 'src/app/home/groups/store/actions';
import {
  Hub,
  HubHandlersProvider,
  HubMethod,
} from 'src/app/shared/hubs/hubHandlersProvider';
import {sendMessageSuccessAction} from 'src/app/home/groups/store/messages/actions';
import {
  addRoleToMemberSuccessAction,
  getMemberRolesAction,
  removeRoleFromMemberSuccessAction,
} from 'src/app/home/groups/store/members/actions';
import {getRolePermissionsAction} from 'src/app/home/groups/store/roles/actions';
import {getAllowedActionsAction} from 'src/app/home/groups/store/access-control/actions';
import {PermissionOverrideDto} from 'src/app/home/groups/store/types/role-permission-override';

interface GroupDto {
  groupId: string;
  groupName: string;
}
interface InvitationDto {
  groupId: string;
  invitationId: string;
  code: string;
}
interface ChannelDto {
  groupId: string;
  channelId: string;
  channelName: string;
}
interface PermissionDto {
  name: string;
  code: string;
}
interface RolePermissionDto {
  groupId: string;
  roleId: string;
  permission: PermissionDto;
}
interface RoleDto {
  groupId: string;
  roleId: string;
  name: string;
  priority: number;
}
interface MemberRoleDto {
  userId: string;
  role: RoleDto;
}
interface RolePermissionOverridesDto {
  groupId: string;
  channelId: string;
  roleId: string;
  overrides: PermissionOverrideDto;
}
interface MemberPermissionOverridesDto {
  groupId: string;
  channelId: string;
  userId: string;
  overrides: PermissionOverrideDto;
}
interface AllowedActionDto {
  groupId: string;
}
const onGroupRemoved: HubMethod<GroupDto> = (store, data) => {
  store.dispatch(removeGroupSuccessAction({groupId: data.groupId}));
};
const onInvitationCreated: HubMethod<InvitationDto> = (store, data) => {
  store.dispatch(getInvitationsAction({groupId: data.groupId}));
};
const onInvitationRevoked: HubMethod<InvitationDto> = (store, data) => {
  store.dispatch(getInvitationsAction({groupId: data.groupId}));
};
const onChannelCreated: HubMethod<ChannelDto> = (store, data) => {
  store.dispatch(getChannelsAction({groupId: data.groupId}));
};
const onChannelRemoved: HubMethod<ChannelDto> = (store, data) => {
  store.dispatch(getChannelsAction({groupId: data.groupId}));
};
const onMessageCreated: HubMethod<MessageDto> = (store, data) => {
  store.dispatch(sendMessageSuccessAction({message: data}));
};

const onRoleAddedToMember: HubMethod<MemberRoleDto> = (store, data) => {
  store.dispatch(
    getMemberRolesAction({groupId: data.role.groupId, userId: data.userId})
  );
};
const onRoleRemovedFromMember: HubMethod<MemberRoleDto> = (store, data) => {
  store.dispatch(
    getMemberRolesAction({groupId: data.role.groupId, userId: data.userId})
  );
};
const onPermissionAddedToRole: HubMethod<RolePermissionDto> = (store, data) => {
  store.dispatch(
    getRolePermissionsAction({groupId: data.groupId, roleId: data.roleId})
  );
};

const onPermissionRemovedFromRole: HubMethod<RolePermissionDto> = (
  store,
  data
) => {
  store.dispatch(
    getRolePermissionsAction({groupId: data.groupId, roleId: data.roleId})
  );
};

const onRolePermissionOverridesChanged: HubMethod<RolePermissionOverridesDto> = (
  store,
  data
) => {};
const onMemberPermissionOverridesChanged: HubMethod<MemberPermissionOverridesDto> = (
  store,
  data
) => {};

const onAllowedActionsChanged: HubMethod<AllowedActionDto> = (store, data) => {
  store.dispatch(getAllowedActionsAction({groupId: data.groupId}));
  store.dispatch(getChannelsAction({groupId: data.groupId}));
};

const hubProvider: HubHandlersProvider = () => ({
  onGroupRemoved: onGroupRemoved,
  onInvitationCreated: onInvitationCreated,
  onInvitationRevoked: onInvitationRevoked,
  onChannelCreated: onChannelCreated,
  onChannelRemoved: onChannelRemoved,
  onMessageCreated: onMessageCreated,
  onRoleAddedToMember: onRoleAddedToMember,
  onRoleRemovedFromMember: onRoleRemovedFromMember,
  onPermissionAddedToRole: onPermissionAddedToRole,
  onPermissionRemovedFromRole: onPermissionRemovedFromRole,
  onAllowedActionsChanged: onAllowedActionsChanged,
});

export const groupsHub: Hub = {
  hubName: 'groups/hub',
  hubHandlersProvider: hubProvider,
};
