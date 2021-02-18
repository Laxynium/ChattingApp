import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {Message} from "src/app/home/groups/store/messages/message.reducer";

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
  }): Observable<Message[]> {
    return this.http.get<Message[]>(
      this.messagesApi(r.groupId, r.channelId)
    );
  }
  public sendMessage(r: {
    groupId: string;
    channelId: string;
    messageId: string;
    content: string;
  }): Observable<Message> {
    return this.http.post<Message>(
      `${this.messagesApi(r.groupId, r.channelId)}`,
      r
    );
  }
}
