import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable, zip} from 'rxjs';
import {concatMap, map} from 'rxjs/operators';
import {ChannelDto} from 'src/app/home/groups/services/responses/group.dto';
import {environment} from 'src/environments/environment';
import {RolePermissionOverride} from 'src/app/home/groups/store/channels/channel.override.role.reducer';
import {MemberPermissionOverride} from 'src/app/home/groups/store/channels/channel.override.member.reducer';
import {PermissionOverrideType} from 'src/app/home/groups/store/types';

@Injectable()
export class ChannelsService {
  private groupApi = `${environment.apiUrl}/groups`;

  private channelsApi(groupId) {
    return `${this.groupApi}/${groupId}/channels`;
  }

  constructor(private http: HttpClient) {}

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

  public getChannels(groupId: String): Observable<ChannelDto[]> {
    return this.http.get<ChannelDto[]>(this.channelsApi(groupId));
  }

  public removeChannel(request: {
    groupId: string;
    channelId: string;
  }): Observable<Object> {
    return this.http.delete(
      `${this.channelsApi(request.groupId)}/${request.channelId}`
    );
  }

  public renameChannel(r: ChannelDto) {
    return this.http.put(`${this.channelsApi(r.groupId)}/${r.channelName}`, {
      groupId: r.groupId,
      channelId: r.channelId,
      name: r.channelName,
    });
  }

  public updateRolePermissionOverrides(r: {
    groupId: string;
    channelId: string;
    roleId: string;
    overrides: RolePermissionOverride[];
  }): Observable<Object> {
    return this.http.put(
      `${this.channelsApi(r.groupId)}/${r.channelId}/permission-overrides/role`,
      r
    );
  }

  public updateMemberPermissionOverrides(r: {
    groupId: string;
    channelId: string;
    memberUserId: string;
    overrides: MemberPermissionOverride[];
  }): Observable<Object> {
    return this.http.put(
      `${this.channelsApi(r.groupId)}/${
        r.channelId
      }/permission-overrides/member`,
      r
    );
  }

  public getRolePermissionOverrides(r: {
    groupId: string;
    channelId: string;
    roleId: string;
  }): Observable<RolePermissionOverride[]> {
    return zip(
      this.http.get<RolePermissionOverride[]>(
        `${this.channelsApi(r.groupId)}/${
          r.channelId
        }/permission-overrides/role/${r.roleId}`
      ),
      this.http.get<PermissionResponseDto[]>(
        `${this.channelsApi(r.groupId)}/${r.channelId}/permission-overrides`
      )
    ).pipe(
      map(([os, ps]) => {
        return ps.reduce(
          (agg: RolePermissionOverride[], cur: PermissionResponseDto) => {
            return [
              ...agg,
              <RolePermissionOverride>{
                permission: cur.name,
                type: os.some((o) => o.permission == cur.name)
                  ? os.find((o) => o.permission == cur.name).type
                  : PermissionOverrideType.Neutral,
              },
            ];
          },
          []
        );
      })
    );
  }

  public getMemberPermissionOverrides(r: {
    groupId: string;
    channelId: string;
    memberUserId: string;
  }): Observable<MemberPermissionOverride[]> {
    return zip(
      this.http.get<MemberPermissionOverride[]>(
        `${this.channelsApi(r.groupId)}/${
          r.channelId
        }/permission-overrides/member/${r.memberUserId}`
      ),
      this.http.get<PermissionResponseDto[]>(
        `${this.channelsApi(r.groupId)}/${r.channelId}/permission-overrides`
      )
    ).pipe(
      map(([os, ps]) => {
        return ps.reduce(
          (agg: MemberPermissionOverride[], cur: PermissionResponseDto) => {
            return [
              ...agg,
              <MemberPermissionOverride>{
                permission: cur.name,
                type: os.some((o) => o.permission == cur.name)
                  ? os.find((o) => o.permission == cur.name).type
                  : PermissionOverrideType.Neutral,
              },
            ];
          },
          []
        );
      })
    );
  }
}

interface PermissionResponseDto {
  name: string;
  code: string;
}
