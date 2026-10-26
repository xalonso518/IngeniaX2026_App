import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-error-input',
  standalone: true,
  templateUrl: './error-input.component.html',
  styleUrl: './error-input.component.css',
})
export class ErrorInputComponent {
  @Input() form_input: any = null;
}
