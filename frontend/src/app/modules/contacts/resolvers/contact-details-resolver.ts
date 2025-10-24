import { ResolveFn, Router } from '@angular/router';
import { ContactService } from '../services/contact';
import { inject } from '@angular/core';
import { catchError, of } from 'rxjs';

export const contactDetailsResolver: ResolveFn<boolean> = (route, state) => {
  const contactService = inject(ContactService);
  const router = inject(Router);

  const contactId = route.paramMap.get('contactId')!;
  return contactService.getContact(contactId).pipe(
    catchError((_) => {
      router.navigate(['/404']);
      return of(null as any);
    })
  );
};
