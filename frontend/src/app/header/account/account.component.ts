import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { AsyncPipe, NgIf } from '@angular/common';
import { LoginComponent } from '../login/login.component';
import { RegisterComponent } from '../register/register.component';
import { LanguagePickerComponent } from '../language-picker/language-picker.component';
import { notLoggedInSubject } from '../../models/events';
import { Subscription } from 'rxjs';
import { UserInfoComponent } from '../user-info/user-info.component';
import { BackendService } from '../../services/backend.service';
import { RouterLink } from '@angular/router';
import { SendResetPasswordComponent } from '../send-reset-password/send-reset-password.component';

@Component({
  // eslint-disable-next-line @angular-eslint/component-selector
  selector: 'app-account',
  standalone: true,
  imports: [
    NgIf,
    AsyncPipe,
    LoginComponent,
    RegisterComponent,
    LanguagePickerComponent,
    UserInfoComponent,
    RouterLink,
    SendResetPasswordComponent,
  ],
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

  public constructor(
    private auth: AuthService,
    private backendService: BackendService
  ) {}

  public ngOnInit(): void {
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    this.subscription = notLoggedInSubject.subscribe(_ => {
      this.showLoginPopup();
    });
  }

  public ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  public showLoginPopup() {
    this.showLogin = true;
  }

  public showRegisterPopup() {
    this.showRegister = true;
  }

  public showResetPasswordPopup() {
    this.showResetPassword = true;
  }

  public logout() {
    this.auth.logout();
  }

  public openUserInfo() {
    this.showUserInfo = !this.showUserInfo;
  }
}
