import {HttpClient, HttpResponse} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {map} from 'rxjs/operators';
import {CurrentUserInterface} from 'src/app/shared/types/currentUser.interface';
import {environment} from 'src/environments/environment';
import {SignUpRequestInterface} from '../types/signUpRequest.interface';

@Injectable()
export class IdentityService {
  constructor(private http: HttpClient) {}
  signUp(payload: SignUpRequestInterface): Observable<CurrentUserInterface> {
    const url = `${environment.apiUrl}/identity/sign-up`;
    return this.http.post(url, payload).pipe(map((_) => ({})));
  }
}
