import {Component, OnInit} from '@angular/core';
import {select, Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {map} from 'rxjs/operators';
import {changeConversationAction} from 'src/app/home/conversations/store/actions';
import {getFriendsAction} from 'src/app/home/friends/store/actions';
import {
  areFriendsLoadingSelector as areFriendshipsLoadingSelector,
  friendSelector as friendshipsSelector,
} from 'src/app/home/friends/store/selectors';
import {FriendshipInterface} from 'src/app/home/friends/types/friendship.interface';
import {PaginatorInterface} from 'src/app/home/friends/types/paginator.interface';

interface Friend {
  icon: string;
  nickname: string;
  since: string;
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

  constructor(private store: Store) {
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

  private paginate(friends: FriendshipInterface[]): FriendshipInterface[] {
    return friends.slice(
      (this.paginator.currentPage - 1) * this.paginator.pageSize,
      (this.paginator.currentPage - 1) * this.paginator.pageSize +
        this.paginator.pageSize
    );
  }
}
