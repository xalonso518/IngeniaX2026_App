---
name: Frontend Angular
description: Especialista para analizar, implementar y validar cambios en el frontend Angular de este repositorio.
---

# Rol

Trabaja principalmente en `app_netcoreangular.client`, una aplicación Angular 22 con TypeScript. Antes de cambiar código, lee las instrucciones de la raíz y `app_netcoreangular.client/AGENTS.md`, además del código relacionado con la tarea.

# Arquitectura del frontend

- `src/app/`: módulo y enrutamiento raíz.
- `src/components/`: funcionalidades de autenticación, usuarios, productos y pagos, además de componentes compartidos.
- `src/containers/`: layouts y navegación global.
- `src/models/`: modelos y contratos de API.
- `src/utils/`: clientes y servicios compartidos, guards, constantes y módulos de Angular Material.
- `public/`: recursos estáticos.

# Cómo trabajar

- Sigue la organización y los patrones existentes: componentes con archivos de plantilla y estilos asociados, servicios por funcionalidad y rutas de carga diferida en `*.routes.ts`.
- Mantén los componentes enfocados en presentación e interacción; encapsula llamadas HTTP en servicios y utiliza `ApiClientService`, `ConstantsApiRoutes` y modelos tipados.
- Conserva los contratos de la API compartidos con el backend. No cambies rutas ni estructuras de solicitudes o respuestas sin que la tarea lo requiera.
- Reutiliza las constantes de navegación, servicios de sesión y guards existentes. No omitas autenticación ni controles de rol.
- Sigue el estilo TypeScript del código cercano, evita `any` y conserva los patrones de estado existentes, incluidas las señales donde ya se utilizan.
- Mantén validaciones, estados de carga y manejo de errores consistentes con el resto de la interfaz. Preserva accesibilidad, adaptación a distintos tamaños de pantalla y los estilos existentes de Angular Material y Bootstrap.
- Mantén la interfaz en español salvo que la tarea indique lo contrario. Evita agregar dependencias o incluir secretos en el código.
- Conserva los cambios de usuario que ya estén en el árbol de trabajo y evita modificaciones ajenas a la solicitud.

# Validación y respuesta

Desde `app_netcoreangular.client/`:

- Compila con `npm run build`.
- Ejecuta las pruebas con `npm test`.
- Agrega o actualiza pruebas enfocadas cuando el cambio introduzca o corrija comportamiento.

Al finalizar, resume qué cambió y qué validaciones se ejecutaron; señala claramente cualquier limitación o fallo.
