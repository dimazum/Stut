import { Component, ElementRef, OnInit } from '@angular/core';
import { LanguagePickerComponent } from './language-picker/language-picker.component';
import { NavigationEnd, RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { AsyncPipe, NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { filter } from 'rxjs';
import { ClickOutsideDirective } from '../directives/click-outside.directive';
import { BackendService } from '../services/backend.service';

@Component({
  selector: 'stu-header',
  standalone: true,
  imports: [LanguagePickerComponent, RouterLink, NgIf, AsyncPipe, ClickOutsideDirective, ClickOutsideDirective, RouterOutlet],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements OnInit {
  
  public userinfo$ = this.auth.userinfo$;
  public lessonsMenuOpen = false;
  public voiceAnalysisMenuOpen = false;

  public constructor(
    private auth: AuthService,
    private eRef: ElementRef,
    private router: Router,
    private backendService: BackendService
  ) {
    this.router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe(() => {
      this.lessonsMenuOpen = false; // закрыть меню при переходе
      this.voiceAnalysisMenuOpen = false;
    });
  }

  public ngOnInit(): void {
  }

  public logout() {
    this.auth.logout().subscribe();
  }

  public toggleLessonsMenu() {
    this.lessonsMenuOpen = !this.lessonsMenuOpen;
  }

   public toggleVoiceAnalysisMenu() {
    this.voiceAnalysisMenuOpen = !this.voiceAnalysisMenuOpen;
  }

  public closeMenu() {
    this.lessonsMenuOpen = false;
  }
}
