export class ProductoForm {
  constructor() {
    this.nombre = "";
    this.codigo = "";
    this.descripcion = "";
    this.precio = 0;
    this.existencia = 0;
  }
  nombre: string;
  codigo: string;
  descripcion: string;
  precio: number;
  existencia: number;
}

export class ProductoFormEditar extends ProductoForm {
  constructor() {
    super();
    this.productoId = 0;
    this.imagen = "";
  }
  productoId: number;
  imagen: string;
}

export class ProductoFormSimple {
  constructor() {
    this.productoId = 0;
    this.status = 0;
  }
  productoId: number;
  status: number;
}
