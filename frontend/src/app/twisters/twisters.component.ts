import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { BackendService } from '../services/backend.service';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-twisters',
  standalone: true,
  imports: [NgFor],
  templateUrl: './twisters.component.html',
  styleUrl: './twisters.component.css'
})
export class TwistersComponent implements OnInit, OnDestroy {
  twisters?: Array<Array<string>> = [];
  public counter = 1;

constructor( private backendService:BackendService) {
}
  ngOnDestroy(): void {
    //this.counter = 1;
  }

  ngOnInit(): void {

    this.backendService.getTwisters().subscribe(
      (data => {
        this.twisters = data;
        console.log(data);
        
      })
    )
  }

  public getCounter(): number{
    this.counter = this.counter + 1;
    //console.log(this.counter);

    return this.counter;
  }
}

