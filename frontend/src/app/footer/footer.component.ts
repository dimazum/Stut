import { Component, OnDestroy, OnInit } from '@angular/core';
import { TimerComponent } from '../common/timer/timer.component';
import { startLessonSubject, notLoggedInSubject } from '../models/events';
import { NgClass, NgIf } from '@angular/common';
import { SpeechRecognitionService } from '../services/speech-recognition.service';
import { BackendService } from '../services/backend.service';
import { TriggerModalComponent } from '../triggers/trigger-modal/trigger-modal.component';
import { Subscription } from 'rxjs';
import { VisualizerComponent } from '../visualizer-component/visualizer-component.component';
import { AudioRecorderService } from '../services/audio-recorder.service';
import { DailyLessonStatus } from '../models/enums';
import { DayLessonDto, VoiceAnalysisUpdateDto } from '../models/models';
import { TextareaAutoClearComponent } from '../common/textarea-auto-clear/textarea-auto-clear.component';
import { VoiceAnalysisService } from '../services/voice-analysis.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'stu-footer',
  standalone: true,
  imports: [TextareaAutoClearComponent,TimerComponent, NgClass, TriggerModalComponent, VisualizerComponent, NgIf],
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css'],
})
export class FooterComponent implements OnInit, OnDestroy{
  public isEnabled = false;
  public startBtnName = 'Начать';
  public wordsCounter = 0;
  public speedCounter? = 0;
  public text = '';
  public dailyLesson? : DayLessonDto;
  public timeLeftInSec: number = 0;
  private recognitionSub?: Subscription;
  private voiceSubscription!: Subscription;
  private authSubscription!: Subscription;
  voiceData: VoiceAnalysisUpdateDto | null = null;

  public constructor(
    private speechRecognitionService: SpeechRecognitionService,
    private backendService: BackendService,
    private audioRecorderService : AudioRecorderService,
    private voiceService: VoiceAnalysisService,
    private authService: AuthService,

  ) {}

  ngOnInit(): void {

    this.authSubscription = this.authService.userinfo$.subscribe(x => 
    {
      if(x?.logged_in){
            this.backendService.getDailyLesson().subscribe( x => {
            this.dailyLesson = x;
            this.timeLeftInSec = this.dailyLesson?.leftInSec ?? 0;
            this.speechRecognitionService.setSpokenWords(x?.wordsSpoken);
            this.wordsCounter = x?.wordsSpoken;
          });
      }
    })


    this.voiceSubscription = this.voiceService.voiceAnalysis$.subscribe(data => {
      if (data) {
        this.voiceData = data;
      }
    });

  }
//переделать в toggle
  public startListening() {
    
    if(!this.authService.isLoggedIn()){
      notLoggedInSubject.next(null);
      return;
    }

    this.isEnabled = !this.isEnabled;


    if (this.isEnabled) {   

      this.backendService.startLesson().subscribe(x => {
        this.dailyLesson = x;

        startLessonSubject.next({enabled: true, secondsRemaining : x.leftInSec});

        this.audioRecorderService.startRecording();
      });

      this.startBtnName = 'Стоп';

      this.recognitionSub = this.speechRecognitionService.recognitionResult.subscribe(result => {
        this.text = result.text ?? '';
        this.wordsCounter = result.wordCount ?? 0;
        //this.speedCounter = result.wpm;
      });
    } else {
      this.startBtnName = 'Начать';

      if(this.dailyLesson?.status !== DailyLessonStatus.Finished){
        
      }

      this.backendService.pauseLesson(this.dailyLesson!.id, this.wordsCounter, 0).subscribe(x => {
        this.dailyLesson = x;

        startLessonSubject.next({enabled: false, secondsRemaining : x.leftInSec});

        this.audioRecorderService.stop();
      });

      this.recognitionSub?.unsubscribe();
      this.recognitionSub = undefined;
    }
  }

  public onTimerFinished() {
    if(this.dailyLesson?.status === DailyLessonStatus.Finished ||
      this.dailyLesson?.status === DailyLessonStatus.Rewarded){
      return;
    }
    this.backendService.finishLesson(this.dailyLesson!.id, this.wordsCounter as number, 0).subscribe(
      x =>{
            this.dailyLesson = x
            startLessonSubject.next({enabled: false, secondsRemaining : 0});
            this.isEnabled = false;
            this.startBtnName = 'Start';
        }
    );
    //this.audioRecorderService.stopRecording().subscribe();
  }

    ngOnDestroy(): void {
     this.voiceSubscription.unsubscribe();
     this.authSubscription.unsubscribe()
     
  }
}
