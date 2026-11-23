import { Injectable } from '@angular/core';
import { ApiClientService } from '../../../utils/services/api-client.service';
import { ApiResponse } from '../../../models/shared/ApiResponse';
import { Observable } from 'rxjs/internal/Observable';
import { ConstantsApiRoutes } from '../../../utils/constants/ConstantsApiRoutes';
import { PagoForm } from '../../../models/pago/PagoForm';

@Injectable()
export class PagoService {
  constructor(
    private apiClient: ApiClientService
  ) {
  }

  public checkout(form: PagoForm): Observable<ApiResponse<string>> {
    return this.apiClient.apiPost<string, PagoForm>(ConstantsApiRoutes.PAGO_CHECKOUT, form);
  }



}
