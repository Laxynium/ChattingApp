export interface ICurrentGroup {
  readonly groupId: string;
  readonly tag: string;
}
export class CurrentGroup implements ICurrentGroup {
  tag = 'current_group';
  constructor(public groupId: string) {
    if (groupId == null || groupId.trim() === '') {
      throw new Error('Group id cannot be null or empty string');
    }
  }
}
export class EmptyCurrentGroup implements ICurrentGroup {
  tag = 'empty_current_group';
  groupId: '';
}
