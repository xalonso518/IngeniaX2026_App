#!/usr/bin/env python3
"""Prepara el Markdown para Word/PDF: extrae los diagramas Mermaid,
los renderiza a PNG y genera un Markdown de construcción con imágenes.
"""
import argparse
import os
import re
import shutil
import subprocess
import sys

ROOT = os.path.dirname(os.path.abspath(__file__))
SOURCE = os.path.join(ROOT, "..", "IngeniaX2026_Descripcion_Producto.md")
BUILD_MD = os.path.join(ROOT, "build.md")
DIAGRAMS = os.path.join(ROOT, "diagrams")

MERMAID_RE = re.compile(r"```mermaid\n(.*?)```", re.DOTALL)


def extract(source: str):
    os.makedirs(DIAGRAMS, exist_ok=True)
    # Limpiar diagramas de ejecuciones anteriores.
    for name in os.listdir(DIAGRAMS):
        if name.endswith((".mmd", ".png")):
            os.remove(os.path.join(DIAGRAMS, name))
    counter = 0

    def replace(match):
        nonlocal counter
        counter += 1
        name = f"diagram-{counter:02d}"
        mmd_path = os.path.join(DIAGRAMS, name + ".mmd")
        with open(mmd_path, "w", encoding="utf-8") as handle:
            handle.write(match.group(1).strip() + "\n")
        return f"![Diagrama {counter}](diagrams/{name}.png){{width=15cm}}"

    rendered = MERMAID_RE.sub(replace, source)
    # Mantener juntos los subtítulos y su tabla inmediata.
    rendered = re.sub(
        r"(^### .+\n\n(?:\|.+\|\n)+)",
        lambda m: (
            m.group(1)
            if m.group(1).startswith("### Documento de descripción")
            else "::: keep\n" + m.group(1).rstrip() + "\n:::\n"
        ),
        rendered,
        flags=re.MULTILINE,
    )
    # Etiquetas en negrita seguidas de una tabla (p. ej. parámetros de búsqueda).
    rendered = re.sub(
        r"(^\*\*.+\*\*\n\n(?:\|.+\|\n)+)",
        lambda m: "::: keep\n" + m.group(1).rstrip() + "\n:::\n",
        rendered,
        flags=re.MULTILINE,
    )
    # Salto de página después de la portada para HTML/PDF.
    rendered = rendered.replace(
        "---\n\n## Control del documento",
        "---\n\n::: pagebreak\n:::\n\n## Control del documento",
        1,
    )
    # El índice comienza en una página propia.
    rendered = rendered.replace(
        "---\n\n## Contenido",
        "---\n\n::: pagebreak\n:::\n\n## Contenido",
        1,
    )
    # Saltos de página reales para Word (se ignoran en HTML/PDF).
    page_break = (
        "```{=openxml}\n"
        '<w:p><w:r><w:br w:type="page" /></w:r></w:p>\n'
        "```\n\n"
    )
    rendered = rendered.replace("::: pagebreak\n:::", page_break + "::: pagebreak\n:::")
    rendered = re.sub(
        r"(?m)^(# \d+\. )",
        lambda m: page_break + m.group(1),
        rendered,
    )
    with open(BUILD_MD, "w", encoding="utf-8") as handle:
        handle.write(rendered)
    return counter


def render(mmdc: str):
    diagrams = sorted(f for f in os.listdir(DIAGRAMS) if f.endswith(".mmd"))
    puppeteer = os.path.join(ROOT, "puppeteer.json")
    for name in diagrams:
        src = os.path.join(DIAGRAMS, name)
        out = os.path.join(DIAGRAMS, name.replace(".mmd", ".png"))
        cmd = [
            mmdc,
            "-i", src,
            "-o", out,
            "-b", "white",
            "--size", "2000",
            "-s", "2",
            "-p", puppeteer,
        ]
        print(f"Renderizando {name}...")
        subprocess.run(cmd, check=True, stdout=subprocess.DEVNULL)
    return len(diagrams)


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--render", action="store_true", help="Renderizar diagramas con mmdc")
    parser.add_argument("--mmdc", default=os.environ.get("MMDC", "mmdc"))
    args = parser.parse_args()

    with open(SOURCE, encoding="utf-8") as handle:
        source = handle.read()

    total = extract(source)
    print(f"Diagramas extraídos: {total}")

    if args.render:
        mmdc = shutil.which(args.mmdc) or args.mmdc
        if not os.path.exists(mmdc):
            sys.exit(f"No se encontró mmdc en: {mmdc}")
        rendered = render(mmdc)
        print(f"Diagramas renderizados: {rendered}")


if __name__ == "__main__":
    main()
