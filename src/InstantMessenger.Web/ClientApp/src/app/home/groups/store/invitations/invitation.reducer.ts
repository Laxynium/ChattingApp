import {createEntityAdapter, EntityState} from '@ngrx/entity';
import {InvitationDto} from 'src/app/home/groups/store/types/invitation';
import {createReducer, on} from '@ngrx/store';
import {
  generateInvitationAction, generateInvitationFailureAction, generateInvitationSuccessAction,
  getInvitationsSuccessAction,
  revokeInvitationSuccessAction,
} from 'src/app/home/groups/store/invitations/actions';

export interface GeneratedInvitation {
  groupId: string;
  invitationId: string;
  code: string;
  isBeingGenerated: boolean;
}

export interface InvitationsState extends EntityState<InvitationDto> {
  isLoading: boolean;
  generated: GeneratedInvitation | null;
}

export const invitationAdapter = createEntityAdapter<InvitationDto>({
  selectId: (x) => x.invitationId,
});
export const invitationReducer = createReducer(
  invitationAdapter.getInitialState({
    isLoading: false,
    generated: null,
  }),
  on(revokeInvitationSuccessAction, (s, {invitationId}) => ({
    ...invitationAdapter.removeOne(invitationId, s)
  })),
  on(getInvitationsSuccessAction, (s, {invitations}) => ({
    ...invitationAdapter.setAll(invitations, s)
  })),
  on(generateInvitationAction, (s) => ({
    ...s,
    generated: {
      isBeingGenerated: true,
    },
  })),
  on(generateInvitationSuccessAction, (s,a)=> ({
    ...s,
    generated: {
      groupId: a.groupId,
      invitationId: a.invitationId,
      isBeingGenerated: false,
      code: a.code
    }
  })),
  on(generateInvitationFailureAction, (s,a)=>({
    ...s,
    generated: {
      ...s.generated,
      isBeingGenerated: false,
      code: null,
    }
  }))
);
