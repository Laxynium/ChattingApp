import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {Store} from '@ngrx/store';
import {tap} from 'rxjs/operators';
import {logoutActiion} from 'src/app/identity/store/actions/logout.actions';
import {faBars} from '@fortawesome/free-solid-svg-icons';

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
  constructor(public router: Router, private store: Store) {}
  toggleMenuIcon = faBars;
  isMenuVisible = true;
  ngOnInit(): void {}

  logout() {
    this.store.dispatch(logoutActiion());
  }
}
