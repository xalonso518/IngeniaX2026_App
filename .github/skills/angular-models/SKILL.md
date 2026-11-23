---
name: angular-models
description: Create or update typed Angular/TypeScript models in app_netcoreangular.client/src/models while preserving the API contract and project conventions.
---

# Modelos Angular del proyecto

Usa esta skill cuando una tarea requiera crear, modificar o elegir un modelo de datos para el frontend. El alcance es `app_netcoreangular.client/src/models`; no cubre las clases de dominio del backend.

## Procedimiento

1. **Confirma el contrato y el uso.** Revisa el modelo más cercano en `src/models`, sus consumidores y, si representa una solicitud o respuesta HTTP, el DTO correspondiente del backend y el servicio que lo usa. Identifica si es un formulario de entrada, una vista de respuesta, un tipo compartido o un valor interno.
2. **Elige la forma más simple que exprese el dato.**
   - Usa `interface` para objetos de datos planos, especialmente solicitudes y respuestas de API.
   - Usa `type` para uniones, intersecciones, alias de primitivas o composición de tipos cuando haga el modelo más claro.
   - Usa `class` solo cuando se necesite una instancia en tiempo de ejecución, un constructor con valores iniciales o comportamiento propio. No conviertas una respuesta JSON a una clase solo para tiparla.
3. **Modela el contrato con precisión.** Conserva los nombres de propiedades y valores que intercambia la API. Distingue entre propiedad opcional (`propiedad?: T`) y propiedad presente que admite `null` (`propiedad: T | null`); no las trates como equivalentes. No declares una fecha como `Date` si el servicio entrega una cadena JSON y no la convierte explícitamente.
4. **Usa tipos seguros.** Evita `any`. Prefiere el tipo concreto; usa `unknown` en límites no confiables y estrecha el tipo antes de acceder a sus propiedades. Reutiliza modelos compartidos cuando la estructura y el significado sean realmente iguales.
5. **Ubica y nombra el modelo según el dominio.** Colócalo en la subcarpeta existente más cercana (`auth`, `producto`, `usuario`, `shared`, etc.) y sigue el estilo de nombres PascalCase de los archivos y tipos próximos. Separa solicitudes y vistas cuando sus formas o propósitos difieran; no crees abstracciones genéricas para una sola forma de datos.
6. **Actualiza los consumidores necesarios.** Busca usos del tipo y ajusta solo las referencias directamente afectadas. No cambies rutas, propiedades ni formas de la API para acomodar el modelo; resuelve discrepancias preservando el contrato, y explica cualquier transformación en la capa de servicio existente si es requerida.
7. **Valida.** Ejecuta `npm run build` desde `app_netcoreangular.client/`. Ejecuta también las pruebas enfocadas o `npm test` cuando el cambio altere comportamiento en tiempo de ejecución. Para una interfaz o alias exclusivamente estático, no agregues pruebas que solo repitan su declaración.

## Ejemplos

Objeto plano de respuesta: usa un tipo estructural y conserva la representación enviada por el servidor.

```ts
export interface ProductoSimpleView {
  productoId: number;
  nombre: string;
  precio: number;
  fechaCreacion: string;
}
```

Formulario con valores iniciales en tiempo de ejecución: una clase es apropiada si los consumidores instancian el formulario para obtener esos valores.

```ts
export class ProductoForm {
  nombre = "";
  precio = 0;
}
```

Evita usar `any` o cambiar la representación de la respuesta por conveniencia local:

```ts
// Evitar: pierde la comprobación de tipos y asume una conversión que quizá no existe.
export interface ProductoView {
  metadata: any;
  fechaCreacion: Date;
}
```
