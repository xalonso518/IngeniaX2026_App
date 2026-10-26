import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-chip-status',
  imports: [],
  templateUrl: './chip-status.component.html',
  styleUrl: './chip-status.component.css',
})
export class ChipStatusComponent {
  @Input() status: number = 0;
}
