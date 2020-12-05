import {Component, OnInit} from '@angular/core';
import {NgbActiveModal, NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {map} from 'rxjs/operators';
import {changeConversationAction} from 'src/app/home/conversations/store/actions';
import {
  getFriendsAction,
  removeFriendAction,
} from 'src/app/home/friends/store/actions';
import {
  areFriendsLoadingSelector as areFriendshipsLoadingSelector,
  friendSelector as friendshipsSelector,
} from 'src/app/home/friends/store/selectors';
import {FriendshipInterface} from 'src/app/home/friends/types/friendship.interface';
import {PaginatorInterface} from 'src/app/home/friends/types/paginator.interface';

@Component({
  selector: 'ngbd-modal-confirm',
  template: `
    <div class="modal-header">
      <h4 class="modal-title" id="modal-title">Friendship deletion</h4>
      <button
        type="button"
        class="close"
        aria-describedby="modal-title"
        (click)="modal.dismiss('Cross click')"
      >
        <span aria-hidden="true">&times;</span>
      </button>
    </div>
    <div class="modal-body">
      <p><strong>Are you sure you want to delete this friend?</strong></p>
      <p>
        Conversation history will be permamently deleted
        <span class="text-danger">This operation can not be undone.</span>
      </p>
    </div>
    <div class="modal-footer">
      <button
        type="button"
        class="btn btn-outline-secondary"
        (click)="modal.dismiss('cancel click')"
      >
        Cancel
      </button>
      <button
        type="button"
        class="btn btn-danger"
        (click)="modal.close('Ok click')"
      >
        Ok
      </button>
    </div>
  `,
})
export class DeleteFriendConfirmModal {
  constructor(public modal: NgbActiveModal) {}
}

@Component({
  selector: 'app-all-friends',
  templateUrl: './all-friends.component.html',
  styleUrls: ['./all-friends.component.scss'],
})
export class AllFriendsComponent implements OnInit {
  paginator: PaginatorInterface = {
    currentPage: 1,
    pageSize: 4,
    totalSize: 0,
  };

  $friendships: Observable<FriendshipInterface[]>;
  $areFriendsLoading: Observable<boolean>;
  $totalSize: Observable<number>;

  constructor(private store: Store, private modalSerivce: NgbModal) {
    const friendships = this.store.pipe(select(friendshipsSelector));
    this.$friendships = friendships.pipe(map((x) => this.paginate(x)));
    this.$totalSize = friendships.pipe(map((x) => x.length));
    this.$areFriendsLoading = this.store.pipe(
      select(areFriendshipsLoadingSelector)
    );
  }

  ngOnInit(): void {
    this.refreshFriends();
  }

  refreshFriends() {
    this.store.dispatch(getFriendsAction());
  }

  goToConversation(conversationId: string) {
    this.store.dispatch(
      changeConversationAction({conversationId: conversationId})
    );
  }

  removeFriend(friendshipId: string) {
    this.modalSerivce
      .open(DeleteFriendConfirmModal)
      .result.then((_) =>
        this.store.dispatch(removeFriendAction({friendshipId: friendshipId}))
      )
      .catch((r) => {});
  }

  private paginate(friends: FriendshipInterface[]): FriendshipInterface[] {
    return friends.slice(
      (this.paginator.currentPage - 1) * this.paginator.pageSize,
      (this.paginator.currentPage - 1) * this.paginator.pageSize +
        this.paginator.pageSize
    );
  }
}
