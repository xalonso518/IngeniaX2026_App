import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { SessionService } from '../../../utils/services/session.service';
import { LoadingService } from '../../shared/loading/loading.service';
import { UsuarioFormEditar } from '../../../models/usuario/UsuarioForm';
import { ConstantsCatalogos, IMG_EXT, MODULOS } from '../../../utils/constants/ConstantsCatalogos';
import { MESSAGES_ERROR, MESSAGES_SUCCESS } from '../../../utils/constants/ConstantsMessages';
import { HandleErrorService } from '../../../utils/services/handle-error.service';
import { FormControl, FormsModule } from '@angular/forms';
import { FileInput } from '../../../models/file/FileInput';
import { FileInputComponent } from '../../shared/file-input/file-input.component';
import { ConfigService } from '../../../utils/services/config.service';
import { ProductoService } from '../services/producto.service';
import { ProductoFormEditar } from '../../../models/producto/ProductoForm';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-producto-editar',
  styleUrl: './producto-editar.component.css',
  templateUrl: './producto-editar.component.html',
  imports: [RouterLink, FormsModule, FileInputComponent],
  providers: [ProductoService]
})
export class ProductoEditarComponent {
  // PROPIEDADES
  submitted = signal(false);
  submittedImg = signal(false);
  id: string = "";

  // Catalogos
  errores = MESSAGES_ERROR;

  // Formulario editar
  form = signal(new ProductoFormEditar());
  private _snackBar = inject(MatSnackBar);

  // Perfil imagen
  imgExt = IMG_EXT;
  img: string = "";
  file: FileInput = new FileInput(null, "", "");

  // CONSTRUCTOR
  constructor(
    private productoService: ProductoService,
    public sessionService: SessionService,
    public configService: ConfigService,
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public loadingService: LoadingService,
    private handleError: HandleErrorService
  ) {

  }

  // FUNCIONES
  // Inicial
  ngOnInit(): void {
    // Obtenemos valor del url
    var parametro = this.activatedRoute.snapshot.paramMap.get('id');
    this.id = parametro != null ? parametro : "";
    // Iniciamos obtencion de datos
    this.getUsuario();
  }

  async getUsuario() {
    // Mostramos loading
    this.loadingService.show();

    // Llamamos API
    this.productoService.obtener(this.id).subscribe({
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
  async editar(form: ProductoFormEditar) {
    // Mostramos loadings
    this.submitted.set(true);
    this.loadingService.show();

    // Llamamos API
    this.productoService.editar(this.id, form)
      .subscribe({
        next: res => {
          // Validamos response
          if (res.success) {
            // Mostramos mensaje
            this._snackBar.open("Producto editado.", "", {verticalPosition: "top"});
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

  // IMAGEN
  getFileInput(file: FileInput) {
    if (file != null) {
      this.file = file;
    }
  }

  // Evento submit
  submitImg(form: FormControl) {
    // Validamos imagen
    if (this.file != null && this.file.file != null) {
      this.guardarImg();
    } else {
      //this.toastr.error(MESSAGES_ERROR.NO_VALID, MODULOS.USU + " - ");
    }
  }

  // Guardar imagen perfil
  guardarImg() {
    // Cremos formulario
    let form: FormData = new FormData();
    form.append("archivo", this.file.file);
    form.append("nombre", this.file.name);
    form.append("productoId", this.form().productoId.toString());

    // Mostramos loadings
    this.submittedImg.set(true);
    this.loadingService.show();

    // Llamamos API
    this.productoService.editarImg(form)
      .subscribe({
        next: res => {
          // Validamos response
          if (res.success) {
            // Mostramos mensaje
            this._snackBar.open("Imágen de producto editada.", "", {verticalPosition: "top"});
          } else this.handleError.showError(res);
        },
        error: err => {
          // Ocultamos loadings
          this.loadingService.hide();
          this.submittedImg.set(false);

          // Mostramos errores
          this.handleError.showError(err);
        },
        complete: () => {
          // Ocultamos loadings
          this.loadingService.hide();
          this.submittedImg.set(false);
        }
      });
  }
}
