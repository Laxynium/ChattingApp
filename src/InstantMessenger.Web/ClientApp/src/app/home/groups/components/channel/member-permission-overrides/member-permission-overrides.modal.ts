import {Component, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {Store} from '@ngrx/store';
import {ChannelDto} from 'src/app/home/groups/services/responses/group.dto';
import {updateChannelMemberPermissionOverridesAction} from 'src/app/home/groups/store/channels/actions';
import {Member} from "src/app/home/groups/store/members/member.reducer";
import {MemberPermissionOverride} from "src/app/home/groups/store/channels/channel.override.member.reducer";

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
        <app-member-permission-overrides
          (overridesChanged)="onPermissionsChanged($event)"
          [member]="member"
          [channel]="channel"
        ></app-member-permission-overrides>
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
export class MemberPermissionOverridesModal implements OnInit {
  private changedOverrides: MemberPermissionOverride[] = [];
  @Input() member: Member;
  @Input() channel: ChannelDto;
  constructor(public modal: NgbActiveModal, private store: Store) {}
  ngOnInit(): void {}
  onPermissionsChanged(event) {
    this.changedOverrides = event;
  }
  updateOverrides() {
    this.store.dispatch(
      updateChannelMemberPermissionOverridesAction({
        groupId: this.member.groupId,
        channelId: this.channel.channelId,
        memberUserId: this.member.userId,
        overrides: this.changedOverrides.map((p) => ({...p})),
      })
    );
    this.modal.close('save');
  }
}
