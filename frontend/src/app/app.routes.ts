import { Routes } from '@angular/router';
import { LoginComponent } from './modules/auth/pages/login/login';
import { ContactListComponent } from './modules/contacts/pages/contact-list/contact-list';
import { loggedInGuard } from './modules/core/guards/logged-in-guard';
import { RegisterFormComponent } from './modules/auth/pages/register/register-form';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterFormComponent },
  { path: 'contacts', component: ContactListComponent },
  {
    path: 'contacts/:contactId',
    loadComponent: () =>
      import('./modules/contacts/pages/contact-details/contact-details').then(
        (m) => m.ContactDetailsComponent
      ),
    canActivate: [loggedInGuard]
  },
  {
    path: 'contact/new',
    loadComponent: () =>
      import('./modules/contacts/pages/contact-form/contact-form').then(
        (m) => m.ContactFormComponent
      ),
    canActivate: [loggedInGuard]
  },
  {
    path: 'contacts/edit/:id',
    loadComponent: () =>
      import('./modules/contacts/pages/contact-form/contact-form').then(
        (m) => m.ContactFormComponent
      ),
    canActivate: [loggedInGuard]
  }
];
