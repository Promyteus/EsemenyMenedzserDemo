import { Routes } from '@angular/router';
import { antiAuthGuard, authGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: '', redirectTo: 'events', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => import('./components/login/login').then(m => m.Login),
    canActivate: [antiAuthGuard]
  },
  {
    path: 'events',
    loadComponent: () => import('./components/event-list/event-list').then(m => m.EventList),
    canActivate: [authGuard]
  },
  {
    path: 'events/new',
    loadComponent: () => import('./components/event-form/event-form').then(m => m.EventForm),
    canActivate: [authGuard]
  },
  {
    path: 'events/edit/:id',
    loadComponent: () => import('./components/event-form/event-form').then(m => m.EventForm),
    canActivate: [authGuard]
  },
  { path: '**', redirectTo: 'events' }
];
