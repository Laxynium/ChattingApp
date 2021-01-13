import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {filter, first} from 'rxjs/operators';
import {createRoleAction} from 'src/app/home/groups/store/roles/actions';
import {creatingRoleSelector} from 'src/app/home/groups/store/roles/selectors';
import {currentGroupSelector} from 'src/app/home/groups/store/groups/selectors';

@Component({
  selector: 'app-add-role',
  templateUrl: './add-role.component.html',
  styleUrls: ['./add-role.component.scss'],
})
export class AddRoleComponent implements OnInit {
  form: FormGroup;
  $creatingRole: Observable<boolean>;
  constructor(private fb: FormBuilder, private store: Store) {
    this.form = this.fb.group({
      roleName: ['', Validators.required],
    });
    this.$creatingRole = this.store.pipe(select(creatingRoleSelector));
  }

  ngOnInit(): void {}

  addRole() {
    const roleName = this.form.value.roleName;
    this.store
      .pipe(
        select(currentGroupSelector),
        filter((g) => g != null),
        first()
      )
      .subscribe((g) => {
        this.store.dispatch(
          createRoleAction({groupId: g.groupId, roleName: roleName})
        );
        this.form.setValue({roleName: ''});
      });
  }
  get roleName() {
    return this.form.get('roleName');
  }
}
