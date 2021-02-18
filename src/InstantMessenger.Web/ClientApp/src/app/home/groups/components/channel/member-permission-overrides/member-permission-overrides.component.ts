import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {first} from 'rxjs/operators';
import {ChannelDto} from 'src/app/home/groups/services/responses/group.dto';
import {getChannelMemberPermissionOverridesAction} from 'src/app/home/groups/store/channels/actions';
import {
  memberOverridesLoadingSelector,
  memberOverridesSelector,
} from 'src/app/home/groups/store/channels/selectors';
import {Member} from 'src/app/home/groups/store/members/member.reducer';
import {MemberPermissionOverride} from 'src/app/home/groups/store/channels/channel.override.member.reducer';
import {PermissionOverrideType} from 'src/app/home/groups/store/types';

@Component({
  selector: 'app-member-permission-overrides',
  templateUrl: './member-permission-overrides.component.html',
  styleUrls: ['./member-permission-overrides.component.scss'],
})
export class MemberPermissionOverridesComponent implements OnInit {
  @Input() channel: ChannelDto;
  @Input() member: Member;
  @Output() overridesChanged = new EventEmitter<MemberPermissionOverride[]>();
  $overridesLoading: Observable<boolean>;
  $overrides: Observable<MemberPermissionOverride[]>;

  OverrideType = PermissionOverrideType;

  updateOverrides: MemberPermissionOverride[] = [];

  constructor(private store: Store) {
    this.$overrides = this.store.pipe(select(memberOverridesSelector));
    this.$overridesLoading = this.store.pipe(
      select(memberOverridesLoadingSelector)
    );
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
    const value: PermissionOverrideType = event.target.value;
    this.$overrides.pipe(first()).subscribe((os) => {
      const permIdx = this.updateOverrides.findIndex(
        (x) => x.permission == name
      );
      if (permIdx == -1) {
        this.updateOverrides.push({
          permission: name,
          type: value,
          memberUserId: this.member.userId,
          channelId: this.channel.channelId,
        });
      } else {
        this.updateOverrides[permIdx] = {
          permission: name,
          type: value,
          memberUserId: this.member.userId,
          channelId: this.channel.channelId,
        };
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
