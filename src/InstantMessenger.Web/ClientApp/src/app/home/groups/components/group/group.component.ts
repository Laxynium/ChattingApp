import {Component, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {NgbModal, NgbNav} from '@ng-bootstrap/ng-bootstrap';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {first} from 'rxjs/operators';
import {CreateChannelModal} from 'src/app/home/groups/components/group/create-channel.modal';
import {InvitationsModal} from 'src/app/home/groups/components/group/invitations.modal';
import {ManageRolesModal} from 'src/app/home/groups/components/group/manage-roles.modal';
import {
  ChannelDto,
  GroupDto,
} from 'src/app/home/groups/services/responses/group.dto';
import {
  getChannelsAction,
  removeChannelAction,
} from 'src/app/home/groups/store/actions';
import {
  channelsSelector,
  currentGroupSelector,
} from 'src/app/home/groups/store/selectors';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.scss'],
})
export class GroupComponent implements OnInit {
  @ViewChild(NgbNav) channelsNav: NgbNav;
  $currentGroup: Observable<GroupDto>;
  $channels: Observable<ChannelDto[]>;
  constructor(
    private store: Store,
    private router: Router,
    private modal: NgbModal
  ) {
    this.$currentGroup = this.store.pipe(select(currentGroupSelector));
    this.$channels = this.store.pipe(select(channelsSelector));
  }

  ngOnInit(): void {
    this.$currentGroup.subscribe((g) => {
      if (!g) {
        this.router.navigateByUrl('/groups');
      }
      this.store.dispatch(getChannelsAction({groupId: g.groupId}));
    });
  }

  channelDropdownChange(event) {
    event.preventDefault();
    event.stopImmediatePropagation();
  }

  openCreateChannelModal() {
    this.modal.open(CreateChannelModal);
  }

  removeChannel(channelId) {
    this.store.pipe(select(currentGroupSelector), first()).subscribe((g) => {
      this.store.dispatch(
        removeChannelAction({channelId: channelId, groupId: g.groupId})
      );
    });
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
}
