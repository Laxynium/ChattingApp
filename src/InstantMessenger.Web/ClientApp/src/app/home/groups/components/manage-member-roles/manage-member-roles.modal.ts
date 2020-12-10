import {Component, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {MemberDto} from 'src/app/home/groups/store/types/member';

@Component({
  selector: 'ngbd-modal-confirm',
  template: `
    <div class="modal-header">
      <h4>Manage member roles</h4>
    </div>
    <div class="modal-body">
      <app-manage-member-roles [member]="member"></app-manage-member-roles>
    </div>
    <div class="modal-footer">
      <button
        type="button"
        class="btn btn-outline-dark"
        (click)="modal.close('Close click')"
      >
        Close
      </button>
    </div>
  `,
})
export class ManageMemberRolesModal implements OnInit {
  @Input() member: MemberDto;
  constructor(public modal: NgbActiveModal) {}
  ngOnInit(): void {}
}
