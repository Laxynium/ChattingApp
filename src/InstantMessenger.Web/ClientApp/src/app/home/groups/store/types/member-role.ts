import {RoleDto} from 'src/app/home/groups/store/types/role';

export interface MemberRoleDto {
  groupId: string;
  userId: string;
  role: RoleDto;
}
