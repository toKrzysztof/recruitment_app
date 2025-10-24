import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth';
import {
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { ValidationErrorsComponent } from '../../../shared/validation-errors/validation-errors';
import { MessageService } from 'primeng/api';
import { Button } from 'primeng/button';
import { errorsIncludeEmailTaken } from '../../utils/email-taken';

@Component({
  selector: 'app-login',
  imports: [ValidationErrorsComponent, ReactiveFormsModule, RouterLink, Button],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class LoginComponent {
  private fb: NonNullableFormBuilder = inject(NonNullableFormBuilder);
  private messageService = inject(MessageService);
  protected loginForm = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  });
  protected loading: boolean = false;

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(): void {
    this.loading = true;
    const loginData = this.loginForm.getRawValue();

    this.authService.login(loginData).subscribe({
      next: (response) => {
        this.authService.setToken(response.jwtToken);
        this.router.navigate(['/contacts']);
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Failure',
          detail: 'Invalid username or password'
        });
        this.loading = false;
      }
    });
  }
}
