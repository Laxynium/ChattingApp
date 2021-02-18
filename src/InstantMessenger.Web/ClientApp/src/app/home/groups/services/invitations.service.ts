import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {concatMap} from 'rxjs/operators';
import {environment} from 'src/environments/environment';
import {
  ExpirationTimeType,
  Invitation, UsageCounterType
} from "src/app/home/groups/store/invitations/reducer";

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
  }): Observable<Invitation[]> {
    return this.http.get<Invitation[]>(
      `${this.groupApi}/${request.groupId}/invitations`
    );
  }
}

export interface GenerateInvitationRequest {
  groupId: string;
  invitationId: string;
  expirationTime: {
    type: ExpirationTimeType;
    period: string;
  };
  usageCounter: {
    type: UsageCounterType;
    times: number;
  };
}
