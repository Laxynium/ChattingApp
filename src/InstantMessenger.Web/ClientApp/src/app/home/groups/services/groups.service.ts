import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Set} from 'immutable';
import {Observable} from 'rxjs';
import {concatMap, map} from 'rxjs/operators';
import {
  ChannelDto,
  GroupDto,
} from 'src/app/home/groups/services/responses/group.dto';
import {AllowedAction} from 'src/app/home/groups/store/types/allowed-action';
import {
  GenerateInvitationRequest,
  InvitationDto,
} from 'src/app/home/groups/store/types/invitation';
import {environment} from 'src/environments/environment';

@Injectable()
export class GroupsService {
  private groupApi = `${environment.apiUrl}/groups`;
  private channelsApi(groupId) {
    return `${this.groupApi}/${groupId}/channels`;
  }
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

  public generateInvitation(
    request: GenerateInvitationRequest
  ): Observable<{groupId: string; invitationId: string; code: string}> {
    return this.http
      .post(`${this.groupApi}/${request.groupId}/invitations`, request)
      .pipe(
        concatMap((_) =>
          this.http.get<{groupId: string; invitationId: string; code: string}>(
            `${this.groupApi}/${request.groupId}/invitations/${request.invitationId}`
          )
        )
      );
  }

  public revokeInvitation(request: {
    groupId: string;
    invitationId: string;
  }): Observable<Object> {
    return this.http.delete(
      `${this.groupApi}/${request.groupId}/invitations/${request.invitationId}`
    );
  }

  public getGroups(): Observable<GroupDto[]> {
    return this.http.get<GroupDto[]>(this.groupApi);
  }

  public getGroup(groupId): Observable<GroupDto> {
    return this.http.get<GroupDto>(`${this.groupApi}/${groupId}`);
  }

  public getInvitations(request: {
    groupId: string;
  }): Observable<InvitationDto[]> {
    return this.http.get<InvitationDto[]>(
      `${this.groupApi}/${request.groupId}/invitations`
    );
  }

  public createChannel(request: {
    groupId: string;
    channelId: string;
    channelName: string;
  }): Observable<ChannelDto> {
    return this.http.post(this.channelsApi(request.groupId), request).pipe(
      concatMap((_) => {
        return this.http.get<ChannelDto>(
          `${this.channelsApi(request.groupId)}/${request.channelId}`
        );
      })
    );
  }

  public removeChannel(request: {
    groupId: string;
    channelId: string;
  }): Observable<Object> {
    return this.http.delete(
      `${this.channelsApi(request.groupId)}/${request.channelId}`
    );
  }

  public getChannels(groupId: String): Observable<ChannelDto[]> {
    return this.http.get<ChannelDto[]>(this.channelsApi(groupId));
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
