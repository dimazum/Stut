import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Trigger, TriggerResult } from '../../models/models';
import { BackendService } from '../../services/backend.service';
import { DatePipe, NgClass, NgFor } from '@angular/common';

@Component({
  selector: 'app-trigger-modal',
  standalone: true,
  imports: [ReactiveFormsModule, DatePipe, NgFor, NgClass],
  templateUrl: './trigger-modal.component.html',
  styleUrl: './trigger-modal.component.css'
})
export class TriggerModalComponent implements OnInit {
  public triggerForm: FormGroup;
  triggers?: Array<TriggerResult>;
  isOpen: boolean = false;

  constructor(private fb: FormBuilder, private readonly backendService: BackendService) {
      this.triggerForm = this.fb.group({
          value: ['', Validators.required],
          difficulty: ['1', [Validators.required]]
      });
  }

  ngOnInit(): void {
     this.backendService.getTriggers().subscribe(data => this.triggers = data)
  }

  onCreate() {
      if (this.triggerForm.valid) {
        let trigger: Trigger = {
          value: this.triggerForm.value.value,
          difficulty: this.triggerForm.value.difficulty
        }
        this.backendService.createTrigger(trigger).subscribe(()=>{
          this.backendService.getTriggers().subscribe(data => this.triggers = data)
        })
      }
  }

  onDelete(trigger: Trigger){
    this.backendService
    .deleteTrigger(trigger.value)
    .subscribe( ()=> {
      this.backendService.getTriggers().subscribe(data => this.triggers = data)
    });
  }

  onToggle(){
    this.isOpen = !this.isOpen;
  }
}
