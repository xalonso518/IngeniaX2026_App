import { Component, signal } from '@angular/core';
import { email, form, FormField, required, submit } from '@angular/forms/signals';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MESSAGES_ERROR } from '../../../utils/constants/ConstantsMessages';
import { ErrorInputComponent } from '../../shared/error-input/error-input.component';
import { AuthService } from '../services/auth.service';
import { SessionService } from '../../../utils/services/session.service';
import { HandleErrorService } from '../../../utils/services/handle-error.service';
import { ResetForm } from '../../../models/auth/ResetForm';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css',
  imports: [FormField, RouterLink, ErrorInputComponent],
  providers: [AuthService]
})
export class ResetPasswordComponent {

  // PROPIEDADES
  submitted = signal(false);
  urlCallback: string = "";

  // Incio de form
  resetModel = signal<ResetForm>({
    email: 'lgv.alonso@gmail.com'
  });

  // Validaciones de formulario
  resetForm = form(this.resetModel, (schemaPath) => {
    required(schemaPath.email, { message: MESSAGES_ERROR.REQUIRED });
    email(schemaPath.email, { message: MESSAGES_ERROR.NO_VALID });
  });

  // CONSTRUCTOR
  constructor(
    private authService: AuthService,
    private handleError: HandleErrorService,
  ) {
  }

  // FUNCIONES
  // Inicial
  ngOnInit(): void {
  }

  // Evento submit
  onSubmit(event: Event) {
    event.preventDefault();
    this.resetPassword(this.resetModel());
  }

  // Reset del api
  async resetPassword(form: ResetForm) {
  // Mostramos loading del boton
    this.submitted.set(true);
    this.authService.reset(form)
      .subscribe({
        next: res => {
        // Validamos respuesta exitosa
          alert("Nueva contraseñagenerada.");
        },
        error: err => {
        // Mostramos errores
          this.handleError.showError(err);
          // Ocultamos loading del boton
          this.submitted.set(false);
        },
        complete: () => {
        // Ocultamos loading del boton
          this.submitted.set(false);
        }
      }
      );
  }
}
