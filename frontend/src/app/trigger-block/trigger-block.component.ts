import { Component, OnInit } from '@angular/core';
import { BackendService } from '../services/backend.service';
import { KeyValuePipe, NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-trigger-block',
  standalone: true,
  imports: [NgFor, NgIf, KeyValuePipe],
  templateUrl: './trigger-block.component.html',
  styleUrl: './trigger-block.component.css'
})
export class TriggerBlockComponent implements OnInit {
  result: { [key: string]: string[] } = {};
  loading = false;
  error = '';

  constructor(private backendService : BackendService) {

  }
  ngOnInit(): void {
    this.generate();
  }

  generate() {
    this.loading = true;
    this.error = '';
    this.backendService.getTriggerExercises().subscribe({
      next: (data) => {
        this.result = data;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.error = 'Ошибка при получении данных с сервера';
        this.loading = false;
      }
    });
  }
}
