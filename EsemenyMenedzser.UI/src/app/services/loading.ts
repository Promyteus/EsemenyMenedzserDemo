import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class LoadingService {
  private activeRequests = 0;
  loading$ = new BehaviorSubject<boolean>(false);

  show() {
    this.activeRequests++;
    this.loading$.next(true);
  }

  hide() {
    this.activeRequests--;
    if (this.activeRequests <= 0) {
      this.activeRequests = 0;
      this.loading$.next(false);
    }
  }
}
