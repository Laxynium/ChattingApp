import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {select, Store} from '@ngrx/store';
import {tap} from 'rxjs/operators';
import {logoutActiion} from 'src/app/identity/store/actions/logout.actions';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {getCurrentUser} from 'src/app/identity/store/actions/getCurrentUser.actions';
import {Observable} from 'rxjs';
import {
  avatarSelector,
  nicknameSelector,
} from 'src/app/identity/store/selectors';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  links = [
    {title: 'Profile', link: '/profile'},
    {title: 'Friends', link: '/friends'},
    {title: 'Messages', link: '/messages'},
    {title: 'Groups', link: '/groups'},
  ];

  toggleMenuIcon = faBars;
  isMenuVisible = true;
  $nickname: Observable<string>;
  $avatar: Observable<string>;
  constructor(public router: Router, private store: Store) {
    this.$nickname = this.store.pipe(select(nicknameSelector));
    this.$avatar = this.store.pipe(select(avatarSelector));
  }
  ngOnInit(): void {
    this.store.dispatch(getCurrentUser());
  }

  logout() {
    this.store.dispatch(logoutActiion());
  }
}
