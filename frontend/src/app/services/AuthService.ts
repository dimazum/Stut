import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

interface JwtPayload {
  unique_name?: string; // ClaimTypes.Name
  nameid?: string; // ClaimTypes.NameIdentifier
  role?: string | string[]; //ClaimTypes.Roles
  exp?: number;
}

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

  // текущий юзер
  private usernameSubject = new BehaviorSubject<UserInfo | null>(this.getUserInfoFromToken());
  public userinfo$ = this.usernameSubject.asObservable();

  public constructor(
    private httpClient: HttpClient,
    private router: Router
  ) {}

  // регистрация
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public register(data: any): Observable<any> {
    return this.httpClient.post(`${this.baseUrl}/register`, data);
  }

  // логин
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public login(username: string, password: string): Observable<any> {
    return this.httpClient.post(`${this.baseUrl}/login`, { username, password }).pipe(
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      tap((res: any) => {
        localStorage.setItem('token', res.token);

        const userInfo = this.parceJwtToken(res.token);

        this.usernameSubject.next(userInfo ?? null);
      })
    );
  }

  public logout() {
    localStorage.removeItem('token');
    this.usernameSubject.next(null); // сбрасываем юзеринфо
    this.router.navigate(['/login']);
  }

  public isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  private getUserInfoFromToken(): UserInfo | null {
    const token = localStorage.getItem('token');
    if (!token) return null;

    try {
       const userInfo = this.parceJwtToken(token);
      
      return userInfo ?? null;
    } catch {
      return null;
    }
  }

  private parceJwtToken(token: string) : UserInfo | null{
    const decoded = jwtDecode<JwtPayload>(token);

        let roleString: string = '';

        if (typeof decoded.role === "string") {
          roleString = decoded.role;
        } else if (Array.isArray(decoded.role)) {
          roleString = decoded.role.join(", ");
        }

        const userInfo : UserInfo = {
          user_name : decoded.unique_name,
          user_role : roleString,
          logged_in : roleString.trim() !== ""
        }

        return userInfo;
  }
}
