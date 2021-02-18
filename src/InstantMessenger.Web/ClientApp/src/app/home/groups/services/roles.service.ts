import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable, zip} from 'rxjs';
import {concatMap, map} from 'rxjs/operators';
import {environment} from 'src/environments/environment';
import {Role} from "src/app/home/groups/store/roles/role.redcuer";
import {RolePermission} from "src/app/home/groups/store/roles/role.permission.reducer";

@Injectable()
export class RolesService {
  private groupApi = `${environment.apiUrl}/groups`;
  private rolesApi(groupId) {
    return `${this.groupApi}/${groupId}/roles`;
  }
  constructor(private http: HttpClient) {}

  public getRoles(r: {groupId: string}): Observable<Role[]> {
    return this.http.get<Role[]>(`${this.rolesApi(r.groupId)}`);
  }

  public getRolePermissions(r: {
    groupId: string;
    roleId: string;
  }): Observable<RolePermission[]> {
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
          (agg: RolePermission[], cur: PermissionResponseDto) => [
            ...agg,
            <RolePermission>{
              groupId: r.groupId,
              roleId: r.roleId,
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
    permissions: RolePermission[];
  }): Observable<Object> {
    return this.http.put(
      `${this.groupApi}/${r.groupId}/roles/${r.roleId}/permissions`,
      {
        groupId: r.groupId,
        roleId: r.roleId,
        permissions: r.permissions.map((p) => ({
          permission: p.name,
          isOn: p.isOn,
        })),
      }
    );
  }

  public createRole(r: {
    groupId: string;
    roleId: string;
    name: string;
  }): Observable<Role> {
    return this.http
      .post(`${this.rolesApi(r.groupId)}`, r)
      .pipe(
        concatMap((_) =>
          this.http.get<Role>(`${this.rolesApi(r.groupId)}/${r.roleId}`)
        )
      );
  }

  public removeRole(r: {groupId: string; roleId: string}): Observable<Object> {
    return this.http.delete(`${this.rolesApi(r.groupId)}/${r.roleId}`);
  }

  public renameRole(r: Role): Observable<Object> {
    return this.http.put(`${this.rolesApi(r.groupId)}/${r.roleId}`, r);
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

  public moveUpRole(r: {
    groupId: string;
    roleId: string;
  }): Observable<Role[]> {
    return this.http
      .put(`${this.rolesApi(r.groupId)}/move-up`, r)
      .pipe(concatMap((_) => this.getRoles({groupId: r.groupId})));
  }

  public moveDownRole(r: {groupId: string; roleId: string}) {
    return this.http
      .put(`${this.rolesApi(r.groupId)}/move-down`, r)
      .pipe(concatMap((_) => this.getRoles({groupId: r.groupId})));
  }
}

interface PermissionResponseDto {
  name: string;
  code: string;
}
