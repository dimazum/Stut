import { Component, Input, OnChanges, SimpleChanges, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-textarea-auto-clear',
  standalone: true,
  templateUrl: './textarea-auto-clear.component.html',
  styleUrl: './textarea-auto-clear.component.css',
})
export class TextareaAutoClearComponent implements OnChanges, OnDestroy {
  @Input() text: string = '';

  private inactivityTimer?: any;
  private clearTimer?: any;

  ngOnChanges(changes: SimpleChanges) {
    if (changes['text']) {
      // Сбрасываем предыдущие таймеры
      clearTimeout(this.inactivityTimer);
      clearTimeout(this.clearTimer);

      // Таймер на 10 секунд без обновлений
      this.inactivityTimer = setTimeout(() => {
        // Через 5 секунд после 10 секунд бездействия очищаем
        this.clearTimer = setTimeout(() => {
          this.text = '';
        }, 1000);
      }, 7000);
    }
  }

  ngOnDestroy() {
    clearTimeout(this.inactivityTimer);
    clearTimeout(this.clearTimer);
  }
}
