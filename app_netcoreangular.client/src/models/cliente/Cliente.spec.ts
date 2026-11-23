import { describe, expect, it } from "vitest";
import { mapCliente } from "./Cliente";

describe("mapCliente", () => {
  it("converts the API date string to a Date", () => {
    const cliente = mapCliente({
      clienteId: 7,
      nombre: "Cliente de prueba",
      descripcion: "Descripción",
      fechaAlta: "2026-11-13T10:00:00.000Z",
    });

    expect(cliente.fechaAlta).toBeInstanceOf(Date);
    expect(cliente.fechaAlta.toISOString()).toBe("2026-11-13T10:00:00.000Z");
  });

  it("throws when the API date is invalid", () => {
    expect(() =>
      mapCliente({
        clienteId: 7,
        nombre: "Cliente de prueba",
        descripcion: "Descripción",
        fechaAlta: "no es una fecha",
      }),
    ).toThrow(RangeError);
  });
});
