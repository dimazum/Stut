import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-textarea-auto-clear',
  standalone: true,
  templateUrl: './textarea-auto-clear.component.html',
  styleUrl: './textarea-auto-clear.component.css',
})
export class TextareaAutoClearComponent {
  @Input() text: string = '';
}
