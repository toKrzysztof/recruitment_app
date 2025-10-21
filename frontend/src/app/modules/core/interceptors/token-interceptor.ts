import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { environment } from '../../../../../environment/environment';
import { AuthService } from '../services/auth';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  const token = authService.getToken();

  // this is a check that we only include credentials when using our API
  // we don't use any 3rd party APIs but I think this code is useful in case we ever wanted to use 3rd party APIs
  if (
    (token && req.url.toLowerCase().startsWith(environment.apiUrl)) ||
    req.url.toLowerCase().startsWith(environment.apiUrl + '/')
  ) {
    const newReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(newReq);
  }

  return next(req);
};
