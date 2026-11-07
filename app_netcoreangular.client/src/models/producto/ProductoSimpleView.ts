export interface ProductoSimpleView {
  productoId: number;
  nombre: string;
  codigo: string;
  imagen: string;
  precio: number;
  existencia: number;
  status: number;
}
export interface ProductoView {
  productoId: number;
  nombre: string;
  codigo: string;
  descripcion: string;
  imagen: string;
  precio: number;
  existencia: number;
  dateCreated: Date;
  status: number;
}
