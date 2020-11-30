import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {select, Store} from '@ngrx/store';
import {Observable, of} from 'rxjs';
import {catchError, map} from 'rxjs/operators';
import {ProfileService} from 'src/app/home/profile/services/profile.service';
import {uploadAvatar} from 'src/app/home/profile/store/actions/uploadAvatar.actions';
import {
  avatarSelector,
  nicknameSelector,
} from 'src/app/home/profile/store/selectors';
import {currentUser} from 'src/app/identity/store/selectors';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  nickname$: Observable<string>;
  avatar$: Observable<string>;
  form: FormGroup;
  constructor(private fb: FormBuilder, private store: Store) {
    this.nickname$ = this.store.pipe(select(nicknameSelector));
    this.avatar$ = this.store.pipe(select(avatarSelector));
  }
  ngOnInit(): void {
    this.initializeForm();
    this.initializeValues();
  }
  initializeValues() {}
  initializeForm() {
    this.form = this.fb.group({});
  }
  uploadAvatar(files: File[]) {
    this.store.dispatch(
      uploadAvatar({
        request: {
          file: files[0],
        },
      })
    );
  }
  onSubmit() {}
  get email() {
    return this.form.get('email');
  }
  get password() {
    return this.form.get('password');
  }
}
