import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'stu-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule, NgIf],
})
export class LoginComponent {
  username = '';
  password = '';
  errorMessage = '';

  @Output() close = new EventEmitter<void>();

  constructor(
    private authService: AuthService
  ) {}

  onSubmit() {
    this.authService.login(this.username, this.password).subscribe({
      next: () => {
        this.close.emit();       // закрываем попап
      },
      error: err =>
        (this.errorMessage = err.error?.message || 'Login failed'),
    });
  }
}
