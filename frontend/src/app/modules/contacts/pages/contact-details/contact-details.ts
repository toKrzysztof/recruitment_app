import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ContactDetailsDto } from '../../models/contact-details-dto';
import { Button } from 'primeng/button';
import { ContactService } from '../../services/contact';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-contact-details',
  imports: [Button, RouterLink],
  templateUrl: './contact-details.html',
  styleUrl: './contact-details.scss'
})
export class ContactDetailsComponent {
  private destroyRef = inject(DestroyRef);
  private route = inject(ActivatedRoute);
  private contactService = inject(ContactService);
  protected contact = signal<ContactDetailsDto>(this.route.snapshot.data['contact']);
  private messageService = inject(MessageService);
  private router = inject(Router);
  private contactId: string = this.route.snapshot.params['contactId'];
  protected deleting = false;

  protected deleteContact(): void {
    this.contactService
      .deleteContact(this.contactId)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (_) => {
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Contact deleted succesfully!'
          });
          this.router.navigate(['/contacts']);
        },
        error: (_) => {
          this.messageService.add({
            severity: 'error',
            summary: 'Failure',
            detail: 'Failed to delete contact. Please try again later'
          });

          this.deleting = false;
        }
      });
  }
}
