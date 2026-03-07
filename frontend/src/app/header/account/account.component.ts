import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { AsyncPipe, NgIf } from '@angular/common';
import { LoginComponent } from "../login/login.component";
import { RegisterComponent } from "../register/register.component";
import { LanguagePickerComponent } from "../language-picker/language-picker.component";
import { notLoggedInSubject } from '../../models/events';
import { Subscription } from 'rxjs';
import { UserInfoComponent } from "../user-info/user-info.component";
import { BackendService } from '../../services/backend.service';
import { SendResetPasswordComponent } from '../../email/send-reset-password/send-reset-password.component';


@Component({
  selector: 'app-account',
  standalone: true,
  imports: [NgIf, AsyncPipe, LoginComponent, RegisterComponent, LanguagePickerComponent, UserInfoComponent, SendResetPasswordComponent],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css',
})
export class AccountComponent implements OnInit, OnDestroy {
  public userinfo$ = this.auth.userinfo$;
  public rewardPoints$ = this.backendService.getRewardPoints();

  public showLogin = false;
  public showRegister = false;
  public showResetPassword = false;
  public showUserInfo = false;
  private subscription!: Subscription;

  constructor(
    private auth: AuthService,
    private backendService: BackendService
  ) {}

  ngOnInit(): void {
    this.subscription = notLoggedInSubject.subscribe(_ => {
      this.showLoginPopup();
    });
  }

  showLoginPopup() {
    this.showLogin = true;
  }

  showRegisterPopup() {
    this.showRegister = true;
  }

  public showResetPasswordPopup() {
    this.showResetPassword = true;
  }

  public logout() {
    this.auth.logout().subscribe();
  }

  public openUserInfo() {
    this.showUserInfo = !this.showUserInfo;
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
