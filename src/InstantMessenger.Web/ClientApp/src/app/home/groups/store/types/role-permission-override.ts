export enum PermissionOverrideTypeDto {
  Allow = 'Allow',
  Deny = 'Deny',
  Neutral = 'Neutral',
}
export interface PermissionOverrideDto {
  permission: string;
  type: PermissionOverrideTypeDto;
}
