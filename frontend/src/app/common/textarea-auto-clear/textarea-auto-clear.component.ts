import { Component, Input, OnChanges, SimpleChanges, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-textarea-auto-clear',
  standalone: true,
  template: `
    <div class="text-block">
      <textarea placeholder="Распознанный текст..." [value]="text" readonly></textarea>
    </div>
  `,
  styles: [`
    /* Textarea */
.text-block textarea {
  font-size: 8px;
  width: 350px;
  height: 50px;
  resize: none;
  padding: 3px;
  border-radius: 8px;
  border: 1px solid #444;
  background: #2e2e2e;
  color: white;
}

.text-block textarea::placeholder {
    font-size: 12px; /* размер текста placeholder */
    color: #888;      /* цвет текста placeholder */
    font-family: Arial, sans-serif; /* шрифт placeholder */
}
  `]
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
