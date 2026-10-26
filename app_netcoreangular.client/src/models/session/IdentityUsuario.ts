export class IdentityUsuario {
  constructor() {
    this.usuarioId = '';
    this.username = '';
    this.guid = '';
    this.nombre = '';
    this.imagenPerfil = '';
    this.rol = 0;
    this.activarNoficiacionGlobal = false;
  }

  usuarioId: string;
  username: string;
  guid: string;
  nombre: string;
  imagenPerfil: string;
  rol: number;
  activarNoficiacionGlobal: boolean;
}

export class IdentityUsuarioForm {
  constructor() {
    this.userName = '';
  }
  userName: string;
}
