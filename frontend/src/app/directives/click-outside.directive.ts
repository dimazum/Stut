import { Directive, ElementRef, EventEmitter, HostListener, Output } from '@angular/core';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: '[clickOutside]',
  standalone: true,
})
export class ClickOutsideDirective {
  @Output() public clickOutside = new EventEmitter<void>();

  public constructor(private el: ElementRef) {}

  @HostListener('document:click', ['$event.target'])
  public onClick(target: HTMLElement): void {
    if (!this.el.nativeElement.contains(target)) {
      this.clickOutside.emit();
    }
  }
}
