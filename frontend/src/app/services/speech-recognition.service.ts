import { Injectable, OnDestroy } from '@angular/core';
import { startLessonSubject } from '../models/events';
import { Subject, Subscription } from 'rxjs';
import { RecognitionData } from '../models/models';

declare var webkitSpeechRecognition: any;

@Injectable({
  providedIn: 'root'
})
export class SpeechRecognitionService implements OnDestroy {
  private recognition: any;
  private subscription?: Subscription;
  private isRecognitionEnabled = false;

  public recognitionResult = new Subject<RecognitionData>();

  constructor() {
    this.recognition = new webkitSpeechRecognition();
    this.recognition.interimResults = false;
    this.recognition.lang = 'ru-RU';

    this.recognition.onresult = (event: any) => {
      const text = event.results[event.results.length - 1][0].transcript;
      const wordCount = text.trim().split(/\s+/).length;

      const rData = new RecognitionData();
      rData.text = text;
      rData.wordCount = wordCount;

      this.recognitionResult.next(rData);
      console.log('Распознанный текст:', text);
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
