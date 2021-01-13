import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {concatMap} from 'rxjs/operators';
import {
  GenerateInvitationRequest,
  InvitationDto,
} from 'src/app/home/groups/store/types/invitation';
import {environment} from 'src/environments/environment';

@Injectable()
export class InvitationsService {
  private groupApi = `${environment.apiUrl}/groups`;
  private channelsApi(groupId) {
    return `${this.groupApi}/${groupId}/channels`;
  }
  constructor(private http: HttpClient) {}

  public generateInvitation(
    request: GenerateInvitationRequest
  ): Observable<{groupId: string; invitationId: string; code: string}> {
    return this.http
      .post(`${this.groupApi}/${request.groupId}/invitations`, request)
      .pipe(
        concatMap((_) =>
          this.http.get<{groupId: string; invitationId: string; code: string}>(
            `${this.groupApi}/${request.groupId}/invitations/${request.invitationId}`
          )
        )
      );
  }

  public revokeInvitation(request: {
    groupId: string;
    invitationId: string;
  }): Observable<Object> {
    return this.http.delete(
      `${this.groupApi}/${request.groupId}/invitations/${request.invitationId}`
    );
  }

  public getInvitations(request: {
    groupId: string;
  }): Observable<InvitationDto[]> {
    return this.http.get<InvitationDto[]>(
      `${this.groupApi}/${request.groupId}/invitations`
    );
  }
}
