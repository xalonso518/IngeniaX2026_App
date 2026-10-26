export interface UsuarioSimpleView {
  usuarioId: number;
  guid: string;
  username: string;
  nombre: string;
  imagenPerfil: string;
  email: string;
  telefono: string;
  rol: number;
  dateCreated: Date;
  estado: number;
}
export interface UsuarioRelacionSimpleView {
  usuarioId: number;
  username: string;
  nombre: string;
  rol: number;
  estado: number;
}
