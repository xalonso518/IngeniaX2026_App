import { Injectable } from '@angular/core';
import { HttpErrorResponse } from "@angular/common/http";
import { ErrorResponse } from "../../models/shared/ErrorResponse";
import { Observable, throwError } from 'rxjs';
import { inject } from '@angular/core/primitives/di';
import { MESSAGES_ERROR } from '../constants/ConstantsMessages';

@Injectable()
// Servicio para manjear los errores del API
export class HandleErrorService {
  constructor() {}

  // Mostramos error del API
  public showError(error: any) {
    // if (error.error)
    //   this.toastr.error(error.error.message, 'Error - ' + error.error.errorCode);
    // else if (error.errors != undefined) {
    //   var mensaje = this.getFormErrores(error.errors);
    //   this.toastr.error(mensaje, 'Error - E001');
    // } else {
    //   this.toastr.error("Error desconocido.", 'Error - E000');
    // }
  }

  // Mostramos error del API, validación de formularios
  public getFormErrores(errores: any): string {
    var mensaje = "";

    //Obtener datos
    const keys = Object.keys(errores);
    for (const key of keys) {
      mensaje += key + ":\n";
      if (errores[key] != null) {
        for (const eMensaje of errores[key]) {
          mensaje += eMensaje + "\n";
        }    
      }
    }

    return mensaje;
  }

  // Manejar de error pincipal del API, determinamos code status para lanzar errores genéricos
  public handleError(error: HttpErrorResponse) {
    // Error base
    var errorDesconocido = new ErrorResponse(MESSAGES_ERROR.DESCONOCIDO, error.status.toString());

    // Error auth
    if (error.status == 401 || (error.status == 200 && !error.ok && error.error)) {
      var errorAutorizacion = new ErrorResponse(MESSAGES_ERROR.DENEGADO, 'E201');
      return throwError({ error: errorAutorizacion });
    }

    // 405
    if (error.error == null || error.error == undefined) {
      return throwError({ error: errorDesconocido });
    } else {
      return throwError(error.error);
    }
  };
}
