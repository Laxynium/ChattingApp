import {
  ChannelId,
  PermissionOverrideType,
  RoleId,
} from 'src/app/home/groups/store/types';
import {createEntityAdapter, EntityState} from '@ngrx/entity';
import {createReducer, on} from '@ngrx/store';
import {
  getChannelRolePermissionOverridesAction,
  getChannelRolePermissionOverridesFailureAction,
  getChannelRolePermissionOverridesSuccessAction,
  updateChannelRolePermissionOverridesAction,
} from 'src/app/home/groups/store/channels/actions';

export interface RolePermissionOverride {
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
  selectId: (x) => `${x.permission}_${x.roleId}_${x.channelId}`,
});

export const reducer = createReducer(
  adapter.getInitialState({isLoading: false}),
  on(getChannelRolePermissionOverridesAction, (s) => ({...s, isLoading: true})),
  on(getChannelRolePermissionOverridesSuccessAction, (s, a) => ({
    ...adapter.setAll(a.overrides, s),
    isLoading: false,
  })),
  on(getChannelRolePermissionOverridesFailureAction, (s) => ({
    ...s,
    isLoading: false,
  })),
  on(updateChannelRolePermissionOverridesAction, (s, a) => ({
    ...adapter.upsertMany(a.overrides, s),
  }))
);
