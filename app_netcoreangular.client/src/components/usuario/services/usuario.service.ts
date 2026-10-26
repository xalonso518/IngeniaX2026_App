import { Injectable } from '@angular/core';
import { ApiClientService } from '../../../utils/services/api-client.service';
import { ApiResponse } from '../../../models/shared/ApiResponse';
import { Observable } from 'rxjs/internal/Observable';
import { ConstantsApiRoutes } from '../../../utils/constants/ConstantsApiRoutes';
import { UsuarioForm, UsuarioFormEditar, UsuarioFormEditarPass, UsuarioFormSimple } from '../../../models/usuario/UsuarioForm';
import { HttpParams } from '@angular/common/http';
import { UsuarioRelacionSimpleView, UsuarioSimpleView } from '../../../models/usuario/UsuarioSimpleView';

@Injectable()
export class UsuarioService {
  constructor(
    private apiClient: ApiClientService
  ) {
  }

  public buscar(page: number, sortBy: string, rol: string, filtro: string): Observable<ApiResponse<UsuarioSimpleView[]>> {
    let params = new HttpParams();
    if (page != undefined) params = params.set('page', page.toString());
    params = params.set('sortBy', sortBy != null ? sortBy : "");
    params = params.set('rol', rol != null ? rol : "");
    params = params.set('filtro', filtro != null ? filtro : "");
    return this.apiClient.apiGetParam<UsuarioSimpleView[]>(ConstantsApiRoutes.USUARIO_BUSCAR, params);
  }

  public obtener(guid: string): Observable<ApiResponse<UsuarioFormEditar>> {
    return this.apiClient.apiGet<UsuarioFormEditar>(ConstantsApiRoutes.USUARIO_OBTENER + "/" + guid);
  }

  public agregar(form: UsuarioForm): Observable<ApiResponse<string>> {
    return this.apiClient.apiPost<string, UsuarioForm>(ConstantsApiRoutes.USUARIO_AGREGAR, form);
  }

  public editar(guid:string, editar: UsuarioFormEditar): Observable<ApiResponse<string>> {
    return this.apiClient.apiPut<string, UsuarioFormEditar>(ConstantsApiRoutes.USUARIO_EDITAR + "/" + guid, editar);
  }

  public editarPerfil(guid: string, editar: UsuarioFormEditar): Observable<ApiResponse<string>> {
    return this.apiClient.apiPut<string, UsuarioFormEditar>(ConstantsApiRoutes.USUARIO_EDITAR_PERFIL + "/" + guid, editar);
  }

  public editarPassword(guid: string, editar: UsuarioFormEditarPass): Observable<ApiResponse<string>> {
    return this.apiClient.apiPut<string, UsuarioFormEditarPass>(ConstantsApiRoutes.USUARIO_EDITAR_PASSWORD + "/" + guid, editar);
  }

  public editarEstado(form: UsuarioFormSimple): Observable<ApiResponse<string>> {
    return this.apiClient.apiPut<string, UsuarioFormSimple>(ConstantsApiRoutes.USUARIO_EDITAR_ESTADO, form);
  }

  public eliminar(guid: string): Observable<ApiResponse<string>> {
    return this.apiClient.apiDelete<string>(ConstantsApiRoutes.USUARIO_ELIMINAR + "/" + guid);
  }

  // IMG
  public editarImg(form: FormData): Observable<ApiResponse<string>> {
    return this.apiClient.apiPut<string, FormData>(ConstantsApiRoutes.USUARIO_EDITAR_IMG, form);
  }

}
