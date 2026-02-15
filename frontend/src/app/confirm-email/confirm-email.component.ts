import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { ConfirmEmailDto } from '../models/models';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Router } from '@angular/router';

@UntilDestroy()
@Component({
  selector: 'stu-confirm-email',
  standalone: true,
  imports: [],
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.css',
})
export class ConfirmEmailComponent implements OnInit {
  public constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

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

    this.authService
      .confirmEmail(request)
      .pipe(untilDestroyed(this))
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      .subscribe({
        next: () => {
          this.router.navigate(['/']);
        },
        // error: err => {},
      });
  }
}
