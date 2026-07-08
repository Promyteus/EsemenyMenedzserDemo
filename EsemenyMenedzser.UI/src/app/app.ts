import { Component, inject, signal } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AsyncPipe } from '@angular/common';
import { LoadingService } from './services/loading';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Header } from './components/header/header';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, MatProgressSpinnerModule, AsyncPipe, Header],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('EsemenyMenedzser.UI');
  public loadingService = inject(LoadingService);
  public readonly router = inject(Router);
}
