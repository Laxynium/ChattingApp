import {Component, Input, OnInit} from '@angular/core';
import {FormGroup, FormBuilder, Validators} from '@angular/forms';
import {Store} from '@ngrx/store';
import {Observable, of} from 'rxjs';
import {renameRoleAction} from 'src/app/home/groups/store/roles/actions';
import {Role} from "src/app/home/groups/store/roles/role.redcuer";

@Component({
  selector: 'app-rename-role',
  templateUrl: './rename-role.component.html',
  styleUrls: ['./rename-role.component.scss'],
})
export class RenameRoleComponent implements OnInit {
  @Input() role: Role;

  form: FormGroup;
  $renamingRole: Observable<boolean>;
  constructor(private fb: FormBuilder, private store: Store) {
    this.form = this.fb.group({
      roleName: ['', Validators.required],
    });
    this.$renamingRole = of(false);
  }

  ngOnInit(): void {
    this.form.setValue({roleName: this.role.name});
  }

  renameRole() {
    const newName = this.form.value.roleName;
    this.store.dispatch(
      renameRoleAction({role: {...this.role, name: newName}})
    );
  }
  get roleName() {
    return this.form.get('roleName');
  }
}
