import { Component, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MESSAGES_ERROR } from '../../../utils/constants/ConstantsMessages';
import { AuthService } from '../services/auth.service';
import { LoginForm } from '../../../models/auth/LoginForm';
import { SessionService } from '../../../utils/services/session.service';
import { HandleErrorService } from '../../../utils/services/handle-error.service';
import { FormControl, FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  styleUrl: './login.component.css',
  templateUrl: './login.component.html',
  imports: [FormsModule, RouterLink],
  providers: [AuthService]
})
export class LoginComponent {
  // PROPIEDADES
  submitted = signal(false);
  urlCallback: string = "";

  // Catálogo de errores
  errores = MESSAGES_ERROR;

  // Incio de form con valores por defecto
  loginModel = signal<LoginForm>({
    email: 'admin@ingeniax.mx',
    password: 'qweqwe',
  });

  // CONSTRUCTOR
  constructor(
    private authService: AuthService,
    private sessionService: SessionService,
    private activatedRoute: ActivatedRoute,
    private handleError: HandleErrorService,
  ) {
  }

  // FUNCIONES
  // Inicial
  ngOnInit(): void {
    // Obtenemos datos de la url
    this.activatedRoute.queryParams.subscribe(params => {
      if (params['user'] != undefined) this.loginModel().email = params['user'];
      if (params['callBack'] != undefined) this.urlCallback = params['callBack'];
    });
    // Validamos si hay sesión activa
    this.sessionService.validateSession(this.urlCallback);
  }

  // Evento submit
  submit(form: FormControl) {
    //event.preventDefault();
    this.login(this.loginModel());

  }

  // Login del api
  async login(form: LoginForm) {
    // Mostramos loading del boton
    this.submitted.set(true);

    // Llamamos API enviando el objeto form
    this.authService.login(form)
      .subscribe({
        next: res => {
          // Validamos respuesta exitosa
          if (res.success) {
            // Iniciamos session en spa
            this.sessionService.setSession(res.payload.token, this.urlCallback);
          } else this.handleError.showError(res);
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
