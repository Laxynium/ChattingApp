import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {mergeMap} from 'rxjs/operators';
import {Profile} from 'src/app/home/profile/types/profile';
import {UploadAvatarRequest} from 'src/app/home/profile/types/uploadAvatar.request';
import {environment} from 'src/environments/environment';

@Injectable()
export class ProfileService {
  private profilesUrl = `${environment.apiUrl}/profiles`;
  private avatarUrl = `${this.profilesUrl}/avatar`;
  constructor(private http: HttpClient) {}
  uploadAvatar(request: UploadAvatarRequest): Observable<Profile> {
    const data = new FormData();
    data.append('image', request.file, request.file.name);
    return this.http
      .post(this.avatarUrl, data)
      .pipe(mergeMap(() => this.getProfile()));
  }
  getProfile() {
    return this.http.get<Profile>(this.profilesUrl);
  }
}
