import { Component, OnInit } from '@angular/core';
import { BackendService } from '../services/backend.service';

@Component({
  selector: 'stu-random-text',
  standalone: true,
  imports: [],
  templateUrl: './random-text.component.html',
  styleUrl: './random-text.component.css',
})
export class RandomTextComponent implements OnInit {
  public article = '';
  public title = '';

  public constructor(private backendService: BackendService) {}

  public ngOnInit(): void {
    this.getArticle();
  }

  private getArticle(): void {
    this.backendService.getRandomArticle().subscribe(data => {
      this.article = data.content;
      this.title = data.title;
    });
  }
}
