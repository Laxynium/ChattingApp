import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {InvitationInterface} from 'src/app/home/friends/types/invitation.interface';
import {environment} from 'src/environments/environment';

@Injectable()
export class FriendsService {
  private friendsUrl = `${environment.apiUrl}/friendships`;
  constructor(private http: HttpClient) {}
  sendInvitation(nickname: string) {
    return this.http.post(this.friendsUrl, {
      receiverNickname: nickname,
    });
  }
  getAllInvitations() {
    return this.http
      .get<InvitationInterface[]>(`${this.friendsUrl}/invitations/pending`)
      .pipe();
  }
}
