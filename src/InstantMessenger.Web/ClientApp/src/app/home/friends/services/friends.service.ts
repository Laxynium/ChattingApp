import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {forkJoin, Observable, of, zip} from 'rxjs';
import {switchMap} from 'rxjs/operators';
import {FriendshipInterface} from 'src/app/home/friends/types/friendship.interface';
import {
  InvitationFullInterface,
  InvitationInterface,
} from 'src/app/home/friends/types/invitation.interface';
import {UserInterface} from 'src/app/shared/types/user.interface';
import {environment} from 'src/environments/environment';

interface FriendshipDto {
  friendshipId: string;
  me: string;
  friend: string;
  createdAt: string;
}

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
  acceptInvitation(invitationId: string) {
    return this.http.post(`${this.friendsUrl}/invitations/accept`, {
      invitationId: invitationId,
    });
  }
  rejectInvitation(invitationId: string) {
    return this.http.post(`${this.friendsUrl}/invitations/reject`, {
      invitationId: invitationId,
    });
  }
  cancelInvitation(invitationId: string) {
    return this.http.post(`${this.friendsUrl}/invitations/cancel`, {
      invitationId: invitationId,
    });
  }
  getAllInvitations(): Observable<InvitationFullInterface[]> {
    return this.http
      .get<InvitationInterface[]>(`${this.friendsUrl}/invitations/pending`)
      .pipe(
        switchMap((inviations) => {
          if (inviations.length == 0) return of([]);

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
                  isLoading: false,
                })
              )
            );
          });
          return forkJoin(fullInvitations);
        })
      );
  }

  getFriends(): Observable<FriendshipInterface[]> {
    return this.http.get<FriendshipDto[]>(`${this.friendsUrl}`).pipe(
      switchMap((friendships) => {
        if (friendships.length == 0) return of([]);

        const fullFriendships = friendships.map((friendship) => {
          return this.http
            .get<UserInterface>(
              `${this.identityUrl}/users/${friendship.friend}`
            )
            .pipe(
              switchMap((x) =>
                of<FriendshipInterface>({
                  id: friendship.friendshipId,
                  createdAt: friendship.createdAt,
                  friend: x,
                })
              )
            );
        });
        return forkJoin(fullFriendships);
      })
    );
  }
}
