import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  private readonly authService = inject(AuthService);

  onLogout(): void {
    this.authService.logout().subscribe();
  }
}
