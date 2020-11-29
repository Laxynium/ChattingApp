import {HttpClient, HttpHeaders, HttpResponse} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {map, mergeMap} from 'rxjs/operators';
import {CurrentUserInterface} from 'src/app/shared/types/currentUser.interface';
import {environment} from 'src/environments/environment';
import {SignUpRequestInterface} from '../types/signUpRequest.interface';
import {ActivateRequestInterface} from '../types/ActivateRequest.interface';
import {SignInRequestInterface} from 'src/app/identity/types/signInRequest.interface';
import {SignInResponseInterface} from '../../shared/types/signIn.response';
import {ForgotPasswordRequestInterface} from '../types/forgotPasswordRequest.interface';
import {ResetPasswordRequestInterface} from '../types/resetPasswordRequest.interface';

@Injectable()
export class IdentityService {
  [x: string]: any;
  constructor(private http: HttpClient) {}
  signUp(payload: SignUpRequestInterface): Observable<{}> {
    const url = `${environment.apiUrl}/identity/sign-up`;
    return this.http.post(url, payload).pipe(map((_) => ({})));
  }
  activate(payload: ActivateRequestInterface): Observable<{}> {
    const url = `${environment.apiUrl}/identity/activate`;
    return this.http.post(url, payload).pipe(map((_) => ({})));
  }

  signIn(payload: SignInRequestInterface): Observable<CurrentUserInterface> {
    const url = `${environment.apiUrl}/identity/sign-in`;
    const meUrl = `${environment.apiUrl}/identity/me`;
    return this.http.post<SignInResponseInterface>(url, payload).pipe(
      mergeMap((r) => {
        const headers = new HttpHeaders({
          Authorization: `Bearer ${r.token}`,
        });
        return this.http
          .get<any>(meUrl, {headers: headers})
          .pipe(
            map<any, CurrentUserInterface>((response) => ({
              id: response.id,
              nickname: response.nickname,
              email: response.email,
              token: r.token,
            }))
          );
      })
    );
  }

  forgotPassword(payload: ForgotPasswordRequestInterface): Observable<{}> {
    const url = `${environment.apiUrl}/identity/forgot-password`;
    return this.http.post(url, payload).pipe(map((_) => ({})));
  }

  resetPassword(payload: ResetPasswordRequestInterface): Observable<{}> {
    const url = `${environment.apiUrl}/identity/reset-password`;
    return this.http.post(url, payload).pipe(map((_) => ({})));
  }
}
