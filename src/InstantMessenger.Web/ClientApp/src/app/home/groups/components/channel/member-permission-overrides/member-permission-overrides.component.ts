import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {first} from 'rxjs/operators';
import {ChannelDto} from 'src/app/home/groups/services/responses/group.dto';
import {getChannelMemberPermissionOverridesAction} from 'src/app/home/groups/store/channels/actions';
import {
  overridesLoadingSelector,
  overridesSelector,
} from 'src/app/home/groups/store/channels/selectors';
import {MemberDto} from 'src/app/home/groups/store/types/member';
import {
  PermissionOverrideDto,
  PermissionOverrideTypeDto,
} from 'src/app/home/groups/store/types/role-permission-override';

@Component({
  selector: 'app-member-permission-overrides',
  templateUrl: './member-permission-overrides.component.html',
  styleUrls: ['./member-permission-overrides.component.scss'],
})
export class MemberPermissionOverridesComponent implements OnInit {
  @Input() channel: ChannelDto;
  @Input() member: MemberDto;
  @Output() overridesChanged = new EventEmitter<PermissionOverrideDto[]>();
  $overridesLoading: Observable<boolean>;
  $overrides: Observable<PermissionOverrideDto[]>;

  OverrideType = PermissionOverrideTypeDto;

  updateOverrides: PermissionOverrideDto[] = [];
  constructor(private store: Store) {
    this.$overrides = this.store.pipe(select(overridesSelector));
    this.$overridesLoading = this.store.pipe(select(overridesLoadingSelector));
  }

  ngOnInit(): void {
    this.store.dispatch(
      getChannelMemberPermissionOverridesAction({
        groupId: this.channel.groupId,
        channelId: this.channel.channelId,
        memberUserId: this.member.userId,
      })
    );
  }
  onOverrideChange(event) {
    const name = event.target.name;
    const value: PermissionOverrideTypeDto = event.target.value;
    this.$overrides.pipe(first()).subscribe((os) => {
      const permIdx = this.updateOverrides.findIndex(
        (x) => x.permission == name
      );
      if (permIdx == -1) {
        this.updateOverrides.push({permission: name, type: value});
      } else {
        this.updateOverrides[permIdx] = {permission: name, type: value};
      }
      const original = os.find((o) => o.permission == name);
      if (original.type == value) {
        const toRemove = this.updateOverrides.findIndex(
          (x) => x.permission == name
        );
        this.updateOverrides.splice(toRemove, 1);
      }
      this.overridesChanged.emit(this.updateOverrides);
    });
  }
}
