import { Component, Inject, signal } from '@angular/core';
import { FormControl, FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { UsuarioSimpleView } from '../../../models/usuario/UsuarioSimpleView';
import { UsuarioFormEditarPass } from '../../../models/usuario/UsuarioForm';
import { MESSAGES_ERROR, MESSAGES_SUCCESS } from '../../../utils/constants/ConstantsMessages';
import { Router, ActivatedRoute } from '@angular/router';
import { HandleErrorService } from '../../../utils/services/handle-error.service';
import { SessionService } from '../../../utils/services/session.service';
import { LoadingService } from '../../shared/loading/loading.service';
import { UsuarioService } from '../services/usuario.service';
import { MODULOS } from '../../../utils/constants/ConstantsCatalogos';
import { MATERIAL_MODULES_MODAL } from '../../../utils/material/materialModules';

@Component({
  selector: 'app-usuario-editar-password',
  templateUrl: './usuario-editar-password.component.html',
  styleUrl: './usuario-editar-password.component.css',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatButtonModule,
    ...MATERIAL_MODULES_MODAL,
    FormsModule
  ],
  providers: [UsuarioService],
})
export class UsuarioEditarPasswordComponent {
  // ATRIBUTOS
  // Objeto complejo recibido
  usuario: UsuarioSimpleView = {
      usuarioId: 0,
      guid: '',
      username: '',
      nombre: '',
      imagenPerfil: '',
      email: '',
      telefono: '',
      rol: 0,
      dateCreated: new Date,
      estado: 0
  };

  // Datos para form
  submitted = signal(false);
  form = signal(new UsuarioFormEditarPass());
  passwordConfirm = signal("");

  // Catalogos
  errores = MESSAGES_ERROR;

  // CONSTRUCTOR
  constructor(
    private usuarioService: UsuarioService,
    public sessionService: SessionService,
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public loadingService: LoadingService,
    private handleError: HandleErrorService,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<UsuarioEditarPasswordComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { usuario: UsuarioSimpleView }) {
  }

  // Init
  ngOnInit(): void {
    // Validamos si tiene data de lo contrario cerramos
    if (this.data != null && this.data.usuario != null) {
      this.usuario = this.data.usuario;
    } else {
      this.onNoClick();
    }
  }

  // Cerrar modal si valor
  onNoClick(): void {
    this.dialogRef.close(false);
  }

  // Submit boton
  submit(formControl: FormControl) {
    // Actualizamos valores form
    this.form.update(current => ({
      ...current,
      guid: this.usuario.guid,
      usuarioId: this.usuario.usuarioId
    }));

    // Ejecutamos editar
    this.editar(this.form());
  }

  // Api editar
  async editar(form: UsuarioFormEditarPass) {
  // Mostramos loadings
    this.submitted.set(true);
    this.loadingService.show();

    // Llamamos API
    this.usuarioService.editarPassword(form.guid, form)
      .subscribe({
        next: res => {
          if (res.success) {
            // Mostrar mensaje exito
            //this.toastr.success(MESSAGES_SUCCESS.EXITO_ACTUALIZADO, MODULOS.USU);

            // Cerrar modal
            this.dialogRef.close(true);
          } else this.handleError.showError(res);
        },
        error: err => {
          this.loadingService.hide();
          this.submitted.set(false);
          this.handleError.showError(err);
        },
        complete: () => {
          this.loadingService.hide();
          this.submitted.set(false);
        }
      });
  }

}
