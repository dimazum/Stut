import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ArticleData } from '../models/models';

@Component({
  selector: 'app-random-text',
  standalone: true,
  imports: [],
  templateUrl: './random-text.component.html',
  styleUrl: './random-text.component.css'
})
export class RandomTextComponent {

  baseUrl = 'http://localhost:5000'
  article = '';
  title = '';

  constructor(private httpClient:HttpClient) {    
  }

  ngOnInit(): void {
    this.getArticle();
  }

  private getArticle(): void{

    this.httpClient.get<ArticleData>(`${this.baseUrl}/api/article/random`).subscribe(
      (data) => {
        console.log(data);
        this.article = data.content;
        this.title = data.title;
      });

  }
}