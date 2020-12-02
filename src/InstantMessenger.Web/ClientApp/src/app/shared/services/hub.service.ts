import {Injectable} from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from '@microsoft/signalr';
import {Store} from '@ngrx/store';
import {distinctUntilChanged} from 'rxjs/operators';
import {friendshipsHub} from 'src/app/home/friends/hub/hub.handlers';
import {currentUserSelector} from 'src/app/identity/store/selectors';
import {Hub} from 'src/app/shared/hubs/hubHandlersProvider';
import {CurrentUserInterface} from 'src/app/shared/types/currentUser.interface';
import {environment} from 'src/environments/environment';

const hubs: Hub[] = [friendshipsHub];

@Injectable({
  providedIn: 'root',
})
export class HubService {
  private connections: {[key: string]: HubConnection} = {};
  constructor(private store: Store) {
    store
      .select(currentUserSelector)
      .pipe(
        distinctUntilChanged((x, y) => {
          if (x === null && y === null) return true;
          if (x !== null && y === null) return false;
          if (x === null && y !== null) return false;
          return x.id === y.id && x.token === y.token;
        })
      )
      .subscribe((currentUser) => {
        if (currentUser == null) {
          //means someone has logged out
          this.closeAllConnections();
          return;
        }
        hubs.forEach((h) => {
          this.updateConnection(currentUser, h);
        });
      });
  }
  private closeAllConnections() {
    Object.keys(this.connections)
      .map((k) => this.connections[k])
      .forEach((c) => {
        if (c && c.state == HubConnectionState.Connected) {
          this.stopOConnection(c);
        }
      });
  }

  private updateConnection(currentUser: CurrentUserInterface, hub: Hub) {
    const hubName = hub.hubName;
    const connection = this.connections[hubName];
    if (connection && connection.state == HubConnectionState.Connected) {
      this.stopOConnection(connection);
    }
    const newConnection = this.buildConnection(currentUser, hub);
    this.connections[hubName] = newConnection;
    this.registerHandlers(newConnection, hub);
    this.startConnection(newConnection, hubName);
  }

  private buildConnection(
    currentUser: CurrentUserInterface,
    hub: Hub
  ): HubConnection {
    return new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/${hub.hubName}`, {
        accessTokenFactory: () => currentUser.token,
      })
      .configureLogging(LogLevel.Debug)
      .build();
  }

  private registerHandlers(connection: HubConnection, hub: Hub) {
    const handlers = hub.hubHandlersProvider();
    const map = Object.keys(handlers).map((k) => ({
      key: k,
      handler: handlers[k],
    }));
    map.forEach((h) => {
      connection.off(h.key);
      connection.on(h.key, (data) => {
        h.handler(this.store, data);
      });
    });
  }

  private startConnection(newConnection: HubConnection, hubName: string) {
    newConnection
      .start()
      .then(() => {
        console.log(
          `Connection[id=${newConnection.connectionId}] to hub ${hubName} has been established`
        );
      })
      .catch((e) => {
        console.log(`Error occured while connectiong to hub ${hubName}`);
        console.log(e);
      });
  }

  private stopOConnection(connection: HubConnection) {
    const id = connection.connectionId;
    connection
      .stop()
      .then(() => console.log(`Old connection[id=${id}] has been stopped.`))
      .catch(() =>
        console.log(`Error occurred while stopping old connection[id=${id}].`)
      );
  }
}
