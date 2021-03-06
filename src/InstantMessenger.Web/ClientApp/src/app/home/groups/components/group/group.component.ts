import {Component, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {NgbModal, NgbNav} from '@ng-bootstrap/ng-bootstrap';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {first, map} from 'rxjs/operators';
import {CreateChannelModal} from 'src/app/home/groups/components/group/create-channel.modal';
import {InvitationsModal} from 'src/app/home/groups/components/group/invitations.modal';
import {ManageChannelPermissionsModal} from 'src/app/home/groups/components/channel/manage-channel-permissions.modal';
import {ManageRolesModal} from 'src/app/home/groups/components/group/manage-roles.modal';
import {MembersModal} from 'src/app/home/groups/components/members/members.modal';
import {
  ChannelDto,
  GroupDto,
} from 'src/app/home/groups/services/responses/group.dto';
import {getAllowedActionsAction} from 'src/app/home/groups/store/access-control/actions';
import {loadCurrentGroupAction} from 'src/app/home/groups/store/groups/actions';
import {currentGroupSelector} from 'src/app/home/groups/store/groups/selectors';
import {RenameGroupModal} from 'src/app/home/groups/components/rename-group/rename-group.modal';
import {RenameChannelModal} from 'src/app/home/groups/components/rename-channel/rename-channel.modal';
import {
  getChannelsAction,
  changeCurrentChannelAction,
  removeChannelAction,
} from 'src/app/home/groups/store/channels/actions';
import {channelsSelector} from 'src/app/home/groups/store/channels/selectors';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.scss'],
})
export class GroupComponent implements OnInit {
  @ViewChild(NgbNav) channelsNav: NgbNav;
  $currentGroup: Observable<GroupDto>;
  $channels: Observable<ChannelDto[]>;
  $groupId: Observable<string>;
  constructor(
    private store: Store,
    private route: ActivatedRoute,
    private modal: NgbModal
  ) {
    this.$currentGroup = this.store.pipe(select(currentGroupSelector));
    this.$channels = this.store.pipe(select(channelsSelector));
    this.$groupId = this.route.params.pipe(map((p) => p['id']));
  }

  ngOnInit(): void {
    this.$groupId.subscribe((groupId) => {
      this.store.dispatch(loadCurrentGroupAction({groupId: groupId}));
      this.store.dispatch(getChannelsAction({groupId: groupId}));
      this.store.dispatch(getAllowedActionsAction({groupId: groupId}));
    });
  }

  ngAfterViewInit(): void {
    this.channelsNav.select(1);
  }

  navigateToChannel($event) {
    const nextChannelId = $event.nextId;
    this.$groupId.pipe(first()).subscribe((groupId) => {
      this.store.dispatch(
        changeCurrentChannelAction({groupId: groupId, channelId: nextChannelId})
      );
    });
  }
  channelDropdownChange(event) {
    event.preventDefault();
    event.stopImmediatePropagation();
  }

  openCreateChannelModal() {
    this.modal.open(CreateChannelModal);
  }

  removeChannel(channelId: string) {
    this.store.pipe(select(currentGroupSelector), first()).subscribe((g) => {
      this.store.dispatch(
        removeChannelAction({channelId: channelId, groupId: g.groupId})
      );
    });
  }

  openManagePermissions(channel: ChannelDto) {
    const modal = this.modal.open(ManageChannelPermissionsModal, {
      scrollable: true,
    });
    modal.componentInstance.channel = channel;
  }

  openInvitationsModal() {
    this.modal.open(InvitationsModal, {
      beforeDismiss: () => {
        return false;
      },
      scrollable: true,
    });
  }
  openManageRolesModal() {
    this.modal.open(ManageRolesModal, {
      beforeDismiss: () => {
        return false;
      },
      scrollable: true,
    });
  }
  openMembersModal() {
    this.modal.open(MembersModal, {
      beforeDismiss: () => {
        return false;
      },
      scrollable: true,
    });
  }
  openRenameGroupModal() {
    this.$currentGroup.pipe(first()).subscribe((g) => {
      const modal = this.modal.open(RenameGroupModal);
      modal.componentInstance.group = g;
    });
  }
  openRenameChannelModal(channel: ChannelDto) {
    const modal = this.modal.open(RenameChannelModal);
    modal.componentInstance.channel = channel;
  }
}
