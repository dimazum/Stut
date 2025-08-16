import { Component } from '@angular/core';
import { TimerComponent } from '../common/timer/timer.component';
import { startLessonSubject } from '../models/events';
import { NgClass } from '@angular/common';
import { SpeechRecognitionService } from '../services/speech-recognition.service';
import { BackendService } from '../services/backend.service';
import { TriggerModalComponent } from '../triggers/trigger-modal/trigger-modal.component';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [TimerComponent, NgClass, TriggerModalComponent],
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent {
  isEnabled = false;
  startBtnName = 'Start';

  wordsCounter? = 0;
  speedCounter? = 0;
  text? = '';

  private recognitionSub?: Subscription; // <-- подписка на результаты распознавания

  constructor(
    private speechRecognitionService: SpeechRecognitionService,
    private backendService: BackendService
  ) {}

  startListening(): void {
    this.isEnabled = !this.isEnabled;

    // уведомляем сервис о старте/стопе урока
    startLessonSubject.next(this.isEnabled);

    if (this.isEnabled) {
      this.speechRecognitionService.Start();
      this.startBtnName = 'Stop';

      // сохраняем подписку на результаты распознавания
      this.recognitionSub = this.speechRecognitionService.recognitionResult
        .subscribe(result => {
          this.text = result.text;
          this.wordsCounter = result.wordCount;
          //this.speedCounter = result.wpm;
        });

    } else {
      this.speechRecognitionService.Stop();
      this.startBtnName = 'Start';

      // отписка от Subject
      this.recognitionSub?.unsubscribe();
      this.recognitionSub = undefined;
    }
  }
}
