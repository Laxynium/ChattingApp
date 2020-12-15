import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {combineLatest, forkJoin, Observable, zip} from 'rxjs';
import {combineAll, concatMap, map} from 'rxjs/operators';
import {PermissionDto} from 'src/app/home/groups/store/types/permission';
import {RoleDto} from 'src/app/home/groups/store/types/role';
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
