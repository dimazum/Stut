import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../services/notification.service';
import { CommonModule } from '@angular/common';

export interface Notification {
  type: 'success' | 'error';
  message: string;
}

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notification-component.component.html',
  styleUrl: './notification-component.component.css',
})
export class NotificationComponent implements OnInit {
  notifications: Notification[] = [];

  constructor(private ns: NotificationService) {}

  ngOnInit() {
    this.ns.notifications$.subscribe((n) => {
      this.notifications.push(n);

      // Автоудаление через 5 секунд
      setTimeout(() => {
        this.notifications = this.notifications.filter(notif => notif !== n);
      }, 5000);
    });
  }
}