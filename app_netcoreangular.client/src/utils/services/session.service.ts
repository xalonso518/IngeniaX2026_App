import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../models/shared/ApiResponse';
import { ApiClientService } from './api-client.service';
import { TokenDecode } from '../../models/session/TokenDecode';
import { ConstantsApiRoutes } from '../constants/ConstantsApiRoutes';
import { IdentityUsuario } from '../../models/session/IdentityUsuario';
import { ConstantsSpaRoutes } from '../constants/ConstantsSpaRoutes';

@Injectable()
// Servicio para mantener la session del usuario
export class SessionService {
  redirectUrl: string = "";
  private _iUsuario: IdentityUsuario = new IdentityUsuario();
 
  constructor(
    private apiClient: ApiClientService,
    private router: Router) {
  }

  // SESSION STORAGE
  // Crea token en localStorage y crea la sesion de usuario en el servicio identityUserService
  public setSession(token: string, callBack: string): void {

    // Obtenemos valores del token decode
    var tokenDecode = this.getTokenDecode(token);
    if (this.validateTokenDecode(tokenDecode)) {
      // Si es válido
      // Almacenamos token en local storage
      localStorage.setItem('token', token);
      localStorage.setItem('expires_at', tokenDecode.expires);

      // Creamos objeto identity con datos del token
      if (this.createIdentityUsuarioToken(tokenDecode)) {
        // Validamos si hay url para redireccionar
        if (callBack != null || callBack != undefined) this.sessionRedirect(callBack);
        else this.sessionRedirect();
      } else this.sessionRedirect(ConstantsSpaRoutes.SPA_LOGIN);
    } else alert("Token invalido");
  }

  // Log-out
  public logout(): void {
    // Removemos token del localstorage
    localStorage.removeItem('token');
    localStorage.removeItem('expires_at');

    // Reseteamos identidad del usuario
    this.resetIdentityUsuario();

    // Navegamos a la pantalla login
    this.router.navigate([ConstantsSpaRoutes.SPA_LOGIN]);
  }

  // Redirect
  public sessionRedirect(url?: string, callBack?: string): void {
    //Te lleva a login con un link de respuesta
    if (url && callBack) this.router.navigate([url], { queryParams: { callBack: callBack } });
    else {
      // Navegamos a una url, dependiendo el callback y el rol
      if (url) this.router.navigate([url]);
      else switch (this._iUsuario.rol) {
        case 1: this.router.navigate([ConstantsSpaRoutes.SPA_USUARIO_BUSCAR]);
          break;
        default: this.router.navigate([ConstantsSpaRoutes.SPA_USUARIO_BUSCAR]);
          break;
      }
    }
  }

  // Valida sesion en single-layout
  public validateSession(url?: string): void {
    if (this.isTokenSet()) {
      if (this.isTokenActive()) {
        if (this.isAuthenticated()) {
          this.sessionRedirect(url);
        } else {
          this.getIdentityUsuarioFromToken(url);
        }
      }
      else this.logout();
    }
  }

  // Valida la existencia de token y expires en el localStorage
  public isTokenSet(): boolean {
    var token = localStorage.getItem('token');
    var expires_at = localStorage.getItem('expires_at');

    if (token != null && expires_at != null && token != "" && expires_at != "") {
      return true;
    }
    return false;
  }

  // Valida la caducidad ddel token
  public isTokenActive(): boolean {
    var expires_at = localStorage.getItem('expires_at');
    if (expires_at != null) {
      var expires = Number.parseInt(expires_at);
      var timeNow = Math.floor(new Date().getTime() / 1000);
      if (expires > timeNow) {
        return true;
      }
    }
    return false;
  }

  // Valida la existencia del Objeto identityUser y sus atributos
  public isAuthenticated(): boolean {
    if (this._iUsuario == null || this._iUsuario == undefined) {
      return false;
    } else {
      if (this._iUsuario.username != 'NA' && this._iUsuario.rol != null) return true;
    }
    return false;
  }

  // TOKEN MANAGE
  // Valida objeto TokenDecode
  private validateTokenDecode(token: TokenDecode): boolean {
    if (token == null) return false;
    if (token.id == '' || token.id == undefined) return false;
    if (token.username == 'NA' || token.username == undefined) return false;
    if (token.expires == '' || token.expires == undefined) return false;
    if (token.jti == '' || token.jti == undefined) return false;
    return true;
  }

  // Decode el Token
  private getTokenDecode(token: string): TokenDecode {
    var tokenDecode = new TokenDecode();
    tokenDecode.username = 'NA';
    if (token != null) {
      try {
        var t: any = jwtDecode(token);
        if (t != null) {
          tokenDecode.username = t.user;
          tokenDecode.rol = t.rol;
          tokenDecode.id = t.id;
          tokenDecode.guid = t.guid;
          tokenDecode.nombre = t.nom;
          tokenDecode.imagenPerfil = t.img;
          tokenDecode.expires = t.exp;
          tokenDecode.jti = t.jti;
        }
      } catch (e) {
        console.log("-> Error: " + e);
      }
    }
    return tokenDecode;
  }


