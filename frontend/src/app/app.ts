import { Component, inject, signal } from '@angular/core';
import { Router, RouterOutlet, RouterLink } from '@angular/router';
import { Button } from 'primeng/button';
import { PrimeNG } from 'primeng/config';
import { ToastModule } from 'primeng/toast';
import { AuthService } from './modules/core/services/auth';
import { toSignal } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToastModule, Button, RouterLink],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('recruitment_app_frontend');
  private router = inject(Router);
  private authService = inject(AuthService);
  protected userEmail$ = this.authService.userEmail$;
  protected userEmail = toSignal(this.userEmail$);
  private readonly primeng = inject(PrimeNG);

  public ngOnInit(): void {
    this.primeng.overlayOptions = {
      appendTo: 'body'
    };
  }

  protected logout(): void {
    this.authService.logout();
    this.router.navigate(['/contacts']);
  }
}
