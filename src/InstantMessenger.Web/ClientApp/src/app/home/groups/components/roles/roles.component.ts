import {Component, OnInit} from '@angular/core';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {filter, first} from 'rxjs/operators';
import {ManageRolePermissionsModal} from 'src/app/home/groups/components/manage-role-permissions/manage-role-permissions.modal';
import {RenameRoleModal} from 'src/app/home/groups/components/rename-role/rename-role.modal';
import {
  getRolesAction,
  moveDownRoleAction,
  moveUpRoleAction,
  removeRoleAction,
} from 'src/app/home/groups/store/roles/actions';
import {rolesSelector} from 'src/app/home/groups/store/roles/selectors';
import {currentGroupSelector} from 'src/app/home/groups/store/groups/selectors';
import {Role} from "src/app/home/groups/store/roles/role.redcuer";

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss'],
})
export class RolesComponent implements OnInit {
  $roles: Observable<Role[]>;
  constructor(private store: Store, private modal: NgbModal) {
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

  goToRolePermissions(role: Role) {
    const modalRef = this.modal.open(ManageRolePermissionsModal, {
      scrollable: true,
      beforeDismiss: () => {
        return false;
      },
    });
    modalRef.componentInstance.role = role;
  }

  moveUp(role: Role) {
    this.store.dispatch(
      moveUpRoleAction({groupId: role.groupId, roleId: role.roleId})
    );
  }

  moveDown(role: Role) {
    this.store.dispatch(
      moveDownRoleAction({groupId: role.groupId, roleId: role.roleId})
    );
  }

  removeRole(role: Role) {
    this.store.dispatch(
      removeRoleAction({groupId: role.groupId, roleId: role.roleId})
    );
  }
  openRenameRoleModal(role: Role) {
    const modal = this.modal.open(RenameRoleModal);
    modal.componentInstance.role = role;
  }
}
