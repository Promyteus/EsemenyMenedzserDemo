import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Router, RouterModule } from '@angular/router';
import { EventService } from '../../services/event';
import { EventItem } from '../../models/event.model';

@Component({
  selector: 'app-event-list',
  standalone: true,
  imports: [CommonModule, RouterModule, MatTableModule, MatSortModule, MatButtonModule, MatIconModule],
  templateUrl: './event-list.html',
  styleUrl: './event-list.css',
})
export class EventList implements OnInit{
  private eventService = inject(EventService);
  private router = inject(Router);

  displayedColumns: string[] = ['name', 'place', 'capacity', 'actions'];
  dataSource = new MatTableDataSource<EventItem>([]);

  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit() {
    this.loadEvents();
  }

  loadEvents() {
    this.eventService.getEvents().subscribe(events => {
      this.dataSource.data = events;
      this.dataSource.sort = this.sort;
    });
  }

  deleteEvent(id: number) {
    if (confirm('Are you sure you want to delete this event?')) {
      this.eventService.deleteEvent(id).subscribe(() => this.loadEvents());
    }
  }

  editEvent(id: number) {
    this.router.navigate(['/events/edit', id]);
  }
}
