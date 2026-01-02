import { Component } from '@angular/core';
import { AuthService } from '../../services/AuthService';
import { AsyncPipe, NgIf } from '@angular/common';
import { Router} from '@angular/router';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [NgIf, AsyncPipe],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent {
  public userinfo$ = this.auth.userinfo$;

  constructor(private auth: AuthService, private router: Router) {  
  }

   goToLogin() {
    this.router.navigate(['/login']);
  }

  public logout() {
    this.auth.logout();
  }
}
