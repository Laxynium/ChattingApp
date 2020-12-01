import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {requiredFileType} from 'src/app/home/profile/components/profile/validators';
import {getProfile} from 'src/app/home/profile/store/actions/getProfile.actions';
import {uploadAvatar} from 'src/app/home/profile/store/actions/uploadAvatar.actions';
import {
  avatarSelector,
  nicknameSelector,
} from 'src/app/home/profile/store/selectors';
import {changeNicknameAction} from 'src/app/identity/store/actions/changeNickname.actions';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  nickname$: Observable<string>;
  avatar$: Observable<string>;
  avatarForm: FormGroup;
  nicknameForm: FormGroup;
  constructor(
    private fb: FormBuilder,
    private modalService: NgbModal,
    private store: Store
  ) {
    this.nickname$ = this.store.pipe(select(nicknameSelector));
    this.avatar$ = this.store.pipe(select(avatarSelector));
  }
  ngOnInit(): void {
    this.initializeForm();
    this.initializeValues();
  }
  initializeValues() {
    this.store.dispatch(getProfile());
  }
  initializeForm() {
    this.avatarForm = this.fb.group({
      avatar: [
        '',
        Validators.compose([Validators.required, requiredFileType('png')]),
      ],
    });
    this.nicknameForm = this.fb.group({
      nickname: ['', Validators.required],
    });
    this.nickname$.subscribe((n) => {
      this.nicknameForm.setValue({nickname: n});
    });
  }
  uploadAvatar(files: File[]) {
    this.store.dispatch(
      uploadAvatar({
        request: {
          file: files[0],
        },
      })
    );
    this.modalService.dismissAll();
  }
  changeNickname() {
    const nickname = this.nicknameForm.value.nickname;
    this.store.dispatch(changeNicknameAction({request: {nickname: nickname}}));
  }
  open(content) {
    this.modalService
      .open(content, {ariaLabelledBy: 'modal-basic-title'})
      .result.then(
        () => {},
        () => {}
      );
  }
  get avatar() {
    return this.avatarForm.get('avatar');
  }
  get nickname() {
    return this.nicknameForm.get('nickname');
  }
}
