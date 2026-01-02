import { Component } from '@angular/core';
import { AuthService } from '../../services/AuthService';
import { AsyncPipe, NgIf } from '@angular/common';
import { LoginComponent } from "../login/login.component";
import { RegisterComponent } from "../register/register.component";
import { LanguagePickerComponent } from "../language-picker/language-picker.component";

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [NgIf, AsyncPipe, LoginComponent, RegisterComponent, LanguagePickerComponent],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent {
  public userinfo$ = this.auth.userinfo$;
  public showLogin = false;
  public showRegister = false;

  constructor(private auth: AuthService) {  
  }

   showLoginPopup() {
    this.showLogin = true;
  }

  showRegisterPopup() {
    this.showRegister = true;
  }

  public logout() {
    this.auth.logout();
  }
}
