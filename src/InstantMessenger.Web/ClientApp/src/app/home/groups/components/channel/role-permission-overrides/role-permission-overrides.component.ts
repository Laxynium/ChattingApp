import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {first} from 'rxjs/operators';
import {ChannelDto} from 'src/app/home/groups/services/responses/group.dto';
import {getChannelRolePermissionOverridesAction} from 'src/app/home/groups/store/channels/actions';
import {
  roleOverridesLoadingSelector,
  roleOverridesSelector,
} from 'src/app/home/groups/store/channels/selectors';
import {Role} from 'src/app/home/groups/store/roles/role.redcuer';
import {RolePermissionOverride} from 'src/app/home/groups/store/channels/channel.override.role.reducer';
import {PermissionOverrideType} from 'src/app/home/groups/store/types';

@Component({
  selector: 'app-role-permission-overrides',
  templateUrl: './role-permission-overrides.component.html',
  styleUrls: ['./role-permission-overrides.component.scss'],
})
export class RolePermissionOverridesComponent implements OnInit {
  @Input() channel: ChannelDto;
  @Input() role: Role;
  @Output() overridesChanged = new EventEmitter<RolePermissionOverride[]>();
  $overridesLoading: Observable<boolean>;
  $overrides: Observable<RolePermissionOverride[]>;

  OverrideType = PermissionOverrideType;

  updateOverrides: RolePermissionOverride[] = [];

  constructor(private store: Store) {
    this.$overrides = this.store.pipe(select(roleOverridesSelector));
    this.$overridesLoading = this.store.pipe(
      select(roleOverridesLoadingSelector)
    );
  }

  ngOnInit(): void {
    this.store.dispatch(
      getChannelRolePermissionOverridesAction({
        groupId: this.channel.groupId,
        channelId: this.channel.channelId,
        roleId: this.role.roleId,
      })
    );
  }

  onOverrideChange(event) {
    const name = event.target.name;
    const value: PermissionOverrideType = event.target.value;
    this.$overrides.pipe(first()).subscribe((os) => {
      const permIdx = this.updateOverrides.findIndex(
        (x) => x.permission == name
      );
      if (permIdx == -1) {
        this.updateOverrides.push({
          permission: name,
          type: value,
          roleId: this.role.roleId,
          channelId: this.channel.channelId,
        });
      } else {
        this.updateOverrides[permIdx] = {
          permission: name,
          type: value,
          roleId: this.role.roleId,
          channelId: this.channel.channelId,
        };
      }
      const original = os.find((o) => o.permission == name);
      if (original.type == value) {
        const toRemove = this.updateOverrides.findIndex(
          (x) => x.permission == name
        );
        this.updateOverrides.splice(toRemove, 1);
      }
      this.overridesChanged.emit(this.updateOverrides);
    });
  }
}
