import {NgModule} from '@angular/core';
import {ScrollToBottomDirective} from './directives/scroll-to-bottom.directive';

@NgModule({
  declarations: [ScrollToBottomDirective],
  exports: [ScrollToBottomDirective],
})
export class SharedModule {}
