import { inject } from '@angular/core';
import { AuthService } from '../services/auth';
import { CanActivateFn, Router } from '@angular/router';

export const loggedInGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const loggedIn = authService.isLoggedIn();
  const router = inject(Router);

  if (loggedIn) {
    return true;
  } else {
    router.navigate(['/login']);
    return false;
  }
};
