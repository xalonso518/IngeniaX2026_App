import { Injectable } from '@angular/core';
import { ApiClientService } from '../../../utils/services/api-client.service';
import { ApiResponse } from '../../../models/shared/ApiResponse';
import { Observable } from 'rxjs/internal/Observable';
import { ConstantsApiRoutes } from '../../../utils/constants/ConstantsApiRoutes';
import { HttpParams } from '@angular/common/http';
import { ProductoForm, ProductoFormEditar, ProductoFormSimple } from '../../../models/producto/ProductoForm';
import { ProductoSimpleView } from '../../../models/producto/ProductoSimpleView';

@Injectable()
export class ProductoService {
  constructor(
    private apiClient: ApiClientService
  ) {
  }

  public buscar(page: number, sortBy: string, filtro: string): Observable<ApiResponse<ProductoSimpleView[]>> {
    let params = new HttpParams();
    if (page != undefined) params = params.set('page', page.toString());
    params = params.set('sortBy', sortBy != null ? sortBy : "");
    params = params.set('filtro', filtro != null ? filtro : "");
    return this.apiClient.apiGetParam<ProductoSimpleView[]>(ConstantsApiRoutes.PRODUCTO_BUSCAR, params);
  }

  public obtener(id: string): Observable<ApiResponse<ProductoFormEditar>> {
    return this.apiClient.apiGet<ProductoFormEditar>(ConstantsApiRoutes.PRODUCTO_OBTENER + "/" + id);
  }

  public agregar(form: ProductoForm): Observable<ApiResponse<string>> {
    return this.apiClient.apiPost<string, ProductoForm>(ConstantsApiRoutes.PRODUCTO_AGREGAR, form);
  }

  public editar(id: string, editar: ProductoFormEditar): Observable<ApiResponse<string>> {
    return this.apiClient.apiPut<string, ProductoFormEditar>(ConstantsApiRoutes.PRODUCTO_EDITAR + "/" + id, editar);
  }

  public editarStatus(form: ProductoFormSimple): Observable<ApiResponse<string>> {
    return this.apiClient.apiPut<string, ProductoFormSimple>(ConstantsApiRoutes.PRODUCTO_EDITAR_ESTADO, form);
  }

  public eliminar(id: string): Observable<ApiResponse<string>> {
    return this.apiClient.apiDelete<string>(ConstantsApiRoutes.PRODUCTO_ELIMINAR + "/" + id);
  }

  // IMG
  public editarImg(form: FormData): Observable<ApiResponse<string>> {
    return this.apiClient.apiPut<string, FormData>(ConstantsApiRoutes.PRODUCTO_EDITAR_IMG, form);
    //return this.apiClient.apiPut<string, FormData>(ConstantsApiRoutes.PRODUCTO_EDITAR_IMG_BLOB, form);
  }


}
