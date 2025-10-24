import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../../../environment/environment';
import { LoginRequestDto } from '../models/login-request-dto';
import { LoginResponseDto } from '../models/login-response-dto';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { RegisterRequestDto } from '../models/register-request-dto';
import { EMAILCLAIMJWTTOKENKEY, TOKEN_KEY } from '../../auth/constants/jwt-constants';
import { JwtDecoded } from '../models/jwt-decoded';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/auth`;
  private userEmailSubject = new BehaviorSubject<string | null>(this.getUserEmail());

  userEmail$ = this.userEmailSubject.asObservable();

  constructor(private http: HttpClient) {}

  isLoggedIn(): boolean {
    const token = localStorage.getItem(TOKEN_KEY);
    if (!token) {
      return false;
    }

    try {
      const decodedToken = jwtDecode<JwtPayload>(token);
      const currentTime = Date.now() / 1000; // Current time in seconds

      // Check if the token is expired
      if (decodedToken.exp === undefined || decodedToken.exp === null) {
        return false;
      }

      return decodedToken.exp > currentTime;
    } catch (error) {
      console.error('Error decoding token:', error);
      return false;
    }
  }

  login(credentials: LoginRequestDto): Observable<LoginResponseDto> {
    return this.http.post<LoginResponseDto>(`${this.apiUrl}/login`, credentials);
  }

  register(credentials: RegisterRequestDto): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(`${this.apiUrl}/register`, credentials);
  }

  getUserEmail(): string | null {
    const token = localStorage.getItem(TOKEN_KEY);
    if (!token) {
      return null;
    }
    try {
      const decodedToken: JwtDecoded = jwtDecode<JwtPayload>(token);
      if (
        decodedToken[EMAILCLAIMJWTTOKENKEY] === undefined ||
        decodedToken[EMAILCLAIMJWTTOKENKEY] === null
      ) {
        return null;
      }

      return decodedToken[EMAILCLAIMJWTTOKENKEY];
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  setToken(token: string): void {
    localStorage.setItem(TOKEN_KEY, token);
    this.userEmailSubject.next(this.getUserEmail());
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    this.userEmailSubject.next(null);
  }
}
