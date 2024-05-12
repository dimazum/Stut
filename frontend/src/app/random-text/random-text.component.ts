import { Component } from '@angular/core';
import { BackendService } from '../services/backend.service';

@Component({
  selector: 'app-random-text',
  standalone: true,
  imports: [],
  templateUrl: './random-text.component.html',
  styleUrl: './random-text.component.css'
})
export class RandomTextComponent {
  article = '';
  title = '';

  constructor(private backendServcie:BackendService) {    
  }

  ngOnInit(): void {
    this.getArticle();
  }

  private getArticle(): void{

    this.backendServcie.getRandomArticle().subscribe(
      (data) => {
        this.article = data.content;
        this.title = data.title;
      });
  }
}