export class ConstantsApiRoutes {

  /* API ROUTES*/
  /* AUTH */
  public static get LOGIN(): string { return "/api/auth/login"; };
  public static get RESET(): string { return "/api/auth/reset"; };

  /* USUARIO */
  public static get USUARIO_AGREGAR(): string { return "/api/usuario/agregar"; };
  public static get USUARIO_EDITAR(): string { return "/api/usuario/editar"; };
  public static get USUARIO_EDITAR_PERFIL(): string { return "/api/usuario/perfil"; };
  public static get USUARIO_EDITAR_PASSWORD(): string { return "/api/usuario/password"; };
  public static get USUARIO_BUSCAR(): string { return "/api/usuario/buscar"; };
  public static get USUARIO_OBTENER(): string { return "/api/usuario/obtener"; };
  public static get USUARIO_EDITAR_ESTADO(): string { return "/api/usuario/estado"; };
  public static get USUARIO_ELIMINAR(): string { return "/api/usuario/eliminar"; };
  public static get USUARIO_EDITAR_IMG(): string { return "/api/usuario/img"; };
  public static get USUARIO_EDITAR_IMG_EXTERNAL(): string { return "/api/usuario/imgExternal"; };
  public static get PERFIL_USER(): string { return "/api/usuario/perfil"; };

  /* PRODUCTO */
  public static get PRODUCTO_AGREGAR(): string { return "/api/producto/agregar"; };
  public static get PRODUCTO_EDITAR(): string { return "/api/producto/editar"; }
  public static get PRODUCTO_BUSCAR(): string { return "/api/producto/buscar"; };
  public static get PRODUCTO_OBTENER(): string { return "/api/producto/obtener"; };
  public static get PRODUCTO_EDITAR_ESTADO(): string { return "/api/producto/estado"; };
  public static get PRODUCTO_ELIMINAR(): string { return "/api/producto/eliminar"; }
  public static get PRODUCTO_EDITAR_IMG(): string { return "/api/producto/img"; }

  /* PAGO */
  public static get PAGO_CHECKOUT(): string { return "/api/pago/checkout"; };
}
