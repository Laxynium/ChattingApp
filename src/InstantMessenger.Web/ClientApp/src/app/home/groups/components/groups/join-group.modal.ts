import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'ngbd-modal-confirm',
  template: `
    <div class="modal-header">
      <h4 class="modal-title" id="modal-title">Join group</h4>
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
          <label for="invitationCode">Inivtation code</label>
          <div class="input-group">
            <input
              formControlName="invitationCode"
              name="invitationCode"
              type="text"
            />
          </div>
          <div
            *ngIf="
              invitationCode.invalid &&
              (invitationCode.dirty || invitationCode.touched)
            "
            class="text-danger"
          >
            <div *ngIf="invitationCode.errors.required">
              Invitation code is required
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
        [disabled]="!invitationCode.valid"
        type="button"
        class="btn btn-success"
        (click)="joinGroup()"
      >
        Ok
      </button>
    </div>
  `,
})
export class JoinGroupModal implements OnInit {
  form: FormGroup;
  constructor(public modal: NgbActiveModal, private fb: FormBuilder) {
    this.form = this.fb.group({
      invitationCode: ['', Validators.required],
    });
  }
  ngOnInit(): void {}

  get invitationCode() {
    return this.form.get('invitationCode');
  }

  joinGroup() {
    console.log('abc');
    this.modal.close();
  }
}
