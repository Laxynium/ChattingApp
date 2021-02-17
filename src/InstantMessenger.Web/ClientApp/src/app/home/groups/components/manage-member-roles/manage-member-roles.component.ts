import {Component, Input, OnInit} from '@angular/core';
import {select, Store} from '@ngrx/store';
import {Observable, zip} from 'rxjs';
import {map} from 'rxjs/operators';
import {
  addRoleToMemberAction,
  getMemberRolesAction,
  removeRoleFromMemberAction,
} from 'src/app/home/groups/store/members/actions';
import {memberRolesSelector} from 'src/app/home/groups/store/members/selectors';
import {getRolesAction} from 'src/app/home/groups/store/roles/actions';
import {rolesSelector} from 'src/app/home/groups/store/roles/selectors';
import {MemberDto} from 'src/app/home/groups/store/types/member';
import {RoleDto} from 'src/app/home/groups/store/types/role';

@Component({
  selector: 'app-manage-member-roles',
  templateUrl: './manage-member-roles.component.html',
  styleUrls: ['./manage-member-roles.component.scss'],
})
export class ManageMemberRolesComponent implements OnInit {
  selectedRole: RoleDto;
  @Input() member: MemberDto;
  $memberRoles: Observable<RoleDto[]>;
  $rolesToPick: Observable<RoleDto[]>;

  constructor(private store: Store) {
    this.$memberRoles = this.store.pipe(select(memberRolesSelector));
    this.$rolesToPick = zip(
      this.store.pipe(
        select(rolesSelector),
        map((rs) => rs.filter((r) => r.priority != -1))
      ),
      this.$memberRoles
    ).pipe(
      map(([roles, memberRoles]) => {
        return roles.filter(
          (r) => !memberRoles.some((mr) => mr.roleId == r.roleId)
        );
      })
    );
  }

  ngOnInit(): void {
    this.store.dispatch(getRolesAction({groupId: this.member.groupId}));
    this.store.dispatch(
      getMemberRolesAction({
        groupId: this.member.groupId,
        userId: this.member.userId,
        memberId: this.member.memberId,
      })
    );
  }

  addRoleToMember() {
    if (!this.selectedRole) return;
    this.store.dispatch(
      addRoleToMemberAction({
        memberRole: {
          memberId: this.member.memberId,
          userId: this.member.userId,
          groupId: this.member.groupId,
          role: this.selectedRole,
        },
      })
    );
    this.selectedRole = null;
  }

  removeRoleFromMember(role: RoleDto) {
    this.store.dispatch(
      removeRoleFromMemberAction({
        memberRole: {
          memberId: this.member.memberId,
          userId: this.member.userId,
          groupId: this.member.groupId,
          role: role,
        },
      })
    );
  }
}
