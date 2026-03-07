import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'stu-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf],
})
export class RegisterComponent {
  public registerForm: FormGroup;
  public errorMessage: string = '';
  @Output() close = new EventEmitter<void>();

  public constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private readonly toasterService: ToastrService
  ) {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  public onSubmit() {
    if (this.registerForm.valid) {
      this.authService.register(this.registerForm.value).subscribe({
        next: res => {
          this.toasterService.info('Successfully registered. Verification email was sent');
          this.close.emit();
          window.location.href = '/';
        },
        error: err => {
          this.errorMessage = err.error?.message || 'Ошибка регистрации';
        },
      });
    }
  }
}
