import {HttpClient, HttpHeaders, HttpResponse} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {map, mergeMap} from 'rxjs/operators';
import {CurrentUserInterface} from 'src/app/shared/types/currentUser.interface';
import {environment} from 'src/environments/environment';
import {SignInRequestInterface} from 'src/app/identity/types/signInRequest.interface';
import {UploadAvatarRequest} from 'src/app/identity/types/uploadAvatar.request';
import {SignUpRequestInterface} from 'src/app/identity/types/signUpRequest.interface';
import {ActivateRequestInterface} from 'src/app/identity/types/activateRequest.interface';
import {SignInResponseInterface} from 'src/app/shared/types/signIn.response';
import {ForgotPasswordRequestInterface} from 'src/app/identity/types/forgotPasswordRequest.interface';
import {ResetPasswordRequestInterface} from 'src/app/identity/types/resetPasswordRequest.interface';

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
              avatar: null,
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

  changeNickname(nickname: string): Observable<string> {
    const url = `${environment.apiUrl}/identity/nickname`;
    return this.http.put(url, {nickname: nickname}).pipe(map(() => nickname));
  }

  uploadAvatar(request: UploadAvatarRequest): Observable<CurrentUserInterface> {
    const url = `${environment.apiUrl}/identity/avatar`;
    const data = new FormData();
    data.append('image', request.file, request.file.name);
    return this.http
      .post(url, data)
      .pipe(mergeMap(() => this.getCurrentUser()));
  }

  getCurrentUser() {
    const url = `${environment.apiUrl}/identity/me`;
    return this.http.get<CurrentUserInterface>(url);
  }
}
