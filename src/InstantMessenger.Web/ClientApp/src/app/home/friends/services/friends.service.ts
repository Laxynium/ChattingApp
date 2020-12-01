import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {forkJoin, merge, Observable, of, zip} from 'rxjs';
import {map, mergeMap, switchMap} from 'rxjs/operators';
import {
  InvitationFullInterface,
  InvitationInterface,
} from 'src/app/home/friends/types/invitation.interface';
import {UserInterface} from 'src/app/home/friends/types/user.interface';
import {environment} from 'src/environments/environment';

@Injectable()
export class FriendsService {
  private friendsUrl = `${environment.apiUrl}/friendships`;
  private identityUrl = `${environment.apiUrl}/identity`;
  constructor(private http: HttpClient) {}
  sendInvitation(nickname: string) {
    return this.http.post(this.friendsUrl, {
      receiverNickname: nickname,
    });
  }
  getAllInvitations(): Observable<InvitationFullInterface[]> {
    return this.http
      .get<InvitationInterface[]>(`${this.friendsUrl}/invitations/pending`)
      .pipe(
        mergeMap((inviations) => {
          const fullInvitations = inviations.map((i) => {
            const receiver = this.http.get<UserInterface>(
              `${this.identityUrl}/users/${i.receiverId}`
            );
            const sender = this.http.get<UserInterface>(
              `${this.identityUrl}/users/${i.senderId}`
            );
            return zip(receiver, sender).pipe(
              switchMap((x) =>
                of<InvitationFullInterface>({
                  invitationId: i.invitationId,
                  createdAt: i.createdAt,
                  status: i.status,
                  type: i.type,
                  receiver: x[0],
                  sender: x[1],
                })
              )
            );
          });
          return forkJoin(fullInvitations);
        })
      );
  }
}
