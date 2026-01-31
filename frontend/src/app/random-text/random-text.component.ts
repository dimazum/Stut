import { Component, OnInit } from '@angular/core';
import { BackendService } from '../services/backend.service';
import { NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-random-text',
  standalone: true,
  imports: [NgFor, FormsModule],
  templateUrl: './random-text.component.html',
  styleUrl: './random-text.component.css',
})
export class RandomTextComponent implements OnInit {

  public article = '';
  public title = '';
  public topic = '';
  public source = ''
  public categories: string[] = [];
  public selectedCategory = '';

  public constructor(private backendServcie: BackendService) {}

  public ngOnInit(): void {
    this.getArticle(true);
  }

  private getArticle(init: boolean): void {
    this.backendServcie.getRandomArticle(this.selectedCategory).subscribe(data => {
      this.article = data.content;
      this.title = data.title;
      this.topic = data.topic;
      this.source = data.source;
      this.categories = data.categories;
      this.selectedCategory = init? data.initCategory : data.topic;
    });
  }


  getRandomText() {
    this.getArticle(false);
  }
}
