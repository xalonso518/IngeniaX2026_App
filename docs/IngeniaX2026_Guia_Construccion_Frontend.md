# IngeniaX 2026

## Guía paso a paso para construir el frontend

### Angular, componentes, rutas y servicios

| Dato | Descripción |
|---|---|
| Propósito | Aprender a crear y conectar las piezas principales de la aplicación Angular |
| Audiencia | Estudiantes y desarrolladores principiantes |
| Alcance | `app_netcoreangular.client` |
| Fuente | Código y configuración actuales del repositorio |

> **Nota sobre el historial:** esta guía propone una secuencia didáctica basada en el frontend que existe hoy. No afirma que los archivos se hayan creado históricamente en ese orden. Los comandos de Angular CLI de esta guía se comprobaron en modo `--dry-run` para generar archivos en las carpetas y con los sufijos usados por el proyecto.

---

# Contenido

1. [Qué construye el frontend](#1-qué-construye-el-frontend)
2. [Preparar el entorno](#2-preparar-el-entorno)
3. [Arranque y estructura de Angular](#3-arranque-y-estructura-de-angular)
4. [Crear un componente](#4-crear-un-componente)
5. [Crear un service](#5-crear-un-service)
6. [Crear modelos y respetar los contratos de la API](#6-crear-modelos-y-respetar-los-contratos-de-la-api)
7. [Configurar rutas y navegación](#7-configurar-rutas-y-navegación)
8. [Organizar containers, componentes compartidos y utils](#8-organizar-containers-componentes-compartidos-y-utils)
9. [Seguir dos flujos completos](#9-seguir-dos-flujos-completos)
10. [Estilos, formularios y estados de pantalla](#10-estilos-formularios-y-estados-de-pantalla)
11. [Ejecutar, compilar y probar](#11-ejecutar-compilar-y-probar)
12. [Generar el PDF y glosario](#12-generar-el-pdf-y-glosario)

# 1. Qué construye el frontend

El frontend es la aplicación web que presenta pantallas, recoge acciones de las personas usuarias y se comunica con la API. IngeniaX utiliza Angular 22, TypeScript, Angular Material, Bootstrap y RxJS. El cliente y el backend ASP.NET Core comparten contratos de datos y rutas HTTP; esta guía se concentra en la aplicación Angular.

Una operación de pantalla suele conectar varias piezas:

```text
Ruta Angular
    ↓
Componente de pantalla ─── modelo tipado para entrada/salida
    ↓
Servicio de funcionalidad
    ↓
ApiClientService ── ConstantsApiRoutes ── HTTP ── API ASP.NET Core
    ↑
estado de carga, errores y datos presentados en la plantilla
```

Separar estas piezas hace que los componentes se enfoquen en la interacción y la presentación, mientras que los servicios concentran la comunicación y los modelos describen los datos.

# 2. Preparar el entorno

Se necesita Git, Node.js compatible con Angular 22, npm y las dependencias del proyecto. `package.json` identifica Angular CLI y el gestor npm usado por el repositorio.

Para obtener e iniciar el proyecto:

```bash
git clone https://github.com/xalonso518/IngeniaX2026_App.git
cd IngeniaX2026_App/app_netcoreangular.client
npm install
npm start
```

El script `start` prepara certificados de desarrollo ASP.NET y ejecuta Angular con HTTPS. El proyecto también incluye proxy y configuración de SPA para su integración con el servidor .NET.

# 3. Arranque y estructura de Angular

## 3.1 Arranque de la aplicación

`src/main.ts` inicia `AppModule`. Ese módulo importa `AppRoutingModule`, registra servicios compartidos y declara el componente raíz. `angular.json` configura el proyecto, compilación, estilos globales, recursos y servidor local.

El frontend combina un módulo raíz tradicional con componentes modernos. `App` y `SingleLayoutComponent` se declaran en `AppModule`; las pantallas de funcionalidades se cargan desde archivos de rutas y usan sus imports de plantilla en el propio componente. Al crear un componente nuevo, confirma su modo de uso y no lo declares además en un módulo si se generó como standalone.

## 3.2 Mapa de carpetas

| Carpeta | Responsabilidad |
|---|---|
| `src/app/` | Componente raíz y configuración global de rutas |
| `src/components/` | Funcionalidades por dominio y componentes compartidos |
| `src/containers/` | Layouts, barra de navegación y menú lateral |
| `src/models/` | Formularios y contratos TypeScript de solicitudes/respuestas |
| `src/utils/constants/` | Rutas SPA/API, mensajes y catálogos |
| `src/utils/services/` | Cliente HTTP, sesión, guards, configuración y manejo de errores |
| `src/utils/material/` | Agrupaciones de módulos Angular Material |
| `public/` | Recursos estáticos |

Las funcionalidades se agrupan por dominio: autenticación, usuarios, productos, pagos y notificaciones. Cada dominio puede contener componentes, un archivo de rutas, servicios y modelos relacionados.

# 4. Crear un componente

Un componente controla una parte de la interfaz. Por convención, el proyecto separa la clase TypeScript, la plantilla HTML, los estilos CSS y una prueba `spec.ts`.

Desde `app_netcoreangular.client/`, se puede crear una pantalla nueva de producto así:

```bash
npm run ng -- generate component producto-detalle \
  --path src/components/producto \
  --project app_netcoreangular.client \
  --type component \
  --standalone
```

El comando genera una carpeta `producto-detalle/` con:

- `producto-detalle.component.ts`: metadatos y lógica de interacción.
- `producto-detalle.component.html`: estructura visual.
- `producto-detalle.component.css`: estilos locales.
- `producto-detalle.component.spec.ts`: prueba inicial del componente.

`--path` coloca los archivos en la estructura personalizada de este repositorio. `--type component` conserva el sufijo `.component` y `--standalone` permite utilizar el componente directamente desde las rutas y declarar los imports que necesita. No uses `--skip-tests` si quieres mantener la prueba generada.

El componente debe manejar estado de vista (por ejemplo, señales para carga y resultados), recibir o presentar datos y delegar las llamadas HTTP al servicio. Para la prueba inicial, verifica que el componente se cree; después, agrega casos sobre el comportamiento que implementes.

# 5. Crear un service

Un service encapsula lógica que puede reutilizarse, como consultar la API. Para crear el servicio de detalle en el dominio de productos:

```bash
npm run ng -- generate service producto-detalle \
  --path src/components/producto/services \
  --project app_netcoreangular.client \
  --type service
```

El comando genera `producto-detalle.service.ts` y `producto-detalle.service.spec.ts`. El sufijo `--type service` conserva la convención del repositorio.

Un servicio de funcionalidad debe:

1. Recibir `ApiClientService` por inyección de dependencias.
2. Usar `ConstantsApiRoutes` para seleccionar el endpoint del backend.
3. Aceptar modelos tipados como argumentos y devolver `Observable<ApiResponse<T>>`.
4. Dejar que el componente gestione la suscripción y los estados de presentación; centralizar el manejo común de errores con los servicios existentes.

Ejemplo basado en el patrón de `ProductoService`:

```typescript
@Injectable()
export class ProductoDetalleService {
  constructor(private apiClient: ApiClientService) {}

  obtener(id: string): Observable<ApiResponse<ProductoFormEditar>> {
    return this.apiClient.apiGet<ProductoFormEditar>(
      ConstantsApiRoutes.PRODUCTO_OBTENER + '/' + id
    );
  }
}
```

El servicio generado también necesita el decorador `@Injectable`. Decide su alcance de inyección según su uso: los servicios de funcionalidades existentes, como `ProductoService`, se registran en el componente que los usa; servicios compartidos, como `ApiClientService` o `SessionService`, se registran en `AppModule`. Si el generador creó `providedIn: 'root'`, no agregues además una provisión local sin una razón.

# 6. Crear modelos y respetar los contratos de la API

Los modelos describen la forma de los datos y dan comprobación estática al compilar:

- `models/producto/ProductoForm.ts` define tipos de entrada para agregar o editar.
- `models/producto/ProductoSimpleView.ts` representa datos de productos en listados.
- `models/auth/LoginForm.ts` y `TokenResponse.ts` describen el inicio de sesión.
- `models/shared/ApiResponse.ts` define el contenedor común de respuesta.

Antes de crear otro modelo, busca si ya hay uno para esa solicitud o respuesta. Cuando haga falta, ubícalo en el dominio correspondiente, conserva los nombres y tipos compatibles con el backend y evita `any`.

Las rutas de la aplicación (`ConstantsSpaRoutes`) no son las rutas HTTP de la API. Para una petición, usa las constantes de `ConstantsApiRoutes`; no escribas una URL distinta directamente dentro del componente.

# 7. Configurar rutas y navegación

El enrutamiento raíz de `app-routing-module.ts` separa la autenticación del layout principal y carga funcionalidades bajo demanda:

- `auth.routes.ts` registra inicio de sesión y restablecimiento.
- `producto.routes.ts` declara las pantallas de búsqueda, alta y edición.
- `usuario.routes.ts` y `pago.routes.ts` siguen el mismo patrón.

Para conectar la pantalla nueva, importa su componente y añade una ruta en `producto.routes.ts`, por ejemplo:

```typescript
{ path: 'detalle/:id', component: ProductoDetalleComponent }
```

El parámetro `:id` se obtiene con `ActivatedRoute`. La ruta de funcionalidad ya está conectada al árbol raíz mediante `loadChildren`; por eso, normalmente basta actualizar su archivo `*.routes.ts`.

Usa `ConstantsSpaRoutes` al navegar desde TypeScript para evitar duplicar rutas. Revisa cómo se aplican los guards de sesión o rol en la ruta concreta antes de afirmar que está protegida: que exista un guard en `utils` no significa que todas las rutas lo utilicen.

# 8. Organizar containers, componentes compartidos y utils

## 8.1 Containers y navegación global

Los containers dibujan la estructura que rodea a las pantallas:

- `SingleLayoutComponent` da el marco para las pantallas de autenticación.
- `MainLayoutComponent` agrupa barra superior, menú lateral, indicador de carga y `RouterOutlet` para la aplicación autenticada.
- `MainNavBarComponent` y `MainSideBarComponent` ofrecen navegación y acciones globales.

Una pantalla completa debe colocarse bajo el layout adecuado; un control reutilizable no debería duplicar toda la navegación.

## 8.2 Componentes compartidos

`components/shared/` contiene piezas visuales reutilizables: campos de archivo, errores de entrada, indicadores de carga, confirmación y mensajes sin resultados. Antes de crear un componente visual nuevo, verifica si uno existente resuelve el mismo caso.

## 8.3 Servicios y constantes compartidas

En `utils/services/` se encuentran responsabilidades que atraviesan dominios:

- `ApiClientService` adjunta el token cuando corresponde y ofrece métodos HTTP tipados.
- `ConfigService` obtiene la URL base de la API.
- `SessionService` administra el estado de sesión y la identidad en el cliente.
- `AuthGuard` y `RoleGuard` contienen reglas de acceso reutilizables.
- `HandleErrorService`, `ErrorValidationService` y `LoadingService` unifican estados y mensajes.
- `DownloadService` y `FileUtilitiesService` apoyan el tratamiento de archivos.

`ConstantsMessages`, `ConstantsCatalogos`, `ConstantsApiRoutes` y `ConstantsSpaRoutes` evitan repetir valores entre pantallas.

# 9. Seguir dos flujos completos

## 9.1 Inicio de sesión

```text
/auth/login
  → LoginComponent recoge LoginForm
  → AuthService llama a ApiClientService
  → ConstantsApiRoutes.LOGIN identifica el endpoint público
  → ApiResponse<TokenResponse> devuelve el token
  → SessionService establece la sesión y redirige
```

El login es público, así que `AuthService` utiliza el método `apiPostSimple`, que no adjunta un token previo. La guía no reproduce credenciales de desarrollo ni valores de tokens.

## 9.2 Búsqueda de productos

```text
/producto
  → ProductoBuscarComponent presenta filtro, orden y paginador
  → ProductoService.buscar crea HttpParams
  → ApiClientService.apiGetParam envía la solicitud autenticada
  → ApiResponse<ProductoSimpleView[]> entrega resultados y paginación
  → señales actualizan la plantilla y sus estados
```

Al revisar el flujo en código, compara la ruta de pantalla (`/producto`) con la ruta de API (`/api/producto/buscar`), sigue el tipo de respuesta y confirma que la plantilla muestre resultados, carga, error y estado vacío.

# 10. Estilos, formularios y estados de pantalla

Los componentes siguen el formato de plantilla HTML y hoja CSS externa. `angular.json` carga el tema de Angular Material, Bootstrap y estilos globales; los estilos específicos se mantienen en el componente.

En las pantallas existentes se usan formularios de Angular, `signal` para algunos estados y RxJS `Observable` para operaciones HTTP. Al agregar una operación, muestra y retira el estado de carga tanto en éxito como en error, presenta errores con `HandleErrorService`, conserva etiquetas accesibles y verifica el diseño en distintos tamaños.

# 11. Ejecutar, compilar y probar

Desde `app_netcoreangular.client/`:

```bash
npm start
npm run build
npm test
```

`npm run build` compila el frontend. En la configuración revisada, el build terminó correctamente, aunque produjo una advertencia por el tamaño inicial del bundle y otra por un `DatePipe` no usado.

**Estado actual de pruebas:** `npm test` no alcanza a ejecutar las pruebas porque Angular rechaza la propiedad `karmaConfig` que aparece en `angular.json` para el builder `@angular/build:unit-test`. Aunque existe un `karma.conf.js` basado en Jasmine/Karma, la configuración requiere corregirse para que las pruebas corran. Esta limitación pertenece a la configuración existente; no se modifica como parte de esta guía.

El README del cliente y sus dependencias pueden describir otro runner. Para una nueva guía, sigue la configuración efectiva de `angular.json`, no supongas que una prueba se ejecutó solo porque existe su archivo `spec.ts`.

# 12. Generar el PDF y glosario

Desde la raíz del repositorio, genera el PDF con Pandoc y Google Chrome:

```bash
./docs/build/build_frontend_guide.sh
```

El comando produce `docs/IngeniaX2026_Guia_Construccion_Frontend.pdf`, usa el estilo de impresión compartido y mantiene HTML temporal fuera de las salidas de las otras guías.

| Término | Significado |
|---|---|
| Componente | Clase, plantilla y estilos que implementan una parte de la interfaz |
| Container | Componente que organiza el layout y la navegación de varias pantallas |
| Guard | Regla que puede permitir o impedir la activación de una ruta |
| Modelo | Tipo TypeScript que representa datos de entrada o salida |
| Ruta | Dirección del cliente SPA o endpoint HTTP, según el contexto |
| Service | Clase inyectable que agrupa operaciones reutilizables o llamadas HTTP |
| Standalone | Componente que declara sus propios imports y no requiere declaración en un `NgModule` |
