# IngeniaX 2026

## Guía paso a paso para construir el backend

### ASP.NET Core, Entity Framework Core y SQL Server

| Dato | Descripción |
|---|---|
| Propósito | Explicar cómo organizar y conectar las piezas principales del backend |
| Audiencia | Estudiantes y desarrolladores principiantes |
| Alcance | API ASP.NET Core de IngeniaX |
| Fuente | Código actual del repositorio |

> **Nota sobre el historial:** esta guía explica una secuencia didáctica basada en la estructura que existe hoy. El repositorio no permite confirmar el orden histórico exacto en que se escribió cada archivo. Los comandos de creación son una ruta de referencia, no una afirmación de que todos se ejecutaron exactamente así.

---

# Contenido

1. [Qué se va a construir](#1-qué-se-va-a-construir)
2. [Preparar el entorno y obtener el código](#2-preparar-el-entorno-y-obtener-el-código)
3. [Crear la solución y organizar carpetas](#3-crear-la-solución-y-organizar-carpetas)
4. [Modelar datos y configurar Entity Framework](#4-modelar-datos-y-configurar-entity-framework)
5. [Separar el acceso a datos con repositorios](#5-separar-el-acceso-a-datos-con-repositorios)
6. [Agregar utilidades y servicios](#6-agregar-utilidades-y-servicios)
7. [Proteger la API con autenticación y políticas](#7-proteger-la-api-con-autenticación-y-políticas)
8. [Exponer operaciones con controladores](#8-exponer-operaciones-con-controladores)
9. [Conectar los componentes en Program.cs](#9-conectar-los-componentes-en-programcs)
10. [Ejecutar y comprobar el backend](#10-ejecutar-y-comprobar-el-backend)
11. [Configurar secretos y continuar el desarrollo](#11-configurar-secretos-y-continuar-el-desarrollo)
12. [Glosario](#12-glosario)

# 1. Qué se va a construir

El backend es la parte del sistema que recibe peticiones HTTP, aplica reglas de negocio, consulta o modifica datos y devuelve respuestas. En IngeniaX está implementado como una API ASP.NET Core con controladores, Entity Framework Core y SQL Server. El proyecto también está integrado con una aplicación Angular; esta guía se concentra en el servidor.

Una petición normalmente recorre estas capas:

```text
Aplicación cliente
        │ petición HTTP
        ▼
Controller ── coordina el endpoint y valida la petición
        ├──────── Repository ─────── AppDataContext ─────── SQL Server
        └──────── Service ────────── proveedor externo
        │
        ▼ respuesta HTTP con un modelo de respuesta
```

Cada carpeta tiene una responsabilidad. Esa separación ayuda a encontrar código, probarlo y cambiar una parte sin convertir un controlador en el lugar donde vive toda la aplicación.

# 2. Preparar el entorno y obtener el código

## 2.1 Herramientas

- **.NET SDK 10**, que coincide con el destino `net10.0` de `App_NetCoreAngular.Server.csproj`.
- **Git** para clonar y versionar el proyecto.
- **SQL Server** accesible desde el entorno local, con una base de datos configurada para la aplicación.
- Un editor como Visual Studio o Visual Studio Code.
- Para trabajar con migraciones, las herramientas de Entity Framework Core (`dotnet-ef`).

El backend ya declara en su archivo de proyecto dependencias para EF Core con SQL Server, JWT Bearer, OpenAPI/Swagger, SignalR, SendGrid y Stripe. En una aplicación nueva se agregan los paquetes que se necesitan; no es necesario agregar todos por anticipado.

## 2.2 Descargar y compilar el repositorio

Para trabajar sobre este proyecto:

```bash
git clone https://github.com/xalonso518/IngeniaX2026_App.git
cd IngeniaX2026_App
dotnet restore
dotnet build App_NetCoreAngular.Server/App_NetCoreAngular.Server.csproj
```

Clonar obtiene el historial y los archivos del repositorio. `restore` resuelve las dependencias de NuGet y `build` comprueba que el servidor compile.

# 3. Crear la solución y organizar carpetas

## 3.1 Punto de partida para una solución nueva

Si se empieza desde cero con el SDK de .NET 10, una base posible es:

```bash
dotnet new sln -n App_NetCoreAngular
dotnet new webapi -n App_NetCoreAngular.Server --use-controllers --framework net10.0
dotnet sln App_NetCoreAngular.slnx add App_NetCoreAngular.Server/App_NetCoreAngular.Server.csproj
```

La plantilla genera el esqueleto inicial. Después se agregan las carpetas de la aplicación y las dependencias necesarias. En este repositorio la solución se llama `App_NetCoreAngular.slnx` y el backend está en `App_NetCoreAngular.Server/`. No ejecutes estos comandos de creación dentro de una copia ya existente: podrían crear archivos que no necesitas.

## 3.2 Mapa de carpetas del backend

| Carpeta | Responsabilidad principal |
|---|---|
| `Controllers/` | Rutas HTTP, validación de entrada y coordinación de una operación |
| `Models/` | Entidades, formularios de entrada y modelos de respuesta |
| `Repository/` | Consultas y persistencia de usuarios, autenticación, productos y clientes |
| `EntityFramework/` | `AppDataContext`, paginación y migraciones de base de datos |
| `Services/` | Integraciones como tokens, correo, SignalR, almacenamiento y pagos |
| `Utility/` | Funciones reutilizables, como validación de archivos y hashing |
| `Policies/` | Requisitos y reglas de autorización |
| `Enums/` | Enumeraciones compartidas por el servidor |
| `Providers/` | Proveedores concretos, por ejemplo el manejo de archivos locales |

El archivo `Program.cs` es el punto de composición: registra las dependencias y define el pipeline HTTP.

# 4. Modelar datos y configurar Entity Framework

## 4.1 Distinguir entidad, formulario y vista

En `Models/Productos/` se puede observar la separación entre:

- `Producto.cs`: entidad que representa un producto persistido.
- `ProductoForm.cs` y otros formularios: datos que se aceptan al crear o editar.
- `ProductoView.cs` o `ProductoSimpleView.cs`: datos preparados para responder a una consulta.

No todas las propiedades de una entidad deben aceptarse directamente desde el cliente. Un formulario limita y valida la entrada; una vista evita devolver datos internos que no se necesitan.

Ejemplo reducido de una entidad:

```csharp
public class Producto
{
    public long ProductoId { get; set; }
    public required string Nombre { get; set; }
    public required decimal Precio { get; set; }
}
```

El modelo real contiene más propiedades y atributos de validación y mapeo. Los formularios utilizan atributos como `[Required]` y `[MaxLength]`; con `[ApiController]`, ASP.NET Core puede validar automáticamente el modelo recibido.

## 4.2 Crear el contexto

`EntityFramework/AppDataContext.cs` hereda de `DbContext`. Cada propiedad `DbSet<T>` representa una colección de entidades que EF Core puede consultar y guardar:

```csharp
public DbSet<Usuario> Usuarios { get; set; }
public DbSet<Producto> Productos { get; set; }
public DbSet<Cliente> Clientes { get; set; }
```

El contexto recibe opciones desde la configuración de servicios. El proveedor de SQL Server se establece en `Program.cs`; la conexión debe suministrarse de forma privada y no escribirse como un valor real en esta guía o en Git.

## 4.3 Mantener el esquema con migraciones

Una migración registra un cambio en el esquema y permite aplicarlo de manera reproducible. El repositorio mantiene migraciones para sus entidades. Después de cambiar un modelo, el flujo habitual es:

```bash
dotnet ef migrations add NombreDescriptivo \
  --project App_NetCoreAngular.Server/App_NetCoreAngular.Server.csproj \
  --startup-project App_NetCoreAngular.Server/App_NetCoreAngular.Server.csproj

dotnet ef database update \
  --project App_NetCoreAngular.Server/App_NetCoreAngular.Server.csproj \
  --startup-project App_NetCoreAngular.Server/App_NetCoreAngular.Server.csproj
```

Revisa el código de la migración generada antes de aplicarla. No edites a mano una migración que ya se aplicó en un entorno compartido.

# 5. Separar el acceso a datos con repositorios

Un repositorio concentra operaciones de persistencia y evita que cada controlador construya sus propias consultas a la base de datos.

En `Repository/ProductoRep/`:

- `IProductoRepository` declara las operaciones que pueden solicitar otras capas.
- `ProductoRepository` implementa esas operaciones con `AppDataContext`.

El controlador depende de la interfaz, no necesita conocer detalles de SQL Server:

```csharp
private readonly IProductoRepository _productoRepository;

public ProductoController(IProductoRepository productoRepository)
{
    _productoRepository = productoRepository;
}
```

Un flujo de lectura que existe en el código es:

1. `ProductoController.Buscar` recibe los filtros de la petición.
2. Llama a `IProductoRepository.Buscar`.
3. `ProductoRepository` construye la consulta con EF Core y selecciona los campos de salida.
4. `PageQuery<T>` aplica paginación.
5. La operación devuelve un `ResponseApi<T>` al controlador, que elige el estado HTTP.

Las interfaces también permiten sustituir una implementación en pruebas o registrar una implementación distinta sin cambiar la ruta HTTP.

# 6. Agregar utilidades y servicios

## 6.1 Utilidades

Las utilidades contienen funciones pequeñas reutilizables que no dependen de una petición específica:

- `FilesUtilities` y `FilesValidationUtilities` ayudan con metadatos, extensiones y tamaño de archivos.
- `GuidUtilities` trabaja con identificadores.
- `HashUtilities` agrupa operaciones relacionadas con contraseñas.
- `ErrorMessages` centraliza mensajes usados por validaciones.

La utilidad no sustituye una política de seguridad. Las validaciones deben ejecutarse antes de guardar o usar un archivo y las reglas deben revisarse según el riesgo del tipo de archivo y su destino.

## 6.2 Servicios

Un servicio encapsula una capacidad reutilizable o una integración:

- `TokenService` genera tokens JWT.
- `MailSendGrid` envía correo mediante SendGrid.
- `NotificacionSignalR` coordina notificaciones en tiempo real.
- `FileLocal` y `BlobStorage` gestionan almacenamiento de archivos.
- `StripeService` agrupa operaciones relacionadas con pagos.

El servicio recibe sus dependencias por constructor y se registra en el contenedor de inyección de dependencias. Así, un controlador puede pedir una operación de servicio sin crear manualmente el cliente de un proveedor externo.

## 6.3 Respuestas compartidas

`Models/Shared/ResponseApi<T>` proporciona una forma común para comunicar éxito, contenido, error y, cuando aplica, información de paginación. Los genéricos permiten utilizar el mismo formato con distintos tipos de resultado, por ejemplo `ResponseApi<ProductoView>`.

# 7. Proteger la API con autenticación y políticas

## 7.1 Autenticación JWT

La autenticación responde **quién realiza la petición**. En el flujo de inicio de sesión actual:

1. `AuthController.Login` recibe un `LoginForm`.
2. Consulta `IAuthRepository` para verificar el usuario activo.
3. Si la verificación tiene éxito, `TokenService` crea un JWT con claims del usuario.
4. El controlador devuelve un `TokenUsuario` dentro de `ResponseApi<T>`.
5. En peticiones posteriores, el middleware valida el token Bearer.

El secreto de firma, el emisor y la audiencia son configuración del entorno; no se deben copiar en el código, en una captura de pantalla ni en un commit.

## 7.2 Autorización

La autorización responde **qué puede hacer el usuario autenticado**. En `Policies/`:

- Un `Requirement` expresa la condición requerida.
- Un `Handler` evalúa los claims y satisface el requisito cuando corresponde.

`Program.cs` registra las políticas como `UserActive` y `RolAdmin`, además de sus handlers. Los controladores aplican la política con `[Authorize(Policy = "...")]`. Las rutas públicas se marcan explícitamente con `[AllowAnonymous]`.

## 7.3 Nota importante sobre contraseñas

El código actual permite seguir el recorrido por `HashUtilities`, pero `HashPassword` utiliza MD5. MD5 no es adecuado para almacenar contraseñas en una aplicación nueva. No copies ese algoritmo como recomendación: usa un hasher diseñado para contraseñas, como `PasswordHasher<TUser>` de ASP.NET Core Identity, y planifica cómo migrar los registros existentes antes de cambiar el inicio de sesión.

# 8. Exponer operaciones con controladores

Un controlador traduce HTTP a llamadas internas. No debería contener consultas directas a SQL Server ni implementar integraciones de proveedores.

En `ProductoController` se observan atributos como:

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductoController : ControllerBase
```

Con esta ruta, las acciones definen subrutas mediante atributos como `[HttpGet("buscar")]` o `[HttpDelete("eliminar/{id}")]`. Los parámetros se enlazan desde la ruta, la consulta o el cuerpo de la petición. El controlador coordina repositorios y servicios y traduce el resultado a un estado HTTP, por ejemplo `Ok(...)` o `BadRequest(...)`.

Para comprender un endpoint, sigue este orden:

1. **Ruta y método HTTP:** identifica cómo se invoca.
2. **Autorización:** revisa `[Authorize]`, políticas o `[AllowAnonymous]`.
3. **Entrada:** identifica el formulario y sus validaciones.
4. **Dependencias:** observa las interfaces y servicios recibidos por constructor.
5. **Resultado:** determina qué respuesta se devuelve en éxito y error.

Al añadir una operación, comprueba que persista los datos en el repositorio, que valide la entrada y que conserve el contrato que espera el cliente.

# 9. Conectar los componentes en Program.cs

`Program.cs` es el lugar donde se arma la aplicación. En este proyecto cumple cuatro funciones principales:

1. **Registrar servicios:** controladores, OpenAPI/Swagger, Entity Framework, repositorios, servicios, SignalR y handlers de autorización.
2. **Configurar seguridad y comunicación:** JWT, políticas y CORS.
3. **Construir el pipeline HTTP:** archivos estáticos, HTTPS, CORS, autenticación y autorización.
4. **Mapear rutas:** controladores y el hub de SignalR.

La inyección de dependencias permite declarar qué implementación usar:

```csharp
builder.Services.AddDbContext<AppDataContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddTransient<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<TokenService>();
```

El ejemplo muestra el patrón, no el valor de `connectionString`. `Transient` crea una instancia al resolver el servicio; `Scoped` comparte una instancia durante una petición. La duración debe elegirse según el estado y las dependencias del servicio.

El orden del pipeline importa. Por ejemplo, la autenticación debe ejecutarse antes de la autorización para que las políticas puedan evaluar al usuario autenticado. Swagger/OpenAPI está habilitado para facilitar la exploración de endpoints, con configuración adicional en desarrollo.

# 10. Ejecutar y comprobar el backend

Desde la raíz del repositorio:

```bash
dotnet restore
dotnet build App_NetCoreAngular.Server/App_NetCoreAngular.Server.csproj
dotnet run --project App_NetCoreAngular.Server/App_NetCoreAngular.Server.csproj
```

Cuando la aplicación inicia, revisa la dirección HTTPS que informa la terminal. En el entorno de desarrollo, explora Swagger para comprobar las rutas y los modelos publicados. Para probar rutas protegidas necesitarás una configuración local válida y un token obtenido mediante el flujo de autenticación; nunca pongas credenciales reales en documentación o ejemplos.

Ante un error de conexión, verifica primero la configuración de SQL Server y que la base de datos tenga las migraciones aplicadas. Ante un error HTTP, revisa el método y la ruta, el cuerpo de entrada, el token y la política requerida.

# 11. Configurar secretos y continuar el desarrollo

`Program.cs` consume configuración para la conexión de SQL Server, la firma y validación de JWT, servicios de correo y Stripe. Sus nombres de configuración no son credenciales; los valores sí deben permanecer fuera del repositorio.

Para desarrollo local puedes usar Secret Manager:

```bash
dotnet user-secrets init --project App_NetCoreAngular.Server/App_NetCoreAngular.Server.csproj
dotnet user-secrets set "TokenSettings:Secret" "<secreto-local>" \
  --project App_NetCoreAngular.Server/App_NetCoreAngular.Server.csproj
```

Configura de la misma manera los demás valores que requiera tu entorno, utilizando valores propios y privados. No guardes secretos en `appsettings.json`, no reutilices una clave de ejemplo y evita mostrar valores reales en logs. Antes de publicar, vuelve a compilar, revisa las migraciones y confirma que las políticas de autorización protejan las rutas correctas.

## Generar el PDF de esta guía

Con Pandoc y Google Chrome instalados, desde la raíz del repositorio ejecuta:

```bash
./docs/build/build_backend_guide.sh
```

El comando crea `docs/IngeniaX2026_Guia_Construccion_Backend.pdf` a partir de este Markdown, reutiliza el estilo de impresión del proyecto y mantiene sus archivos intermedios en una carpeta temporal.

# 12. Glosario

| Término | Significado |
|---|---|
| API | Interfaz que permite a otros programas solicitar operaciones mediante HTTP |
| Controller | Clase que recibe y responde a peticiones HTTP |
| Entidad | Objeto que representa datos persistidos |
| EF Core | ORM de .NET que mapea objetos a tablas y ejecuta consultas |
| Inyección de dependencias | Mecanismo que proporciona a una clase los componentes que necesita |
| JWT | Token firmado que transporta claims para autenticar peticiones |
| Migración | Descripción versionada de un cambio en el esquema de base de datos |
| Repository | Componente que agrupa operaciones de consulta y persistencia |
| Service | Componente que ofrece una operación o integra un proveedor externo |
| Utility | Función auxiliar compartida que no representa por sí sola un flujo HTTP |
