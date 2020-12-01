import {Component, OnInit} from '@angular/core';

interface Friend {
  icon: string;
  nickname: string;
  since: string;
}

const FRIENDS: Friend[] = [
  {
    icon: 'assets/profile-placeholder.png',
    nickname: 'user2',
    since: '2020/09/01',
  },
  {
    icon: 'assets/profile-placeholder.png',
    nickname: 'user2',
    since: '2020/10/01',
  },
  {
    icon: 'assets/profile-placeholder.png',
    nickname: 'user3',
    since: '2020/11/01',
  },
  {
    icon: 'assets/profile-placeholder.png',
    nickname: 'user4',
    since: '2020/12/01',
  },
];

@Component({
  selector: 'app-all-friends',
  templateUrl: './all-friends.component.html',
  styleUrls: ['./all-friends.component.scss'],
})
export class AllFriendsComponent implements OnInit {
  friends = FRIENDS;
  page = 1;
  pageSize = 4;
  collectionSize = FRIENDS.length;
  constructor() {}

  ngOnInit(): void {
    this.refreshFriends();
  }

  refreshFriends() {
    this.friends = FRIENDS.slice(
      (this.page - 1) * this.pageSize,
      (this.page - 1) * this.pageSize + this.pageSize
    );
  }
}
