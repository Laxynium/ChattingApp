import {CommonModule} from '@angular/common';
import {NgModule} from '@angular/core';
import {ReactiveFormsModule} from '@angular/forms';
import {RouterModule} from '@angular/router';
import {HomeComponent} from './components/home/home.component';
import {AuthGuard} from '../identity/guards/auth.guard';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

const routes = [
  {
    path: '',
    component: HomeComponent,
    canActivate: [AuthGuard],
    children: [],
  },
  {path: '**', redirectTo: '/'},
];

@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
    NgbModule,
  ],
  providers: [],
})
export class HomeModule {}
