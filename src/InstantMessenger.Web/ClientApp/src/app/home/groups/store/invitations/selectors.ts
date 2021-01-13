import {createSelector} from '@ngrx/store';
import {GroupsStateInterface} from 'src/app/home/groups/store/reducers';
import {groupsFeatureSelector} from 'src/app/home/groups/store/groups/selectors';
import {InvitationDto} from 'src/app/home/groups/store/types/invitation';

export const currentInvitationSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): {code: string; isBeingGenerated: boolean} => ({
    code: s.generatedInvitation.code,
    isBeingGenerated: s.generatedInvitation.isBeingGenerated,
  })
);

export const invitationsSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): InvitationDto[] => s.invitations
);

export const isInvitationBeingGeneratedSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): boolean => s.generatedInvitation.isBeingGenerated
);
