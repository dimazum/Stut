import { CommonModule } from '@angular/common';
import { Component, ElementRef, HostListener, Input, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CharItem, Histogram, TriggerResult } from '../models/models';
import { BackendService } from '../services/backend.service';

@Component({
  selector: 'app-histogram',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './histogram.component.html',
  styleUrl: './histogram.component.css'
})
export class HistogramComponent {
  @Input() name: string = '';
  @Input() initTrigger: string = '';

  histogram! : Histogram;
  data: CharItem[] = [];
  triggers: TriggerResult[] = [];
  selectedTriggerVal : string = '';

  constructor(private backendService :BackendService) {}
    ngOnInit(): void {

      this.selectedTriggerVal = this.initTrigger;

      const text = this.getHistogramText(this.initTrigger);

      this.getHistogram(this.name, text);

      this.backendService.getTriggers().subscribe(x => this.triggers = x);
  }

  getHistogram(name: string, text: string){
    this.backendService.getHistogram(name, text, false)
    .subscribe({
      next: (histogram) => {
        this.histogram = histogram;
        this.data = histogram.chars;
      },
      error: (err) => {
      }
    });
  }

  updateText(){
    this.getHistogram('', '')
  }

  getBar(data: CharItem[], i: number, ratio: number ){
    let prevAir = data[i - 1]?.air ?? 0;
    return ((data[i].air - prevAir) * ratio) + prevAir
  }

  onTriggerClick(trigger: string){
    this.selectedTriggerVal = trigger;
    let text = this.getHistogramText(trigger);

    this.getHistogram('', text);
  }

  isSelected(triggerVal: string){
    return this.selectedTriggerVal === triggerVal;
  }

  getHistogramText(trigger: string){
    let text = '';

    if(this.name === 'HistBlock'){
      text = `🔒${trigger}, 🔒${trigger}, 🔒${trigger}, 🔒${trigger}`;
    }
    else if(this.name === 'HistBP'){
      text = `${trigger}, ${trigger}, ${trigger}, ${trigger}`;
    }
    else if(this.name === 'HistDia'){
      text = trigger;
    }

    return text;
  }
}