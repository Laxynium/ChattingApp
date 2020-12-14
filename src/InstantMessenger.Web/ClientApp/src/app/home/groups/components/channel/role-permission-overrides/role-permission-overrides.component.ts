import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {select, Store} from '@ngrx/store';
import {Observable, of} from 'rxjs';
import {first} from 'rxjs/operators';
import {ChannelDto} from 'src/app/home/groups/services/responses/group.dto';
import {getChannelRolePermissionOverridesAction} from 'src/app/home/groups/store/channels/actions';
import {
  roleOverridesLoadingSelector,
  roleOverridesSelector,
} from 'src/app/home/groups/store/channels/selectors';
import {RoleDto} from 'src/app/home/groups/store/types/role';
import {
  PermissionOverrideDto,
  PermissionOverrideTypeDto,
} from 'src/app/home/groups/store/types/role-permission-override';

@Component({
  selector: 'app-role-permission-overrides',
  templateUrl: './role-permission-overrides.component.html',
  styleUrls: ['./role-permission-overrides.component.scss'],
})
export class RolePermissionOverridesComponent implements OnInit {
  @Input() channel: ChannelDto;
  @Input() role: RoleDto;
  @Output() overridesChanged = new EventEmitter<PermissionOverrideDto[]>();
  $overridesLoading: Observable<boolean>;
  $overrides: Observable<PermissionOverrideDto[]>;

  OverrideType = PermissionOverrideTypeDto;

  updateOverrides: PermissionOverrideDto[] = [];
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
    const value: PermissionOverrideTypeDto = event.target.value;
    this.$overrides.pipe(first()).subscribe((os) => {
      const permIdx = this.updateOverrides.findIndex(
        (x) => x.permission == name
      );
      if (permIdx == -1) {
        this.updateOverrides.push({permission: name, type: value});
      } else {
        this.updateOverrides[permIdx] = {permission: name, type: value};
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
interface OverrideDto {
  permissionName: string;
  state: OverrideState;
}
enum OverrideState {
  Deny,
  Neutral,
  Allow,
}
