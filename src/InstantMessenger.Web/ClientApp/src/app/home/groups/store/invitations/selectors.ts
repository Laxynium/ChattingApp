import {createSelector} from '@ngrx/store';
import {InvitationDto} from 'src/app/home/groups/store/types/invitation';
import {currentGroupSelector, invitationsStateSelector} from "../selectors";
import {invitationAdapter, InvitationsState} from 'src/app/home/groups/store/invitations/invitation.reducer';
import {Group} from "src/app/home/groups/store/groups/group.reducer";

const {selectAll} = invitationAdapter.getSelectors();

export const currentInvitationSelector = createSelector(
  invitationsStateSelector,
  (s: InvitationsState): {code: string; isBeingGenerated: boolean} => ({
    code: s?.generated?.code ?? '',
    isBeingGenerated: s?.generated?.isBeingGenerated ?? false,
  })
);

export const invitationsSelector = createSelector(
  invitationsStateSelector,
  currentGroupSelector,
  (s: InvitationsState,g: Group): InvitationDto[] => selectAll(s).filter(x=>x.groupId == g.id)
);

export const isInvitationBeingGeneratedSelector = createSelector(
  invitationsStateSelector,
  (s: InvitationsState): boolean => s.generated.isBeingGenerated
);
