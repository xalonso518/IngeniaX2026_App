import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { SessionService } from './session.service';
import { ConstantsSpaRoutes } from '../constants/ConstantsSpaRoutes';

// Valida condicinoes en ruta
export const RoleGuard = (allowedRoles: number[]): CanActivateFn => {
  return (route, state) => {
    // Inyectamos servicios
    const sessionService = inject(SessionService);
    const router = inject(Router);

    // Obtenemos valores de ruta y rol
    const url: string = state.url;
    const userRole = sessionService.getUsuarioRol();

    //Validar token en local storage, activo y con campos validos      
    if (sessionService.isTokenSet()) {
      if (sessionService.isTokenActive() && sessionService.isAuthenticated()) {
        // Valida rol
        if (allowedRoles.includes(userRole)) {
          return true;
        } else {
          // Pagina forbiden
          return router.parseUrl(ConstantsSpaRoutes.SPA_LOGIN);
          return false;
        }
      }
      else {
        // Limpiamos session
        sessionService.logout();
        return false;
      }
    } else {
      // Sin token, nos dirigimos a login
      sessionService.sessionRedirect(ConstantsSpaRoutes.SPA_LOGIN, encodeURI(url));
      return false;
    }

  };
};
