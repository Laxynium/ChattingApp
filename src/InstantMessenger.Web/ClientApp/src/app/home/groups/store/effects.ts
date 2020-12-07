import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {from, of} from 'rxjs';
import {catchError, map, switchMap} from 'rxjs/operators';
import {GroupsService} from 'src/app/home/groups/services/groups.service';
import {
  changeCurrentGroupAction,
  changeCurrentGroupFailureAction,
  changeCurrentGroupSuccessAction,
  createChannelAction,
  createChannelFailureAction,
  createChannelSuccessAction,
  createGroupAction,
  createGroupFailureAction,
  createGroupSuccessAction,
  generateInvitationAction,
  generateInvitationFailureAction,
  generateInvitationSuccessAction,
  getChannelsAction,
  getChannelsFailureAction,
  getChannelsSuccessAction,
  getGroupsAction,
  getGroupsFailureAction,
  getGroupsSuccessAction,
  getInvitationsAction,
  getInvitationsFailureAction,
  getInvitationsSuccessAction,
  removeChannelAction,
  removeChannelFailureAction,
  removeChannelSuccessAction,
  removeGroupAction,
  removeGroupFailureAction,
  removeGroupSuccessAction,
  revokeInvitationAction,
  revokeInvitationFailureAction,
  revokeInvitationSuccessAction,
} from 'src/app/home/groups/store/actions';
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {v4 as guid} from 'uuid';

@Injectable()
export class GroupsEffects {
  $createGroup = createEffect(() =>
    this.actions$.pipe(
      ofType(createGroupAction),
      switchMap(({groupName}) => {
        const request = {
          groupId: guid(),
          groupName: groupName,
        };
        return this.groupsService.createGroup(request).pipe(
          map((r) => createGroupSuccessAction({group: r})),
          catchError((response) =>
            of(
              createGroupFailureAction(),
              requestFailedAction({error: response})
            )
          )
        );
      })
    )
  );

  $removeGroup = createEffect(() =>
    this.actions$.pipe(
      ofType(removeGroupAction),
      switchMap((request) => {
        return this.groupsService.removeGroup(request).pipe(
          map((r) => removeGroupSuccessAction({groupId: request.groupId})),
          catchError((response) =>
            of(
              removeGroupFailureAction(),
              requestFailedAction({error: response})
            )
          )
        );
      })
    )
  );

  $generateInvitation = createEffect(() =>
    this.actions$.pipe(
      ofType(generateInvitationAction),
      switchMap((request) => {
        return this.groupsService
          .generateInvitation({
            groupId: request.groupId,
            invitationId: guid(),
            expirationTime: request.expirationTime,
            usageCounter: request.usageCounter,
          })
          .pipe(
            map((r) => {
              return generateInvitationSuccessAction({
                groupId: r.groupId,
                invitationId: r.invitationId,
                code: r.code,
              });
            }),
            catchError((response) =>
              of(
                generateInvitationFailureAction(),
                requestFailedAction({error: response})
              )
            )
          );
      })
    )
  );

  $revokeInvitation = createEffect(() =>
    this.actions$.pipe(
      ofType(revokeInvitationAction),
      switchMap((r) => {
        return this.groupsService.revokeInvitation(r).pipe(
          map(() =>
            revokeInvitationSuccessAction({invitationId: r.invitationId})
          ),
          catchError((response) =>
            of(
              revokeInvitationFailureAction(),
              requestFailedAction({error: response})
            )
          )
        );
      })
    )
  );

  $getInvitations = createEffect(() =>
    this.actions$.pipe(
      ofType(getInvitationsAction),
      switchMap(({groupId}) => {
        return this.groupsService.getInvitations({groupId}).pipe(
          map((r) => getInvitationsSuccessAction({invitations: r})),
          catchError((response) =>
            of(
              getInvitationsFailureAction(),
              requestFailedAction({error: response})
            )
          )
        );
      })
    )
  );

  $getGroups = createEffect(() =>
    this.actions$.pipe(
      ofType(getGroupsAction),
      switchMap((_) => {
        return this.groupsService.getGroups().pipe(
          map((r) => getGroupsSuccessAction({groups: r})),
          catchError((response) =>
            of(getGroupsFailureAction(), requestFailedAction({error: response}))
          )
        );
      })
    )
  );
  $changeGroup = createEffect(() =>
    this.actions$.pipe(
      ofType(changeCurrentGroupAction),
      switchMap(({groupId}) => {
        return from(this.router.navigateByUrl(`/groups/${groupId}`)).pipe(
          map((r) =>
            r
              ? changeCurrentGroupSuccessAction({groupId})
              : changeCurrentGroupFailureAction()
          ),
          catchError((e) => of(changeCurrentGroupFailureAction()))
        );
      })
    )
  );
  $createChannel = createEffect(() =>
    this.actions$.pipe(
      ofType(createChannelAction),
      switchMap(({groupId, channelName}) => {
        const request = {
          groupId: groupId,
          channelId: guid(),
          channelName: channelName,
        };
        return this.groupsService.createChannel(request).pipe(
          map((r) => createChannelSuccessAction({channel: r})),
          catchError((response) =>
            of(
              createChannelFailureAction(),
              requestFailedAction({error: response})
            )
          )
        );
      })
    )
  );

  $removeChannel = createEffect(() =>
    this.actions$.pipe(
      ofType(removeChannelAction),
      switchMap((request) => {
        return this.groupsService.removeChannel(request).pipe(
          map((_) =>
            removeChannelSuccessAction({channelId: request.channelId})
          ),
          catchError((response) =>
            of(
              removeChannelFailureAction(),
              requestFailedAction({error: response})
            )
          )
        );
      })
    )
  );

  $getChannels = createEffect(() =>
    this.actions$.pipe(
      ofType(getChannelsAction),
      switchMap(({groupId}) => {
        return this.groupsService.getChannels(groupId).pipe(
          map((r) => getChannelsSuccessAction({channels: r})),
          catchError((response) =>
            of(
              getChannelsFailureAction(),
              requestFailedAction({error: response})
            )
          )
        );
      })
    )
  );
  constructor(
    private actions$: Actions,
    private groupsService: GroupsService,
    private router: Router
  ) {}
}
