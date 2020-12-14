import {Component, OnInit} from '@angular/core';
import {
  ActivatedRoute,
  ActivatedRouteSnapshot,
  NavigationEnd,
  Router,
} from '@angular/router';
// import {ActivatedRoute, UrlSegment} from '@angular/router';
import {select, Store} from '@ngrx/store';
import {Observable, zip} from 'rxjs';
import {
  filter,
  first,
  map,
  mergeMap,
  tap,
  withLatestFrom,
} from 'rxjs/operators';
import {ChannelDto} from 'src/app/home/groups/services/responses/group.dto';
import {loadCurrentChannelAction} from 'src/app/home/groups/store/actions';
import {getMessagesAction} from 'src/app/home/groups/store/messages/actions';

@Component({
  selector: 'app-channel',
  templateUrl: './channel.component.html',
  styleUrls: ['./channel.component.scss'],
})
export class ChannelComponent implements OnInit {
  $channelId: Observable<string>;
  $groupId: Observable<string>;
  $channel: Observable<ChannelDto>;
  constructor(private route: ActivatedRoute, private store: Store) {
    this.$groupId = this.route.parent.paramMap.pipe(map((pm) => pm.get('id')));
    this.$channelId = this.route.paramMap.pipe(
      map((pm) => pm.get('channelId'))
    );
  }

  ngOnInit(): void {
    this.$channelId
      .pipe(
        tap((channelId) => {
          this.store.dispatch(loadCurrentChannelAction({channelId: channelId}));
        })
      )
      .subscribe();
  }
}
