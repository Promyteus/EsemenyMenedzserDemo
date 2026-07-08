import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private apiUrl = 'http://localhost:7087/api/auth';

  private loggedInSubject = new BehaviorSubject<boolean>(false);
  currentUser$ = this.loggedInSubject.asObservable();

  login(credentials: any) {
    // `{ withCredentials: true }` is crucial for the browser to accept the cookie from the server!
    return this.http.post(`${this.apiUrl}/login`, credentials, { withCredentials: true }).pipe(
      tap(() => {
        this.loggedInSubject.next(true);
        this.router.navigate(['/events']);
      })
    );
  }

  logout() {
    return this.http.post(`${this.apiUrl}/logout`, {}, { withCredentials: true }).pipe(
      tap(() => {
        this.loggedInSubject.next(false);
        this.router.navigate(['/login']);
      })
    ).subscribe();
  }

  isLoggedIn(): boolean {
    return this.loggedInSubject.value;
  }
}
