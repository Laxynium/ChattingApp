import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {forkJoin, Observable, of, zip} from 'rxjs';
import {map, switchMap} from 'rxjs/operators';
import {
  ConversationResponseInterface,
  MessageResponseInterface,
} from 'src/app/home/conversations/types/responseTypes/conversation.response';
import {UserInterface} from 'src/app/shared/types/user.interface';
import {environment} from 'src/environments/environment';

@Injectable()
export class ConversationsService {
  private identityUrl = `${environment.apiUrl}/identity`;
  private privateMessagesUrl = `${environment.apiUrl}/privateMessages`;
  constructor(private http: HttpClient) {}

  getLatestConversations(
    count: number
  ): Observable<ConversationResponseInterface[]> {
    return this.http
      .get<ConversationDto[]>(
        `${environment.apiUrl}/privateMessages/conversations/latest?number=${count}`
      )
      .pipe(
        switchMap((conversations) => {
          if (conversations.length == 0) return of([]);

          const result = conversations.map((c) => {
            const first = this.http.get<UserInterface>(
              `${this.identityUrl}/users/${c.firstParticipant}`
            );
            const second = this.http.get<UserInterface>(
              `${this.identityUrl}/users/${c.secondParticipant}`
            );

            return zip(first, second).pipe(
              switchMap((x) =>
                of<ConversationResponseInterface>({
                  conversationId: c.conversationId,
                  firstParticipant: x[0],
                  secondParticipant: x[1],
                  messages: [],
                })
              )
            );
          });
          return forkJoin(result);
        })
      );
  }

  getConversation(
    conversationId: string
  ): Observable<ConversationResponseInterface> {
    return this.http
      .get<ConversationDto>(
        `${environment.apiUrl}/privateMessages/conversations/${conversationId}`
      )
      .pipe(
        switchMap((c) => {
          const first = this.http.get<UserInterface>(
            `${this.identityUrl}/users/${c.firstParticipant}`
          );
          const second = this.http.get<UserInterface>(
            `${this.identityUrl}/users/${c.secondParticipant}`
          );
          const messages = this.http.get<MessageDto[]>(
            `${this.privateMessagesUrl}/${c.conversationId}`
          );
          const result = zip(first, second, messages).pipe(
            switchMap((x) =>
              of<ConversationResponseInterface>(toConversationResponse(c, x))
            )
          );
          return result;
        })
      );
  }

  sendMessage(
    conversationId: string,
    content: string
  ): Observable<MessageResponseInterface> {
    return this.http
      .post<MessageDto>(`${this.privateMessagesUrl}`, {
        conversationId: conversationId,
        text: content,
      })
      .pipe(
        map((r) => {
          return toMessageResponse(r);
        })
      );
  }
}

function toConversationResponse(
  c: ConversationDto,
  x: [UserInterface, UserInterface, MessageDto[]]
): ConversationResponseInterface {
  return {
    conversationId: c.conversationId,
    firstParticipant: x[0],
    secondParticipant: x[1],
    messages: x[2].map((m) => toMessageResponse(m)),
  };
}

function toMessageResponse(m: MessageDto): MessageResponseInterface {
  return {
    messageId: m.messageId,
    from: m.from,
    to: m.to,
    content: m.text,
    createdAt: m.createdAt,
    readAt: m.readAt,
  };
}

interface ConversationDto {
  conversationId: string;
  firstParticipant: string;
  secondParticipant: string;
}

interface MessageDto {
  messageId: string;
  conversationId: string;
  from: string;
  to: string;
  text: string;
  createdAt: string;
  readAt: string;
}
