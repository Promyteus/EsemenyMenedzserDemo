import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { EventService } from '../../services/event';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-event-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, MatCardModule, MatInputModule, MatButtonModule, MatSnackBarModule],
  templateUrl: './event-form.html',
  styleUrl: './event-form.css',
})
export class EventForm implements OnInit {
  private fb = inject(FormBuilder);
  private eventService = inject(EventService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  eventId?: number;
  isEditMode = false;

  eventForm = this.fb.group({
    name: ['', Validators.required],
    location: ['', [Validators.required, Validators.maxLength(100)]],
    country: [''],
    capacity: [null as number | null, [Validators.min(0)]]
  });

  ngOnInit() {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.eventId = +idParam;
      this.isEditMode = true;
      this.eventService.getEvent(this.eventId).subscribe(event => {
        this.eventForm.patchValue(event);
      });
    }
  }

  onSubmit() {
    if (this.eventForm.invalid) return;

    const eventData = this.eventForm.value as any;

    if (this.isEditMode && this.eventId) {
      eventData.id = this.eventId;
      this.eventService.updateEvent(eventData).subscribe({
        next: () => {
          this.showSuccess('Esemény sikeresen frissítve!');
          this.navigateBack();
        },
        error: (err) => {
          this.showError(`Mentési hiba (${err.status}): A szerver elutasította a kérést!`);
        }
      });
    } else {
      this.eventService.createEvent(eventData).subscribe({
        next: () => {
          this.showSuccess('Esemény sikeresen létrehozva!');
          this.navigateBack();
        },
        error: (err) => {
          this.showError(`Mentési hiba (${err.status}): Nem sikerült létrehozni az eseményt!`);
        }
      });
    }
  }
  showError(message: string) {
    this.snackBar.open(message, 'Close', { duration: 5000, panelClass: ['error-snackbar'] });

  }
  showSuccess(message: string) {
    this.snackBar.open(message, 'OK', { duration: 3000, panelClass: ['success-snackbar'] });
  }

  navigateBack() {
    this.router.navigate(['/events']);
  }
}
