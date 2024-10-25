import { Component, OnDestroy, OnInit } from '@angular/core';
import { BackendService } from '../services/backend.service';
import { Trigger, TriggerResult, TriggerTaskResult } from '../models/models';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { StutEventSystem } from '../services/stut-event-system';
import { firstValueFrom, Observable, Subject, switchMap, takeUntil } from 'rxjs';

@Component({
  selector: 'app-triggers',
  standalone: true,
  imports: [NgFor, DatePipe, NgIf],
  templateUrl: './triggers.component.html',
  styleUrl: './triggers.component.css'
})
export class TriggersComponent implements OnInit, OnDestroy  {

  triggers?: Array<TriggerResult>;
  triggerTasks?: Array<TriggerTaskResult>;
  private unsubscribe$ = new Subject<void>();

  constructor(private backendService: BackendService,
    private eventSystem: StutEventSystem
  ) {
    
  }

  ngOnInit(): void {
    this.backendService.getTriggers().subscribe(data => this.triggers = data)

    this.eventSystem.trigger$.pipe(
      takeUntil(
      this.unsubscribe$),
      switchMap(triggerValue => {

        return this.backendService.getTriggerTasks(triggerValue.value)

      })
    ).subscribe(data => this.triggerTasks = data)
  }

  //TODO вынести в базовый класс
  ngOnDestroy(): void {
    this.unsubscribe$.next()
  }

}
