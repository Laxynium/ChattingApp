import {Component, Input, OnInit} from '@angular/core';
import {NgbActiveModal, NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {filter, first} from 'rxjs/operators';
import {RolePermissionOverridesModal} from 'src/app/home/groups/components/channel/role-permission-overrides/role-permission-overrides.modal';
import {ChannelDto} from 'src/app/home/groups/services/responses/group.dto';
import {getMembersAction} from 'src/app/home/groups/store/members/actions';
import {membersSelector} from 'src/app/home/groups/store/members/selectors';
import {getRolesAction} from 'src/app/home/groups/store/roles/actions';
import {rolesSelector} from 'src/app/home/groups/store/roles/selectors';
import {currentGroupSelector} from 'src/app/home/groups/store/selectors';
import {MemberDto} from 'src/app/home/groups/store/types/member';
import {RoleDto} from 'src/app/home/groups/store/types/role';

@Component({
  selector: 'ngbd-modal-confirm',
  template: `
    <div class="modal-header">
      <ul ngbNav #nav="ngbNav" [(activeId)]="active" class="nav-tabs">
        <li [ngbNavItem]="1">
          <a ngbNavLink>Roles</a>
          <ng-template ngbNavContent>
            <ul class="list-group conversations-body">
              <li
                *ngFor="let role of $roles | async"
                class="list-group-item d-flex justify-content-between align-items-center list-group-item-action"
              >
                <span>{{ role.name }}</span>
                <div>
                  <button
                    class="btn btn-outline-primary"
                    (click)="openManageRolesPermissions(role)"
                  >
                    <fa-icon [icon]="['fas', 'sign-in-alt']"></fa-icon>
                  </button>
                </div>
              </li>
            </ul>
          </ng-template>
        </li>
        <li [ngbNavItem]="2">
          <a ngbNavLink>Members</a>
          <ng-template ngbNavContent>
            <ul class="list-group conversations-body">
              <li
                *ngFor="let member of $members | async"
                class="list-group-item d-flex justify-content-between align-items-center list-group-item-action"
              >
                <div class="member-name">
                  <img
                    class="rounded-circle member-avatar"
                    src="{{ member.avatar }}"
                  />
                  <span>{{ member.name }}</span>
                </div>
                <button
                  class="btn btn-outline-primary"
                  (click)="openManageRolesPermissions(member)"
                >
                  <fa-icon [icon]="['fas', 'sign-in-alt']"></fa-icon>
                </button>
              </li>
            </ul>
          </ng-template>
        </li>
      </ul>
    </div>
    <div class="modal-body">
      <div [ngbNavOutlet]="nav"></div>
    </div>
    <div class="modal-footer">
      <button
        type="button"
        class="btn btn-outline-dark"
        (click)="modal.close('Close click')"
      >
        Close
      </button>
    </div>
  `,
  styles: [
    `
      .member-avatar {
        width: 64px;
        height: 64px;
      }
      .member-name {
        position: relative;
      }
      .owner-icon {
        position: absolute;
      }
    `,
  ],
})
export class ManageChannelPermissionsModal implements OnInit {
  @Input() channel: ChannelDto;
  active = 1;
  $roles: Observable<RoleDto[]>;
  $members: Observable<MemberDto[]>;
  constructor(
    public modal: NgbActiveModal,
    private store: Store,
    private modalService: NgbModal
  ) {
    this.$roles = this.store.pipe(select(rolesSelector));
    this.$members = this.store.pipe(select(membersSelector));
  }
  ngOnInit(): void {
    this.store
      .pipe(
        select(currentGroupSelector),
        filter((g) => g != null),
        first()
      )
      .subscribe((g) => {
        this.store.dispatch(getRolesAction({groupId: g.groupId}));
        this.store.dispatch(getMembersAction({groupId: g.groupId}));
      });
  }
  openManageMemberPermissions(member: MemberDto) {}
  openManageRolesPermissions(role: RoleDto) {
    const modal = this.modalService.open(RolePermissionOverridesModal, {
      scrollable: true,
      beforeDismiss: () => false,
    });
    modal.componentInstance.role = role;
    modal.componentInstance.channel = this.channel;
  }
}
