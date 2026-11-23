# Instrucciones del frontend

Estas pautas complementan el `AGENTS.md` de la raíz y aplican a la aplicación Angular 22 de este directorio.

## Estructura

- `src/app/`: módulo y enrutamiento raíz de la aplicación.
- `src/components/`: funcionalidades agrupadas por dominio (`auth`, `usuario`, `producto`, `pago`) y componentes compartidos.
- `src/containers/`: diseños y navegación global de la aplicación.
- `src/models/`: tipos de datos y contratos de API.
- `src/utils/`: servicios compartidos, guards, constantes y módulos de Angular Material.
- `public/`: recursos estáticos.

Las rutas de funcionalidades se cargan de forma diferida desde sus archivos `*.routes.ts`. Conserva esa organización y las convenciones de nombres `*.component.ts`, `*.component.html`, `*.component.css` y `*.spec.ts`.

## Desarrollo

- Mantén los componentes enfocados en presentación e interacción; encapsula llamadas HTTP en servicios de la funcionalidad.
- Usa `ApiClientService` para comunicarte con la API, `ConstantsApiRoutes` para sus rutas y los modelos existentes para tipar solicitudes y respuestas. Conserva los contratos que comparte el backend.
- Sigue los patrones existentes de Angular, TypeScript y RxJS; utiliza tipos concretos en lugar de `any` y conserva el estilo de estado del código cercano (incluidas las señales donde ya se usan).
- Para navegación y sesión, reutiliza las constantes de `ConstantsSpaRoutes`, `AuthGuard`, `SessionService` y los guards existentes. No omitas controles de autenticación o roles.
- Muestra errores mediante los servicios compartidos existentes. Mantén validaciones y estados de carga coherentes con las pantallas vecinas.
- Conserva Angular Material, Bootstrap y los estilos propios por componente. Mantén la interfaz en español, adaptable y accesible.
- Evita incluir credenciales o secretos en el código. No agregues dependencias sin necesidad.

## Comandos

Ejecuta estos comandos desde `app_netcoreangular.client/`:

- Iniciar el servidor local: `npm start`
- Compilar: `npm run build`
- Ejecutar pruebas: `npm test`

Las pruebas existentes usan Jasmine/Karma y requieren Chrome. La URL base de la API se configura en `src/utils/services/config.service.ts`.
