import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';
import { Observable, tap } from 'rxjs';
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
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']); 
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  getUsername(): string | null {
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
