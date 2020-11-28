import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {CurrentUserInterface} from 'src/app/shared/types/currentUser.interface';
import {SignUpRequestInterface} from '../types/signUpRequest.interface';

@Injectable()
export class IdentityService {
  signUp(payload: SignUpRequestInterface): Observable<CurrentUserInterface> {
    const observable: Observable<CurrentUserInterface> = new Observable((s) => {
      setTimeout(() => {
        s.error({message: 'dupa'});
        s.next({
          id: '1234',
          createdAt: '1',
          email: payload.email,
          token: '123',
        });
        s.complete();
      }, 300);
    });
    return observable;
  }
}
