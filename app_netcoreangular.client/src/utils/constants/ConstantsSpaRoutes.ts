export class ConstantsSpaRoutes {
  /* ANGULAR ROUTES */
  public static get SPA_LOGIN(): string { return "/auth/login"; };
  public static get SPA_RESET(): string { return "/auth/reset"; };

  /* USUARIO */
  public static get SPA_USUARIO_BUSCAR(): string { return "/usuario"; };
  public static get SPA_USUARIO_AGREGAR(): string { return "/usuario/agregar"; };
  public static get SPA_USUARIO_EDITAR(): string { return "/usuario/editar"; };


  /* PRODUCTO */
  public static get SPA_PRODUCTO_BUSCAR(): string { return "/producto"; };
  public static get SPA_PRODUCTO_AGREGAR(): string { return "/producto/agregar"; };
  public static get SPA_PRODUCTO_EDITAR(): string { return "/producto/editar"; };
  public static get SPA_PRODUCTO_IMAGEN_EDITAR(): string { return "/producto/imagen/editar"; };
}
