---
name: aspnet-user-models
description: Create or update ASP.NET Core user models in App_NetCoreAngular.Server/Models/Usuarios while preserving validation, persistence boundaries, and API contracts.
---

# Modelos ASP.NET Core de usuarios

Usa esta skill al crear o modificar modelos C# del dominio de usuarios en `App_NetCoreAngular.Server/Models/Usuarios`. Toma como referencia los modelos existentes, pero elige el tipo según su responsabilidad; no copies propiedades o atributos sin confirmar su propósito.

## Procedimiento

1. **Identifica para qué sirve el modelo.** Revisa los modelos cercanos y sus usos en `UsuarioController`, repositorios y, cuando corresponda, `AppDataContext`.
   - Una entidad como `Usuario` representa datos persistidos y contiene la configuración de EF Core.
   - Un formulario como `UsuarioForm` o `UsuarioFormEditar` representa datos de entrada para una operación específica.
   - Una vista como `UsuarioSimpleView` o `UsuarioLoginView` representa datos de salida y solo debe exponer lo que el consumidor necesita.
2. **Ubícalo y nómbralo de forma coherente.** Coloca el archivo en `Models/Usuarios`, usa el namespace `App_NetCoreAngular.Server.Models.Usuarios`, nombres de archivo y tipos PascalCase, y un nombre que exprese la operación o el propósito. Mantén una clase por archivo, salvo los tipos pequeños y estrechamente relacionados que el proyecto ya agrupe.
3. **Define propiedades con tipos y nulabilidad precisos.** Con `Nullable` habilitado, usa `string?` solo para valores opcionales y `required` para miembros que deben inicializarse. El modificador C# `required` impone inicialización al compilar; `[Required]` participa en la validación del request. Alinea ambos con la obligatoriedad real del contrato, no los uses como sinónimos.
4. **Valida requests en el límite HTTP.** Usa Data Annotations como `[Required]`, `[MaxLength]` y `[MinLength]` con los mensajes de `Utility.Error.ErrorMessages` cuando corresponda. Ajusta cada límite al contrato real y evita atributos contradictorios. Recuerda que `[Required]` en un tipo valor no impide por sí solo todos sus valores predeterminados; agrega una validación explícita si el dominio debe rechazar uno.
5. **Protege la frontera entre modelo, persistencia y API.**
   - Coloca `[Table]`, `[Key]` y atributos de persistencia solo en la entidad EF. No agregues formularios ni vistas al `DbContext` como entidades.
   - No devuelvas la entidad de base de datos directamente desde un endpoint; usa una vista limitada a los campos requeridos.
   - No incluyas `Password` ni otros secretos en vistas. El formulario puede aceptar una contraseña si la operación lo requiere, pero el hash y la persistencia pertenecen al flujo de servicio/controlador/repositorio existente, no al modelo de transporte.
   - Si una propiedad cambia el esquema de una entidad, revisa `AppDataContext` y genera una migración de EF Core. No edites migraciones aplicadas a mano.
6. **Conserva los contratos existentes.** Antes de cambiar o reutilizar propiedades, verifica sus mapeos y consumidores Angular. Conserva nombres, tipos, nulabilidad, enums como `RolUsuario` y `Estado`, y la forma de respuesta `ResponseApi<T>` salvo que la tarea solicite expresamente un cambio de contrato. No cambies configuración global de serialización para corregir un modelo aislado.
7. **Mantén los modelos enfocados.** No pongas acceso a datos, llamadas HTTP, hashing, autorización ni lógica de negocio en estas clases. Evita duplicar reglas, agregar campos que el endpoint no usa o crear una abstracción compartida para un único formulario.
8. **Valida los cambios.** Compila desde la raíz con `dotnet build App_NetCoreAngular.Server/App_NetCoreAngular.Server.csproj`. Si cambió un contrato consumido por Angular, revisa también los tipos y usos relacionados del frontend; si cambió el esquema, revisa la migración generada.

## Ejemplo: formulario de entrada

Para un request, define solo los campos que acepta la operación y valida longitudes de acuerdo con su contrato. El siguiente ejemplo es ilustrativo; verifica los límites antes de adaptarlo:

```csharp
using System.ComponentModel.DataAnnotations;
using App_NetCoreAngular.Server.Utility.Error;

namespace App_NetCoreAngular.Server.Models.Usuarios
{
    public class UsuarioContactoForm
    {
        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        [MaxLength(250, ErrorMessage = ErrorMessages.Max250)]
        public required string Nombre { get; set; }

        [MaxLength(50, ErrorMessage = ErrorMessages.Max50)]
        public string? Email { get; set; }
    }
}
```

No agregues datos de persistencia a este formulario ni reutilices esta clase como respuesta. Crea una vista separada y explícita para cada respuesta que necesite el consumidor.
