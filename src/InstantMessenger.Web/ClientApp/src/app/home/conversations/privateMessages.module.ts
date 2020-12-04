import {CommonModule} from '@angular/common';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {StoreModule} from '@ngrx/store';
import {EffectsModule} from '@ngrx/effects';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {reducers} from 'src/app/home/conversations/store/reducers';
import {ConversationsEffects} from 'src/app/home/conversations/store/effects';
import {ConversationsService} from 'src/app/home/conversations/services/conversations.service';
import {ScrollToBottomDirective} from 'src/app/shared/directives/scroll-to-bottom.directive';
import {ConversationsComponent} from './components/conversations/conversations.component';
import {ConversationComponent} from 'src/app/home/conversations/components/conversation/conversation.component';

@NgModule({
  declarations: [
    ConversationComponent,
    ScrollToBottomDirective,
    ConversationsComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbModule,
    FormsModule,
    FontAwesomeModule,
    StoreModule.forFeature('conversations', reducers),
    EffectsModule.forFeature([ConversationsEffects]),
  ],
  providers: [ConversationsService],
})
export class ConversationsModule {}
