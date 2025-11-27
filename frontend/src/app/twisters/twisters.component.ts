import { Component, OnInit } from '@angular/core';
import { BackendService } from '../services/backend.service';
import { NgFor } from '@angular/common';

@Component({
  selector: 'stu-twisters',
  standalone: true,
  imports: [NgFor],
  templateUrl: './twisters.component.html',
  styleUrl: './twisters.component.css',
})
export class TwistersComponent implements OnInit {
  public twisters?: Array<Array<string>> = [];

  public constructor(private backendService: BackendService) {}

  public ngOnInit(): void {
    this.backendService.getTwisters().subscribe(data => {
      this.twisters = data;
    });
  }
}
