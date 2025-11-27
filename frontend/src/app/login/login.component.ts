import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { AuthService } from '../services/AuthService';

@Component({
  selector: 'stu-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule, NgIf],
})
export class LoginComponent {
  public username = '';
  public password = '';
  public errorMessage = '';

  public constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  public onSubmit() {
    this.authService.login(this.username, this.password).subscribe({
      next: () => this.router.navigate(['/']), // перенаправляем
      error: err => (this.errorMessage = err.error.message || 'Login failed'),
    });
  }
}
