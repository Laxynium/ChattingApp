import {Component, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {Store} from '@ngrx/store';
import {ChannelDto} from 'src/app/home/groups/services/responses/group.dto';

@Component({
  selector: 'ngbd-modal-confirm',
  template: `
    <div class="modal-header">
      <h4 class="modal-title" id="modal-title">Rename channel</h4>
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
      <app-rename-channel [channel]="channel"></app-rename-channel>
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
export class RenameChannelModal implements OnInit {
  @Input() channel: ChannelDto;
  constructor(public modal: NgbActiveModal, private store: Store) {}
  ngOnInit(): void {}
}
