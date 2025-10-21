import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import {
  Validators,
  ReactiveFormsModule,
  NonNullableFormBuilder
} from '@angular/forms';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MessageService } from 'primeng/api';
import { AuthService } from '../../../core/services/auth';
import { ValidationErrorsComponent } from '../../../shared/validation-errors/validation-errors';
import { DatePicker } from 'primeng/datepicker';
import { ContactService } from '../../services/contact';

@Component({
  selector: 'app-contact-form',
  imports: [ReactiveFormsModule, ValidationErrorsComponent, DatePicker],
  templateUrl: './contact-form.html',
  styleUrl: './contact-form.scss'
})
export class ContactFormComponent {
  private fb: NonNullableFormBuilder = inject(NonNullableFormBuilder);
  private router = inject(Router);
  private messageService = inject(MessageService);
  private destroyRef = inject(DestroyRef);
  private contactService = inject(ContactService);
  protected loading = false;

  protected filteredCountries: string[] = [];
  protected maxDate = new Date();

  protected contactForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    phoneNumber: ['', [Validators.required]],
    dateOfBirth: [],
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

    if (this.contactForm.valid) {
      const dateOfBirth = this.parseDate(this.contactForm.value.dateOfBirth!);

      const registerData = {
        ...this.contactForm.getRawValue(),
        dateOfBirth: dateOfBirth
      };

      this.contactService
        .createContact(registerData)
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe({
          next: (_) => {
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Contact added succesfully!'
            });
            this.router.navigate(['/contacts']);
          },
          error: (_) => {
            console.log('test', _);
            this.messageService.add({
              severity: 'error',
              summary: 'Failure',
              detail: 'Failed to add contact'
            });
            this.loading = false;
          }
        });
    }
  }
}
