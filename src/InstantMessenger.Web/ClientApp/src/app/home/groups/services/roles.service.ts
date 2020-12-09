import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {forkJoin, Observable, zip} from 'rxjs';
import {concatMap, map} from 'rxjs/operators';
import {PermissionDto} from 'src/app/home/groups/store/types/permission';
import {RoleDto} from 'src/app/home/groups/store/types/role';
import {environment} from 'src/environments/environment';

@Injectable()
export class RolesService {
  private groupApi = `${environment.apiUrl}/groups`;
  private rolesApi(groupId) {
    return `${this.groupApi}/${groupId}/roles`;
  }
  constructor(private http: HttpClient) {}

  public getRoles(r: {groupId: string}): Observable<RoleDto[]> {
    return this.http.get<RoleDto[]>(`${this.rolesApi(r.groupId)}`);
  }

  public getRolePermissions(r: {
    groupId: string;
    roleId: string;
  }): Observable<PermissionDto[]> {
    return zip(
      this.http.get<PermissionResponseDto[]>(
        `${this.groupApi}/${r.groupId}/roles/${r.roleId}/permissions`
      ),
      this.http.get<PermissionResponseDto[]>(
        `${this.groupApi}/${r.groupId}/permissions`
      )
    ).pipe(
      map(([rolePermissions, allPermissions]) =>
        allPermissions.reduce(
          (agg: PermissionDto[], cur: PermissionResponseDto) => [
            ...agg,
            <PermissionDto>{
              name: cur.name,
              code: cur.code,
              isOn: rolePermissions.some((x) => x.name == cur.name),
            },
          ],
          []
        )
      )
    );
  }

  public updateRolePermissions(r: {
    groupId: string;
    roleId: string;
    permissions: PermissionDto[];
  }): Observable<Object> {
    const requests = r.permissions.map((p) => {
      if (p.isOn) {
        return this.http.post(
          `${this.groupApi}/${r.groupId}/roles/${r.roleId}/permissions`,
          {
            groupId: r.groupId,
            roleId: r.roleId,
            permissionName: p.name,
          }
        );
      } else {
        return this.http.delete(
          `${this.groupApi}/${r.groupId}/roles/${r.roleId}/permissions/${p.name}`
        );
      }
    });
    return forkJoin(requests);
  }

  public createRole(r: {
    groupId: string;
    roleId: string;
    name: string;
  }): Observable<RoleDto> {
    return this.http
      .post(`${this.rolesApi(r.groupId)}`, r)
      .pipe(
        concatMap((_) =>
          this.http.get<RoleDto>(`${this.rolesApi(r.groupId)}/${r.roleId}`)
        )
      );
  }

  public removeRole(r: {groupId: string; roleId: string}): Observable<Object> {
    return this.http.delete(`${this.rolesApi(r.groupId)}/${r.roleId}`);
  }

  public addPermission(r: {
    groupId: string;
    roleId: string;
    permissionName: string;
  }): Observable<Object> {
    return this.http.post(
      `${this.rolesApi(r.groupId)}/${r.roleId}/permissions`,
      r
    );
  }

  public removePermission(r: {
    groupId: string;
    roleId: string;
    permissionName: string;
  }): Observable<Object> {
    return this.http.delete(
      `${this.rolesApi(r.groupId)}/${r.roleId}/permissions/${r.permissionName}`
    );
  }
}

interface PermissionResponseDto {
  name: string;
  code: string;
}
