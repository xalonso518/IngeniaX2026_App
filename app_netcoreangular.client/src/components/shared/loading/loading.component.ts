import { Component, Injectable } from '@angular/core';
import { MatProgressSpinner, MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-loading',
  standalone: true,
  templateUrl: './loading.component.html',
  styleUrl: './loading.component.css',
  imports: [MatProgressSpinner]
})
export class LoadingComponent {
  /** Loading ctor */
  constructor() {

  }
}

