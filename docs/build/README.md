# Generación de la documentación

Este directorio contiene el pipeline que convierte el documento Markdown en Word y PDF.

## Archivos

| Archivo | Propósito |
|---|---|
| `../IngeniaX2026_Descripcion_Producto.md` | Fuente única del documento, con diagramas Mermaid |
| `../IngeniaX2026_Guia_Construccion_Backend.md` | Guía paso a paso del backend |
| `../IngeniaX2026_Guia_Construccion_Frontend.md` | Guía paso a paso del frontend Angular |
| `build.py` | Extrae los diagramas y genera `build.md` con imágenes |
| `style.css` | Estilo de impresión para el PDF |
| `fix_docx.py` | Ajusta el DOCX a A4, márgenes y metadatos |
| `build.sh` | Ejecuta todo el pipeline |
| `build_backend_guide.sh` | Genera el PDF de la guía del backend |
| `build_frontend_guide.sh` | Genera el PDF de la guía del frontend |
| `diagrams/` | Diagramas `.mmd` y `.png` generados |
| `build.md` | Markdown intermedio con referencias a las imágenes |
| `build.html` | HTML intermedio para la impresión a PDF |

## Requisitos

- Python 3
- Pandoc 2.12 o superior
- Google Chrome
- [mermaid-cli](https://github.com/mermaid-js/mermaid-cli) (`mmdc`)

Instalación de Mermaid CLI:

```bash
npm install -g @mermaid-js/mermaid-cli
```

El archivo `puppeteer.json` apunta a Google Chrome en macOS. En otro sistema, ajusta `executablePath` o elimina esa clave para usar el navegador que descarga Puppeteer.

## Uso

```bash
cd docs/build
./build.sh
```

Si `mmdc` no está en el `PATH`:

```bash
MMDC=/ruta/a/mmdc ./build.sh
```

Las guías de backend y frontend se generan de manera independiente:

```bash
./build_backend_guide.sh
./build_frontend_guide.sh
```

Cada script utiliza un directorio temporal para sus archivos intermedios y no modifica los diagramas del documento de producto.

## Resultado

```text
docs/
├── IngeniaX2026_Descripcion_Producto.{md,docx,pdf}
├── IngeniaX2026_Guia_Construccion_Backend.{md,pdf}
└── IngeniaX2026_Guia_Construccion_Frontend.{md,pdf}
```

El Markdown conserva los bloques Mermaid para renderizarse en GitHub o VS Code. Word y PDF usan las imágenes PNG generadas a partir de esos bloques.
