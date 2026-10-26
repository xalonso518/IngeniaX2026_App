export class UsuarioForm {
  constructor() {
    this.username = "";
    this.password = "";
    this.nombre = "";
    this.email = "";
    this.telefono = "";
    this.rol = 2;
  }
  username: string;
  password: string;
  nombre: string;
  email: string;
  telefono: string;
  rol: number;
}
export class UsuarioFormEditar extends UsuarioForm {
  constructor() {
    super();
    this.guid = "";
    this.usuarioId = 0;
    this.imagenPerfil = "";
  }
  usuarioId: number;
  guid: string;
  imagenPerfil: string;
}

export class UsuarioFormSimple {
  constructor() {
    this.guid = "";
    this.usuarioId = 0;
    this.estado = 0;
  }
  usuarioId: number;
  guid: string;
  estado: number;
}

export class UsuarioFormEditarPass {
  constructor() {
    this.guid = "";
    this.usuarioId = 0;
    this.password = "";
  }
  usuarioId: number;
  guid: string;
  password: string;
}
