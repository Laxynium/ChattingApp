import {Component, OnInit} from '@angular/core';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {map, reduce, tap} from 'rxjs/operators';
import {getPendingInvitationsAction} from 'src/app/home/friends/store/actions';
import {
  arePendingInvitationsLoadingSelector,
  pendingInvitationsSelector,
} from 'src/app/home/friends/store/selectors';
import {InvitationInterface} from 'src/app/home/friends/types/invitation.interface';
import {PaginatorInterface} from 'src/app/home/friends/types/paginator.interface';

@Component({
  selector: 'app-pending-invitations',
  templateUrl: './pending-invitations.component.html',
  styleUrls: ['./pending-invitations.component.scss'],
})
export class PendingInvitationsComponent implements OnInit {
  paginator: PaginatorInterface = {
    currentPage: 1,
    pageSize: 4,
    totalSize: 0,
  };

  $invitations: Observable<InvitationInterface[]>;
  $arePendingInvitationsLoading: Observable<boolean>;
  $totalSize: Observable<number>;
  constructor(private store: Store) {
    const pendingInvitations = this.store.pipe(
      select(pendingInvitationsSelector)
    );
    this.$invitations = pendingInvitations.pipe(map((x) => this.paginate(x)));
    this.$totalSize = pendingInvitations.pipe(map((x) => x.length));
    this.$arePendingInvitationsLoading = this.store.pipe(
      select(arePendingInvitationsLoadingSelector)
    );
  }

  ngOnInit(): void {
    this.store.dispatch(getPendingInvitationsAction());
  }
  refreshInvitations() {
    this.store.dispatch(getPendingInvitationsAction());
  }
  private paginate(invitations: InvitationInterface[]): InvitationInterface[] {
    return invitations.slice(
      (this.paginator.currentPage - 1) * this.paginator.pageSize,
      (this.paginator.currentPage - 1) * this.paginator.pageSize +
        this.paginator.pageSize
    );
  }
}
