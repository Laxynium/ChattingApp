import {Component, OnInit} from '@angular/core';
import {Observable} from 'rxjs';
import {RolesService} from 'src/app/home/groups/services/roles.service';
import {PermissionDto} from 'src/app/home/groups/store/types/permission';
import {v4 as guid} from 'uuid';

@Component({
  selector: 'app-manage-role-permissions',
  templateUrl: './manage-role-permissions.component.html',
  styleUrls: ['./manage-role-permissions.component.scss'],
})
export class ManageRolePermissionsComponent implements OnInit {
  $permissions: Observable<PermissionDto[]>;
  constructor(private rolesService: RolesService) {}

  ngOnInit(): void {
    this.$permissions = this.rolesService.getPermissions({groupId: guid()});
  }
}
