import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { SendResetPasswordDto } from '../../models/models';

@UntilDestroy()
@Component({
  selector: 'stu-send-reset-password',
  standalone: true,
  imports: [FormsModule, NgIf, ReactiveFormsModule],
  templateUrl: './send-reset-password.component.html',
  styleUrl: './send-reset-password.component.css',
})
export class SendResetPasswordComponent {
  @Output() public closeEvent: EventEmitter<void> = new EventEmitter<void>();

  public sendResetPasswordForm: FormGroup;
  public errorMessage: string = '';

  public constructor(
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService,
    private readonly toasterService: ToastrService
  ) {
    this.sendResetPasswordForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  public onSubmit() {
    if (this.sendResetPasswordForm.valid) {
      const request: SendResetPasswordDto = this.sendResetPasswordForm.value;

      this.authService
        .sendResetPassword(request)
        .pipe(untilDestroyed(this))
        .subscribe({
          // eslint-disable-next-line @typescript-eslint/no-explicit-any
          next: (res: any) => {
            this.toasterService.info('Письмо на смену пароля отправлено');
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
