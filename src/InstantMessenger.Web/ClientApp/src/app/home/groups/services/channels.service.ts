import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable, zip} from 'rxjs';
import {map} from 'rxjs/operators';
import {ChannelDto} from 'src/app/home/groups/services/responses/group.dto';
import {
  PermissionOverrideDto,
  PermissionOverrideTypeDto,
} from 'src/app/home/groups/store/types/role-permission-override';
import {environment} from 'src/environments/environment';

@Injectable()
export class ChannelsService {
  private groupApi = `${environment.apiUrl}/groups`;
  private channelsApi(groupId) {
    return `${this.groupApi}/${groupId}/channels`;
  }
  constructor(private http: HttpClient) {}

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
    overrides: PermissionOverrideDto[];
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
    overrides: PermissionOverrideDto[];
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
  }): Observable<PermissionOverrideDto[]> {
    return zip(
      this.http.get<PermissionOverrideDto[]>(
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
          (agg: PermissionOverrideDto[], cur: PermissionResponseDto) => {
            return [
              ...agg,
              <PermissionOverrideDto>{
                permission: cur.name,
                type: os.some((o) => o.permission == cur.name)
                  ? os.find((o) => o.permission == cur.name).type
                  : PermissionOverrideTypeDto.Neutral,
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
  }): Observable<PermissionOverrideDto[]> {
    return zip(
      this.http.get<PermissionOverrideDto[]>(
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
          (agg: PermissionOverrideDto[], cur: PermissionResponseDto) => {
            return [
              ...agg,
              <PermissionOverrideDto>{
                permission: cur.name,
                type: os.some((o) => o.permission == cur.name)
                  ? os.find((o) => o.permission == cur.name).type
                  : PermissionOverrideTypeDto.Neutral,
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
