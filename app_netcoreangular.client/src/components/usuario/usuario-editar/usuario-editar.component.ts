import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { SessionService } from '../../../utils/services/session.service';
import { LoadingService } from '../../shared/loading/loading.service';
import { UsuarioFormEditar } from '../../../models/usuario/UsuarioForm';
import { ConstantsCatalogos, IMG_EXT, MODULOS } from '../../../utils/constants/ConstantsCatalogos';
import { MESSAGES_ERROR, MESSAGES_SUCCESS } from '../../../utils/constants/ConstantsMessages';
import { UsuarioService } from '../services/usuario.service';
import { HandleErrorService } from '../../../utils/services/handle-error.service';
import { FormControl, FormsModule } from '@angular/forms';
import { ConfigService } from '../../../utils/services/config.service';
import { ConstantsSpaRoutes } from '../../../utils/constants/ConstantsSpaRoutes';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-usuario-editar',
  imports: [RouterLink, FormsModule],
  providers: [UsuarioService],
  templateUrl: './usuario-editar.component.html',
  styleUrl: './usuario-editar.component.css',
})
export class UsuarioEditarComponent {
  // PROPIEDADES
  submitted = signal(false);
  submittedImg = signal(false);
  guid: string = "";
  private _snackBar = inject(MatSnackBar);

  // Catalogos
  usuarioRoles = ConstantsCatalogos.USUARIO_ROLES;
  errores = MESSAGES_ERROR;

  // Formulario editar
  form = signal(new UsuarioFormEditar());

  // Perfil imagen
  imgExt = IMG_EXT;
  img: string = "";

  // CONSTRUCTOR
  constructor(
    private router: Router,
    private usuarioService: UsuarioService,
    public sessionService: SessionService,
    public configService: ConfigService,
    public activatedRoute: ActivatedRoute,
    public loadingService: LoadingService,
    private handleError: HandleErrorService
  ) {

  }

  // FUNCIONES
  // Inicial
  ngOnInit(): void {
    // Obtenemos valor del url
    var parametro = this.activatedRoute.snapshot.paramMap.get('guid');
    this.guid = parametro != null ? parametro : "";
    // Iniciamos obtencion de datos
    this.getUsuario();
  }

  async getUsuario() {
    // Mostramos loading
    this.loadingService.show();

    // Llamamos API
    this.usuarioService.obtener(this.guid).subscribe({
      next: res => {
        // Validamos resutaldos
        if (res.payload != null) {
          // Set datos
          this.form.set(res.payload);
        }
      },
      error: err => {
        // Ocultamos loading
        this.loadingService.hide();
        this.handleError.showError(err);
      },
      complete: () => {
        // Ocultamos loading
        this.loadingService.hide();
      }
    });
  }

  // Evento submit
  submit(form: FormControl) {
    this.editar(this.form());
  }

  // Editar usuario API
  async editar(form: UsuarioFormEditar) {
    // Mostramos loadings
    this.submitted.set(true);
    this.loadingService.show();

    // Llamamos API
    this.usuarioService.editar(this.guid, form)
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
