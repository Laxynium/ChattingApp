import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {CreateGroupModal} from 'src/app/home/groups/components/groups/create-group.modal';
import {JoinGroupModal} from 'src/app/home/groups/components/groups/join-group.modal';
import {GroupDto} from 'src/app/home/groups/services/responses/group.dto';
import {
  changeCurrentGroupAction,
  getGroupsAction,
  leaveGroupAction,
  removeGroupAction,
} from 'src/app/home/groups/store/groups/actions';
import {
  groupsLoadingSelector,
  groupsSelector,
} from 'src/app/home/groups/store/groups/selectors';
import {currentUserSelector} from 'src/app/identity/store/selectors';
import {CurrentUserInterface} from 'src/app/shared/types/currentUser.interface';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.scss'],
})
export class GroupsComponent implements OnInit {
  groups$: Observable<GroupDto[]>;
  groupsLoading$: Observable<boolean>;
  currentUser$: Observable<CurrentUserInterface>;
  constructor(private modalService: NgbModal, private store: Store) {
    this.groups$ = this.store.pipe(select(groupsSelector));
    this.groupsLoading$ = this.store.pipe(select(groupsLoadingSelector));
    this.currentUser$ = this.store.pipe(select(currentUserSelector));
  }

  ngOnInit(): void {
    this.store.dispatch(getGroupsAction());
  }
  openCreateGroupModal() {
    this.modalService.open(CreateGroupModal);
  }
  openJoinGroupModal() {
    this.modalService.open(JoinGroupModal);
  }
  goToGroup(groupId: string) {
    this.store.dispatch(changeCurrentGroupAction({groupId}));
  }
  removeGroup(groupId: string) {
    this.store.dispatch(removeGroupAction({groupId}));
  }
  leaveGroup(groupId: string) {
    this.store.dispatch(leaveGroupAction({groupId}));
  }
}
