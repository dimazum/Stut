import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { Observable, Subscription, timer } from 'rxjs';
import { startLessonSubject } from '../../models/events';
import { DatePipe, DecimalPipe  } from '@angular/common';

@Component({
  selector: 'app-timer',
  standalone: true,
  imports: [DatePipe, DecimalPipe ],
  templateUrl: './timer.component.html',
  styleUrl: './timer.component.css'
})
export class TimerComponent implements OnInit, OnDestroy{
  timer? : Observable<number>;
  @Output() onFinished = new EventEmitter();
  private subscription?: Subscription;
  private finished : boolean  = false;

  get minutes(): number {
    return Math.floor(this.timeLeft / 60);
  }

  get seconds(): number {
    return this.timeLeft % 60;
  }

  ngOnInit(): void {  
    startLessonSubject.subscribe((isEnabled) => isEnabled ? this.startTimer() : this.stopTimer())

    this.timer = timer(1000, 1000);
  }

  timeLeft: number = 1500;


  startTimer() {
    this.subscription= this.timer?.subscribe(() => {
          if (this.timeLeft > 0) {
              this.timeLeft--;
          }
          else if (this.timeLeft == 0 && !this.finished){
            this.finished = true;
            this.onFinished.next(null);
          }
      });
  }

  stopTimer(){
    this.subscription?.unsubscribe();
  }

  resetTimer() {
    this.timeLeft = 1500;
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}

