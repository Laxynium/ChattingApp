import {createFeatureSelector, createSelector} from '@ngrx/store';
import {AppStateInterface} from 'src/app/shared/types/appState.interface';
import {GroupsModuleState} from 'src/app/home/groups/store/reducers';

export const groupsFeatureSelector = createFeatureSelector<
  AppStateInterface,
  GroupsModuleState
>('groups');

export const groupModuleSelector = createSelector(
  groupsFeatureSelector,
  (s) => s
);

export const groupsStateSelector = createSelector(
  groupModuleSelector,
  (s) => s.groups
);
export const channelsStateSelector = createSelector(
  groupModuleSelector,
  (s) => s.channels
);

export const currentGroupSelector = createSelector(
  groupsStateSelector,
  (s) => s.entities[s.currentGroup]
);
export const currentChannelSelector = createSelector(
  channelsStateSelector,
  (s) => s.entities[s.currentChannel]
);

export const invitationsStateSelector = createSelector(
  groupModuleSelector,
  (s) => s.invitations
);
export const roleOverridesStateSelector = createSelector(
  groupModuleSelector,
  (s) => s.roleOverrides
);
export const memberOverridesStateSelector = createSelector(
  groupModuleSelector,
  (s) => s.memberOverrides
);

export const rolesStateSelector = createSelector(
  groupModuleSelector,
  (s) => s.roles
);

export const rolePermissionsStateSelector = createSelector(
  groupModuleSelector,
  (s) => s.rolePermissions
);

export const membersStateSelector = createSelector(
  groupModuleSelector,
  (s) => s.members
);

export const memberRolesStateSelector = createSelector(
  groupModuleSelector,
  (s) => s.memberRoles
);

export const allowedActionsStateSelector = createSelector(
  groupModuleSelector,
  (s) => s.allowedActions
);

export const messagesStateSelector = createSelector(
  groupModuleSelector,
  (s) => s.messages
);
