# Guía del proyecto

Este repositorio contiene una API ASP.NET Core en `App_NetCoreAngular.Server` y una aplicación Angular en `app_netcoreangular.client`.

## Pautas

- Antes de cambiar código, revisa el contexto cercano y sigue las convenciones existentes.
- Mantén los cambios acotados y conserva el trabajo local que no esté relacionado.
- No incluyas secretos ni credenciales en el código o la configuración.
- Si cambias una parte del proyecto, ejecuta las pruebas o compilación correspondientes.

## Comandos

- Compilar la solución: `dotnet build App_NetCoreAngular.slnx`
- Compilar Angular: `cd app_netcoreangular.client && npm run build`
- Ejecutar pruebas de Angular: `cd app_netcoreangular.client && npm test`
