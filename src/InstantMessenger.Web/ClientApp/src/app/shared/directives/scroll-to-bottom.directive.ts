import {Directive, ElementRef} from '@angular/core';

@Directive({
  selector: '[scroll-to-bottom]',
})
export class ScrollToBottomDirective {
  constructor(private el: ElementRef) {
    this.el.nativeElement.scrollTop = this.el.nativeElement.scrollHeight;
  }

  ngAfterViewInit() {
    this.scrollToBottom();
  }
  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  public scrollToBottom() {
    // this.el.nativeElement.scrollTop = this.el.nativeElement.scrollHeight;
    const el: HTMLDivElement = this.el.nativeElement;
    el.scrollTop = Math.max(0, el.scrollHeight - el.offsetHeight);
  }
}
