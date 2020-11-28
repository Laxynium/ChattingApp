import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  links = [
    { title: 'One', fragment: 'one'},
    { title: 'Two', fragment: 'two'},
  ]
  constructor(public route: ActivatedRoute){}
}
