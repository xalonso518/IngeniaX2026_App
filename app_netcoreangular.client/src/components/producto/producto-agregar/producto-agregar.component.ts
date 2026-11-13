import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UsuarioForm } from '../../../models/usuario/UsuarioForm';
import { MESSAGES_ERROR, MESSAGES_SUCCESS } from '../../../utils/constants/ConstantsMessages';
import { HandleErrorService } from '../../../utils/services/handle-error.service';
import { ConstantsCatalogos, MODULOS } from '../../../utils/constants/ConstantsCatalogos';
import { FormControl, FormsModule } from '@angular/forms';
import { LoadingService } from '../../shared/loading/loading.service';
import { ProductoService } from '../services/producto.service';
import { ProductoForm } from '../../../models/producto/ProductoForm';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-producto-agregar',
  styleUrl: './producto-agregar.component.css',
  templateUrl: './producto-agregar.component.html',
  imports: [RouterLink, FormsModule],
  providers: [ProductoService]
})
export class ProductoAgregarComponent {
  // PROPIEDADES
  submitted = signal(false);
  private _snackBar = inject(MatSnackBar);

  // Catalogos
  errores = MESSAGES_ERROR;

  // Form
  form = new ProductoForm();

  // CONSTRUCTOR
  constructor(
    private productoService: ProductoService,
    private handleError: HandleErrorService,
    private loadingService: LoadingService,
  ) {
  }

  // FUNCIONES
  // Inicial
  ngOnInit(): void {

  }

  // Evento submit
  submit(formAgregar: FormControl) {
    this.agregar(this.form);
  }

  // Agregar usuario api
  async agregar(form: ProductoForm) {
    // Mostramos loadings
    this.submitted.set(true);
    this.loadingService.show();

    // Llamamos API
    this.productoService.agregar(form)
      .subscribe({
        next: res => {
          // Validamos response
          if (res.success) {
            // Mostramos mensaje
            this._snackBar.open("Producto creado.", "", {verticalPosition: "top", duration: 3000});

          } else this.handleError.showError(res);
        },
        error: err => {
          // Ocultamos loadings
          this.loadingService.hide();
          this.submitted.set(false);

          // Mostramos errores
          this.handleError.showError(err);
        },
        complete: () => {
          // Ocultamos loadings
          this.loadingService.hide();
          this.submitted.set(false);
        }
      });
  }
}
