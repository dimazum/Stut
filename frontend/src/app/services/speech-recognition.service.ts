import { Injectable, OnDestroy, OnInit } from '@angular/core';
import { startLessonSubject } from '../models/events';
import { Subject, Subscription } from 'rxjs';
import { RecognitionData } from '../models/models';

declare var webkitSpeechRecognition: any;

@Injectable({
  providedIn: 'root'
})
export class SpeechRecognitionService implements OnInit, OnDestroy {
  private recognition: any;
  private subscription?: Subscription;
  private isRecognitionEnabled = false;

  public recognitionResult = new Subject<RecognitionData>();

  constructor() {
  }

  public Start(): void{
    this.isRecognitionEnabled = true;

    this.recognition = new webkitSpeechRecognition();
    this.recognition.interimResults = false;
    this.recognition.lang = 'ru-RU';
    this.recognition.start();

    this.recognition.onresult = (data : any) => {
      const text = data.results[data.results.length - 1][0].transcript
      var resultWordsCount = text.trim().split(/\s+/).length;

      let rData = new RecognitionData();
      rData.text = text;
      rData.wordCount = resultWordsCount;

      this.recognitionResult.next(rData);
      console.log(text);
    };

    this.recognition.onend = () => {
      if (this.isRecognitionEnabled) {
        this.recognition.start();
      }
    };
  }

  public Stop():void{
    this.isRecognitionEnabled = false;
    this.recognition.stop();
  }

  ngOnInit(): void {
    var subscription = startLessonSubject.subscribe((isEnabled) => isEnabled? this.recognition.start() : this.recognition.stop())
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}
