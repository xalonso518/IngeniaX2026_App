---
name: angular-add-component
description: Create an Angular add-form component following the product-agregar flow, project conventions, and safe form/API handling.
---

# Crear componentes Angular de tipo agregar

Usa esta skill para agregar una entidad nueva desde un formulario Angular, tomando como referencia `src/components/producto/producto-agregar/`. Adapta el patrón al dominio solicitado; no copies nombres, campos ni errores del componente de producto.

## Procedimiento

1. **Investiga el flujo existente.** Revisa el componente de agregar más cercano, su plantilla y pruebas, junto con el modelo de formulario, el método `agregar` del servicio, la ruta de la funcionalidad y las constantes de navegación. Confirma los campos, tipos y respuesta esperada en el contrato de la API. Conserva controles de autenticación y roles existentes.
2. **Crea los archivos de la funcionalidad.** Sigue la organización `src/components/<dominio>/<dominio>-agregar/` con `*.component.ts`, `*.component.html`, `*.component.css` y `*.component.spec.ts`. Registra la ruta en el archivo `*.routes.ts` del dominio cuando corresponda. Usa estilos y utilidades de Bootstrap/Angular Material ya presentes; mantén el CSS local mínimo.
3. **Implementa un formulario tipado.**
   - Usa el modelo de solicitud existente o crea uno enfocado en `src/models`; no uses `any` ni cambies el contrato del backend.
   - Sigue el patrón de formularios cercanos. Si es un formulario dirigido por plantilla, importa `FormsModule`, enlaza campos con `[(ngModel)]` y asigna `name` e `id` a cada control.
   - Usa un tipo correcto para el formulario, como `NgForm`, no `FormControl` para representar un formulario completo. Comprueba su validez antes de llamar al servicio y marca controles como tocados si se intenta enviar un formulario inválido.
   - Mantén etiquetas asociadas a sus controles, validaciones compatibles con el contrato y mensajes de error mediante `MESSAGES_ERROR`. Deshabilita Guardar mientras el formulario sea inválido o el envío esté en curso; muestra el indicador de carga correspondiente.
4. **Conecta el servicio con estados completos.** Llama al método del servicio tipado; no hagas HTTP directamente desde el componente. Usa `LoadingService`, `HandleErrorService` y `MatSnackBar` según el flujo existente. Distingue respuesta exitosa, respuesta de negocio no exitosa y error de transporte. Limpia `submitted` y el indicador de carga en todas las salidas (por ejemplo, con `finalize` de RxJS), evitando duplicar la limpieza en `error` y `complete`. Navega tras el éxito solo si el flujo existente del dominio lo requiere y usa `ConstantsSpaRoutes`.
5. **Mantén la clase simple.** Usa una señal `submitted` para el estado de envío cuando siga el patrón del proyecto. No añadas un `ngOnInit` vacío, imports sin usar, lógica de validación duplicada ni `async` a un método que solo se suscribe a un `Observable`.
6. **Prueba los comportamientos importantes.** En el spec, usa `TestBed` y dependencias simuladas. Cubre al menos la creación/configuración del formulario y, según el alcance, el envío válido, la prevención del envío inválido y la reacción ante éxito o error. No dependas de la API real ni te limites a `should create` cuando haya lógica de envío.
7. **Valida.** Desde `app_netcoreangular.client/`, ejecuta `npm run build` y la prueba enfocada; ejecuta `npm test` cuando el cambio lo requiera. Sigue el runner configurado y no modifiques su configuración para resolver un problema ajeno a la funcionalidad.

## Estructura de referencia

El componente mantiene el modelo de formulario y el estado de envío; el servicio contiene la llamada HTTP:

```ts
submitted = signal(false);
form = new EntidadForm();

submit(formulario: NgForm): void {
  if (formulario.invalid || this.submitted()) {
    formulario.form.markAllAsTouched();
    return;
  }

  this.submitted.set(true);
  this.loadingService.show();

  this.entidadService.agregar(this.form)
    .pipe(finalize(() => {
      this.loadingService.hide();
      this.submitted.set(false);
    }))
    .subscribe({
      next: respuesta => {
        if (!respuesta.success) {
          this.handleError.showError(respuesta);
          return;
        }

        this.snackBar.open('Registro creado.', '', {
          verticalPosition: 'top',
          duration: 3000,
        });
      },
      error: error => this.handleError.showError(error),
    });
}
```

Importa `NgForm` y `finalize` desde sus módulos correctos y ajusta nombres, tipos y navegación a la funcionalidad real. La respuesta de negocio no exitosa se maneja dentro de `next`; `finalize` garantiza la limpieza tanto en éxito como en error.

Plantilla dirigida por plantilla:

```html
<form #agregarForm="ngForm" (ngSubmit)="submit(agregarForm)">
  <label for="nombre" class="form-label">Nombre *</label>
  <input
    id="nombre"
    name="nombre"
    class="form-control"
    [(ngModel)]="form.nombre"
    #nombre="ngModel"
    required
    maxlength="250"
  >

  @if (nombre.invalid && (nombre.dirty || nombre.touched)) {
    <span class="span-error">{{ errores.REQUIRED }}</span>
  }

  <button type="submit" [disabled]="agregarForm.invalid || submitted()">
    @if (submitted()) {
      <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
    } @else {
      <span class="material-symbols-rounded">save</span>
    }
    Guardar
  </button>
</form>
```

Usa en cada campo la validación y el mensaje adecuados; no muestres `REQUIRED` para cualquier clase de error. Añade atributos de accesibilidad y conserva el diseño adaptable de la pantalla.
