import { Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MATERIAL_MODULES_MODAL } from '../../../utils/material/materialModules';

@Component({
  selector: 'app-modal-exito',
  templateUrl: './modal-exito.component.html',
  styleUrl: './modal-exito.component.css',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatButtonModule,
    ...MATERIAL_MODULES_MODAL
  ],
  providers: [],
})
export class ModalExitoComponent {
  // ATRIBUTOS
  modulo = ""; // Nombre del modilo, usuario, operacion
  valor = ""; // Valor a eliminar

  // CONTRUCTOR
  constructor(
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<ModalExitoComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { modulo: string, valor: string }) {
  }

  // Init
  ngOnInit(): void {
    // Si recibe valores los asigansmos, de lo contrario cerramos
    if (this.data != null && this.data.modulo != null) {
      this.modulo = this.data.modulo;
      this.valor = this.data.valor;
    } else {
      this.onNoClick();
    }
  }

  // Cerrar
  onNoClick(): void {
    this.dialogRef.close(true);
  }

}
