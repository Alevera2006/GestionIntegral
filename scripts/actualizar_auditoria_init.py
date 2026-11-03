import re
from pathlib import Path

INIT_SQL = Path("src/GestionIntegralDB/ScriptsInitial/Init.sql")
text = INIT_SQL.read_text(encoding="utf-8")

# Definir reemplazos exactos (search -> replacement)
reemplazos = [
    # Contactos
    (
        """    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_Contactos""",
        """    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_Contactos"""
    ),
    # Stock
    (
        """    FechaUltimoMovimiento DATETIME2 NULL,

    CONSTRAINT PK_Stock""",
        """    FechaUltimoMovimiento DATETIME2 NULL,
    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_Stock"""
    ),
    # MovimientosStock
    (
        """    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL
        CONSTRAINT DF_MovimientosStock_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_MovimientosStock""",
        """    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL
        CONSTRAINT DF_MovimientosStock_FechaCreacion
        DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_MovimientosStock"""
    ),
    # MovimientoStockDetalle
    (
        """    IdUsuarioCreacion        INT NOT NULL,
    FechaCreacion            DATETIME2 NOT NULL
        CONSTRAINT DF_MovimientoStockDetalle_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_MovimientoStockDetalle""",
        """    IdUsuarioCreacion        INT NOT NULL,
    FechaCreacion            DATETIME2 NOT NULL
        CONSTRAINT DF_MovimientoStockDetalle_FechaCreacion
        DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_MovimientoStockDetalle"""
    ),
    # Depositos - agregar FechaBaja
    (
        """    Activo                BIT NOT NULL
        CONSTRAINT DF_Depositos_Activo DEFAULT 1,
    IdUsuarioCreacion     INT NOT NULL,""",
        """    Activo                BIT NOT NULL
        CONSTRAINT DF_Depositos_Activo DEFAULT 1,
    FechaBaja             DATETIME2 NULL,
    IdUsuarioCreacion     INT NOT NULL,"""
    ),
    # EstadosCaja - agregar FechaBaja
    (
        """    Activo               BIT NOT NULL
        CONSTRAINT DF_EstadosCaja_Activo DEFAULT 1,

    IdUsuarioCreacion    INT NOT NULL,""",
        """    Activo               BIT NOT NULL
        CONSTRAINT DF_EstadosCaja_Activo DEFAULT 1,
    FechaBaja            DATETIME2 NULL,

    IdUsuarioCreacion    INT NOT NULL,"""
    ),
    # MediosPago - agregar FechaBaja
    (
        """    Activo               BIT NOT NULL
        CONSTRAINT DF_MediosPago_Activo DEFAULT 1,

    IdUsuarioCreacion    INT NOT NULL,""",
        """    Activo               BIT NOT NULL
        CONSTRAINT DF_MediosPago_Activo DEFAULT 1,
    FechaBaja            DATETIME2 NULL,

    IdUsuarioCreacion    INT NOT NULL,"""
    ),
    # Monedas - agregar FechaBaja
    (
        """    Activo               BIT NOT NULL
        CONSTRAINT DF_Monedas_Activo DEFAULT 1,
    IdUsuarioCreacion    INT NOT NULL,""",
        """    Activo               BIT NOT NULL
        CONSTRAINT DF_Monedas_Activo DEFAULT 1,
    FechaBaja            DATETIME2 NULL,
    IdUsuarioCreacion    INT NOT NULL,"""
    ),
    # TiposMovimientoCaja - agregar FechaBaja
    (
        """    Signo                SMALLINT NOT NULL,
    Activo               BIT NOT NULL
        CONSTRAINT DF_TiposMovimientoCaja_Activo DEFAULT 1,

    IdUsuarioCreacion    INT NOT NULL,""",
        """    Signo                SMALLINT NOT NULL,
    Activo               BIT NOT NULL
        CONSTRAINT DF_TiposMovimientoCaja_Activo DEFAULT 1,
    FechaBaja            DATETIME2 NULL,

    IdUsuarioCreacion    INT NOT NULL,"""
    ),
    # TiposMovimientoStock - agregar FechaBaja
    (
        """    Signo                 SMALLINT NOT NULL,

    Activo                BIT NOT NULL
        CONSTRAINT DF_TiposMovimientoStock_Activo DEFAULT 1,

    IdUsuarioCreacion     INT NOT NULL,""",
        """    Signo                 SMALLINT NOT NULL,

    Activo                BIT NOT NULL
        CONSTRAINT DF_TiposMovimientoStock_Activo DEFAULT 1,
    FechaBaja             DATETIME2 NULL,

    IdUsuarioCreacion     INT NOT NULL,"""
    ),
]

for search, replace in reemplazos:
    if search not in text:
        print("NO ENCONTRADO:\n", search[:100], "...")
    else:
        text = text.replace(search, replace)
        print("Reemplazado bloque.")

INIT_SQL.write_text(text, encoding="utf-8")
print("Init.sql actualizado.")
