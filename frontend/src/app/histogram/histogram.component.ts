import { CommonModule } from '@angular/common';
import { Component, ElementRef, HostListener, Input, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CharItem, Histogram } from '../models/models';
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
  @Input() initText: string = '';

  histogram! : Histogram;
  data: CharItem[] = [];

  constructor(private backendService :BackendService) {}
    ngOnInit(): void {

      this.getHistogram(this.name, this.initText);
  }

  getHistogram(name: string, text: string){
    this.backendService.getOrCreateHistogram(name, text, false)
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
}

