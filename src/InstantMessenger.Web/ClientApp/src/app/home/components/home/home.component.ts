import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  links = [
    {title: 'Profile', fragment: 'one'},
    {title: 'Friends', fragment: 'two'},
    {title: 'Messages', fragment: 'Four'},
    {title: 'Groups', fragment: 'Three'},
  ];
  constructor(public route: ActivatedRoute) {}

  ngOnInit(): void {}
}
