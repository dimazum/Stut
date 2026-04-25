import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

export interface Notification {
  type: 'success' | 'error' | 'warning';
  message: string;
}

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private _notifications = new Subject<Notification>();
  notifications$ = this._notifications.asObservable();

  showError(message: string) {
    this._notifications.next({ type: 'error', message });
  }

  showWarning(message: string) {
    this._notifications.next({ type: 'warning', message });
  }

  showSuccess(message: string) {
    this._notifications.next({ type: 'success', message });
  }
}