  //IDENTITY OBJECT
  /**
   * Limpia el objeto _iUsuario
   */
  resetIdentityUsuario() {
    this._iUsuario = new IdentityUsuario();
  }

  public getApiIdentityUsuario(): Observable<ApiResponse<IdentityUsuario>> {
    return this.apiClient.apiGet<IdentityUsuario>(ConstantsApiRoutes.USUARIO_AGREGAR);
  }

  // Obtiene valores de usuario del token
  public setIdentityUsuarioFromToken() {
    var token = localStorage.getItem('token');
    if (token != null) {
      var tokenDecode = this.getTokenDecode(token);
      var username = tokenDecode.username;
      if (username != 'NA' && username != undefined) {
        // Creamos identity
        this.createIdentityUsuarioToken(tokenDecode);
        return true;
      }
    }
    return false;
  }

  public getIdentityUsuarioFromToken(callBack?: string) {
    var token = localStorage.getItem('token');
    if (token != null) {
      var tokenDecode = this.getTokenDecode(token);
      var username = tokenDecode.username;
      if (username != 'NA' && username != undefined) {
        // Creamos identity
        this.createIdentityUsuarioToken(tokenDecode);

        //Validamos re-direccionamiento
        if (this.router.url == ConstantsSpaRoutes.SPA_LOGIN) this.sessionRedirect();
        else {
          if (callBack) this.sessionRedirect(callBack);
          else this.sessionRedirect(this.router.url);
        }
      }
    }
  }

  // Obtiene valores de usuario llamado a la API
  public getIdentityUsuarioFromApiOLD(callBack?: string) {
    var token = localStorage.getItem('token');
    if (token != null) {
      var username = this.getTokenDecode(token).username;
      if (username != 'NA' && username != undefined) {

        this.getApiIdentityUsuario()
          .subscribe({
            next: res => {
              if (res.success) {
                //Aignamos datos a identity
                this._iUsuario = res.payload;

                //Validamos re-direccionamiento
                if (this.router.url == ConstantsSpaRoutes.SPA_LOGIN) this.sessionRedirect();
                else {
                  if (callBack) this.sessionRedirect(callBack);
                  else this.sessionRedirect(this.router.url);
                }
              } else alert("Token invlaido");
            },
            error: error => {
              //this.toastr.error(error.error.message, 'Error - ' + error.error.errorCode);
              this.sessionRedirect(ConstantsSpaRoutes.SPA_LOGIN);
            }
          });
      }
    }
  }

  // Usuario
  createIdentityUsuarioToken(token: TokenDecode): boolean {
    if (token != null && token.username != 'NA') {
      this._iUsuario = new IdentityUsuario();
      this._iUsuario.username = token.username;
      this._iUsuario.guid = token.guid;
      this._iUsuario.usuarioId = token.id;
      this._iUsuario.nombre = token.nombre;
      this._iUsuario.imagenPerfil = token.imagenPerfil;
      this._iUsuario.rol = Number.parseInt(token.rol);
      return true;
    }
    return false;
  }

  getIdentityUser(): IdentityUsuario {
    return this._iUsuario;
  }

  getUsuarioImagen(): string {
    return this._iUsuario != null ? this._iUsuario.imagenPerfil : "";
  }

  getUsuarioRol(): number {
    return this._iUsuario != null ? this._iUsuario.rol : 0;
  }

  getUsuarioUsername(): string {
    return this._iUsuario != null ? this._iUsuario.username : "";
  }

  getUsuarioNombre(): string {
    return this._iUsuario != null ? this._iUsuario.nombre : "";
  }

  getUsuarioId(): string {
    return this._iUsuario != null ? this._iUsuario.usuarioId : "";
  }

  getUsuarioGuid(): string {
    return this._iUsuario != null ? this._iUsuario.guid : "";
  }

  getUsuarioActivarNotificacionGlobal(): boolean {
    return this._iUsuario != null ? this._iUsuario.activarNoficiacionGlobal : false;

  }

  setIdentityUsuario(usuario: IdentityUsuario) {
    this._iUsuario = usuario;
  }

  setUsuarioImagen(img: string) {
    this._iUsuario.imagenPerfil = img;
  }

  setUsuarioNombre(nombre: string) {
    this._iUsuario.nombre = nombre;
  }

  setUsuarioActivarNotificacionGlobal(activarNotificacion: boolean) {
    this._iUsuario.activarNoficiacionGlobal = activarNotificacion;
  }

  hasPermisoWrite(permiso: number): boolean {
    if (permiso == 2 || permiso == 3 || permiso == 4) return true;
    else return false;
  }

  hasPermisoRead(permiso: number): boolean {
    if (permiso == 2 || permiso == 4) return true;
    else return false;
  }

}
