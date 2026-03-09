import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'stu-user-info',
  templateUrl: './user-info.component.html',
  styleUrl: './user-info.component.css',
  standalone: true,
  imports: [],
})
export class UserInfoComponent {

  @Output() close = new EventEmitter<void>();

  constructor(private authService: AuthService) {}

  logout() {
    this.authService.logout().subscribe();
    this.close.emit();
  }
}
