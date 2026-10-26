import { Injectable } from '@angular/core';
import {
  ActivatedRoute,
  CanActivate, Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanActivateChild
} from '@angular/router';
import { SessionService } from "./session.service";
import { ConstantsSpaRoutes } from '../constants/ConstantsSpaRoutes';

@Injectable()
// Servicio para authenticar rutas (OLD)
export class AuthGuard implements CanActivate, CanActivateChild {
  constructor(
    private sessionService: SessionService
  ) {

  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    // Obtenemos ruta
    let url: string = state.url;

    //Validar token, session
    if (this.sessionService.isTokenSet()) {
      if (this.sessionService.isTokenActive() && this.sessionService.isAuthenticated()) {

        // Validamos rol
        if (this.sessionService.getUsuarioRol() == 1) return true;
        else {

          this.sessionService.sessionRedirect(ConstantsSpaRoutes.SPA_LOGIN, encodeURI(url));
          return false;
        }

      }
      else {
        this.sessionService.logout();
        return false;
      }
    } else {
      this.sessionService.sessionRedirect(ConstantsSpaRoutes.SPA_LOGIN, encodeURI(url));
    }
    return false;
  }


  canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return this.canActivate(route, state);
  }
}
