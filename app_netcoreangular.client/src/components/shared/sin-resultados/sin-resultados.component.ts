import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-sin-resultados',
  imports: [],
  templateUrl: './sin-resultados.component.html',
  styleUrl: './sin-resultados.component.css',
})
export class SinResultadosComponent {
  @Input() boton_text: string = "+ Nuevo";

}
