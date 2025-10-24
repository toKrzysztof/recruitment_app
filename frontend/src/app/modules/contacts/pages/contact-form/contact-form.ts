import { Component, DestroyRef, OnInit, inject, signal } from '@angular/core';
import {
  Validators,
  ReactiveFormsModule,
  NonNullableFormBuilder
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
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
import { ContactForm } from '../../models/contact-form';

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
  private route = inject(ActivatedRoute);

  protected loading = false;
  protected categories: any[] = [];
  protected subcategories: any[] = [];
  protected maxDate = new Date();
  protected contact = signal<ContactDetailsDto | undefined>(
    this.route.snapshot.data['contact']
  );

  protected showSubcategory = false;
  // enum would be better
  protected isOtherCategory = false;

  protected contactForm = this.fb.group<ContactForm>({
    email: this.fb.control('', [Validators.required, Validators.email]),
    firstName: this.fb.control('', Validators.required),
    lastName: this.fb.control('', Validators.required),
    password: this.fb.control('', [Validators.required, Validators.minLength(8)]),
    phoneNumber: this.fb.control('', Validators.required),
    dateOfBirth: this.fb.control(null, Validators.required),
    category: this.fb.control(null, Validators.required),
    subcategory: this.fb.control(null, Validators.maxLength(200))
  });

  ngOnInit(): void {
    this.loadCategories();
    if (this.contact() !== undefined) {
      const { id, category, subcategory, dateOfBirth, ...upsertData } = this.contact()!;
      this.contactForm.patchValue({
        ...upsertData,
        category: null,
        subcategory: null,
        dateOfBirth: new Date(dateOfBirth)
      });
    }
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

    this.showSubcategory = true;

    if (selectedCategory.name === 'Other') {
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

  protected getActionLabel(): string {
    if (this.loading) {
      return this.contact() === undefined ? 'Creating...' : 'Updating...';
    } else {
      return this.contact() === undefined ? 'Create' : 'Update';
    }
  }

  protected onSubmit(): void {
    if (this.contactForm.invalid) return;

    this.loading = true;
    const form = this.contactForm.getRawValue();

    const dateOfBirth = form.dateOfBirth
      ? this.parseDate(form.dateOfBirth.getTime())
      : null;

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

    if (this.contact() === undefined) {
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
    } else {
      this.contactService
        .updateContact(payload, this.contact()!.id!)
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe({
          next: () => {
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Contact updated successfully!'
            });
            this.router.navigate(['/contacts']);
          },
          error: (err) => {
            console.error(err);
            this.messageService.add({
              severity: 'error',
              summary: 'Failure',
              detail: 'Failed to update contact'
            });
            this.loading = false;
          }
        });
    }
  }
}
