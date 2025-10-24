import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { ContactService } from '../../services/contact';
import { ContactDto } from '../../models/contact-dto';
import { Router, RouterLink } from '@angular/router';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { HttpHeaders, HttpParams } from '@angular/common/http';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { InputText } from 'primeng/inputtext';

@Component({
  selector: 'app-contact-list',
  imports: [TableModule, InputText, RouterLink],
  templateUrl: './contact-list.html',
  styleUrl: './contact-list.scss'
})
export class ContactListComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  protected totalContacts: number = 0;
  protected pageSize: number = 10;
  protected contacts = signal<ContactDto[]>([]);
  protected loading = false;
  private columnFilters: { [key: string]: string } = {
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    category: '',
    subcategory: ''
  };
  private sortField: string | null = null;
  private sortOrder: 'asc' | 'desc' | null = null;

  constructor(private contactService: ContactService, private router: Router) {}

  ngOnInit(): void {
    this.loadContacts({ first: 0, rows: this.pageSize });
  }

  loadContacts(event: TableLazyLoadEvent): void {
    this.loading = true;

    const page = Math.floor((event.first ?? 0) / (event.rows ?? this.pageSize)) + 1;
    const rows = event.rows ?? this.pageSize;

    // sort
    if (event.sortField) {
      this.sortField = event.sortField as string;
      this.sortOrder = event.sortOrder === 1 ? 'asc' : 'desc';
    }

    // Build filter params
    let params = new HttpParams()
      .set('pageNumber', page)
      .set('pageSize', rows.toString());

    if (this.sortField) {
      params = params.set('sortBy', this.sortField).set('sort', this.sortOrder!);
    }

    // append each filter if non-empty
    Object.keys(this.columnFilters).forEach((field) => {
      const value = this.columnFilters[field];
      if (value && value.trim().length > 0) {
        params = params.set(field, value.trim());
      }
    });

    this.contactService
      .getContacts(params)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res) => {
          const pagination = this.extractPaginationInfo(res.headers);
          this.contacts.set(res.body!);
          this.totalContacts = pagination.totalItems;
          this.loading = false;
        },
        error: (e) => {
          console.error('Error loading contacts:', e);
          this.loading = false;
        }
      });
  }

  onColumnFilterChange(field: string, value: string): void {
    // update filter value
    this.columnFilters[field] = value;
    // reset to page 1
    this.loadContacts({
      first: 0,
      rows: this.pageSize,
      sortField: this.sortField,
      sortOrder: this.sortOrder === 'asc' ? 1 : -1
    });
  }

  private extractPaginationInfo(headers: HttpHeaders): { totalItems: number } {
    const paginationHeader = headers.get('pagination');
    if (paginationHeader) {
      try {
        return JSON.parse(paginationHeader);
      } catch (e) {
        console.error('Error parsing pagination header:', e);
      }
    }
    return { totalItems: 0 };
  }

  editContact(id: number): void {
    this.router.navigate([`/contacts/edit/${id}`]);
  }
}
