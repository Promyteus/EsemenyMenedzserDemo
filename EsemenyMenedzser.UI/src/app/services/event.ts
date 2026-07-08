import { inject, Injectable, Service } from '@angular/core';
import { Observable } from 'rxjs';
import { EventItem } from '../models/event.model';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class EventService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7087/api/events';

  getEvents(): Observable<EventItem[]> {
    return this.http.get<EventItem[]>(this.apiUrl);
  }

  getEvent(id: number): Observable<EventItem> {
    return this.http.get<EventItem>(`${this.apiUrl}/${id}`);
  }

  createEvent(event: EventItem): Observable<EventItem> {
    return this.http.post<EventItem>(this.apiUrl, event);
  }

  updateEvent(event: EventItem): Observable<EventItem> {
    return this.http.put<EventItem>(`${this.apiUrl}`, event);
  }

  deleteEvent(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
