import { NgIf } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'stu-language-picker',
  standalone: true,
  imports: [NgIf],
  templateUrl: './language-picker.component.html',
  styleUrl: './language-picker.component.css',
})
export class LanguagePickerComponent {
  public currentLang = 'RU';
  public allLanguages = ['RU', 'ENG'];
  public isShown = false;

  public mouseEnter(): void {
    this.isShown = true;
  }

  public mouseLeave(): void {
    this.isShown = false;
  }
}
