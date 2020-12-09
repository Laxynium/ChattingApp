import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {concatMap} from 'rxjs/operators';
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

  public getPermissions(r: {groupId: string}): Observable<PermissionDto[]> {
    return this.http.get<PermissionDto[]>(
      `${this.groupApi}/${r.groupId}/permissions`
    );
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
