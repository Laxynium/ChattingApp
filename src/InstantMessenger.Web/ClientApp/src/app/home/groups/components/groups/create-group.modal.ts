import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {Store} from '@ngrx/store';
import {createGroupAction} from 'src/app/home/groups/store/groups/actions';

@Component({
  selector: 'ngbd-modal-confirm',
  template: `
    <div class="modal-header">
      <h4 class="modal-title" id="modal-title">Create group</h4>
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
          <label for="groupName">Group name</label>
          <div class="input-group">
            <input formControlName="groupName" name="groupName" type="text" />
          </div>
          <div
            *ngIf="groupName.invalid && (groupName.dirty || groupName.touched)"
            class="text-danger"
          >
            <div *ngIf="groupName.errors.required">Group name is required</div>
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
        [disabled]="!groupName.valid"
        type="button"
        class="btn btn-success"
        (click)="createGroup()"
      >
        Ok
      </button>
    </div>
  `,
})
export class CreateGroupModal implements OnInit {
  form: FormGroup;
  constructor(
    public modal: NgbActiveModal,
    private fb: FormBuilder,
    private store: Store
  ) {
    this.form = this.fb.group({
      groupName: ['', Validators.required],
    });
  }
  ngOnInit(): void {}

  get groupName() {
    return this.form.get('groupName');
  }

  createGroup() {
    const groupName = this.form.value.groupName;
    this.store.dispatch(createGroupAction({groupName: groupName}));
    this.modal.close();
  }
}
