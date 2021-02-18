import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {forkJoin, Observable, of} from 'rxjs';
import {switchMap} from 'rxjs/operators';
import {UserInterface} from 'src/app/shared/types/user.interface';
import {environment} from 'src/environments/environment';
import {Member} from "src/app/home/groups/store/members/member.reducer";
import {GroupId, RoleId, UserId} from "src/app/home/groups/store/types";
import {Role} from "src/app/home/groups/store/roles/role.redcuer";

@Injectable()
export class MembersService {
  private identityUrl = `${environment.apiUrl}/identity`;
  private groupApi = `${environment.apiUrl}/groups`;
  private membersApi(groupId) {
    return `${this.groupApi}/${groupId}/members`;
  }
  constructor(private http: HttpClient) {}

  public getMembers(r: {groupId: string}): Observable<Member[]> {
    return this.http.get<Member[]>(`${this.membersApi(r.groupId)}`).pipe(
      switchMap((members) => {
        if (members.length == 0) return of([]);

        return forkJoin(
          members.map((m) => {
            return this.http
              .get<UserInterface>(`${this.identityUrl}/users/${m.userId}`)
              .pipe(
                switchMap((u) =>
                  of<Member>({
                    groupId: m.groupId,
                    userId: m.userId,
                    memberId: m.memberId,
                    name: m.name,
                    avatar: u.avatar,
                    isOwner: m.isOwner,
                    createdAt: m.createdAt,
                  })
                )
              );
          })
        );
      })
    );
  }

  public kickMember(r: {groupId: string; userId: string}): Observable<Object> {
    return this.http.delete(`${this.membersApi(r.groupId)}/${r.userId}/kick`);
  }

  public addRoleToMember(r: {groupId: GroupId, userId: UserId,roleId: RoleId}): Observable<Object> {
    return this.http.post(`${this.membersApi(r.groupId)}/${r.userId}/roles`, {
      groupId: r.groupId,
      memberUserId: r.userId,
      roleId: r.roleId,
    });
  }

  public removeRoleFromMember(r: {groupId: GroupId, userId: UserId, roleId: RoleId}): Observable<Object> {
    return this.http.delete(
      `${this.membersApi(r.groupId)}/${r.userId}/roles/${r.roleId}`
    );
  }

  public getMemberRoles(r: {
    groupId: string;
    userId: string;
  }): Observable<Role[]> {
    return this.http.get<Role[]>(
      `${this.membersApi(r.groupId)}/${r.userId}/roles`
    );
  }
}
