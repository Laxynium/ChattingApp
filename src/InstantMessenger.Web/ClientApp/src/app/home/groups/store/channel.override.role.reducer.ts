import {ChannelId, PermissionOverrideType, RoleId} from './types';
import {createEntityAdapter, EntityState} from '@ngrx/entity';
import {createReducer, on} from '@ngrx/store';
import {
  getChannelRolePermissionOverridesAction,
  getChannelRolePermissionOverridesFailureAction,
  getChannelRolePermissionOverridesSuccessAction,
  updateChannelRolePermissionOverridesAction,
} from './channels/actions';

export interface RolePermissionOverride {
  id: string;
  channelId: ChannelId;
  roleId: RoleId;
  permission: string;
  type: PermissionOverrideType;
}

export interface RolePermissionOverridesState
  extends EntityState<RolePermissionOverride> {
  isLoading: boolean;
}

export const adapter = createEntityAdapter<RolePermissionOverride>({
  selectId: (x) => x.id,
});

export const reducer = createReducer(
  adapter.getInitialState({isLoading: false}),
  on(getChannelRolePermissionOverridesAction, (s) => ({...s, isLoading: true})),
  on(getChannelRolePermissionOverridesSuccessAction, (s, a) => ({
    ...adapter.setAll(
      a.overrides.map((x) => ({
        id: `${x.permission}_${a.roleId}_${a.channelId}`,
        channelId: a.channelId,
        permission: x.permission,
        roleId: a.roleId,
        type: <PermissionOverrideType>(<unknown>x.type),
      })),
      s
    ),
    isLoading: false,
  })),
  on(getChannelRolePermissionOverridesFailureAction, (s) => ({
    ...s,
    isLoading: false,
  })),
  on(updateChannelRolePermissionOverridesAction, (s, a) => ({
    ...adapter.upsertMany(
      a.overrides.map((x) => ({
        id: `${x.permission}_${a.roleId}_${a.channelId}`,
        channelId: a.channelId,
        permission: x.permission,
        roleId: a.roleId,
        type: <PermissionOverrideType>(<unknown>x.type),
      })),
      s
    ),
  }))
);
