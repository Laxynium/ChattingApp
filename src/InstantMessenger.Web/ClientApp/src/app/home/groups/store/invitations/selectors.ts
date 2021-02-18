import {createSelector} from '@ngrx/store';
import {currentGroupSelector, invitationsStateSelector} from "../selectors";
import {
  Invitation,
  invitationAdapter,
  InvitationsState
} from 'src/app/home/groups/store/invitations/reducer';
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
  (s: InvitationsState,g: Group): Invitation[] => selectAll(s).filter(x=>x.groupId == g.id)
);

export const isInvitationBeingGeneratedSelector = createSelector(
  invitationsStateSelector,
  (s: InvitationsState): boolean => s.generated.isBeingGenerated
);
