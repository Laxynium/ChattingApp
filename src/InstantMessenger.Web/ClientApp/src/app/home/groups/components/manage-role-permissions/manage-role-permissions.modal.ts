import {Component, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {Store} from '@ngrx/store';
import {updateRolePermissionsAction} from 'src/app/home/groups/store/roles/actions';
import {Role} from "src/app/home/groups/store/roles/role.redcuer";
import {RolePermission} from "src/app/home/groups/store/roles/role.permission.reducer";

@Component({
  selector: 'ngbd-manage-role-permissions-modal',
  template: `
    <div class="modal-header">
      <h4>Manage role permissions</h4>
      <button
        type="button"
        class="close"
        aria-describedby="modal-title"
        (click)="modal.close('btn-click')"
      >
        <span aria-hidden="true">&times;</span>
      </button>
    </div>
    <div class="modal-body">
      <div>
        <app-manage-role-permissions
          (rolePermissionsChanged)="onPermissionsChanged($event)"
          [role]="role"
        ></app-manage-role-permissions>
      </div>
    </div>
    <div class="modal-footer">
      <button
        type="button"
        class="btn btn-outline-success"
        (click)="updateRolePermissions()"
      >
        Save
      </button>
      <button
        type="button"
        class="btn btn-outline-dark"
        (click)="modal.close('btn-click')"
      >
        Close
      </button>
    </div>
  `,
})
export class ManageRolePermissionsModal implements OnInit {
  private changedRolePermissions: RolePermission[] = [];
  @Input()
  public role: Role;
  constructor(public modal: NgbActiveModal, private store: Store) {}
  ngOnInit(): void {}
  onPermissionsChanged(event) {
    this.changedRolePermissions = event;
  }
  updateRolePermissions() {
    this.store.dispatch(
      updateRolePermissionsAction({
        groupId: this.role.groupId,
        roleId: this.role.roleId,
        permissions: this.changedRolePermissions.map((p) => ({...p})),
      })
    );
    this.modal.close('save');
  }
}
