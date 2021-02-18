import {Component, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {Store} from '@ngrx/store';
import {ChannelDto} from 'src/app/home/groups/services/responses/group.dto';
import {updateChannelRolePermissionOverridesAction} from 'src/app/home/groups/store/channels/actions';
import {Role} from "src/app/home/groups/store/roles/role.redcuer";
import {RolePermissionOverride} from "src/app/home/groups/store/channels/channel.override.role.reducer";

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
        <app-role-permission-overrides
          (overridesChanged)="onPermissionsChanged($event)"
          [role]="role"
          [channel]="channel"
        ></app-role-permission-overrides>
      </div>
    </div>
    <div class="modal-footer">
      <button
        type="button"
        class="btn btn-outline-success"
        (click)="updateOverrides()"
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
export class RolePermissionOverridesModal implements OnInit {
  private changedOverrides: RolePermissionOverride[] = [];
  @Input() role: Role;
  @Input() channel: ChannelDto;
  constructor(public modal: NgbActiveModal, private store: Store) {}
  ngOnInit(): void {}
  onPermissionsChanged(event) {
    this.changedOverrides = event;
  }
  updateOverrides() {
    this.store.dispatch(
      updateChannelRolePermissionOverridesAction({
        groupId: this.role.groupId,
        channelId: this.channel.channelId,
        roleId: this.role.roleId,
        overrides: this.changedOverrides.map((p) => ({...p})),
      })
    );
    this.modal.close('save');
  }
}
