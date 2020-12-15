import {Component, Input, OnInit} from '@angular/core';
import {FormGroup, FormBuilder, Validators} from '@angular/forms';
import {Store} from '@ngrx/store';
import {Observable, of} from 'rxjs';
import {ChannelDto} from 'src/app/home/groups/services/responses/group.dto';
import {renameChannelAction} from 'src/app/home/groups/store/channels/actions';

@Component({
  selector: 'app-rename-channel',
  templateUrl: './rename-channel.component.html',
  styleUrls: ['./rename-channel.component.scss'],
})
export class RenameChannelComponent implements OnInit {
  @Input() channel: ChannelDto;
  form: FormGroup;
  $renamingChannel: Observable<boolean>;
  constructor(private fb: FormBuilder, private store: Store) {
    this.form = this.fb.group({
      channelName: ['', Validators.required],
    });
    this.$renamingChannel = of(false);
  }

  ngOnInit(): void {
    this.form.setValue({channelName: this.channel.channelName});
  }

  renameChannel() {
    const newName = this.form.value.channelName;
    this.store.dispatch(
      renameChannelAction({channel: {...this.channel, channelName: newName}})
    );
  }
  get channelName() {
    return this.form.get('channelName');
  }
}
