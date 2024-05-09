import { Component } from "@angular/core";
import { LanguagePickerComponent } from "./language-picker/language-picker.component";
import { RouterLink } from "@angular/router";

@Component({
    selector: 'app-header',
    standalone: true,
    imports:[LanguagePickerComponent, RouterLink],
    templateUrl: './header.component.html',
    styleUrl: './header.component.css'
  })
export class HeaderComponent{
}