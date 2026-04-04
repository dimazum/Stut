import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Trigger, TriggerResult } from '../../models/models';
import { BackendService } from '../../services/backend.service';
import { CommonModule } from '@angular/common';
import { EMPTY, Subscription, catchError, of, switchMap } from 'rxjs';
import { StutEventSystem } from '../../services/stut-event-system';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'stu-trigger-modal',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './trigger-modal.component.html',
  styleUrl: './trigger-modal.component.css',
})
export class TriggerModalComponent implements OnInit, OnDestroy {
  public triggerForm: FormGroup;
  public triggers?: Array<TriggerResult>;
  public isOpen: boolean = false;
  public userinfo$ = this.authServeice.userinfo$;
  public subscription!: Subscription;
  public selectedTrigger: string|null = null;

  public constructor(
    private fb: FormBuilder,
    private readonly backendService: BackendService,
    private readonly eventSystem: StutEventSystem,
    private authServeice: AuthService
    
  ) {
    this.triggerForm = this.fb.group({
      value: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    // this.subscription = this.authServeice.userinfo$.subscribe(x => 
    //   {
    //     if(x?.logged_in){
    //       this.backendService
    //       .getTriggers()
    //       .subscribe(data => (this.triggers = data));
    //     }
    //   }
    // )
  }

  public onCreate() {
    if (this.triggerForm.valid) {
      const trigger: Trigger = {
        value: this.triggerForm.value.value,
        difficulty: this.triggerForm.value.difficulty,
      };

      this.triggerForm.get('value')?.reset('');

      this.backendService
        .createTrigger(trigger)
        .pipe(
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

  public onDelete(trigger: Trigger) {
    this.backendService.deleteTrigger(trigger.value).subscribe(() => {
      this.backendService.getTriggers().subscribe(data => (this.triggers = data));
    });
  }

  public onToggle() {
    this.isOpen = !this.isOpen;

    if(this.isOpen){
      this.backendService
          .getTriggers()
          .subscribe(data => (this.triggers = data));
    }
    else{
      this.selectedTrigger = null;
    }
  }

  public onTriggerSelect(trigger: string){
    this.selectedTrigger = trigger;
  }

  public onChangeDifficulty(trigger: TriggerResult){
    let newDifficulty = 0;
    if(trigger.difficulty < 2)
    {
        newDifficulty = ++trigger.difficulty
    }
    
    this.backendService
          .changeTriggerDifficulty(trigger.value, newDifficulty)
          .subscribe(data => (this.triggers = data));

  }

  public onTriggerDbClicked(trigger: string){    
    window.location.href = `/practiceWord?trigger=${encodeURIComponent(trigger)}`;
  }

  public isTriggerSelected(trigger: string){
    return this.selectedTrigger === trigger
  }

  public goToPracticeWord() {
    window.location.href = `/practiceWord?trigger=${encodeURIComponent(this.selectedTrigger!)}`;
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
