import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import {
  Validators,
  ReactiveFormsModule,
  NonNullableFormBuilder
} from '@angular/forms';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MessageService } from 'primeng/api';
import { ValidationErrorsComponent } from '../../../shared/validation-errors/validation-errors';
import { DatePicker } from 'primeng/datepicker';
import { ContactService } from '../../services/contact';
import { InputText } from 'primeng/inputtext';
import { Button } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { CategoryService } from '../../services/category';
import { CategoryDto } from '../../models/category-dto';
import { ContactDetailsDto } from '../../models/contact-details-dto';

@Component({
  selector: 'app-contact-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    ValidationErrorsComponent,
    DatePicker,
    InputText,
    Button,
    SelectModule
  ],
  templateUrl: './contact-form.html',
  styleUrl: './contact-form.scss'
})
export class ContactFormComponent implements OnInit {
  private fb: NonNullableFormBuilder = inject(NonNullableFormBuilder);
  private router = inject(Router);
  private messageService = inject(MessageService);
  private destroyRef = inject(DestroyRef);
  private contactService = inject(ContactService);
  private categoryService = inject(CategoryService);

  protected loading = false;
  protected categories: any[] = [];
  protected subcategories: any[] = [];
  protected maxDate = new Date();

  protected showSubcategory = false;
  // enum would be better
  protected isOtherCategory = false;

  protected contactForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(8)]],
    phoneNumber: ['', Validators.required],
    dateOfBirth: [null, Validators.required],
    category: [null, Validators.required],
    subcategory: [null, Validators.maxLength(200)]
  });

  ngOnInit(): void {
    this.loadCategories();
  }

  private loadCategories(): void {
    this.categoryService
      .getCategories()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res) => (this.categories = res),
        error: () =>
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Failed to load categories'
          })
      });
  }

  protected onCategoryChange(selectedCategory: any): void {
    if (!selectedCategory) return;

    this.isOtherCategory = selectedCategory.name === 'Other';
    this.showSubcategory = true;

    if (this.isOtherCategory) {
      this.contactForm.patchValue({ subcategory: null });
      this.subcategories = [];
    } else {
      this.loadSubcategories(selectedCategory.id);
    }
  }

  private loadSubcategories(categoryId: number): void {
    this.categoryService
      .getSubcategories(categoryId)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res) => (this.subcategories = res),
        error: () =>
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Failed to load subcategories'
          })
      });
  }

  private parseDate(isoDate: number): string {
    const date = new Date(isoDate);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  protected onSubmit(): void {
    if (this.contactForm.invalid) return;

    this.loading = true;
    const form = this.contactForm.getRawValue();

    const dateOfBirth = form.dateOfBirth ? this.parseDate(form.dateOfBirth) : null;

    let payload: ContactDetailsDto = {
      firstName: form.firstName,
      lastName: form.lastName,
      email: form.email,
      phoneNumber: form.phoneNumber,
      password: form.password,
      dateOfBirth: dateOfBirth!,
      category: { name: (form.category! as CategoryDto).name },
      subcategory: form.subcategory
    };

    this.contactService
      .createContact(payload)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Contact added successfully!'
          });
          this.router.navigate(['/contacts']);
        },
        error: (err) => {
          console.error(err);
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
