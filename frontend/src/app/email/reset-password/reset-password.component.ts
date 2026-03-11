import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth.service';
import { ResetPasswordDto } from '../../models/models';

@UntilDestroy()
@Component({
  selector: 'stu-reset-password',
  standalone: true,
  imports: [NgIf, ReactiveFormsModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css',
})
export class ResetPasswordComponent implements OnInit {
  @Output() public closeEvent: EventEmitter<void> = new EventEmitter<void>();

  public resetPasswordForm: FormGroup;
  public errorMessage: string = '';

  private userId: string | null = null;
  private token: string | null = null;

  public constructor(
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly toasterService: ToastrService
  ) {
    this.resetPasswordForm = this.formBuilder.group({
      password: ['', [Validators.required]],
    });
  }

  public ngOnInit(): void {
    const params = new URLSearchParams(window.location.search);

    const userId = params.get('userId');
    const token = params.get('token');

    if (!userId || !token) {
      throw new Error('User id or token were not provided');
    }

    this.userId = userId;
    this.token = token;

    const request: ResetPasswordDto = {
      userId: userId,
      token: token,
      password: '',
    };

    this.authService
      .resetPassword(request)
      .pipe(untilDestroyed(this))
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      .subscribe({
        next: () => {
          this.router.navigate(['/']);
        },
        // error: err => {},
      });
  }

  public onSubmit() {
    if (this.resetPasswordForm.valid) {
      const request: ResetPasswordDto = {
        userId: this.userId,
        token: this.token,
        password: this.resetPasswordForm.value.password,
      };

      this.authService
        .resetPassword(request)
        .pipe(untilDestroyed(this))
        .subscribe({
          // eslint-disable-next-line @typescript-eslint/no-explicit-any
          next: (res: any) => {
            this.toasterService.info('Password was reset successfully');
            this.closeEvent.emit();
            window.location.href = '/';
          },
          // eslint-disable-next-line @typescript-eslint/no-explicit-any
          error: (err: any) => {
            this.errorMessage = err.error?.message || 'Ошибка';
          },
        });
    }
  }
}
