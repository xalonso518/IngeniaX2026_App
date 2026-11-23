#!/usr/bin/env python3
"""Ajustes finales del DOCX: tamaño A4, márgenes y metadatos."""
import os
import sys
import zipfile

SECTPR = (
    "<w:sectPr>"
    '<w:pgSz w:w="11906" w:h="16838" />'
    '<w:pgMar w:top="1134" w:right="1134" w:bottom="1134" w:left="1134" '
    'w:header="709" w:footer="709" w:gutter="0" />'
    "</w:sectPr>"
)

CORE = {
    "<dc:title></dc:title>": "<dc:title>IngeniaX 2026 — Descripción del producto</dc:title>",
    "<dc:creator></dc:creator>": "<dc:creator>Equipo de desarrollo IngeniaX 2026</dc:creator>",
    "<dc:subject></dc:subject>": "<dc:subject>Descripción funcional, técnica y de alcance</dc:subject>",
    "<dc:description></dc:description>": "<dc:description>Plataforma Empresarial de Gestión Comercial</dc:description>",
    "<cp:keywords></cp:keywords>": "<cp:keywords>IngeniaX, Angular, ASP.NET Core, arquitectura, módulos, alcance</cp:keywords>",
}


def main(path: str) -> None:
    tmp = path + ".tmp"
    with zipfile.ZipFile(path) as source, zipfile.ZipFile(
        tmp, "w", zipfile.ZIP_DEFLATED
    ) as target:
        for item in source.infolist():
            data = source.read(item.filename)
            if item.filename == "word/document.xml":
                xml = data.decode("utf-8")
                for variant in ("<w:sectPr />", "<w:sectPr/>"):
                    xml = xml.replace(variant, SECTPR)
                data = xml.encode("utf-8")
            elif item.filename == "docProps/core.xml":
                xml = data.decode("utf-8")
                for old, new in CORE.items():
                    xml = xml.replace(old, new)
                data = xml.encode("utf-8")
            target.writestr(item, data)
    os.replace(tmp, path)


if __name__ == "__main__":
    if len(sys.argv) != 2:
        sys.exit("Uso: fix_docx.py <archivo.docx>")
    main(sys.argv[1])
