import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {Store} from '@ngrx/store';
import {tap} from 'rxjs/operators';
import {logoutActiion} from 'src/app/identity/store/actions/logout.actions';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  links = [
    {title: 'Profile', fragment: 'one', link: '/profile'},
    {title: 'Friends', fragment: 'two', link: '/friends'},
    {title: 'Messages', fragment: 'Four', link: '/messages'},
    {title: 'Groups', fragment: 'Three', link: '/groups'},
  ];
  constructor(public router: Router, private store: Store) {}

  ngOnInit(): void {}

  logout() {
    this.store.dispatch(logoutActiion());
  }
}
