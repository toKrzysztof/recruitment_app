import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { ContactService } from '../../services/contact';
import { ContactDto } from '../../models/contact-dto';
import { Router } from '@angular/router';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { HttpHeaders } from '@angular/common/http';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-contact-list',
  imports: [TableModule],
  templateUrl: './contact-list.html',
  styleUrl: './contact-list.scss'
})
export class ContactListComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  protected totalContacts: number = 0;
  protected pageSize: number = 10;
  protected contacts = signal<ContactDto[]>([]);
  protected loading = false;

  constructor(private contactService: ContactService, private router: Router) {}

  ngOnInit(): void {
    this.loadContacts({ first: 0, rows: this.pageSize });
  }

  loadContacts(event: TableLazyLoadEvent): void {
    this.loading = true;
    const page = Math.floor(event.first! / event.rows!) + 1;
    this.contactService
      .getContacts(page, event.rows!)
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
