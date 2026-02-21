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
  @ViewChild('chartContainer') chart!: ElementRef;
  @ViewChild('scrollContainer') scrollContainer!: ElementRef;

  @Input() name: string = '';
  @Input() initText: string = '';
  @Input() saveToDb: boolean = true;

  draggingIndex: number | null = null;
  histogram! : Histogram;
  data: CharItem[] = [];
  public isDevMode: boolean = false;

  constructor(private backendService :BackendService) {}
  ngOnInit(): void {


    this.backendService.getOrCreateHistogram(this.name, this.initText, this.saveToDb)
    .subscribe({
      next: (histogram) => {
        this.histogram = histogram;
        this.data = histogram.chars;
      },
      error: (err) => {
      }
    });
  }

ngAfterViewInit() {

  const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        // ждём 2 секунды после появления
        setTimeout(() => {

          const el = this.scrollContainer.nativeElement;

          const interval = setInterval(() => {

            el.scrollLeft += 0;//2

            if (el.scrollLeft >= el.scrollWidth - el.clientWidth) {
              clearInterval(interval);
            }

          }, 16);

        }, 2000);

        observer.disconnect(); // чтобы не запускалось повторно
      }

    });

  }, { threshold: 0.3 });

  observer.observe(this.scrollContainer.nativeElement);
}

  startDrag(index: number, event: MouseEvent) {
    if(this.isDevMode){
      this.draggingIndex = index;
    }
  }

  @HostListener('document:mouseup')
  stopDrag() {
    this.draggingIndex = null;
  }

  @HostListener('document:mousemove', ['$event'])
  onMouseMove(event: MouseEvent) {

    if (this.draggingIndex === null) return;

    const containerRect = this.chart.nativeElement.getBoundingClientRect();

    let newHeight = Math.round(containerRect.bottom - event.clientY);

    // Ограничение по контейнеру
    if (newHeight < 0) newHeight = 0;
    if (newHeight > containerRect.height)
    {
        newHeight = containerRect.height;
    }

    this.histogram.chars[this.draggingIndex].air = newHeight;
  }

  SaveHistogram(){
    this.backendService.saveHistogram(this.histogram).subscribe();
  }

  AddColumn(order :number){
    this.backendService.addHistogramColumn(this.histogram.name, order).subscribe(x =>
      {
        this.histogram = x;
        this.data = x.chars;
      }
    )
  }

  RemoveColumn(order :number){
    this.backendService.remoHistogramColumn(this.histogram.name, order).subscribe(x =>
      {
        this.histogram = x;
        this.data = x.chars;
      }
    )
  }
}
