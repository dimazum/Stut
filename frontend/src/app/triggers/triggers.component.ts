import { Component, OnInit } from '@angular/core';
import { BackendService } from '../services/backend.service';
import { Trigger, TriggerResult, TriggerTaskResult } from '../models/models';
import { DatePipe, NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-triggers',
  standalone: true,
  imports: [NgFor, DatePipe, NgIf],
  templateUrl: './triggers.component.html',
  styleUrl: './triggers.component.css'
})
export class TriggersComponent implements OnInit {

  triggers?: Array<TriggerResult>;
  triggerTasks?: Array<TriggerTaskResult>;

  constructor(private backendService: BackendService) {
    
  }

  ngOnInit(): void {
    this.backendService.getTriggers().subscribe(data => this.triggers = data)
  }

  onClickTrigger(triggerValue: string){
    this.backendService.getTriggerTasks(triggerValue).subscribe(data => this.triggerTasks = data);
    console.log(this.triggerTasks);
  }
}
