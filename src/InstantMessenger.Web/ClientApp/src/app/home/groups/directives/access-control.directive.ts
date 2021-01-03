import {
  Directive,
  ElementRef,
  Input,
  OnChanges,
  Renderer2,
  RendererStyleFlags2,
  SimpleChanges,
} from '@angular/core';
import {Store} from '@ngrx/store';
import {Map} from 'immutable';
import {BehaviorSubject, combineLatest, Observable, Subject} from 'rxjs';
import {getAllowedActionsAction} from 'src/app/home/groups/store/access-control/actions';
import {allowedActionsSelector} from 'src/app/home/groups/store/access-control/selectors';
import {AllowedAction} from 'src/app/home/groups/store/types/allowed-action';

@Directive({
  selector: '[accessControl]',
})
export class AccessControlDirective implements OnChanges {
  @Input('accessControlAction') action: string[];
  @Input('accessControlGroupId') groupId: string;
  @Input('accessControlChannelId') channelId: string;
  $channelId: BehaviorSubject<string>;
  originalDisplay: string;
  $allowedActions: Observable<Map<string, AllowedAction>>;
  constructor(
    private elementRef: ElementRef,
    private store: Store,
    private renderer: Renderer2
  ) {
    this.originalDisplay = this.elementRef.nativeElement.style.display;
    this.$channelId = new BehaviorSubject<string>(null);
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes.channelId) {
      this.$channelId.next(changes.channelId.currentValue);
    }
  }

  ngOnInit(): void {
    this.$allowedActions = this.store.select(allowedActionsSelector);

    combineLatest([this.$allowedActions, this.$channelId]).subscribe(
      ([aa, c]) => {
        this.elementRef.nativeElement.style.display = this.originalDisplay;
        this.renderer.setStyle(
          this.elementRef.nativeElement,
          'display',
          this.originalDisplay
        );
        if (aa.get('all')) {
          return;
        }
        const hide = this.action
          .map((a) => aa.get(a))
          .map((a) => this.isVisible(a, c))
          .every((x) => x == false);
        if (hide) {
          this.renderer.setStyle(
            this.elementRef.nativeElement,
            'display',
            'none',
            RendererStyleFlags2.Important
          );
        }
      }
    );
  }

  isVisible(action: AllowedAction, channelId: string) {
    if (!action) {
      return false;
    }
    if (!action.isChannelSpecific) {
      return true;
    }
    if (!channelId || !action.channels.has(channelId)) {
      return false;
    }
    return true;
  }
}
