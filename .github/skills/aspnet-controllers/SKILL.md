---
name: aspnet-controllers
description: Create or update ASP.NET Core API controllers following UsuarioController, preserving authorization, routes, and response contracts.
---

# Controladores ASP.NET Core

Usa esta skill al crear o modificar controladores del backend. Toma `App_NetCoreAngular.Server/Controllers/UsuarioController.cs` como referencia de rutas y coordinación con repositorios, pero verifica cada decisión con los consumidores y políticas del endpoint; no copies sus omisiones.

## Procedimiento

1. **Rastrea el contrato del endpoint.** Revisa el controlador más cercano, el modelo de request/response, la interfaz de repositorio o servicio, las políticas de autorización y las rutas Angular relacionadas. Confirma método HTTP, ruta, parámetros, cuerpo y forma de respuesta antes de cambiar nada.
2. **Mantén el controlador enfocado en HTTP.** Usa `ControllerBase`, `[ApiController]`, `[Route("api/[controller]")]` y `[Produces("application/json")]` cuando sean coherentes con el resto del backend. El controlador recibe y valida datos, coordina dependencias y traduce resultados a respuestas HTTP; deja consultas y persistencia en repositorios, e integraciones en los servicios existentes.
3. **Usa inyección de dependencias concreta.** Inyecta solo repositorios y servicios que se utilicen, con tipos correspondientes al controlador actual. Evita construir dependencias manualmente, inyectar loggers de otro controlador o dejar parámetros sin uso.
4. **Declara rutas y enlaces de datos sin ambigüedad.** Usa atributos de acción como `[HttpGet("obtener/{guid}")]`, especifica `[FromRoute]`, `[FromQuery]` o `[FromBody]` cuando ayude a expresar el contrato y alinea nombres y tipos entre plantilla de ruta y parámetros. Si un identificador aparece tanto en ruta como en cuerpo, valida que ambos se refieran al mismo recurso; no aceptes datos del cliente que permitan modificar campos no autorizados.
5. **Preserva la autorización.** Conserva `[Authorize]` en controlador y las políticas por acción que correspondan, como `UserActive` o `RolAdmin`. Usa `[AllowAnonymous]` solo cuando el diseño requiera explícitamente un endpoint público. No uses valores por defecto como `"0"` para convertir un claim ausente en una identidad válida ni confíes en identificadores enviados por el cliente para decidir autorización; usa claims y handlers existentes.
6. **Valida requests y protege datos sensibles.** Aprovecha la validación de `[ApiController]` y las Data Annotations de los modelos. Para operaciones con contraseñas, conserva el flujo seguro de autenticación/hash del proyecto; nunca persistas contraseñas en texto claro, las devuelvas en respuestas ni las escribas en logs. Para archivos, delega validación y almacenamiento a las utilidades y servicios existentes.
7. **Traduce resultados de forma consistente.** Devuelve `Task<IActionResult>` y espera con `await` las operaciones asíncronas. Cuando aplique, conserva `ResponseApi<T>` y el mapeo de resultados HTTP consumido por Angular. Diferencia éxito, validación, recurso inexistente y error del servidor de acuerdo con el contrato existente; no respondas `Ok` a fallos ni ocultes errores con respuestas vacías. Evita capturar excepciones genéricas en cada acción; usa el manejo global o captura únicamente errores que puedas traducir y registrar.
8. **Mantén acciones pequeñas y sincronizadas con el contrato.** Una acción debe coordinar una operación; no dupliques reglas de negocio entre controladores. Documenta el endpoint con comentarios XML si aporta información a OpenAPI. No agregues rutas o cambies formas de respuesta sin actualizar explícitamente sus consumidores.
9. **Valida.** Ejecuta `dotnet build App_NetCoreAngular.Server/App_NetCoreAngular.Server.csproj` desde la raíz y las pruebas backend existentes relacionadas. Si cambia un contrato, revisa los tipos, rutas y consumidores Angular; si hay un cambio de esquema, genera y revisa la migración EF Core.

## Ejemplo: acción delegada al repositorio

El patrón conserva la responsabilidad HTTP en el controlador y deja la consulta en el repositorio. Ajusta la política y el estado de error al contrato real:

```csharp
[HttpGet("obtener/{guid}")]
public async Task<IActionResult> Obtener([FromRoute] string guid)
{
    var resultado = await _usuarioRepository.BuscarPorGuid(guid);

    return resultado.Success
        ? Ok(resultado)
        : BadRequest(resultado);
}
```

Antes de reutilizar esta forma, confirma si el fallo significa datos inválidos, recurso no encontrado o un error interno; el estado HTTP no debe elegirse solo por conveniencia.

## Evita

- Hacer consultas a EF Core, escribir archivos o enviar correo directamente desde una acción cuando ya existen repositorios y servicios para ello.
- Omitir o debilitar políticas de autorización al agregar una ruta.
- Confiar en un identificador recibido sin comprobar que pertenece al recurso y operación solicitados.
- Mantener argumentos de ruta, logger o dependencias que la acción no usa.
- Cambiar rutas, nombres de campos, enumeraciones o envolturas `ResponseApi<T>` sin coordinar el contrato con Angular.
