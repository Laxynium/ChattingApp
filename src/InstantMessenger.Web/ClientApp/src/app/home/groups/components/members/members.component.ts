import {Component, OnInit} from '@angular/core';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {filter, first, map} from 'rxjs/operators';
import {ManageMemberRolesModal} from 'src/app/home/groups/components/manage-member-roles/manage-member-roles.modal';
import {
  getMembersAction,
  kickMemberAction,
} from 'src/app/home/groups/store/members/actions';
import {
  membersLoadingSelector,
  membersSelector,
} from 'src/app/home/groups/store/members/selectors';
import {currentGroupSelector} from 'src/app/home/groups/store/groups/selectors';
import {Member} from "src/app/home/groups/store/members/member.reducer";

@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styleUrls: ['./members.component.scss'],
})
export class MembersComponent implements OnInit {
  $members: Observable<Member[]>;
  $membersLoading: Observable<boolean>;
  constructor(private store: Store, private modal: NgbModal) {
    this.$members = this.store.pipe(select(membersSelector));
    this.$membersLoading = this.store.pipe(select(membersLoadingSelector));
  }

  ngOnInit(): void {
    this.store
      .pipe(
        select(currentGroupSelector),
        filter((g) => g != null),
        first()
      )
      .subscribe((g) => {
        this.store.dispatch(getMembersAction({groupId: g.groupId}));
      });
  }
  kickMember(member: Member) {
    this.store.dispatch(
      kickMemberAction({groupId: member.groupId, userId: member.userId, memberId: member.memberId})
    );
  }
  openManageMemberRoles(member: Member) {
    const modalRef = this.modal.open(ManageMemberRolesModal);
    modalRef.componentInstance.member = member;
  }
}
