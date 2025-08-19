import { Component } from "@angular/core";
import { LanguagePickerComponent } from "./language-picker/language-picker.component";
import { RouterLink } from "@angular/router";
import { AuthService } from "../services/AuthService";
import { AsyncPipe, NgIf } from "@angular/common";

@Component({
    selector: 'app-header',
    standalone: true,
    imports:[LanguagePickerComponent, RouterLink, NgIf,AsyncPipe],
    templateUrl: './header.component.html',
    styleUrl: './header.component.css'
  })
export class HeaderComponent{


  username$ = this.auth.username$;

  constructor(private auth: AuthService) {
  }

  logout() {
    this.auth.logout();
  }
}