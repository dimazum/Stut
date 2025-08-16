import { Injectable, OnDestroy } from '@angular/core';
import { startLessonSubject } from '../models/events';
import { ReplaySubject, Subscription } from 'rxjs';
import { RecognitionData } from '../models/models';

declare var webkitSpeechRecognition: any;

@Injectable({
  providedIn: 'root'
})
export class SpeechRecognitionService implements OnDestroy {
  private recognition: any;
  private subscription?: Subscription;
  private isRecognitionEnabled = false;
  private totalWordCount = 0; // накопленный счетчик слов
  private wpm = 0 //скорость в минуту
  private wordsInCurrentWindow = 0; // слова за окно для подсчета скорости
  private lastResultTime = 0; // время последнего распознанного фрагмента
  private windowStartTime: any

  public recognitionResult = new ReplaySubject<RecognitionData>(1);

  constructor() {
    this.recognition = new webkitSpeechRecognition();
    this.recognition.interimResults = false;
    this.recognition.lang = 'ru-RU';

  this.recognition.onresult = (event: any) => {
    const text = event.results[event.results.length - 1][0].transcript;
    const wordCount = text.trim().split(/\s+/).length;

    const now = Date.now();

    // если пауза > 2 секунд — начинаем новый интервал
    if (this.lastResultTime && (now - this.lastResultTime) > 2000) {
      this.wordsInCurrentWindow = 0;
      this.windowStartTime = now;
    }

    this.wordsInCurrentWindow += wordCount;
    this.totalWordCount += wordCount;
    this.lastResultTime = now;

    // длительность интервала в минутах
    let elapsedMinutes = (now - this.windowStartTime) / 60000;

    // если интервал слишком маленький, ставим минимальный 1 секунда
    if (elapsedMinutes < 0.0167) { // 1 секунда = 1/60 минуты
      elapsedMinutes = 0.0167;
    }

    const speed = this.wordsInCurrentWindow / elapsedMinutes;

    const rData = new RecognitionData();
    rData.text = text;
    rData.wordCount = this.totalWordCount;
    rData.wpm = speed;

    this.recognitionResult.next(rData);
    console.log('Распознанный текст:', text, 'WPM:', speed.toFixed(1));
};


    this.recognition.onend = () => {
      if (this.isRecognitionEnabled) {
        this.recognition.start();
      }
    };

    this.recognition.onerror = (event: any) => {
      console.error('Speech recognition error:', event.error);
      // Можно перезапустить распознавание, если ошибка recoverable
      if (this.isRecognitionEnabled && event.error !== 'not-allowed') {
        this.recognition.start();
      }
    };

    this.subscription = startLessonSubject.subscribe((isEnabled) => {
      if (isEnabled) {
        this.Start();
      } else {
        this.Stop();
      }
    });
  }

public Start(): void {
  if (!this.isRecognitionEnabled) {
    this.isRecognitionEnabled = true;
    this.windowStartTime = Date.now(); // начинаем отсчет сразу
    this.lastResultTime = this.windowStartTime;
    this.recognition.start();
  }
}


  public Stop(): void {
    if (this.isRecognitionEnabled) {
      this.isRecognitionEnabled = false;
      this.recognition.stop();
    }
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
    this.Stop();
  }
}
