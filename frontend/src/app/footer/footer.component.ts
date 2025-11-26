import { Component } from '@angular/core';
import { TimerComponent } from '../common/timer/timer.component';
import { startLessonSubject } from '../models/events';
import { NgClass } from '@angular/common';
import { SpeechRecognitionService } from '../services/speech-recognition.service';
import { BackendService } from '../services/backend.service';
import { TriggerModalComponent } from '../triggers/trigger-modal/trigger-modal.component';
import { Subscription } from 'rxjs';

@Component({
  selector: 'stu-footer',
  standalone: true,
  imports: [TimerComponent, NgClass, TriggerModalComponent],
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css'],
})
export class FooterComponent {
  public isEnabled = false;
  public startBtnName = 'Start';
  public wordsCounter? = 0;
  public speedCounter? = 0;
  public text? = '';
  public lessonId: number = 0;

  private recognitionSub?: Subscription; // <-- подписка на результаты распознавания

  public constructor(
    private speechRecognitionService: SpeechRecognitionService,
    private backendService: BackendService
  ) {}

  public startListening(): void {
    this.isEnabled = !this.isEnabled;

    // уведомляем сервис о старте/стопе урока
    startLessonSubject.next(this.isEnabled);

    if (this.isEnabled) {
      this.backendService.startLesson().subscribe(x => {
        this.lessonId = x;
      });

      this.speechRecognitionService.Start();
      this.startBtnName = 'Stop';

      // сохраняем подписку на результаты распознавания
      this.recognitionSub = this.speechRecognitionService.recognitionResult.subscribe(result => {
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

  public onTimerFinished() {
    this.backendService.finishLesson(this.lessonId, this.wordsCounter as number, 0).subscribe();
  }
}
