#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/../.." && pwd)"
SOURCE="$ROOT/docs/IngeniaX2026_Guia_Construccion_Backend.md"
STYLE="$ROOT/docs/build/style.css"
OUTPUT="$ROOT/docs/IngeniaX2026_Guia_Construccion_Backend.pdf"
PANDOC="${PANDOC:-pandoc}"
CHROME="${CHROME:-/Applications/Google Chrome.app/Contents/MacOS/Google Chrome}"

if ! command -v "$PANDOC" >/dev/null 2>&1; then
  echo "No se encontró Pandoc: $PANDOC" >&2
  exit 1
fi

if [[ ! -x "$CHROME" ]]; then
  echo "No se encontró Google Chrome ejecutable: $CHROME" >&2
  exit 1
fi

tmp_dir="$(mktemp -d)"
trap 'rm -rf "$tmp_dir"' EXIT

cp "$STYLE" "$tmp_dir/style.css"
"$PANDOC" "$SOURCE" \
  --output="$tmp_dir/guide.html" \
  --standalone \
  --css=style.css \
  --metadata lang=es \
  --metadata pagetitle="IngeniaX 2026 — Guía de construcción del backend" \
  --metadata author="Equipo de desarrollo IngeniaX 2026"

"$CHROME" \
  --headless=new \
  --disable-gpu \
  --no-sandbox \
  --no-pdf-header-footer \
  --run-all-compositor-stages-before-draw \
  --virtual-time-budget=20000 \
  --print-to-pdf="$tmp_dir/guide.pdf" \
  "file://$tmp_dir/guide.html" >/dev/null 2>&1

mv "$tmp_dir/guide.pdf" "$OUTPUT"
echo "PDF generado: $OUTPUT"
