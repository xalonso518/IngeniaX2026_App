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

/** Gestiona el formulario de alta de usuarios y la respuesta del servicio. */
@Component({
  selector: 'app-usuario-agregar',
  templateUrl: './usuario-agregar.component.html',
  styleUrl: './usuario-agregar.component.css',
  imports: [RouterLink, FormsModule],
  providers: [UsuarioService]
})
export class UsuarioAgregarComponent {
  /** Indica que el alta está en curso y deshabilita el envío repetido. */
  submitted = signal(false);

  /** Notificación visual para confirmar el registro. */
  private _snackBar = inject(MatSnackBar);

  /** Roles disponibles para seleccionar en el formulario. */
  usuarioRoles = ConstantsCatalogos.USUARIO_ROLES;

  /** Mensajes reutilizables para mostrar errores de validación. */
  errores = MESSAGES_ERROR;

  /** Modelo enlazado a los controles del formulario y enviado al servicio. */
  form = new UsuarioForm();

  constructor(
    private router: Router,
    private usuarioService: UsuarioService,
    private handleError: HandleErrorService,
    private loadingService: LoadingService,
  ) {
  }

  ngOnInit(): void {
  
  }

  /** Atiende el envío del formulario y delega el modelo enlazado a sus controles. */
  submit(formAgregar: FormControl) {
    this.agregar(this.form);
  }

  /**
   * Solicita al servicio registrar al usuario y, si tiene éxito, muestra la
   * confirmación y regresa a la búsqueda.
   */
  async agregar(form: UsuarioForm) {
    this.submitted.set(true);
    this.loadingService.show();

    this.usuarioService.agregar(form)
      .subscribe({
        next: res => {
          if (res.success) {
            this._snackBar.open("Usuario creado.", "", {verticalPosition: "top", duration: 3000});
            
            this.router.navigate([ConstantsSpaRoutes.SPA_USUARIO_BUSCAR]);
          } else this.handleError.showError(res);
        },
        error: err => {
          // Los errores no ejecutan `complete`, así que limpian el estado aquí.
          this.loadingService.hide();
          this.submitted.set(false);

          this.handleError.showError(err);
        },
        complete: () => {
          // Al completar normalmente, libera el indicador y permite otro envío.
          this.loadingService.hide();
          this.submitted.set(false);
        }
      });
  }
}
