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
  @Output() public closeEvent: EventEmitter<void> = new EventEmitter<void>();

  public constructor(
    private readonly fb: FormBuilder,
    private readonly authService: AuthService,
    private readonly router: Router,
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
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        next: (res: any) => {
          this.toasterService.info('Successfully registered. Verification email was sent');
          this.closeEvent.emit();
          // this.router.navigate(['/']);
          window.location.href = '/';
        },
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        error: (err: any) => {
          this.errorMessage = err.error?.message || 'Ошибка регистрации';
        },
      });
    }
  }
}
