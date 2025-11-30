import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Trigger, TriggerResult } from '../../models/models';
import { BackendService } from '../../services/backend.service';
import { NgClass, NgFor } from '@angular/common';
import { EMPTY, catchError, of, switchMap } from 'rxjs';
import { StutEventSystem } from '../../services/stut-event-system';

@Component({
  selector: 'stu-trigger-modal',
  standalone: true,
  imports: [ReactiveFormsModule, NgFor, NgClass],
  templateUrl: './trigger-modal.component.html',
  styleUrl: './trigger-modal.component.css',
})
export class TriggerModalComponent implements OnInit {
  public triggerForm: FormGroup;
  public triggers?: Array<TriggerResult>;
  public isOpen: boolean = false;

  public constructor(
    private fb: FormBuilder,
    private readonly backendService: BackendService,
    private readonly eventSystem: StutEventSystem
  ) {
    this.triggerForm = this.fb.group({
      value: ['', Validators.required],
      difficulty: ['1', [Validators.required]],
    });
  }

  public ngOnInit(): void {
    this.backendService.getTriggers().subscribe(data => (this.triggers = data));
  }

  public onCreate(): void {
    if (this.triggerForm.valid) {
      const trigger: Trigger = {
        value: this.triggerForm.value.value,
        difficulty: this.triggerForm.value.difficulty,
      };

      this.backendService
        .createTrigger(trigger)
        .pipe(
          // eslint-disable-next-line @typescript-eslint/no-unused-vars
          switchMap((t: TriggerResult) => this.backendService.getTriggers()),
          switchMap((triggers: Array<TriggerResult>) => (this.triggers = triggers)),
          catchError(x => {
            console.log(x);
            return of(EMPTY);
          })
        )
        .subscribe();
    }
  }

  public onClickTrigger(trigger: Trigger): void {
    this.eventSystem.trigger$.next(trigger);
  }

  public onDelete(trigger: Trigger): void {
    this.backendService.deleteTrigger(trigger.value).subscribe(() => {
      this.backendService.getTriggers().subscribe(data => (this.triggers = data));
    });
  }

  public onToggle(): void {
    this.isOpen = !this.isOpen;
  }
}
