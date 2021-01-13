import {Component, Input, OnInit} from '@angular/core';
import {FormGroup, FormBuilder, Validators} from '@angular/forms';
import {Store} from '@ngrx/store';
import {Observable, of} from 'rxjs';
import {GroupDto} from 'src/app/home/groups/services/responses/group.dto';
import {renameGroupAction} from 'src/app/home/groups/store/groups/actions';

@Component({
  selector: 'app-rename-group',
  templateUrl: './rename-group.component.html',
  styleUrls: ['./rename-group.component.scss'],
})
export class RenameGroupComponent implements OnInit {
  @Input() group: GroupDto;

  form: FormGroup;
  $renamingGroup: Observable<boolean>;
  constructor(private fb: FormBuilder, private store: Store) {
    this.form = this.fb.group({
      groupName: ['', Validators.required],
    });
    this.$renamingGroup = of(false);
  }

  ngOnInit(): void {
    this.form.setValue({groupName: this.group.name});
  }

  renameGroup() {
    const newName = this.form.value.groupName;
    this.store.dispatch(
      renameGroupAction({group: {...this.group, name: newName}})
    );
  }
  get groupName() {
    return this.form.get('groupName');
  }
}
