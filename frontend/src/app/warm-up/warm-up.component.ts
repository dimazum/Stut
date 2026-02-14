import { CommonModule } from '@angular/common';
import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CharItem, Histogram } from '../models/models';
import { BackendService } from '../services/backend.service';
import { HistogramComponent } from "../histogram/histogram.component";

@Component({
  selector: 'app-warm-up',
  standalone: true,
  imports: [CommonModule, FormsModule, HistogramComponent],
  templateUrl: './warm-up.component.html',
  styleUrl: './warm-up.component.css'
})
export class WarmUpComponent implements OnInit {
 
  //Если мы удерживаем воздух мышцы напрягаются -получаем блок
  //"Это база речи.
  //научиться контролировать выдыхаемый воздух
  //Развить аппарат чтобы громко говорить расслабленными мышцами"
  //Нужно убрать зажимы мышц

  constructor(private backendService :BackendService) {
    
  }
  ngOnInit(): void {
  }

}
