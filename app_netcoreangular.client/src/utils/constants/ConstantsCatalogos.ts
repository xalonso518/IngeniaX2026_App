import { WrapCatalogoNumber } from "../../models/shared/WrapCatalogo";

export class ConstantsCatalogos {

  public static get BLOB_URL_PUBLIC(): string {
    return "https://bkprostorageproduction.blob.core.windows.net/public/";
  };

  public static get TOKEN_TARJETA(): string {
    return "";
  };

  public static get USUARIO_ROLES(): WrapCatalogoNumber[] {
    var a = new Array();
    a.push({ key: 1, value: "Admin" });
    a.push({ key: 2, value: "Usuario" });
    return a;
  };

  public static get STATUS(): WrapCatalogoNumber[] {
    var a = new Array();
    a.push({ key: 1, value: "Activo" });
    a.push({ key: 2, value: "Inactivo" });
    a.push({ key: 3, value: "Pendiente" });
    a.push({ key: 99, value: "Baja" });
    return a;
  };
}

export const MODULOS = {
  USU: "Usuario",
  CLI: "Producto"
} as const;


export const ACCIONES = {
  A: "Alta",
  B: "Cliente",

} as const;

export const IMG_EXT = ['png', 'svg', 'jpg', 'jepg'];

export const ROLES = {
  NA: 0,
  ADM: 1,
  USE: 2
} as const;
