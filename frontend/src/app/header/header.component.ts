import { Component } from "@angular/core";
import { LanguagePickerComponent } from "./language-picker/language-picker.component";
import { RouterLink } from "@angular/router";
import { AuthService } from "../services/AuthService";
import { NgIf } from "@angular/common";

@Component({
    selector: 'app-header',
    standalone: true,
    imports:[LanguagePickerComponent, RouterLink, NgIf],
    templateUrl: './header.component.html',
    styleUrl: './header.component.css'
  })
export class HeaderComponent{

  username: string | null;

  constructor(private auth: AuthService) {
    this.username = this.auth.getUsername();
  }

  logout() {
    this.auth.logout();
  }
}