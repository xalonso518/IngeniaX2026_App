import { Component, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ConstantsCatalogos, MODULOS } from '../../../utils/constants/ConstantsCatalogos';
import { MESSAGES_ERROR, MESSAGES_SUCCESS } from '../../../utils/constants/ConstantsMessages';
import { UsuarioSimpleView } from '../../../models/usuario/UsuarioSimpleView';
import { SessionService } from '../../../utils/services/session.service';
import { LoadingService } from '../../shared/loading/loading.service';
import { UsuarioService } from '../services/usuario.service';
import { HandleErrorService } from '../../../utils/services/handle-error.service';
import { PageEvent } from '@angular/material/paginator';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { MATERIAL_MODULES_BUSCAR } from '../../../utils/material/materialModules';
import { MatDialog } from '@angular/material/dialog';
import { UsuarioEditarPasswordComponent } from '../usuario-editar-password/usuario-editar-password.component';
import { ModalEliminarComponent } from '../../shared/modal-eliminar/modal-eliminar.component';
import { UsuarioFormSimple } from '../../../models/usuario/UsuarioForm';
import { SinResultadosComponent } from '../../shared/sin-resultados/sin-resultados.component';
import { ChipStatusComponent } from '../../shared/chip-status/chip-status.component';
import { ConfigService } from '../../../utils/services/config.service';
import { ConstantsSpaRoutes } from '../../../utils/constants/ConstantsSpaRoutes';

@Component({
  selector: 'app-usuario-buscar',
  imports: [FormsModule, DatePipe, SinResultadosComponent, ChipStatusComponent, ...MATERIAL_MODULES_BUSCAR],
  templateUrl: './usuario-buscar.component.html',
  styleUrl: './usuario-buscar.component.css',
  providers: [UsuarioService]
})
export class UsuarioBuscarComponent {
  // ATRIBUTOS
  submitted = signal(false); // Identifica si hay alguna petiicon en proceso

  // Catalogos
  usuarioRoles = ConstantsCatalogos.USUARIO_ROLES;
  errores = MESSAGES_ERROR;

  // Busqueda
  page = 0;
  loading = signal(false);
  usuarios = signal<UsuarioSimpleView[]>([]);
  //selectIndex = -1;

  // Filtrado
  filtro: string = '';
  rol: string = '0';

  // Sort
  sortBy: string = 'user';
  sortByIndex: number = 0;
  sortHead = [
    { key: "nombre", value: "Nombre" },
    { key: "nombre_desc", value: "Nombre (desc)" },
    { key: "user", value: "Usuario" },
    { key: "user_desc", value: "Usuario (desc)" },
    { key: "fecha_desc", value: "Fecha" },
  ]

  // Paginator
  totalRegistros = 0;
  pageSize = 25;

  // CONSTRUCTOR
  constructor(
    private router: Router,
    private loadingService: LoadingService,
    public sessionService: SessionService,
    public configService: ConfigService,
    private handleError: HandleErrorService,
    public dialog: MatDialog,
    private usuarioService: UsuarioService) {
  }

  // FUNCIONES
  // Inicial
  ngOnInit(): void {
    this.startGetDatos();
  }

  // Verificamos idnetiti
  startGetDatos() {
    if (!this.sessionService.getIdentityUser()) {
      setTimeout(() => {
        this.startGetDatos();
      }, 1000);
    } else {
      this.buscarUsuarios();
    }
  }

  // Funciones de busqueda
  // Funcion de sort
  changeSort(key: string, index: number) {
    this.sortByIndex = index;
    this.sortBy = key;
    this.buscarPorPalabra();
  }

  // Funcion filtro palabra
  buscarPorPalabra() {
    this.page = 0;
    //if (this.filtro.trim().length > 3)
    this.buscarUsuarios();
  }

  // Funciona para avanzar en el paginator
  cambiarPagina(event: PageEvent) {
    this.page = event.pageIndex;
    this.buscarUsuarios();
  }

  // Buscar API
  buscarUsuarios() {
    // Limpiamos arreglo
    this.usuarios.set([]);

    // Mostramos loading y submit
    this.loading.set(true);
    this.submitted.set(true);

    this.usuarioService.buscar(this.page + 1, this.sortBy, this.rol, this.filtro)
      .subscribe({
        next: res => {
          // Validamos respuesta
          if (res.payload != null && res.payload.length > 0) {
            // Cargamos datos
            this.usuarios.set(res.payload);
          }

          // Modificamos datos paginator
          if (res.paginator) {
            this.totalRegistros = res.paginator.total;
            this.pageSize = res.paginator.pageSize;
          }
        },
        error: err => {
          // Ocultamos loadings
          this.loading.set(false);
          this.submitted.set(false);

          // Mostramos errores
          this.handleError.showError(err);
        },
        complete: () => {
          // Ocultamos loadings
          this.loading.set(false);
          this.submitted.set(false);
        }
      });
  }


  // Funciones del menu
  // Ir a agregar
  openAgregar() {
    this.router.navigate([ConstantsSpaRoutes.SPA_USUARIO_AGREGAR]);
  }

  // Ir a editar
  openEditar(row: UsuarioSimpleView) {
    this.router.navigate([ConstantsSpaRoutes.SPA_USUARIO_EDITAR, row.guid]);
  }

  // Abrir modal password
  openEditarPassword(row: UsuarioSimpleView) {
    const dialogRef = this.dialog.open(UsuarioEditarPasswordComponent, {
      disableClose: false, autoFocus: false, maxWidth: "100vw", width: "350px", data: { usuario: row }
    });
  }

  // Abrir modal eliminar
  openConfirmarEliminar(row: UsuarioSimpleView) {
    const dialogRef = this.dialog.open(ModalEliminarComponent, {
      disableClose: false, autoFocus: false, maxWidth: "100vw", width: "350px", data: { modulo: MODULOS.USU, valor: row.username }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Si es confirmado, eliminamos
        this.eliminar(row);
      }
    });
  }

  // Funciones API menu
  // Editar status
  async editarEstado(usuario: UsuarioSimpleView, estado: number) {
    // Mostramos loading y submit
    this.submitted.set(true);
    this.loadingService.show();

    // Creamos form
    var form = new UsuarioFormSimple();
    form.guid = usuario.guid;
    form.usuarioId = usuario.usuarioId;
    form.estado = estado;

    // Llamamos api
    this.usuarioService.editarEstado(form)
      .subscribe({
        next: res => {
          // Validamos respuesta
          if (res.success) {
            // Mostramos mensaje éxito
            //this.toastr.success(MESSAGES_SUCCESS.EXITO_ACTUALIZADO, MODULOS.USU);

            // Modificamos estado reigstro actual
            usuario.estado = estado;

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

  // Eliminar registro
  async eliminar(usuario: UsuarioSimpleView) {
    this.submitted.set(true);
    this.loadingService.show();

    this.usuarioService.eliminar(usuario.guid)
      .subscribe({
        next: res => {
          // Validamos respuesta
          if (res.success) {
            // Mostramos mensaje éxito
            //this.toastr.success(MESSAGES_SUCCESS.EXITO_ELIMINADO, MODULOS.USU);

            // Modificamos estado reigstro actual
            usuario.estado = 99;
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
