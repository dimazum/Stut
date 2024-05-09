import { NgIf } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-language-picker',
  standalone: true,
  imports: [NgIf],
  templateUrl: './language-picker.component.html',
  styleUrl: './language-picker.component.css'
})
export class LanguagePickerComponent {
  currentLang = "RU";
  allLangueges = ['RU', 'ENG']
  isShown = false;

  public mouseEnter(){
    this.isShown = true;
  }

  public mouseLeave(){
    this.isShown = false;
  }
}
