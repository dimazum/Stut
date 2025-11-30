import { Component, OnInit } from '@angular/core';
import { BackendService } from '../services/backend.service';
import { TriggerResult, TriggerTaskResult } from '../models/models';
import { NgFor, NgIf } from '@angular/common';
import { StutEventSystem } from '../services/stut-event-system';
import { switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: 'stu-triggers',
  standalone: true,
  imports: [NgFor, NgIf],
  templateUrl: './triggers.component.html',
  styleUrl: './triggers.component.css',
})
export class TriggersComponent implements OnInit {
  public triggers?: Array<TriggerResult>;
  public triggerTasks?: Array<TriggerTaskResult>;

  public constructor(
    private backendService: BackendService,
    private eventSystem: StutEventSystem
  ) {}

  public ngOnInit(): void {
    this.backendService.getTriggers().subscribe(data => (this.triggers = data));

    this.eventSystem.trigger$
      .pipe(
        untilDestroyed(this),
        switchMap(triggerValue => {
          return this.backendService.getTriggerTasks(triggerValue.value);
        })
      )
      .subscribe(data => (this.triggerTasks = data));
  }

  //TODO вынести в базовый класс
}
