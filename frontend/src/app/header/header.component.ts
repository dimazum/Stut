import { Component, ElementRef, HostListener } from "@angular/core";
import { LanguagePickerComponent } from "./language-picker/language-picker.component";
import { NavigationEnd, RouterLink } from "@angular/router";
import { AuthService } from "../services/AuthService";
import { AsyncPipe, NgIf } from "@angular/common";
import { ClickOutsideDirective } from "../directives/click-outside.directive.";
import { Router } from '@angular/router';
import { filter } from "rxjs";

@Component({
    selector: 'app-header',
    standalone: true,
    imports:[LanguagePickerComponent, RouterLink, NgIf,AsyncPipe, ClickOutsideDirective],
    templateUrl: './header.component.html',
    styleUrl: './header.component.css'
  })
export class HeaderComponent{

  username$ = this.auth.username$;
  lessonsMenuOpen = false;

constructor(private auth: AuthService,  private eRef: ElementRef, private router: Router) {

this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {        
        this.lessonsMenuOpen = false; // закрыть меню при переходе
      });
  }

  logout() {
    this.auth.logout();
  }

  toggleLessonsMenu(event: Event) {
    this.lessonsMenuOpen = !this.lessonsMenuOpen;
  }

  closeMenu(){
    this.lessonsMenuOpen = false;
  }
}