---
name: aspnet-repositories
description: Create or update ASP.NET Core data repositories following the UsuarioRep structure, EF Core practices, and API response contracts.
---

# Repositorios ASP.NET Core

Usa esta skill al crear o modificar repositorios del backend. Toma `App_NetCoreAngular.Server/Repository/UsuarioRep/` como referencia para la separación entre interfaz e implementación, y confirma cada convención con los repositorios vecinos antes de aplicarla a otro dominio.

## Procedimiento

1. **Rastrea el contrato y sus consumidores.** Revisa la interfaz y la implementación más cercanas, el modelo, el controlador que invoca el repositorio, `AppDataContext` y el registro en `Program.cs`. Determina qué responsabilidad pertenece al repositorio y qué debe seguir en controlador, servicio o política.
2. **Define una interfaz tipada.** Colócala junto a la implementación bajo `Repository/<Dominio>Rep/`. Declara operaciones del dominio usando modelos concretos y tipos asíncronos (`Task<T>`) para acceso a datos. No devuelvas `IActionResult` ni tipos HTTP desde el repositorio. Usa un namespace coherente entre interfaz, implementación y registro de DI; no reproduzcas inconsistencias de nombres presentes en otros espacios de nombres.
3. **Implementa con dependencias inyectadas.** Recibe `AppDataContext` y otros servicios requeridos por constructor. No instancies servicios manualmente ni introduzcas dependencias que la operación no use. Registra la interfaz e implementación en `Program.cs` usando el ciclo de vida consistente con el resto del proyecto.
4. **Mantén la consulta en SQL hasta necesitar los datos.** Usa operaciones asíncronas de EF Core (`FirstOrDefaultAsync`, `FindAsync`, `SaveChangesAsync`, `ExecuteUpdateAsync`) cuando correspondan. Para lectura, considera `AsNoTracking()`. Construye filtros, orden y proyección dentro de `IQueryable`; proyecta a una vista explícita antes de materializar y pagina en la base de datos. No cargues entidades completas ni secretos que la respuesta no necesita.
5. **Actualiza solo lo solicitado.** Para cambios de entidad, modifica únicamente las propiedades permitidas y actualiza marcas de tiempo conforme al patrón del dominio. Comprueba el resultado de `SaveChangesAsync` o de la operación afectada y maneja explícitamente el caso sin entidad o sin filas actualizadas. No expongas entidades EF directamente al consumidor.
6. **Conserva las respuestas de la API.** Cuando el flujo actual utilice `ResponseApi<T>`, mantén ese contrato con payload, estado de éxito, error y paginador según aplique. El repositorio informa el resultado de datos; el controlador decide el código HTTP. No devuelvas éxito con un payload vacío para ocultar un fallo.
7. **Haz explícitos los errores y respeta el ciclo de vida de EF.** Evita `catch (Exception)` que descarte el error o retorne un mensaje genérico sin registro. Captura solo errores que puedas traducir de manera útil, registra el detalle con `ILogger` y conserva una señal explícita de fallo; deja propagarse los errores inesperados conforme al manejo global del proyecto. `AppDataContext` lo administra DI: no lo envuelvas en `using` ni lo deseches desde un repositorio.
8. **Revisa utilidades compartidas antes de reutilizarlas.** En particular, el `PageQuery<T>.PageSearch` actual envuelve el contexto recibido en `using`; eso puede desechar el `DbContext` administrado por DI. No copies ese patrón. Si necesitas paginación, corrige el ciclo de vida del helper en un cambio acotado o realiza la consulta sin desechar el contexto.
9. **Valida.** Ejecuta `dotnet build App_NetCoreAngular.Server/App_NetCoreAngular.Server.csproj` desde la raíz y las pruebas backend existentes relacionadas. Si el contrato cambia, revisa controladores y tipos Angular consumidores; si cambia el esquema, genera y revisa una migración EF Core.

## Ejemplo: filtrar y proyectar antes de ejecutar la consulta

Adapta los filtros y campos al contrato real. La consulta debe permanecer como `IQueryable` hasta que se aplique orden, paginación o materialización:

```csharp
IQueryable<UsuarioSimpleView> query = _context.Usuarios
    .AsNoTracking()
    .Where(usuario => usuario.Estado != Estado.Baja)
    .Select(usuario => new UsuarioSimpleView
    {
        UsuarioId = usuario.UsuarioId,
        Guid = usuario.Guid,
        Username = usuario.Username,
        Nombre = usuario.Nombre,
        Rol = usuario.Rol,
        DateCreated = usuario.DateCreated,
        Estado = usuario.Estado
    });
```

La proyección limita las columnas leídas y evita incluir `Password` en una vista. Para añadir un repositorio nuevo, mantén sincronizadas las firmas de su interfaz y su implementación, los consumidores y el registro de DI.
