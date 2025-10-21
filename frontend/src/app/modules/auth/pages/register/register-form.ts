import { Component, DestroyRef, inject } from '@angular/core';
import { DatePicker } from 'primeng/datepicker';
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
    username: ['', [Validators.required, Validators.email]],
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    password: ['', Validators.required],
    phoneNumber: ['', [Validators.required]],
    dateOfBirth: [null],
    category: [null]
  });

  private parseDate(isoDate: number): string {
    const date = new Date(isoDate);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  protected onSubmit(): void {
    this.loading = true;

    if (this.registerForm.valid) {
      const dateOfBirth = this.parseDate(this.registerForm.value.dateOfBirth!);

      const registerData = {
        ...this.registerForm.getRawValue(),
        dateOfBirth: dateOfBirth
      };

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
