import { Injectable } from '@angular/core';
import { interval, Subject, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WpmService {

  private totalWords = 0;
  private totalSeconds = 0;

  private wpmSubject = new Subject<number>();
  wpm$ = this.wpmSubject.asObservable();

  constructor() {
    // Каждые 10 секунд пересчет
    interval(10000).subscribe(() => {
      this.totalSeconds += 10;

      if (this.totalSeconds > 0) {
        const minutes = this.totalSeconds / 60;
        const wpm = this.totalWords / minutes;

        this.wpmSubject.next(Math.round(wpm));
      }
    });
  }

  // Добавление текста (вызываешь при вводе пользователя)
  addText(text: string) {
    const words = text.trim().split(/\s+/).filter(Boolean).length;
    this.totalWords += words;
  }

  reset() {
    this.totalWords = 0;
    this.totalSeconds = 0;
  }
}