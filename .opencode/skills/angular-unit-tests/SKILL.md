---
name: angular-unit-tests
description: Write small, focused Angular unit tests that verify component or service behavior using this repository's test setup.
---

# Pruebas unitarias sencillas para Angular

Usa esta skill al crear o actualizar pruebas unitarias en `app_netcoreangular.client`. Prioriza pruebas pequeñas, deterministas y centradas en el comportamiento observable.

## Procedimiento

1. **Revisa el código bajo prueba y su spec cercano.** Identifica el comportamiento público, las dependencias y el patrón de configuración de `TestBed` que usa esa funcionalidad.
2. **Confirma el runner antes de usar APIs específicas.** Revisa `package.json`, `angular.json`, `karma.conf.js` y las pruebas existentes. En este repositorio hay configuración y tipos de más de un runner; no asumas que una API de Jasmine o Vitest funciona sin confirmarlo. Ejecuta las pruebas con `npm test` desde `app_netcoreangular.client/`.
3. **Prueba un resultado por caso.** Nombra cada prueba por el comportamiento que verifica. Prefiere verificar el DOM, una salida, un cambio de estado o el resultado de una llamada frente a inspeccionar detalles privados. No te limites a `should create` cuando haya comportamiento real que probar.
4. **Configura solo lo necesario.**
   - Para componentes, usa `TestBed` y `ComponentFixture`. Importa o declara el componente según su configuración real; llama a `fixture.detectChanges()` cuando se requiera renderizarlo y espera estabilidad solo si hay trabajo asíncrono.
   - Para servicios inyectables, usa `TestBed.inject`. Sustituye dependencias externas solo cuando el caso lo necesite.
   - Para HTTP, usa `HttpTestingController` y el mecanismo de pruebas HTTP que ya siga el proyecto. Simula las respuestas; no dependas de la API real y verifica que no queden solicitudes pendientes.
5. **Mantén las pruebas aisladas y legibles.** Usa datos de prueba mínimos y explícitos. Evita temporizadores, red, estado compartido entre casos y mocks de toda la aplicación. Añade casos de error o límite solo cuando sean relevantes para el comportamiento cambiado.
6. **Valida la prueba.** Ejecuta primero el spec enfocado si el runner lo permite; después usa `npm test` según el alcance. Si el comando falla por configuración o entorno, informa la causa en vez de cambiar la configuración de pruebas como parte de una tarea no relacionada.

## Ejemplo: prueba pequeña de un componente

Este ejemplo sigue el patrón de los componentes importables que ya tienen spec propio. Comprueba que se muestra el indicador, no solo que se pudo construir la instancia.

```ts
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoadingComponent } from './loading.component';

describe('LoadingComponent', () => {
  let fixture: ComponentFixture<LoadingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoadingComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(LoadingComponent);
    fixture.detectChanges();
  });

  it('muestra el indicador de carga', () => {
    expect(
      fixture.nativeElement.querySelector('.spinner_loading'),
    ).not.toBeNull();
  });
});
```

## Evita

- Agregar pruebas que solo repiten que un componente, servicio o propiedad existe cuando la tarea requiere validar comportamiento.
- Probar detalles privados o acoplar la prueba a la implementación interna si puede verificarse el resultado público.
- Usar `any`, acceder a servicios reales, o introducir dependencias nuevas para probar una funcionalidad sencilla.
- Cambiar la configuración global del runner o snapshots extensos para resolver un caso que puede cubrirse con una expectativa directa.
