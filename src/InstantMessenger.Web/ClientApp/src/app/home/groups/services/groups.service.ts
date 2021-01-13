import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Set} from 'immutable';
import {Observable} from 'rxjs';
import {concatMap, map} from 'rxjs/operators';
import {GroupDto} from 'src/app/home/groups/services/responses/group.dto';
import {AllowedAction} from 'src/app/home/groups/store/types/allowed-action';
import {environment} from 'src/environments/environment';

@Injectable()
export class GroupsService {
  private groupApi = `${environment.apiUrl}/groups`;
  constructor(private http: HttpClient) {}

  public createGroup(request: {
    groupId: string;
    groupName: string;
  }): Observable<GroupDto> {
    return this.http
      .post(this.groupApi, {
        groupId: request.groupId,
        groupName: request.groupName,
      })
      .pipe(
        concatMap((_) =>
          this.http.get<GroupDto>(`${this.groupApi}/${request.groupId}`)
        )
      );
  }

  public joinGroup(request: {invitationCode: string}): Observable<Object> {
    return this.http.post(
      `${this.groupApi}/join/${request.invitationCode}`,
      {}
    );
  }

  public removeGroup(request: {groupId: string}): Observable<Object> {
    return this.http.delete(`${this.groupApi}/${request.groupId}`);
  }

  public renameGroup(request: GroupDto): Observable<Object> {
    return this.http.put(`${this.groupApi}/${request.groupId}`, request);
  }

  public getGroups(): Observable<GroupDto[]> {
    return this.http.get<GroupDto[]>(this.groupApi);
  }

  public getGroup(groupId): Observable<GroupDto> {
    return this.http.get<GroupDto>(`${this.groupApi}/${groupId}`);
  }

  public getAllowedActions(groupId: string): Observable<AllowedAction[]> {
    return this.http
      .get<{name: string; isChannelSpecific: boolean; channels: string[]}[]>(
        `${this.groupApi}/${groupId}/allowed-actions`
      )
      .pipe(
        map((aa) =>
          aa.map((a) => ({
            name: a.name,
            isChannelSpecific: a.isChannelSpecific,
            channels: Set(a.channels),
          }))
        )
      );
  }
}
