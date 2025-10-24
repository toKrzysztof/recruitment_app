import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environment/environment';
import { LoginRequestDto } from '../models/login-request-dto';
import { LoginResponseDto } from '../models/login-response-dto';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { RegisterRequestDto } from '../models/register-request-dto';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'jwtToken';
  private readonly apiUrl = `${environment.apiUrl}/auth`;

  constructor(private http: HttpClient) {}

  isLoggedIn(): boolean {
    const token = localStorage.getItem(this.TOKEN_KEY);
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

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  setToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  removeToken(): void {
    localStorage.removeItem(this.TOKEN_KEY);
  }
}
