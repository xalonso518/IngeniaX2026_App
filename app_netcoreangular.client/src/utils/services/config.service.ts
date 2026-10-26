import { Injectable, isDevMode } from '@angular/core';

@Injectable()
// Servicio para obtener el URI del API
export class ConfigService {
  _apiURI: string;

  constructor() {
    if (isDevMode()) this._apiURI = 'http://localhost:5044';
    else this._apiURI = 'https://localhost:7272';
  }

  getApiURI() {
    return this._apiURI;
  }

  getUrlPublicUsuario() {
    return this._apiURI + "/files/img/usuario";
  }

  getUrlPublicProducto() {
    return this._apiURI + "/files/img/producto";
  }
}
