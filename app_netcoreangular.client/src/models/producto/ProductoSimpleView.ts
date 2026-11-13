export interface ProductoSimpleView {
  productoId: number;
  nombre: string;
  codigo: string;
  imagen: string;
  precio: number;
  existencia: number;
  estado: number;
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
  estado: number;
}
