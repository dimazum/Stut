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

constructor( private backendService:BackendService) {
}
  ngOnDestroy(): void {
  }

  ngOnInit(): void {

    this.backendService.getTwisters().subscribe(
      (data => {
        this.twisters = data;
      })
    )
  }
}

