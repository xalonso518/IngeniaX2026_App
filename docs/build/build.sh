#!/usr/bin/env bash
# Genera Word y PDF a partir del Markdown del producto.
# Requisitos: python3, pandoc, Google Chrome y mermaid-cli (mmdc).
# Uso: ./build.sh
#   MMDC=/ruta/a/mmdc ./build.sh   (si mmdc no está en el PATH)
set -euo pipefail

cd "$(dirname "$0")"

SOURCE="../IngeniaX2026_Descripcion_Producto.md"
DOCX="../IngeniaX2026_Descripcion_Producto.docx"
PDF="../IngeniaX2026_Descripcion_Producto.pdf"
CHROME="/Applications/Google Chrome.app/Contents/MacOS/Google Chrome"

if [[ ! -f "$SOURCE" ]]; then
  echo "No se encontró el documento fuente: $SOURCE" >&2
  exit 1
fi

python3 build.py --render --mmdc "${MMDC:-mmdc}"

# Word
pandoc build.md \
  -o "$DOCX" \
  --resource-path=. \
  --metadata lang=es

python3 fix_docx.py "$DOCX"

# HTML intermedio para PDF
pandoc build.md \
  -o build.html \
  --standalone \
  --css=style.css \
  --resource-path=. \
  --metadata pagetitle="IngeniaX 2026 — Descripción del producto" \
  --metadata author="Equipo de desarrollo IngeniaX 2026" \
  --metadata keywords="IngeniaX, Angular, ASP.NET Core, arquitectura, módulos, alcance"

if [[ ! -x "$CHROME" ]]; then
  echo "No se encontró Google Chrome en: $CHROME" >&2
  exit 1
fi

# PDF con Chrome headless
"$CHROME" \
  --headless=new \
  --disable-gpu \
  --no-sandbox \
  --no-pdf-header-footer \
  --run-all-compositor-stages-before-draw \
  --virtual-time-budget=20000 \
  --print-to-pdf="$PWD/$PDF" \
  "file://$PWD/build.html" >/dev/null 2>&1

echo "Generados:"
echo "  $DOCX"
echo "  $PDF"
