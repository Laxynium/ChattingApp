import {Component, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'ngbd-modal-confirm',
  template: `
    <div class="modal-header">
      <ul ngbNav #nav="ngbNav" [(activeId)]="active" class="nav-tabs">
        <li [ngbNavItem]="1">
          <a ngbNavLink>Add role</a>
          <ng-template ngbNavContent><app-add-role></app-add-role></ng-template>
        </li>
        <li [ngbNavItem]="2">
          <a ngbNavLink>Roles</a>
          <ng-template ngbNavContent><app-roles></app-roles></ng-template>
        </li>
      </ul>
    </div>
    <div class="modal-body">
      <div [ngbNavOutlet]="nav"></div>
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
export class ManageRolesModal implements OnInit {
  active = 1;
  constructor(public modal: NgbActiveModal) {}
  ngOnInit(): void {}
}
