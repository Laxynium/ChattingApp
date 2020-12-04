import {Component, OnInit} from '@angular/core';
import {NavigationEnd, Router} from '@angular/router';
import {select, Store} from '@ngrx/store';
import {logoutActiion} from 'src/app/identity/store/actions/logout.actions';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {getCurrentUser} from 'src/app/identity/store/actions/getCurrentUser.actions';
import {Observable} from 'rxjs';
import {
  avatarSelector,
  nicknameSelector,
} from 'src/app/identity/store/selectors';
import {HubService} from 'src/app/shared/services/hub.service';
import {filter, map} from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  links = [
    {title: 'Profile', link: '/profile'},
    {title: 'Friends', link: '/friends'},
    {title: 'Conversations', link: '/conversations'},
    {title: 'Groups', link: '/groups'},
  ];
  $currentPath: Observable<string>;
  toggleMenuIcon = faBars;
  isMenuVisible = true;
  $nickname: Observable<string>;
  $avatar: Observable<string>;
  constructor(
    private router: Router,
    private store: Store,
    private hubService: HubService // there injection is required in order to start all connections to hub
  ) {
    this.$currentPath = this.router.events.pipe(
      filter((e) => e instanceof NavigationEnd),
      map((e) => e as NavigationEnd),
      map((e) => this.map(e.urlAfterRedirects))
    );
    this.$nickname = this.store.pipe(select(nicknameSelector));
    this.$avatar = this.store.pipe(select(avatarSelector));
  }
  ngOnInit(): void {
    this.store.dispatch(getCurrentUser());
  }

  logout() {
    this.store.dispatch(logoutActiion());
  }

  private map(url: string) {
    if (!url) {
      return '/profile';
    }
    if (url === '/') return '/profile';
    const splited = url.split('/', 2);
    const path = splited[1];
    return `/${path}`;
  }
}
