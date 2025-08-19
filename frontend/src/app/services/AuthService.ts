import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

interface JwtPayload {
  unique_name?: string; // ClaimTypes.Name
  nameid?: string;      // ClaimTypes.NameIdentifier
  exp?: number;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  //private baseUrl = environment.apiUrl + '/auth';
  private baseUrl = '/api/auth';

  // текущий юзер
  private usernameSubject = new BehaviorSubject<string | null>(this.getUsernameFromToken());
  username$ = this.usernameSubject.asObservable();

  constructor(private httpClient: HttpClient, private router: Router) {}

  // регистрация
  register(data: any): Observable<any> {
    return this.httpClient.post(`${this.baseUrl}/register`, data);
  }

  // логин
  login(username: string, password: string): Observable<any> {
    return this.httpClient.post(`${this.baseUrl}/login`, { username, password }).pipe(
      tap((res: any) => {
        localStorage.setItem('token', res.token);
        const decoded = jwtDecode<JwtPayload>(res.token);
        this.usernameSubject.next(decoded.unique_name ?? null);
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.usernameSubject.next(null);  // сбрасываем юзернейм
    this.router.navigate(['/login']); 
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  private getUsernameFromToken(): string | null {
    const token = localStorage.getItem('token');
    if (!token) return null;

    try {
      const decoded = jwtDecode<JwtPayload>(token);
      return decoded.unique_name ?? null;
    } catch {
      return null;
    }
  }
}
