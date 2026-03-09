import { Component, OnInit } from '@angular/core';
import { BackendService } from '../services/backend.service';
import { CommonModule, NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Trigger } from '../models/models';

@Component({
  selector: 'app-random-text',
  standalone: true,
  imports: [NgFor, FormsModule, CommonModule],
  templateUrl: './random-text.component.html',
  styleUrl: './random-text.component.css',
})
export class RandomTextComponent implements OnInit {
  private RandomTextDropdownKey = 'randomTextDropdown';
  public article = '';
  public title = '';
  public topic = '';
  public source = ''
  public categories: string[] = [];
  public selectedCategory = '';
  public initCategory = '';

  selectedWord: string | null = null;
  menuPosition = { x: 0, y: 0 };
  contextMenuOpened: boolean = false;

  public constructor(private backendServcie: BackendService) { }

  public ngOnInit(): void {
    
    this.selectedCategory = localStorage.getItem(this.RandomTextDropdownKey) ?? this.initCategory;
    this.getArticle(true);
  }

  private getArticle(init: boolean): void {
    this.backendServcie.getRandomArticle(this.selectedCategory).subscribe(data => {
      this.article = data.content;
      this.title = data.title;
      this.topic = data.topic;
      this.source = data.source;
      this.categories = data.categories;
      this.initCategory = data.initCategory;
    })
  }

  getRandomText() {
    this.getArticle(false);
  }

  onTextClick(event: MouseEvent) {
    event.preventDefault();

    const selection = window.getSelection();
    if (!selection) return;

    const word = selection.toString().trim();

    if (!word) return;

    this.selectedWord = word;
    this.contextMenuOpened = true;
    this.menuPosition = {
      x: event.clientX,
      y: event.clientY
    };
  }

  closeContextMenu() {
    this.contextMenuOpened = false;
    this.selectedWord = null;
  }

  addTrigger() {
    if (!this.selectedWord) return;

    const trigger: Trigger = {
      value: this.selectedWord.slice(0, 100),
      difficulty: 0,
    };

    this.backendServcie
      .createTrigger(trigger)
      .subscribe();

    this.closeContextMenu();
  }

  onSelectionChange(event: any): void {
    localStorage.setItem(this.RandomTextDropdownKey, event.value);
  }
}
