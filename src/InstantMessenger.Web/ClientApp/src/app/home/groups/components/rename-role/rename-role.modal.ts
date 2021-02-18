import {Component, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {Store} from '@ngrx/store';
import {Role} from "src/app/home/groups/store/roles/role.redcuer";

@Component({
  selector: 'ngbd-modal-confirm',
  template: `
    <div class="modal-header">
      <h4 class="modal-title" id="modal-title">Rename role</h4>
      <button
        type="button"
        class="close"
        aria-describedby="modal-title"
        (click)="modal.dismiss('Cross click')"
      >
        <span aria-hidden="true">&times;</span>
      </button>
    </div>
    <div class="modal-body">
      <app-rename-role [role]="role"></app-rename-role>
    </div>
    <div class="modal-footer">
      <button
        type="button"
        class="btn btn-success"
        (click)="modal.close('close')"
      >
        Close
      </button>
    </div>
  `,
})
export class RenameRoleModal implements OnInit {
  @Input() role: Role;
  constructor(public modal: NgbActiveModal, private store: Store) {}
  ngOnInit(): void {}
}
