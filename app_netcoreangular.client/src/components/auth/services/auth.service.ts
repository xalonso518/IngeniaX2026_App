import { Injectable } from '@angular/core';
import { ApiClientService } from '../../../utils/services/api-client.service';
import { ApiResponse } from '../../../models/shared/ApiResponse';
import { Observable } from 'rxjs/internal/Observable';
import { ConstantsApiRoutes } from '../../../utils/constants/ConstantsApiRoutes';
import { LoginForm } from '../../../models/auth/LoginForm';
import { TokenResponse } from '../../../models/auth/TokenResponse';
import { ResetForm } from '../../../models/auth/ResetForm';

@Injectable()
export class AuthService {
  constructor(
    private apiClient: ApiClientService
  ) {
  }

  public login(form: LoginForm): Observable<ApiResponse<TokenResponse>> {
    return this.apiClient.apiPostSimple<TokenResponse, LoginForm>(ConstantsApiRoutes.LOGIN, form);
  }

  public reset(form: ResetForm): Observable<ApiResponse<string>> {
    return this.apiClient.apiPostSimple<string, ResetForm>(ConstantsApiRoutes.RESET, form);
  }
}
