import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {select, Store} from '@ngrx/store';
import {first} from 'rxjs/operators';
import {createChannelAction} from 'src/app/home/groups/store/channels/actions';
import {currentGroupSelector} from 'src/app/home/groups/store/groups/selectors';

@Component({
  selector: 'ngbd-modal-confirm',
  template: `
    <div class="modal-header">
      <h4 class="modal-title" id="modal-title">Create channel</h4>
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
      <form [formGroup]="form">
        <div class="form-group">
          <label for="channelName">Channel name</label>
          <div class="input-group">
            <input
              formControlName="channelName"
              name="channelName"
              type="text"
            />
          </div>
          <div
            *ngIf="
              channelName.invalid && (channelName.dirty || channelName.touched)
            "
            class="text-danger"
          >
            <div *ngIf="channelName.errors.required">
              Channel name is required
            </div>
          </div>
        </div>
      </form>
    </div>
    <div class="modal-footer">
      <button
        type="button"
        class="btn btn-outline-secondary"
        (click)="modal.dismiss('cancel click')"
      >
        Cancel
      </button>
      <button
        [disabled]="!channelName.valid"
        type="button"
        class="btn btn-success"
        (click)="createChannel()"
      >
        Ok
      </button>
    </div>
  `,
})
export class CreateChannelModal implements OnInit {
  form: FormGroup;
  constructor(
    public modal: NgbActiveModal,
    private fb: FormBuilder,
    private store: Store
  ) {
    this.form = this.fb.group({
      channelName: ['', Validators.required],
    });
  }
  ngOnInit(): void {}

  get channelName() {
    return this.form.get('channelName');
  }

  createChannel() {
    this.store.pipe(select(currentGroupSelector), first()).subscribe((g) => {
      const channelName = this.form.value.channelName;
      this.store.dispatch(
        createChannelAction({groupId: g.groupId, channelName: channelName})
      );
      this.modal.close();
    });
  }
}
