import { Routes } from '@angular/router';
import { ContactListComponent } from './modules/contacts/pages/contact-list/contact-list';
import { loggedInGuard } from './modules/core/guards/logged-in-guard';
import { RegisterFormComponent } from './modules/auth/pages/register/register-form';
import { LoginFormComponent } from './modules/auth/pages/login/login-form';
import { contactDetailsResolver } from './modules/contacts/resolvers/contact-details-resolver';
import { NotFound } from './modules/public/pages/not-found/not-found';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginFormComponent },
  { path: 'register', component: RegisterFormComponent },
  { path: 'contacts', component: ContactListComponent },
  {
    path: 'contacts/:contactId',
    loadComponent: () =>
      import('./modules/contacts/pages/contact-details/contact-details').then(
        (m) => m.ContactDetailsComponent
      ),
    canActivate: [loggedInGuard],
    resolve: {
      contact: contactDetailsResolver
    }
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
    canActivate: [loggedInGuard],
    resolve: {
      contact: contactDetailsResolver
    }
  },
  {
    path: '404',
    component: NotFound,
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: '404'
  }
];
