import { Component, OnInit } from '@angular/core';
import { IdentityUsuario } from '../../models/session/IdentityUsuario';
import { SessionService } from '../../utils/services/session.service';
import { MatMenuModule } from '@angular/material/menu';
import { ConfigService } from '../../utils/services/config.service';
import { MatDialog } from '@angular/material/dialog';
import { ConstantsSpaRoutes } from '../../utils/constants/ConstantsSpaRoutes';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-main-nav-bar',
  templateUrl: './main-nav-bar.component.html',
  styleUrl: './main-nav-bar.component.css',
  imports: [MatMenuModule, MatIconModule, MatButtonModule],
})
export class MainNavBarComponent implements OnInit {
  // ATRIBUTOS
  _iUser: IdentityUsuario = new IdentityUsuario(); // Datos del usuario

  /** NavbarInicio ctor */
  constructor(
    public sessionService: SessionService,
    public configService: ConfigService,
    public dialog: MatDialog
  ) {

  }

  // Funcion inicio
  ngOnInit(): void {
    this.initValuesUser();
  }

  // Validamos si vay valores de session
  private initValuesUser() {
    // No hay, validamos token
    if (this.sessionService.getUsuarioUsername() == '' || this.sessionService.getUsuarioRol() == 0) this.getIdentityUser();
    else {
      // Si los hay, obtenemos el objeto del servicio
      this._iUser = this.sessionService.getIdentityUser();
    }
  }

  // Intentamos obtener datos del token
  async getIdentityUser() {
    // Datos de token
    var token = localStorage.getItem('token');
    if (token == null) {
      // No hay datos, regresamos a login
      this.sessionService.sessionRedirect(ConstantsSpaRoutes.SPA_LOGIN);
    } else {
      // Creamos y asignamos datos a identity
      this.sessionService.setIdentityUsuarioFromToken();
      this._iUser = this.sessionService.getIdentityUser();
    }
  }

  // Cerrar sesion
  logout() {
    this.sessionService.logout();
  }
}
