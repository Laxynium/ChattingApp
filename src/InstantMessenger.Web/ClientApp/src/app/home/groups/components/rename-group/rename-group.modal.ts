import {Component, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {Store} from '@ngrx/store';
import {GroupDto} from 'src/app/home/groups/services/responses/group.dto';

@Component({
  selector: 'ngbd-modal-confirm',
  template: `
    <div class="modal-header">
      <h4 class="modal-title" id="modal-title">Rename group</h4>
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
      <app-rename-group [group]="group"></app-rename-group>
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
export class RenameGroupModal implements OnInit {
  @Input() group: GroupDto;
  constructor(public modal: NgbActiveModal, private store: Store) {}
  ngOnInit(): void {}
}
