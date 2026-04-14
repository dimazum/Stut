// ================= user-control.component.ts =================
import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { debounceTime, distinctUntilChanged, switchMap, Subject, of } from 'rxjs';
import { ChatService } from '../services/chat.service';

interface UserDto {
  id: string;
  userName: string;
  email: string;
}

class UserService {
  private http = inject(HttpClient);

  
  searchUsers(name: string) {
    if (!name?.trim()) return of([] as UserDto[]);
    return this.http.get<UserDto[]>(`/api/users?name=${name}`);
  }

  requestTaskControl(userId: string) {
    return this.http.post('/api/users/request-control', { userId });
  }
}

@Component({
  selector: 'app-user-control',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  providers: [UserService],
  templateUrl: './linked-accounts.component.html',
  styleUrls: ['./linked-accounts.component.css']
})
export class LinkedAccountsComponent {
  private service = inject(UserService);
  private chatService = inject(ChatService);

  private search$ = new Subject<string>();

  users = signal<UserDto[]>([]);
  requestingId = signal<string | null>(null);
  loading = false;
  searchText = '';

  constructor() {
    this.search$
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        switchMap(name => {
          this.loading = true;
          return this.service.searchUsers(name);
        })
      )
      .subscribe({
        next: users => {
          this.users.set(users);
          this.loading = false;
        },
        error: () => (this.loading = false)
      });
  }

  onSearchChange(value: string) {
    this.search$.next(value);
  }

  requestControl(userId: string) {

    this.chatService.sendMessage(userId, "$100");

  }
}
