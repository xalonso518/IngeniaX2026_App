import { Component, inject, signal } from '@angular/core';
import {MatSnackBar} from '@angular/material/snack-bar';
import { Router, RouterLink } from '@angular/router';
import { UsuarioService } from '../services/usuario.service';
import { UsuarioForm } from '../../../models/usuario/UsuarioForm';
import { MESSAGES_ERROR, MESSAGES_SUCCESS } from '../../../utils/constants/ConstantsMessages';
import { HandleErrorService } from '../../../utils/services/handle-error.service';
import { ConstantsCatalogos, MODULOS } from '../../../utils/constants/ConstantsCatalogos';
import { FormControl, FormsModule } from '@angular/forms';
import { LoadingService } from '../../shared/loading/loading.service';
import { ConstantsSpaRoutes } from '../../../utils/constants/ConstantsSpaRoutes';

@Component({
  selector: 'app-usuario-agregar',
  templateUrl: './usuario-agregar.component.html',
  styleUrl: './usuario-agregar.component.css',
  imports: [RouterLink, FormsModule],
  providers: [UsuarioService]
})
export class UsuarioAgregarComponent {
  // PROPIEDADES
  submitted = signal(false);
  private _snackBar = inject(MatSnackBar);

  // Catalogos
  usuarioRoles = ConstantsCatalogos.USUARIO_ROLES;
  errores = MESSAGES_ERROR;

  // Form
  form = new UsuarioForm();

  // CONSTRUCTOR
  constructor(
    private router: Router,
    private usuarioService: UsuarioService,
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
  async agregar(form: UsuarioForm) {
    // Mostramos loadings
    this.submitted.set(true);
    this.loadingService.show();

    // Llamamos API
    this.usuarioService.agregar(form)
      .subscribe({
        next: res => {
          // Validamos response
          if (res.success) {
            // Mostramos mensaje
            this._snackBar.open("Usuario creado.", "", {verticalPosition: "top"});
            
            // Regresamos a buscar
            this.router.navigate([ConstantsSpaRoutes.SPA_USUARIO_BUSCAR]);
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
