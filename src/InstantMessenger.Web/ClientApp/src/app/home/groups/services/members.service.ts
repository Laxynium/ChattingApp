import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {forkJoin, Observable, of} from 'rxjs';
import {map, switchMap} from 'rxjs/operators';
import {MemberDto} from 'src/app/home/groups/store/types/member';
import {MemberRoleDto} from 'src/app/home/groups/store/types/member-role';
import {RoleDto} from 'src/app/home/groups/store/types/role';
import {UserInterface} from 'src/app/shared/types/user.interface';
import {environment} from 'src/environments/environment';

@Injectable()
export class MembersService {
  private identityUrl = `${environment.apiUrl}/identity`;
  private groupApi = `${environment.apiUrl}/groups`;
  private membersApi(groupId) {
    return `${this.groupApi}/${groupId}/members`;
  }
  constructor(private http: HttpClient) {}

  public getMembers(r: {groupId: string}): Observable<MemberDto[]> {
    return this.http.get<MemberDto[]>(`${this.membersApi(r.groupId)}`).pipe(
      switchMap((members) => {
        if (members.length == 0) return of([]);

        return forkJoin(
          members.map((m) => {
            return this.http
              .get<UserInterface>(`${this.identityUrl}/users/${m.userId}`)
              .pipe(
                switchMap((u) =>
                  of<MemberDto>({
                    groupId: m.groupId,
                    userId: m.userId,
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

  public addRoleToMember(r: MemberRoleDto): Observable<Object> {
    return this.http.post(`${this.membersApi(r.groupId)}/${r.userId}/roles`, {
      groupId: r.groupId,
      memberUserId: r.userId,
      roleId: r.role.roleId,
    });
  }

  public removeRoleFromMember(r: MemberRoleDto): Observable<Object> {
    return this.http.delete(
      `${this.membersApi(r.groupId)}/${r.userId}/roles/${r.role.roleId}`
    );
  }

  public getMemberRoles(r: {
    groupId: string;
    userId: string;
  }): Observable<RoleDto[]> {
    return this.http.get<RoleDto[]>(
      `${this.membersApi(r.groupId)}/${r.userId}/roles`
    );
  }
}
