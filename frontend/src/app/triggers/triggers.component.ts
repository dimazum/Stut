import { Component, OnDestroy, OnInit } from '@angular/core';
import { BackendService } from '../services/backend.service';
import { TriggerResult, TriggerTaskResult } from '../models/models';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { StutEventSystem } from '../services/stut-event-system';
import { Subject, switchMap, takeUntil } from 'rxjs';

@Component({
  selector: 'stu-triggers',
  standalone: true,
  imports: [NgFor, DatePipe, NgIf],
  templateUrl: './triggers.component.html',
  styleUrl: './triggers.component.css',
})
export class TriggersComponent implements OnInit, OnDestroy {
  public triggers?: Array<TriggerResult>;
  public triggerTasks?: Array<TriggerTaskResult>;
  private unsubscribe$ = new Subject<void>();

  public constructor(
    private backendService: BackendService,
    private eventSystem: StutEventSystem
  ) {}

  public ngOnInit(): void {
    this.backendService.getTriggers().subscribe(data => (this.triggers = data));

    this.eventSystem.trigger$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(triggerValue => {
          return this.backendService.getTriggerTasks(triggerValue.value);
        })
      )
      .subscribe(data => (this.triggerTasks = data));
  }

  //TODO вынести в базовый класс
  public ngOnDestroy(): void {
    this.unsubscribe$.next();
  }
}
