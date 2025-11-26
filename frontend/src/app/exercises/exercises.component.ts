import { Component, OnInit } from '@angular/core';
import { BackendService } from '../services/backend.service';
import { NgFor } from '@angular/common';

@Component({
  selector: 'stu-exercises',
  standalone: true,
  imports: [NgFor],
  templateUrl: './exercises.component.html',
  styleUrl: './exercises.component.css',
})
export class ExercisesComponent implements OnInit {
  public twisters?: Array<Array<string>>;

  public constructor(private backendService: BackendService) {}

  public ngOnInit(): void {
    this.backendService.getExercises().subscribe(data => {
      this.twisters = data;
    });
  }
}
