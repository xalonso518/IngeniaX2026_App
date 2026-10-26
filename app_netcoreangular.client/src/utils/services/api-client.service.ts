import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ConfigService } from './config.service';
import { HandleErrorService } from './handle-error.service';
import { ApiResponse } from '../../models/shared/ApiResponse';
import { Observable } from 'rxjs/internal/Observable';
import { catchError } from 'rxjs/internal/operators/catchError';

@Injectable()
// Servicio principal para llamar API
export class ApiClientService {
  constructor(
    private http: HttpClient,
    private configService: ConfigService,
    private errorService: HandleErrorService) {
  }

  // GET
  public apiGet<T>(url: string): Observable<ApiResponse<T>> {
    var BearerToken = 'Bearer ' + localStorage.getItem('token');
    var httpOptions = { headers: new HttpHeaders() };
    httpOptions.headers = httpOptions.headers.set('Authorization', BearerToken);

    return this.http.get<ApiResponse<any>>(
      this.configService.getApiURI() + url,
      httpOptions
    ).pipe(
      catchError(this.errorService.handleError)
    );
  }

  // GET con parametros
  public apiGetParam<T>(url: string, params: HttpParams): Observable<ApiResponse<T>> {
    var BearerToken = 'Bearer ' + localStorage.getItem('token');
    var httpOptions = { headers: new HttpHeaders(), params: new HttpParams() };
    httpOptions.headers = httpOptions.headers.set('Authorization', BearerToken);
    httpOptions.params = params;

    return this.http.get<ApiResponse<any>>(
      this.configService.getApiURI() + url,
      httpOptions
    ).pipe(
      catchError(this.errorService.handleError)
    );
  }

  // POST
  public apiPost<T, O>(url: string, object: O): Observable<ApiResponse<T>> {
    var BearerToken = 'Bearer ' + localStorage.getItem('token');
    var httpOptions = { headers: new HttpHeaders() };
    httpOptions.headers = httpOptions.headers.set('Authorization', BearerToken);

    return this.http.post<ApiResponse<T>>(
      this.configService.getApiURI() + url,
      object,
      httpOptions
    ).pipe(
      catchError(this.errorService.handleError)
    );
  }

  // POST sin token
  public apiPostSimple<T, O>(url: string, object: O): Observable<ApiResponse<T>> {
    return this.http.post<ApiResponse<T>>(
      this.configService.getApiURI() + url,
      object,
    ).pipe(
      catchError(this.errorService.handleError)
    );
  }

  // PUT
  public apiPut<T, O>(url: string, object: O): Observable<ApiResponse<T>> {
    var BearerToken = 'Bearer ' + localStorage.getItem('token');
    var httpOptions = { headers: new HttpHeaders() };
    httpOptions.headers = httpOptions.headers.set('Authorization', BearerToken);

    return this.http.put<ApiResponse<T>>(
      this.configService.getApiURI() + url,
      object,
      httpOptions
    ).pipe(
      catchError(this.errorService.handleError)
    );
  }

  // DELETE
  public apiDelete<T>(url: string): Observable<ApiResponse<T>> {
    var BearerToken = 'Bearer ' + localStorage.getItem('token');
    var httpOptions = { headers: new HttpHeaders(), params: new HttpParams() };
    httpOptions.headers = httpOptions.headers.set('Authorization', BearerToken);

    return this.http.delete<ApiResponse<any>>(
      this.configService.getApiURI() + url,
      httpOptions
    ).pipe(
      catchError(this.errorService.handleError)
    );
  }

  // DELETE con parametros
  public apiDeleteParams<T>(url: string, params: HttpParams): Observable<ApiResponse<T>> {
    var BearerToken = 'Bearer ' + localStorage.getItem('token');
    var httpOptions = { headers: new HttpHeaders(), params: new HttpParams() };
    httpOptions.headers = httpOptions.headers.set('Authorization', BearerToken);
    httpOptions.params = params;

    return this.http.delete<ApiResponse<any>>(
      this.configService.getApiURI() + url,
      httpOptions
    ).pipe(
      catchError(this.errorService.handleError)
    );
  }
}
