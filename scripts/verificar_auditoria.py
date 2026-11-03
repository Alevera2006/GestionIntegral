import re
from pathlib import Path

INIT_SQL = Path("src/GestionIntegralDB/ScriptsInitial/Init.sql")

# Campos mínimos de EntityBase (obligatorios en todas las tablas)
campos_obligatorios = {
    "IdUsuarioCreacion",
    "FechaCreacion",
    "IdUsuarioUltimaModificacion",
    "FechaUltimaModificacion",
}

# Campos deseables para entidades que soportan baja lógica
campos_baja_logica = {
    "Activo",
    "FechaBaja",
}

text = INIT_SQL.read_text(encoding="utf-8")

# Extraer bloques CREATE TABLE
pattern = re.compile(r"CREATE\s+TABLE\s+(?:dbo\.)?(\w+)\s*\((.*?)\);", re.DOTALL | re.IGNORECASE)

tablas = {}
for match in pattern.finditer(text):
    nombre = match.group(1)
    cuerpo = match.group(2)

    # Extraer nombres de columnas (líneas que empiezan con nombre de columna)
    columnas = set()
    for line in cuerpo.splitlines():
        line = line.strip()
        if not line or line.startswith("CONSTRAINT") or line.startswith("PRIMARY") or line.startswith("FOREIGN") or line.startswith("UNIQUE") or line.startswith("CHECK") or line.startswith("REFERENCES") or line.startswith("ON") or line.startswith(")"):
            continue
        # nombre de columna: palabra o [palabra]
        m = re.match(r"\[?(\w+)\]?", line)
        if m:
            columnas.add(m.group(1))

    tablas[nombre] = columnas

print(f"Total tablas analizadas: {len(tablas)}\n")

obligatorios_faltantes = {}
baja_faltantes = {}

for tabla, columnas in sorted(tablas.items()):
    faltan_obligatorios = campos_obligatorios - columnas
    if faltan_obligatorios:
        obligatorios_faltantes[tabla] = faltan_obligatorios

    faltan_baja = campos_baja_logica - columnas
    if faltan_baja:
        baja_faltantes[tabla] = faltan_baja

if obligatorios_faltantes:
    print("Tablas con campos de auditoría OBLIGATORIOS faltantes (EntityBase):")
    for tabla, faltan in obligatorios_faltantes.items():
        print(f"  - {tabla}: {', '.join(sorted(faltan))}")
else:
    print("Todas las tablas tienen los campos de auditoría obligatorios.")

print()

if baja_faltantes:
    print("Tablas sin campos de baja lógica (Activo/FechaBaja):")
    for tabla, faltan in baja_faltantes.items():
        print(f"  - {tabla}: {', '.join(sorted(faltan))}")
else:
    print("Todas las tablas tienen campos de baja lógica.")
