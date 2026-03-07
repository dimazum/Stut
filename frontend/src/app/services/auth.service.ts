import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { ConfirmEmailDto, ResetPasswordDto, SendResetPasswordDto } from '../models/models';

interface UserInfo {
  user_name?: string;
  user_role?: string;
  logged_in: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = environment.baseUrl + '/api/auth';

  private userSubject = new BehaviorSubject<UserInfo | null>(null);
  public userinfo$ = this.userSubject.asObservable();

  constructor(private httpClient: HttpClient) {
    this.initAuthState();
  }

  // =====================
  // INIT AUTH ON APP START
  // =====================

  private initAuthState(): void {
    this.checkAuth().subscribe({
      next: user => this.userSubject.next(user),
      error: () => this.userSubject.next(null),
    });
  }

  // =====================
  // REGISTER
  // =====================

  register(data: any): Observable<any> {
    return this.httpClient.post(`${this.baseUrl}/register`, data);
  }

  // =====================
  // LOGIN
  // =====================

  login(username: string, password: string): Observable<UserInfo> {
    return this.httpClient
      .post<UserInfo>(`${this.baseUrl}/login`, { username, password }, { withCredentials: true })
      .pipe(
        tap(user => {
          this.userSubject.next(user);
        })
      );
  }

  // =====================
  // LOGOUT
  // =====================

  logout(): Observable<any> {
    return this.httpClient.post(`${this.baseUrl}/logout`, {}, { withCredentials: true }).pipe(
      tap(() => {
        this.userSubject.next(null);
      })
    );
  }

  // =====================
  // CHECK AUTH (MOST IMPORTANT ⭐)
  // =====================

  checkAuth(): Observable<UserInfo> {
    return this.httpClient.get<UserInfo>(`${this.baseUrl}/me`, { withCredentials: true });
  }

  // =====================
  // EMAIL
  // =====================
  public confirmEmail(confirmEmailDto: ConfirmEmailDto): Observable<any> {
    return this.httpClient.post(`${this.baseUrl}/confirm-email`, confirmEmailDto);
  }

  public sendResetPassword(data: SendResetPasswordDto): Observable<any> {
    return this.httpClient.post(`${this.baseUrl}/send-reset-password`, data);
  }

  public resetPassword(data: ResetPasswordDto): Observable<any> {
    return this.httpClient.post(`${this.baseUrl}/reset-password`, data);
  }

  // =====================
  // HELPERS
  // =====================

  isLoggedIn(): boolean {
    return this.userSubject.value?.logged_in === true;
  }

  get currentUser(): UserInfo | null {
    return this.userSubject.value;
  }
}
