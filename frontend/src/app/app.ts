import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PrimeNG } from 'primeng/config';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToastModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('recruitment_app_frontend');
  private readonly primeng = inject(PrimeNG);

  public ngOnInit(): void {
    this.primeng.overlayOptions = {
      appendTo: 'body'
    };
  }
}
