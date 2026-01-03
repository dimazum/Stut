import { Injectable, OnDestroy } from '@angular/core';
import { startLessonSubject } from '../models/events';
import { ReplaySubject, Subscription } from 'rxjs';
import { RecognitionData } from '../models/models';
import { DailyLessonStatus } from '../models/enums';

// eslint-disable-next-line @typescript-eslint/no-explicit-any
declare let webkitSpeechRecognition: any;

@Injectable({
  providedIn: 'root',
})
export class SpeechRecognitionService implements OnDestroy {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  private recognition: any;
  private subscription?: Subscription;
  private isRecognitionEnabled = false;
  private totalWordCount = 0; // накопленный счетчик слов
  private wpm = 0; //скорость в минуту
  private wordsInCurrentWindow = 0; // слова за окно для подсчета скорости
  private lastResultTime = 0; // время последнего распознанного фрагмента
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  private windowStartTime: any;

  public recognitionResult = new ReplaySubject<RecognitionData>(1);

  public constructor() {
    this.recognition = new webkitSpeechRecognition();
    this.recognition.interimResults = false;
    this.recognition.lang = 'ru-RU';


    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    this.recognition.onresult = (event: any) => {
      const text = event.results[event.results.length - 1][0].transcript;
      const wordCount = text.trim().split(/\s+/).length;

      const now = Date.now();

      // если пауза > 2 секунд — начинаем новый интервал
      if (this.lastResultTime && now - this.lastResultTime > 2000) {
        this.wordsInCurrentWindow = 0;
        this.windowStartTime = now;
      }

      this.wordsInCurrentWindow += wordCount;
      this.totalWordCount += wordCount;
      this.lastResultTime = now;

      // длительность интервала в минутах
      let elapsedMinutes = (now - this.windowStartTime) / 60000;

      // если интервал слишком маленький, ставим минимальный 1 секунда
      if (elapsedMinutes < 0.0167) {
        // 1 секунда = 1/60 минуты
        elapsedMinutes = 0.0167;
      }

      const speed = this.wordsInCurrentWindow / elapsedMinutes;

      const rData = new RecognitionData();
      rData.text = text;
      rData.wordCount = this.totalWordCount;
      rData.wpm = speed;

      this.recognitionResult.next(rData);
    };

    this.recognition.onend = () => {
      if (this.isRecognitionEnabled) {
        this.recognition.start();
      }
    };

    this.recognition.onerror = (event: any) => {
      //console.error('Speech recognition error:', event.error);

      if(event.error == 'no-speech'){
        const rData = new RecognitionData();
        rData.text = 'Нет голоса...';
        rData.wordCount = this.totalWordCount;
        rData.wpm = 0;

      this.recognitionResult.next(rData);
      }

      // Можно перезапустить распознавание, если ошибка recoverable
      if (this.isRecognitionEnabled
         && event.error !== 'not-allowed'
         && event.error !=='no-speech') {
        this.recognition.start();
      }
    };

    this.subscription = startLessonSubject.subscribe(dayLesson => {
      if (dayLesson?.enabled) {
        this.start();
      } else {
        this.stop();
      }
    });
  }

  public setSpokenWords(words: number){
      this.totalWordCount = words;
  }

  public start(): void {
    if (!this.isRecognitionEnabled) {
      this.isRecognitionEnabled = true;
      this.windowStartTime = Date.now();
      this.lastResultTime = this.windowStartTime;
      this.recognition.start();
    }
  }

  public stop(): void {
    if (this.isRecognitionEnabled) {
      this.isRecognitionEnabled = false;
      this.recognition.stop();
    }
  }

  public ngOnDestroy(): void {
    this.subscription?.unsubscribe();
    this.stop();
  }
}
