import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { Observable, Subscription, timer } from 'rxjs';
import { startLessonSubject } from '../../models/events';
import { DecimalPipe } from '@angular/common';
import { UntilDestroy } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: 'stu-timer',
  standalone: true,
  imports: [DecimalPipe],
  templateUrl: './timer.component.html',
  styleUrl: './timer.component.css',
})
export class TimerComponent implements OnInit, OnDestroy {
  public timer?: Observable<number>;
  public timeLeft: number = 900;

  @Output() public finishedEvent = new EventEmitter();

  private subscription?: Subscription;
  private finished: boolean = false;

  public get minutes(): number {
    return Math.floor(this.timeLeft / 60);
  }

  public get seconds(): number {
    return this.timeLeft % 60;
  }

  public ngOnInit(): void {
    startLessonSubject.subscribe(isEnabled => (isEnabled ? this.startTimer() : this.stopTimer()));

    this.timer = timer(1000, 1000);
  }

  public startTimer() {
    this.subscription = this.timer?.subscribe(() => {
      if (this.timeLeft > 0) {
        this.timeLeft--;
      } else if (this.timeLeft == 0 && !this.finished) {
        this.finished = true;
        this.finishedEvent.next(null);
      }
    });
  }

  public stopTimer() {
    this.subscription?.unsubscribe();
  }

  public resetTimer() {
    this.timeLeft = 900;
  }

  public ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}
