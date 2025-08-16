// register.component.ts
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BackendService } from '../services/backend.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl:'./register.component.css',
  standalone: true,
  imports:[ReactiveFormsModule, NgIf]
})
export class RegisterComponent {
  registerForm: FormGroup;
  errorMessage: string = '';

  constructor(private fb: FormBuilder, private backendService: BackendService) {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      this.backendService.register(this.registerForm.value).subscribe({
        next: (res) => {
          console.log('Регистрация успешна', res);
        },
        error: (err) => {
          this.errorMessage = err.error?.message || 'Ошибка регистрации';
        },
      });
    }
  }
}
