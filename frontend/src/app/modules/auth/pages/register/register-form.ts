import { Component, DestroyRef, inject } from '@angular/core';
import { ValidationErrorsComponent } from '../../../shared/validation-errors/validation-errors';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import {
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MessageService } from 'primeng/api';
import { AuthService } from '../../../core/services/auth';

@Component({
  selector: 'app-register-form',
  imports: [ValidationErrorsComponent, ReactiveFormsModule, RouterLink],
  templateUrl: './register-form.html',
  styleUrl: './register-form.scss'
})
export class RegisterFormComponent {
  private fb: NonNullableFormBuilder = inject(NonNullableFormBuilder);
  private router = inject(Router);
  private messageService = inject(MessageService);
  private destroyRef = inject(DestroyRef);
  private authService = inject(AuthService);
  protected loading = false;

  protected filteredCountries: string[] = [];
  protected maxDate = new Date();

  protected registerForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  protected onSubmit(): void {
    this.loading = true;

    if (this.registerForm.valid) {
      const registerData = this.registerForm.getRawValue();

      this.authService
        .register(registerData)
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe({
          next: (_) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Success',
              detail: 'Registration succesful!'
            });
            this.router.navigate(['/login']);
          },
          error: (_) => {
            // todo - display email is already taken
            this.messageService.add({
              severity: 'error',
              summary: 'Failure',
              detail: 'Failed to register'
            });
            this.loading = false;
          }
        });
    }
  }
}
