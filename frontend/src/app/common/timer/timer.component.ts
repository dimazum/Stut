import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subscription, timer } from 'rxjs';
import { startLessonSubject } from '../../models/events';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-timer',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './timer.component.html',
  styleUrl: './timer.component.css'
})
export class TimerComponent implements OnInit, OnDestroy{


  timer? : Observable<number>
  private subscription?: Subscription;

  ngOnInit(): void {  
    startLessonSubject.subscribe((isEnabled) => isEnabled ? this.startTimer() : this.stopTimer())

    this.timer = timer(1000, 1000);
  }

  timeLeft: number = 1200;

  startTimer() {
    this.subscription= this.timer?.subscribe(() => {
          if (this.timeLeft > 0) {
              this.timeLeft--;
          } 
      });
  }

  stopTimer(){
    this.subscription?.unsubscribe();
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}

