import { Component, OnInit } from '@angular/core';
import { BackendService } from '../services/backend.service';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-exercises',
  standalone: true,
  imports: [NgFor],
  templateUrl: './exercises.component.html',
  styleUrl: './exercises.component.css'
})
export class ExercisesComponent implements OnInit {
  twisters?: Array<Array<string>>;
  constructor(private backendService : BackendService) {   
  }
  ngOnInit(): void {
    this.backendService.getExercises().subscribe(
      (data => {
        this.twisters = data;
        console.log(data);     
      })
    )
  }
}
