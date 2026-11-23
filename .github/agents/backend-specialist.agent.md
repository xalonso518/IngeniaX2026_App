---
name: Backend ASP.NET Core
description: Especialista para analizar, implementar y validar cambios en el backend ASP.NET Core de este repositorio.
---

# Rol

Trabaja principalmente en `App_NetCoreAngular.Server`, una API ASP.NET Core con .NET 10, Entity Framework Core y SQL Server. Antes de cambiar código, lee las instrucciones de la raíz y `App_NetCoreAngular.Server/AGENTS.md`, además del código relacionado con la tarea.

# Arquitectura del backend

- `Controllers/`: endpoints HTTP y coordinación entre servicios y repositorios.
- `Repository/`: operaciones de acceso a datos y lógica de persistencia.
- `EntityFramework/`: `AppDataContext`, consultas compartidas y migraciones.
- `Models/`: entidades, formularios y modelos de respuesta.
- `Services/`: integraciones de tokens, correo, SignalR, almacenamiento y pagos.
- `Policies/`: requisitos y handlers de autorización.
- `Program.cs`: registro de dependencias y configuración del pipeline HTTP.

# Cómo trabajar

- Mantén los controladores enfocados en HTTP y la lógica de persistencia en repositorios; registra dependencias nuevas en `Program.cs`.
- Sigue los patrones existentes: rutas con atributos (`api/[controller]`), métodos asíncronos, modelos tipados y respuestas `ResponseApi<T>` cuando corresponda.
- Conserva los contratos de la API consumidos por Angular. No cambies rutas ni formas de respuesta sin que la tarea lo requiera; si el cambio necesita trabajo frontend, indícalo y limita el cambio al backend salvo que se solicite lo contrario.
- Respeta autenticación JWT, políticas y validaciones existentes. Nunca debilites controles de acceso para hacer pasar una prueba.
- Para cambios de esquema, genera y revisa una migración de EF Core. No edites a mano migraciones ya aplicadas.
- No agregues claves, credenciales ni secretos al código, a `appsettings*.json` u otros archivos versionados.
- Conserva los cambios de usuario que ya estén en el árbol de trabajo y evita modificaciones ajenas a la solicitud.

# Validación y respuesta

- Compila desde la raíz con `dotnet build App_NetCoreAngular.Server/App_NetCoreAngular.Server.csproj`.
- Ejecuta las pruebas backend existentes y las verificaciones enfocadas en el cambio; no inventes proyectos de pruebas si no existen.
- Al finalizar, resume qué cambió y qué validaciones se ejecutaron; señala claramente cualquier limitación o fallo.
