import {
  Directive,
  ElementRef,
  Input,
  Renderer2,
  RendererStyleFlags2,
} from '@angular/core';
import {Store} from '@ngrx/store';
import {Map} from 'immutable';
import {Observable} from 'rxjs';
import {getAllowedActionsAction} from 'src/app/home/groups/store/access-control/actions';
import {allowedActionsSelector} from 'src/app/home/groups/store/access-control/selectors';
import {AllowedAction} from 'src/app/home/groups/store/types/allowed-action';

@Directive({
  selector: '[accessControl]',
})
export class AccessControlDirective {
  @Input('accessControlAction') action: string[];
  @Input('accessControlGroupId') groupId: string;
  @Input('accessControlChannelId') channelId?: string;

  originalDisplay: string;
  $allowedActions: Observable<Map<string, AllowedAction>>;
  constructor(
    private elementRef: ElementRef,
    private store: Store,
    private renderer: Renderer2
  ) {
    this.originalDisplay = this.elementRef.nativeElement.style.display;
  }

  ngOnInit(): void {
    this.$allowedActions = this.store.select(allowedActionsSelector);

    this.$allowedActions.subscribe((aa) => {
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
        .map((a) => this.isVisible(a))
        .every((x) => x == false);
      if (hide) {
        this.renderer.setStyle(
          this.elementRef.nativeElement,
          'display',
          'none',
          RendererStyleFlags2.Important
        );
      }
    });
  }

  isVisible(action: AllowedAction) {
    if (!action) {
      return false;
    }
    if (!action.isChannelSpecific) {
      return true;
    }
    if (!this.channelId || !action.channels.has(this.channelId)) {
      return false;
    }
    return true;
  }
}
