import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {MessageDto} from 'src/app/home/groups/store/types/message';
import {environment} from 'src/environments/environment';

@Injectable()
export class MesssagesService {
  private groupApi = `${environment.apiUrl}/groups`;
  private messagesApi(groupId, channelId) {
    return `${this.groupApi}/${groupId}/channels/${channelId}/messages`;
  }
  constructor(private http: HttpClient) {}
  public getMessages(r: {
    groupId: string;
    channelId: string;
  }): Observable<MessageDto[]> {
    return this.http.get<MessageDto[]>(
      this.messagesApi(r.groupId, r.channelId)
    );
  }
  public sendMessage(r: {
    groupId: string;
    channelId: string;
    messageId: string;
    content: string;
  }): Observable<MessageDto> {
    return this.http.post<MessageDto>(
      `${this.messagesApi(r.groupId, r.channelId)}`,
      r
    );
  }
}
