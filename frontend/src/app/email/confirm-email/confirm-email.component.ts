import { Component, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { AuthService } from '../../services/auth.service';
import { ConfirmEmailDto } from '../../models/models';

@UntilDestroy()
@Component({
  selector: 'stu-confirm-email',
  standalone: true,
  imports: [],
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.css',
})
export class ConfirmEmailComponent implements OnInit {
  public constructor(private readonly authService: AuthService) {}

  public ngOnInit(): void {
    const params = new URLSearchParams(window.location.search);

    const userId = params.get('userId');
    const token = params.get('token');

    if (!userId || !token) {
      throw new Error('User id or token were not provided');
    }

    const request: ConfirmEmailDto = {
      userId: userId,
      token: token,
    };

    this.authService.confirmEmail(request).pipe(untilDestroyed(this)).subscribe();
  }

  public backToMainPage() {
    window.location.href = '/';
  }
}
