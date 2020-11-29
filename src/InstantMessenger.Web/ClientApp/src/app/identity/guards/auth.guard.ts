import {Injectable} from '@angular/core';
import {Store, select} from '@ngrx/store';
import {CurrentUserInterface} from '../../shared/types/currentUser.interface';
import {Observable} from 'rxjs';
import {
  Router,
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
} from '@angular/router';
import {currentUser} from 'src/app/identity/store/selectors';
import {map, tap} from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  $currentUser: Observable<CurrentUserInterface | null>;
  constructor(private router: Router, private store: Store) {
    this.$currentUser = this.store.pipe(select(currentUser));
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | boolean {
    return this.$currentUser.pipe(
      map((x) => (x ? true : false)),
      tap((x) => {
        if (x === false) {
          this.router.navigate(['/sign-in'], {
            queryParams: {returnUrl: state.url},
          });
        }
      })
    );
  }
}
