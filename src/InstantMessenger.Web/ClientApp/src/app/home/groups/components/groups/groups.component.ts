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
  removeGroupAction,
} from 'src/app/home/groups/store/actions';
import {
  groupsLoadingSelector,
  groupsSelector,
} from 'src/app/home/groups/store/selectors';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.scss'],
})
export class GroupsComponent implements OnInit {
  groups$: Observable<GroupDto[]>;
  groupsLoading$: Observable<boolean>;
  constructor(
    private modalService: NgbModal,
    private store: Store,
    private router: Router
  ) {
    this.groups$ = this.store.pipe(select(groupsSelector));
    this.groupsLoading$ = this.store.pipe(select(groupsLoadingSelector));
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
}
