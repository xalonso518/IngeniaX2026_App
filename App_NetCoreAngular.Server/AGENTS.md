# Instrucciones del backend

Estas pautas complementan el `AGENTS.md` de la raíz y aplican al servidor ASP.NET Core (.NET 10), con Entity Framework Core y SQL Server.

## Organización

- `Controllers/`: endpoints HTTP y coordinación entre servicios y repositorios.
- `Repository/`: acceso a datos y operaciones de usuarios, autenticación y productos.
- `EntityFramework/`: `AppDataContext`, consultas de paginación y migraciones.
- `Models/`: entidades, formularios de entrada y modelos de respuesta.
- `Services/`: integraciones de tokens, correo, SignalR, almacenamiento y pagos.
- `Policies/`: requisitos y handlers de autorización.

## Pautas de cambios

- Mantén la lógica de acceso a datos en repositorios, no en controladores; registra nuevos repositorios y servicios mediante inyección de dependencias en `Program.cs`.
- Sigue el estilo existente de rutas con atributos (`api/[controller]`), métodos asíncronos y respuestas `ResponseApi<T>` cuando corresponda.
- Conserva los contratos de la API que consume Angular. Protege los endpoints con las políticas de autorización existentes y no debilites autenticación ni validaciones.
- Para cambios de esquema, genera y revisa una migración de EF Core; no modifiques a mano migraciones ya aplicadas.
- Mantén credenciales y claves fuera del código y de los archivos versionados.

## Validación

- Compilar el backend desde la raíz: `dotnet build App_NetCoreAngular.Server/App_NetCoreAngular.Server.csproj`
- Ejecutarlo localmente: `dotnet run --project App_NetCoreAngular.Server/App_NetCoreAngular.Server.csproj`
