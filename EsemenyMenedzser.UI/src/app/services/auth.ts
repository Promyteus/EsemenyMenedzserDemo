import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, catchError, Observable, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private apiUrl = 'https://localhost:7087/api/auth';

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  login(credentials: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, credentials).pipe(
      tap(() => {
        this.isAuthenticatedSubject.next(true);
        this.router.navigate(['/events']);
      })
    );
  }

  logout(): Observable<any> {
    return this.http.post(`${this.apiUrl}/logout`, {}).pipe(
      tap(() => {
        this.isAuthenticatedSubject.next(false);
        this.router.navigate(['/login']);
      })
    );
  }

  checkAuthStatus(): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/status`).pipe(
      tap(res => this.isAuthenticatedSubject.next(res)),
      catchError(() => {
        this.isAuthenticatedSubject.next(false);
        this.router.navigate(['/login']);
        return of(false);
      })
    );
  }
}
