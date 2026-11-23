export interface ClienteApiResponse {
  clienteId: number;
  nombre: string;
  descripcion: string;
  fechaAlta: string;
}

export interface Cliente {
  clienteId: number;
  nombre: string;
  descripcion: string;
  fechaAlta: Date;
}

export function mapCliente(response: ClienteApiResponse): Cliente {
  if (typeof response.fechaAlta !== "string") {
    throw new TypeError("fechaAlta debe ser una cadena.");
  }

  const fechaAlta = new Date(response.fechaAlta);
  if (Number.isNaN(fechaAlta.getTime())) {
    throw new RangeError("fechaAlta debe contener una fecha válida.");
  }

  return {
    ...response,
    fechaAlta,
  };
}
