import { Component, ElementRef, OnInit } from '@angular/core';
import { LanguagePickerComponent } from './language-picker/language-picker.component';
import { NavigationEnd, RouterLink } from '@angular/router';
import { AuthService } from '../services/AuthService';
import { AsyncPipe, NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { filter } from 'rxjs';
import { ClickOutsideDirective } from '../directives/click-outside.directive';
import { BackendService } from '../services/backend.service';

@Component({
  selector: 'stu-header',
  standalone: true,
  imports: [LanguagePickerComponent, RouterLink, NgIf, AsyncPipe, ClickOutsideDirective, ClickOutsideDirective],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements OnInit {
  public version: string | undefined = undefined;
  public username$ = this.auth.username$;
  public lessonsMenuOpen = false;

  public constructor(
    private auth: AuthService,
    private eRef: ElementRef,
    private router: Router,
    private backendService: BackendService
  ) {
    this.router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe(() => {
      this.lessonsMenuOpen = false; // закрыть меню при переходе
    });
  }

  public ngOnInit(): void {
    this.backendService.getCurrentVersion().subscribe(data => (this.version = data));
  }

  public logout() {
    this.auth.logout();
  }

  public toggleLessonsMenu() {
    this.lessonsMenuOpen = !this.lessonsMenuOpen;
  }

  public closeMenu() {
    this.lessonsMenuOpen = false;
  }
}
