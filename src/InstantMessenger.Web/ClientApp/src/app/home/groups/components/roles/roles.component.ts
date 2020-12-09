import {Component, OnInit} from '@angular/core';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {filter, first} from 'rxjs/operators';
import {
  getRolesAction,
  removeRoleAction,
} from 'src/app/home/groups/store/roles/actions';
import {rolesSelector} from 'src/app/home/groups/store/roles/selectors';
import {currentGroupSelector} from 'src/app/home/groups/store/selectors';
import {RoleDto} from 'src/app/home/groups/store/types/role';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss'],
})
export class RolesComponent implements OnInit {
  $roles: Observable<RoleDto[]>;
  constructor(private store: Store) {
    this.$roles = this.store.pipe(select(rolesSelector));
  }

  ngOnInit(): void {
    this.store
      .pipe(
        select(currentGroupSelector),
        filter((g) => g != null),
        first()
      )
      .subscribe((g) => {
        this.store.dispatch(getRolesAction({groupId: g.groupId}));
      });
  }
  removeRole(role: RoleDto) {
    this.store.dispatch(
      removeRoleAction({groupId: role.groupId, roleId: role.roleId})
    );
  }
}
