import { Injectable, signal } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class LoadingService {
  //opened = false;
  opened = signal(false);

  constructor() {

  }

  getOpened() {
    return this.opened();
  }

  show() {
    this.opened.set(true);
  }

  hide() {
    this.opened.set(false);
  }
}
