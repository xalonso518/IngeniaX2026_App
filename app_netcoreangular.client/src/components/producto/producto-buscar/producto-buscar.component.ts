import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ConstantsCatalogos, MODULOS } from '../../../utils/constants/ConstantsCatalogos';
import { MESSAGES_ERROR, MESSAGES_SUCCESS } from '../../../utils/constants/ConstantsMessages';
import { SessionService } from '../../../utils/services/session.service';
import { LoadingService } from '../../shared/loading/loading.service';
import { HandleErrorService } from '../../../utils/services/handle-error.service';
import { PageEvent } from '@angular/material/paginator';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { MATERIAL_MODULES_BUSCAR } from '../../../utils/material/materialModules';
import { MatDialog } from '@angular/material/dialog';
import { ModalEliminarComponent } from '../../shared/modal-eliminar/modal-eliminar.component';
import { UsuarioFormSimple } from '../../../models/usuario/UsuarioForm';
import { SinResultadosComponent } from '../../shared/sin-resultados/sin-resultados.component';
import { ChipStatusComponent } from '../../shared/chip-status/chip-status.component';
import { ConfigService } from '../../../utils/services/config.service';
import { ConstantsSpaRoutes } from '../../../utils/constants/ConstantsSpaRoutes';
import { ProductoService } from '../services/producto.service';
import { ProductoSimpleView } from '../../../models/producto/ProductoSimpleView';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  imports: [FormsModule, DatePipe, SinResultadosComponent, ChipStatusComponent, ...MATERIAL_MODULES_BUSCAR],
  selector: 'app-producto-buscar',
  styleUrl: './producto-buscar.component.css',
  templateUrl: './producto-buscar.component.html',
  providers: [ProductoService]
})
export class ProductoBuscarComponent {
  // ATRIBUTOS
  submitted = signal(false); // Identifica si hay alguna petiicon en proceso
  private _snackBar = inject(MatSnackBar);

  // Catalogos
  errores = MESSAGES_ERROR;
  blobUrlPublic = ConstantsCatalogos.BLOB_URL_PUBLIC;

  // Busqueda
  page = 0;
  loading = signal(false);
  productos = signal<ProductoSimpleView[]>([]);

  // Filtrado
  filtro: string = '';

  // Sort
  sortBy: string = 'user';
  sortByIndex: number = 0;
  sortHead = [
    { key: "nombre", value: "Nombre" },
    { key: "nombre_desc", value: "Nombre (desc)" },
    { key: "codigo", value: "Código" },
    { key: "codigo_desc", value: "Código (desc)" }
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
    private productoService: ProductoService) {
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
      this.buscarProductos();
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
    this.buscarProductos();
  }

  // Funciona para avanzar en el paginator
  cambiarPagina(event: PageEvent) {
    this.page = event.pageIndex;
    this.buscarProductos();
  }

  // Buscar API
  buscarProductos() {
    // Limpiamos arreglo
    this.productos.set([]);

    // Mostramos loading y submit
    this.loading.set(true);
    this.submitted.set(true);

    this.productoService.buscar(this.page + 1, this.sortBy, this.filtro)
      .subscribe({
        next: res => {
          // Validamos respuesta
          if (res.payload != null && res.payload.length > 0) {
            // Cargamos datos
            this.productos.set(res.payload);
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
    this.router.navigate([ConstantsSpaRoutes.SPA_PRODUCTO_AGREGAR]);
  }

  // Ir a editar
  openEditar(row: ProductoSimpleView) {
    this.router.navigate([ConstantsSpaRoutes.SPA_PRODUCTO_EDITAR, row.productoId]);
  }
  
  // Ir a pagar
  openPagar(row: ProductoSimpleView) {
    this.router.navigate([ConstantsSpaRoutes.SPA_PRODUCTO_PAGO]);
  }

  // Abrir modal eliminar
  openConfirmarEliminar(row: ProductoSimpleView) {
    const dialogRef = this.dialog.open(ModalEliminarComponent, {
      disableClose: false, autoFocus: false, maxWidth: "100vw", width: "350px", data: { modulo: MODULOS.USU, valor: row.nombre }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Si es confirmado, eliminamos
        this.eliminar(row);
      }
    });
  }

  // Funciones API menu
  // Eliminar registro
  async eliminar(row: ProductoSimpleView) {
    this.submitted.set(true);
    this.loadingService.show();

    this.productoService.eliminar(row.productoId.toString())
      .subscribe({
        next: res => {
          // Validamos respuesta
          if (res.success) {
            // Mostramos mensaje éxito
            this._snackBar.open("Producto eliminado.", "", {verticalPosition: "top"});

            // Modificamos status reigstro actual
            row.estado = 99;
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
