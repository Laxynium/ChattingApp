import {Component, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {Store} from '@ngrx/store';

@Component({
  selector: 'ngbd-manage-role-permissions-modal',
  template: `
    <div class="modal-header">
      <h4>Members</h4>
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
      <app-members></app-members>
    </div>
    <div class="modal-footer">
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
export class MembersModal implements OnInit {
  constructor(public modal: NgbActiveModal) {}
  ngOnInit(): void {}
}
