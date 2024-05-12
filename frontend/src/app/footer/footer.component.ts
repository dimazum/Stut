import { Component } from '@angular/core';
import { TimerComponent } from '../common/timer/timer.component';
import { startLessonSubject } from '../models/events';
import { NgClass } from '@angular/common';
import { SpeechRecognitionService } from '../services/speech-recognition.service';
import { BackendService } from '../services/backend.service';
import { Trigger } from '../models/models';
import { TriggerModalComponent } from '../triggers/trigger-modal/trigger-modal.component';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [TimerComponent, NgClass, TriggerModalComponent],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.css'
})
export class FooterComponent {

constructor(private speechRecognitionService:SpeechRecognitionService, 
  private backendService: BackendService
) {
}

  isEnabled = false;
  startBtnName = 'Start';

  wordsCounter? = 0;
  speedCounter = 59;
  text? = '';

  startListening(): void{

    this.isEnabled = !this.isEnabled;

    startLessonSubject.next(this.isEnabled);

    if(this.isEnabled){
      this.speechRecognitionService.Start();
      this.startBtnName = 'Stop';
      this.speechRecognitionService.recognitionResult
    .subscribe(result =>{
      this.text = result.text;
      this.wordsCounter = result.wordCount;
    });
    }
    else{
      this.speechRecognitionService.Stop();
      this.startBtnName = 'Start'
      this.speechRecognitionService.recognitionResult
    .unsubscribe();
    }
  }

}
