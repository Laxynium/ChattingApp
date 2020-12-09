import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {first} from 'rxjs/operators';
import {getRolePermissionsAction} from 'src/app/home/groups/store/roles/actions';
import {
  rolePermissionsLoadingSelector,
  rolePermissionsSelector,
} from 'src/app/home/groups/store/roles/selectors';
import {PermissionDto} from 'src/app/home/groups/store/types/permission';
import {RoleDto} from 'src/app/home/groups/store/types/role';

@Component({
  selector: 'app-manage-role-permissions',
  templateUrl: './manage-role-permissions.component.html',
  styleUrls: ['./manage-role-permissions.component.scss'],
})
export class ManageRolePermissionsComponent implements OnInit {
  @Input() role: RoleDto;
  @Output() rolePermissionsChanged = new EventEmitter<PermissionDto[]>();
  $permissions: Observable<PermissionDto[]>;
  updatedPermissions: PermissionDto[] = [];
  $permissionsLoading: Observable<boolean>;
  constructor(private store: Store) {
    this.$permissions = this.store.pipe(select(rolePermissionsSelector));
    this.$permissionsLoading = this.store.pipe(
      select(rolePermissionsLoadingSelector)
    );
  }

  ngOnInit(): void {
    this.store.dispatch(
      getRolePermissionsAction({
        groupId: this.role.groupId,
        roleId: this.role.roleId,
      })
    );
  }

  onPermissionChange(event: boolean, permission: PermissionDto) {
    console.log(event, permission);
    this.$permissions.pipe(first()).subscribe(() => {
      const permIdx = this.updatedPermissions.findIndex(
        (x) => x.name == permission.name
      );
      if (permIdx == -1) {
        this.updatedPermissions.push({...permission, isOn: event});
      } else {
        this.updatedPermissions[permIdx] = {
          ...permission,
          isOn: event,
        };
      }

      this.rolePermissionsChanged.emit(this.updatedPermissions);
    });
  }
}
