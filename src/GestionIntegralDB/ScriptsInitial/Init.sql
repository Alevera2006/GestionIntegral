SET XACT_ABORT ON;
USE GestionIntegralDB;
GO
--d
-- Eliminaci¢n de tablas en orden inverso a las dependencias para permitir recrear la base
IF OBJECT_ID('dbo.MovimientoStockDetalle', 'U') IS NOT NULL DROP TABLE dbo.MovimientoStockDetalle;
IF OBJECT_ID('dbo.MovimientosStock', 'U') IS NOT NULL DROP TABLE dbo.MovimientosStock;
IF OBJECT_ID('dbo.Stock', 'U') IS NOT NULL DROP TABLE dbo.Stock;
IF OBJECT_ID('dbo.TiposMovimientoStock', 'U') IS NOT NULL DROP TABLE dbo.TiposMovimientoStock;
IF OBJECT_ID('dbo.Depositos', 'U') IS NOT NULL DROP TABLE dbo.Depositos;
IF OBJECT_ID('dbo.CajaDetalle', 'U') IS NOT NULL DROP TABLE dbo.CajaDetalle;
IF OBJECT_ID('dbo.CajaCabecera', 'U') IS NOT NULL DROP TABLE dbo.CajaCabecera;
IF OBJECT_ID('dbo.EstadosCaja', 'U') IS NOT NULL DROP TABLE dbo.EstadosCaja;
IF OBJECT_ID('dbo.TiposMovimientoCaja', 'U') IS NOT NULL DROP TABLE dbo.TiposMovimientoCaja;
IF OBJECT_ID('dbo.MediosPago', 'U') IS NOT NULL DROP TABLE dbo.MediosPago;
IF OBJECT_ID('dbo.Monedas', 'U') IS NOT NULL DROP TABLE dbo.Monedas;
IF OBJECT_ID('dbo.EstadosProceso', 'U') IS NOT NULL DROP TABLE dbo.EstadosProceso;
IF OBJECT_ID('dbo.EstadosCompra', 'U') IS NOT NULL DROP TABLE dbo.EstadosCompra;
IF OBJECT_ID('dbo.EstadosFactura', 'U') IS NOT NULL DROP TABLE dbo.EstadosFactura;
IF OBJECT_ID('dbo.EstadosPedido', 'U') IS NOT NULL DROP TABLE dbo.EstadosPedido;
IF OBJECT_ID('dbo.MotivosAnulacion', 'U') IS NOT NULL DROP TABLE dbo.MotivosAnulacion;
IF OBJECT_ID('dbo.TiposComprobante', 'U') IS NOT NULL DROP TABLE dbo.TiposComprobante;
IF OBJECT_ID('dbo.Provincias', 'U') IS NOT NULL DROP TABLE dbo.Provincias;
IF OBJECT_ID('dbo.CondicionesIVA', 'U') IS NOT NULL DROP TABLE dbo.CondicionesIVA;
IF OBJECT_ID('dbo.AlicuotasImpuesto', 'U') IS NOT NULL DROP TABLE dbo.AlicuotasImpuesto;
IF OBJECT_ID('dbo.TiposImpuesto', 'U') IS NOT NULL DROP TABLE dbo.TiposImpuesto;
IF OBJECT_ID('dbo.ListaPreciosDetalle', 'U') IS NOT NULL DROP TABLE dbo.ListaPreciosDetalle;
IF OBJECT_ID('dbo.ListasPrecios', 'U') IS NOT NULL DROP TABLE dbo.ListasPrecios;
IF OBJECT_ID('dbo.Productos', 'U') IS NOT NULL DROP TABLE dbo.Productos;
IF OBJECT_ID('dbo.SubCategorias', 'U') IS NOT NULL DROP TABLE dbo.SubCategorias;
IF OBJECT_ID('dbo.Categorias', 'U') IS NOT NULL DROP TABLE dbo.Categorias;
IF OBJECT_ID('dbo.ClienteContactos', 'U') IS NOT NULL DROP TABLE dbo.ClienteContactos;
IF OBJECT_ID('dbo.Contactos', 'U') IS NOT NULL DROP TABLE dbo.Contactos;
IF OBJECT_ID('dbo.Proveedores', 'U') IS NOT NULL DROP TABLE dbo.Proveedores;
IF OBJECT_ID('dbo.Clientes', 'U') IS NOT NULL DROP TABLE dbo.Clientes;
IF OBJECT_ID('dbo.CondicionesPago', 'U') IS NOT NULL DROP TABLE dbo.CondicionesPago;

-- Script para borrar UNICAMENTE las tablas Usuarios y TiposUsuario.
-- Primero elimina todas las FKs de otras tablas (y de estas mismas) que las referencian.
-- El resto de las tablas permanecen intactas, aunque sus columnas de auditoria
-- quedaran sin referencia hasta que se vuelva a ejecutar Init.sql.

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @Sql NVARCHAR(MAX) = N'';

    -- 1. Eliminar todas las FKs que referencian a Usuarios
    SELECT @Sql += N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id))
                + N'.' + QUOTENAME(OBJECT_NAME(parent_object_id))
                + N' DROP CONSTRAINT ' + QUOTENAME(name) + N';' + CHAR(13)
    FROM sys.foreign_keys
    WHERE referenced_object_id = OBJECT_ID('dbo.Usuarios');

    -- 2. Eliminar todas las FKs que referencian a TiposUsuario
    SELECT @Sql += N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id))
                + N'.' + QUOTENAME(OBJECT_NAME(parent_object_id))
                + N' DROP CONSTRAINT ' + QUOTENAME(name) + N';' + CHAR(13)
    FROM sys.foreign_keys
    WHERE referenced_object_id = OBJECT_ID('dbo.TiposUsuario');

    IF LEN(@Sql) > 0
        EXEC sp_executesql @Sql;

    -- 3. Borrar las tablas
    IF OBJECT_ID('dbo.Usuarios', 'U') IS NOT NULL
        DROP TABLE dbo.Usuarios;

    IF OBJECT_ID('dbo.TiposUsuario', 'U') IS NOT NULL
        DROP TABLE dbo.TiposUsuario;

    COMMIT TRANSACTION;

    PRINT 'Tablas Usuarios y TiposUsuario eliminadas correctamente.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;


GO

SET XACT_ABORT ON;
USE GestionIntegralDB;
GO

DECLARE @SuperAdminClaveHash NVARCHAR(500) = N'$2a$11$gk/lE5669hXTdIvf3SGj5OFgxu773hnnt4ub/XOUrqXuKDbFtbds.';

IF LEN(@SuperAdminClaveHash) <> 60
   OR LEFT(@SuperAdminClaveHash, 4) NOT IN (N'$2a$', N'$2b$', N'$2y$')
    THROW 50001, 'SuperAdminClaveHash must be a BCrypt hash.', 1;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID('dbo.TiposUsuario', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.TiposUsuario (
            IdTipoUsuario INT IDENTITY(1,1) NOT NULL,
            Descripcion NVARCHAR(100) NOT NULL,
            Activo BIT NOT NULL
                CONSTRAINT DF_TiposUsuario_Activo
                DEFAULT (1),
            FechaBaja DATETIME2(3) NULL,
            IdUsuarioCreacion INT NOT NULL,
            FechaCreacion DATETIME2(3) NOT NULL
                CONSTRAINT DF_TiposUsuario_FechaCreacion
                DEFAULT (SYSUTCDATETIME()),
            IdUsuarioUltimaModificacion INT NULL,
            FechaUltimaModificacion DATETIME2(3) NULL,

            CONSTRAINT PK_TiposUsuario
                PRIMARY KEY (IdTipoUsuario),

            CONSTRAINT UQ_TiposUsuario_Descripcion
                UNIQUE (Descripcion)
        );
    END

    IF OBJECT_ID('dbo.Usuarios', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.Usuarios (
            IdUsuario INT IDENTITY(1,1) NOT NULL,
            Nombre NVARCHAR(100) NOT NULL,
            Apellido NVARCHAR(100) NOT NULL,
            IdTipoUsuario INT NOT NULL,
            Usuario NVARCHAR(100) NOT NULL,
            ClaveHash NVARCHAR(500) NOT NULL,

            Bloqueado BIT NOT NULL
                CONSTRAINT DF_Usuarios_Bloqueado
                DEFAULT (0),

            Activo BIT NOT NULL
                CONSTRAINT DF_Usuarios_Activo
                DEFAULT (1),

            IdUsuarioCreacion INT NULL,

            FechaCreacion DATETIME2(3) NOT NULL
                CONSTRAINT DF_Usuarios_FechaCreacion
                DEFAULT (SYSUTCDATETIME()),

            IdUsuarioUltimaModificacion INT NULL,
            FechaUltimaModificacion DATETIME2(3) NULL,
            FechaBaja DATETIME2(3) NULL,

            CONSTRAINT PK_Usuarios
                PRIMARY KEY (IdUsuario),

            CONSTRAINT UQ_Usuarios_Usuario
                UNIQUE (Usuario),

            CONSTRAINT FK_Usuarios_TiposUsuario
                FOREIGN KEY (IdTipoUsuario)
                REFERENCES dbo.TiposUsuario(IdTipoUsuario),

            CONSTRAINT CK_Usuarios_FechaBaja
                CHECK (
                    (Activo = 1 AND FechaBaja IS NULL)
                    OR
                    (Activo = 0 AND FechaBaja IS NOT NULL)
                )
        );
    END

    DECLARE @SuperAdminClaveHash NVARCHAR(500) = N'$2a$11$gk/lE5669hXTdIvf3SGj5OFgxu773hnnt4ub/XOUrqXuKDbFtbds.';
    DECLARE @IdTipoAdmin INT;
    DECLARE @IdSuperAdmin INT;

    IF NOT EXISTS (SELECT 1 FROM dbo.TiposUsuario WHERE Descripcion = N'SuperAdministrador')
    BEGIN
        INSERT INTO dbo.TiposUsuario
            (Descripcion, IdUsuarioCreacion)
        VALUES
            (N'SuperAdministrador', 0);

        SET @IdTipoAdmin = CONVERT(INT, SCOPE_IDENTITY());
    END
    ELSE
    BEGIN
        SELECT @IdTipoAdmin = IdTipoUsuario
        FROM dbo.TiposUsuario
        WHERE Descripcion = N'SuperAdministrador';
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE Usuario = N'SuperAdmin')
    BEGIN
        INSERT INTO dbo.Usuarios (
            Nombre,
            Apellido,
            IdTipoUsuario,
            Usuario,
            ClaveHash
        )
        VALUES (
            N'Alejandro',
            N'Vera',
            @IdTipoAdmin,
            N'alevera01',
            @SuperAdminClaveHash
        );

        SET @IdSuperAdmin = CONVERT(INT, SCOPE_IDENTITY());

        UPDATE dbo.Usuarios
        SET IdUsuarioCreacion = @IdSuperAdmin
        WHERE IdUsuario = @IdSuperAdmin;

        UPDATE dbo.TiposUsuario
        SET IdUsuarioCreacion = @IdSuperAdmin
        WHERE IdTipoUsuario = @IdTipoAdmin;
    END
    ELSE
    BEGIN
        UPDATE dbo.Usuarios
        SET ClaveHash = @SuperAdminClaveHash,
            Activo = 1,
            Bloqueado = 0,
            FechaBaja = NULL
        WHERE Usuario = N'SuperAdmin';
    END

    IF OBJECT_ID('dbo.FK_Usuarios_UsuarioCreacion', 'F') IS NULL
    BEGIN
        ALTER TABLE dbo.Usuarios
        ALTER COLUMN IdUsuarioCreacion INT NOT NULL;

        ALTER TABLE dbo.Usuarios
        ADD CONSTRAINT FK_Usuarios_UsuarioCreacion
            FOREIGN KEY (IdUsuarioCreacion)
            REFERENCES dbo.Usuarios(IdUsuario);
    END

    IF OBJECT_ID('dbo.FK_Usuarios_UsuarioModificacion', 'F') IS NULL
    BEGIN
        ALTER TABLE dbo.Usuarios
        ADD CONSTRAINT FK_Usuarios_UsuarioModificacion
            FOREIGN KEY (IdUsuarioUltimaModificacion)
            REFERENCES dbo.Usuarios(IdUsuario);
    END

    IF OBJECT_ID('dbo.FK_TiposUsuario_UsuarioCreacion', 'F') IS NULL
    BEGIN
        ALTER TABLE dbo.TiposUsuario
        ADD CONSTRAINT FK_TiposUsuario_UsuarioCreacion
            FOREIGN KEY (IdUsuarioCreacion)
            REFERENCES dbo.Usuarios(IdUsuario);
    END

    IF OBJECT_ID('dbo.FK_TiposUsuario_UsuarioModificacion', 'F') IS NULL
    BEGIN
        ALTER TABLE dbo.TiposUsuario
        ADD CONSTRAINT FK_TiposUsuario_UsuarioModificacion
            FOREIGN KEY (IdUsuarioUltimaModificacion)
            REFERENCES dbo.Usuarios(IdUsuario);
    END

    COMMIT TRANSACTION;

    SELECT
        u.IdUsuario,
        u.ClaveHash,
        u.Usuario,
        u.IdTipoUsuario,
        tu.Descripcion AS TipoUsuarioDescripcion,
        u.Activo,
        u.Bloqueado,
        u.IdUsuarioCreacion,
        u.FechaCreacion
    FROM dbo.Usuarios u
    INNER JOIN dbo.TiposUsuario tu ON u.IdTipoUsuario = tu.IdTipoUsuario
    WHERE u.Usuario = N'alevera01';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;

CREATE TABLE CondicionesPago
(
    IdCondicionPago      INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,
    Dias                 INT NOT NULL DEFAULT 0,
    Activo               BIT NOT NULL DEFAULT 1,
    FechaBaja            DATETIME2 NULL,
    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_CondicionesPago
        PRIMARY KEY (IdCondicionPago),

    CONSTRAINT FK_CondicionesPago_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_CondicionesPago_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT UQ_CondicionesPago_Codigo
        UNIQUE (Codigo),

    CONSTRAINT CK_CondicionesPago_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
CREATE TABLE Clientes
(
    IdCliente            INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    RazonSocial          VARCHAR(150) NOT NULL,
    NombreFantasia       VARCHAR(150) NULL,

    CUIT                  VARCHAR(13) NULL,
    Email                 VARCHAR(150) NULL,
    Telefono              VARCHAR(50) NULL,

    IdCondicionPago       INT NULL,

    Observaciones         VARCHAR(500) NULL,
    Activo                BIT NOT NULL DEFAULT 1,
    FechaBaja             DATETIME2 NULL,

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_Clientes
        PRIMARY KEY (IdCliente),

    CONSTRAINT FK_Clientes_CondicionesPago
        FOREIGN KEY (IdCondicionPago)
        REFERENCES CondicionesPago(IdCondicionPago),

    CONSTRAINT FK_Clientes_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_Clientes_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT UQ_Clientes_Codigo
        UNIQUE (Codigo),

    CONSTRAINT CK_Clientes_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
CREATE TABLE Proveedores
(
    IdProveedor          INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    RazonSocial          VARCHAR(150) NOT NULL,
    NombreFantasia       VARCHAR(150) NULL,

    CUIT                  VARCHAR(13) NULL,
    Email                 VARCHAR(150) NULL,
    Telefono              VARCHAR(50) NULL,

    IdCondicionPago       INT NULL,

    Observaciones         VARCHAR(500) NULL,
    Activo                BIT NOT NULL DEFAULT 1,
    FechaBaja             DATETIME2 NULL,

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_Proveedores
        PRIMARY KEY (IdProveedor),

    CONSTRAINT FK_Proveedores_CondicionesPago
        FOREIGN KEY (IdCondicionPago)
        REFERENCES CondicionesPago(IdCondicionPago),

    CONSTRAINT FK_Proveedores_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_Proveedores_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT UQ_Proveedores_Codigo
        UNIQUE (Codigo),

    CONSTRAINT CK_Proveedores_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
CREATE TABLE Contactos
(
    IdContacto           INT IDENTITY(1,1) NOT NULL,
    Nombre               VARCHAR(100) NOT NULL,
    Apellido              VARCHAR(100) NULL,
    Cargo                 VARCHAR(100) NULL,
    Telefono              VARCHAR(50) NULL,
    Celular               VARCHAR(50) NULL,
    Email                 VARCHAR(150) NULL,

    Activo                BIT NOT NULL DEFAULT 1,

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_Contactos
        PRIMARY KEY (IdContacto),

    CONSTRAINT FK_Contactos_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
);
CREATE TABLE ClienteContactos
(
    IdCliente             INT NOT NULL,
    IdContacto            INT NOT NULL,
    EsPrincipal           BIT NOT NULL DEFAULT 0,

    CONSTRAINT PK_ClienteContactos
        PRIMARY KEY (IdCliente, IdContacto),

    CONSTRAINT FK_ClienteContactos_Clientes
        FOREIGN KEY (IdCliente)
        REFERENCES Clientes(IdCliente),

    CONSTRAINT FK_ClienteContactos_Contactos
        FOREIGN KEY (IdContacto)
        REFERENCES Contactos(IdContacto)
);
CREATE TABLE Categorias
(
    IdCategoria           INT IDENTITY(1,1) NOT NULL,
    Codigo                VARCHAR(20) NOT NULL,
    Descripcion           VARCHAR(100) NOT NULL,
    Activo                BIT NOT NULL DEFAULT 1,
    FechaBaja             DATETIME2 NULL,

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_Categorias
        PRIMARY KEY (IdCategoria),

    CONSTRAINT FK_Categorias_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_Categorias_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT UQ_Categorias_Codigo
        UNIQUE (Codigo),

    CONSTRAINT CK_Categorias_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
CREATE TABLE SubCategorias
(
    IdSubCategoria        INT IDENTITY(1,1) NOT NULL,
    IdCategoria           INT NOT NULL,

    Codigo                VARCHAR(20) NOT NULL,
    Descripcion           VARCHAR(100) NOT NULL,
    Activo                BIT NOT NULL DEFAULT 1,
    FechaBaja             DATETIME2 NULL,

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_SubCategorias
        PRIMARY KEY (IdSubCategoria),

    CONSTRAINT FK_SubCategorias_Categorias
        FOREIGN KEY (IdCategoria)
        REFERENCES Categorias(IdCategoria),

    CONSTRAINT FK_SubCategorias_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_SubCategorias_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT CK_SubCategorias_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
CREATE TABLE Productos
(
    IdProducto            INT IDENTITY(1,1) NOT NULL,

    Codigo                VARCHAR(50) NOT NULL,
    Descripcion           VARCHAR(200) NOT NULL,

    IdCategoria           INT NOT NULL,
    IdSubCategoria        INT NULL,

    CodigoBarras          VARCHAR(50) NULL,

    Costo                 DECIMAL(18,4) NULL,

    StockMinimo           DECIMAL(18,4) NULL,
    StockMaximo           DECIMAL(18,4) NULL,

    Observaciones         VARCHAR(500) NULL,
    Activo                BIT NOT NULL DEFAULT 1,
    FechaBaja             DATETIME2 NULL,

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_Productos
        PRIMARY KEY (IdProducto),

    CONSTRAINT FK_Productos_Categorias
        FOREIGN KEY (IdCategoria)
        REFERENCES Categorias(IdCategoria),

    CONSTRAINT FK_Productos_SubCategorias
        FOREIGN KEY (IdSubCategoria)
        REFERENCES SubCategorias(IdSubCategoria),

    CONSTRAINT FK_Productos_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_Productos_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT UQ_Productos_Codigo
        UNIQUE (Codigo),

    CONSTRAINT CK_Productos_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
CREATE TABLE ListasPrecios
(
    IdListaPrecio         INT IDENTITY(1,1) NOT NULL,
    Codigo                VARCHAR(20) NOT NULL,
    Descripcion           VARCHAR(100) NOT NULL,

    FechaDesde            DATETIME2 NULL,
    FechaHasta            DATETIME2 NULL,

    Activo                BIT NOT NULL DEFAULT 1,
    FechaBaja             DATETIME2 NULL,

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_ListasPrecios
        PRIMARY KEY (IdListaPrecio),

    CONSTRAINT FK_ListasPrecios_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_ListasPrecios_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT UQ_ListasPrecios_Codigo
        UNIQUE (Codigo),

    CONSTRAINT CK_ListasPrecios_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
CREATE TABLE ListaPreciosDetalle
(
    IdListaPrecioDetalle  INT IDENTITY(1,1) NOT NULL,
    IdListaPrecio         INT NOT NULL,
    IdProducto            INT NOT NULL,

    Precio                DECIMAL(18,4) NOT NULL,

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_ListaPreciosDetalle
        PRIMARY KEY (IdListaPrecioDetalle),

    CONSTRAINT FK_ListaPreciosDetalle_ListasPrecios
        FOREIGN KEY (IdListaPrecio)
        REFERENCES ListasPrecios(IdListaPrecio),

    CONSTRAINT FK_ListaPreciosDetalle_Productos
        FOREIGN KEY (IdProducto)
        REFERENCES Productos(IdProducto),

    CONSTRAINT FK_ListaPreciosDetalle_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_ListaPreciosDetalle_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT UQ_ListaPreciosDetalle
        UNIQUE (IdListaPrecio, IdProducto)
);
CREATE TABLE TiposImpuesto
(
    IdTipoImpuesto       INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,
    Activo               BIT NOT NULL
        CONSTRAINT DF_TiposImpuesto_Activo DEFAULT 1,
    FechaBaja            DATETIME2 NULL,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_TiposImpuesto_FechaCreacion
        DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_TiposImpuesto
        PRIMARY KEY (IdTipoImpuesto),

    CONSTRAINT UQ_TiposImpuesto_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_TiposImpuesto_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_TiposImpuesto_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT CK_TiposImpuesto_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
CREATE TABLE AlicuotasImpuesto
(
    IdAlicuotaImpuesto   INT IDENTITY(1,1) NOT NULL,
    IdTipoImpuesto       INT NOT NULL,

    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,

    Porcentaje           DECIMAL(8,4) NOT NULL,

    FechaDesde           DATE NULL,
    FechaHasta           DATE NULL,

    Activo               BIT NOT NULL
        CONSTRAINT DF_AlicuotasImpuesto_Activo DEFAULT 1,
    FechaBaja            DATETIME2 NULL,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_AlicuotasImpuesto_FechaCreacion
        DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_AlicuotasImpuesto
        PRIMARY KEY (IdAlicuotaImpuesto),

    CONSTRAINT UQ_AlicuotasImpuesto_Codigo
        UNIQUE (Codigo),

    CONSTRAINT CK_AlicuotasImpuesto_Porcentaje
        CHECK (Porcentaje >= 0),

    CONSTRAINT CK_AlicuotasImpuesto_Fechas
        CHECK (
            FechaHasta IS NULL
            OR FechaDesde IS NULL
            OR FechaHasta >= FechaDesde
        ),

    CONSTRAINT FK_AlicuotasImpuesto_TiposImpuesto
        FOREIGN KEY (IdTipoImpuesto)
        REFERENCES TiposImpuesto(IdTipoImpuesto),

    CONSTRAINT FK_AlicuotasImpuesto_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_AlicuotasImpuesto_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT CK_AlicuotasImpuesto_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
CREATE TABLE CondicionesIVA
(
    IdCondicionIVA       INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,

    Activo               BIT NOT NULL
        CONSTRAINT DF_CondicionesIVA_Activo DEFAULT 1,
    FechaBaja            DATETIME2 NULL,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_CondicionesIVA_FechaCreacion
        DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_CondicionesIVA
        PRIMARY KEY (IdCondicionIVA),

    CONSTRAINT UQ_CondicionesIVA_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_CondicionesIVA_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_CondicionesIVA_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT CK_CondicionesIVA_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
CREATE TABLE Provincias
(
    IdProvincia          INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,
    CodigoAFIP           VARCHAR(10) NULL,
    Activo               BIT NOT NULL
        CONSTRAINT DF_Provincias_Activo DEFAULT 1,
    FechaBaja            DATETIME2 NULL,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_Provincias_FechaCreacion
        DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_Provincias
        PRIMARY KEY (IdProvincia),

    CONSTRAINT UQ_Provincias_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_Provincias_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_Provincias_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT CK_Provincias_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
CREATE TABLE TiposComprobante
(
    IdTipoComprobante    INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,

    CodigoAFIP           VARCHAR(10) NULL,

    Activo               BIT NOT NULL
        CONSTRAINT DF_TiposComprobante_Activo DEFAULT 1,
    FechaBaja            DATETIME2 NULL,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_TiposComprobante_FechaCreacion
        DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_TiposComprobante
        PRIMARY KEY (IdTipoComprobante),

    CONSTRAINT UQ_TiposComprobante_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_TiposComprobante_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_TiposComprobante_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT CK_TiposComprobante_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
CREATE TABLE MotivosAnulacion
(
    IdMotivoAnulacion    INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(200) NOT NULL,

    Activo               BIT NOT NULL
        CONSTRAINT DF_MotivosAnulacion_Activo DEFAULT 1,
    FechaBaja            DATETIME2 NULL,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_MotivosAnulacion_FechaCreacion
        DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_MotivosAnulacion
        PRIMARY KEY (IdMotivoAnulacion),

    CONSTRAINT UQ_MotivosAnulacion_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_MotivosAnulacion_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_MotivosAnulacion_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT CK_MotivosAnulacion_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
/*CREATE TABLE MotivosRechazo
(
    IdMotivoRechazo      INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(200) NOT NULL,

    Activo               BIT NOT NULL
        CONSTRAINT DF_MotivosRechazo_Activo DEFAULT 1,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_MotivosRechazo_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_MotivosRechazo
        PRIMARY KEY (IdMotivoRechazo),

    CONSTRAINT UQ_MotivosRechazo_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_MotivosRechazo_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
);*/
CREATE TABLE EstadosPedido
(
    IdEstadoPedido       INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,

    Activo               BIT NOT NULL
        CONSTRAINT DF_EstadosPedido_Activo DEFAULT 1,
    FechaBaja            DATETIME2 NULL,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_EstadosPedido_FechaCreacion
        DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_EstadosPedido
        PRIMARY KEY (IdEstadoPedido),

    CONSTRAINT UQ_EstadosPedido_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_EstadosPedido_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_EstadosPedido_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT CK_EstadosPedido_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
CREATE TABLE EstadosFactura
(
    IdEstadoFactura      INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,

    Activo               BIT NOT NULL
        CONSTRAINT DF_EstadosFactura_Activo DEFAULT 1,
    FechaBaja            DATETIME2 NULL,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_EstadosFactura_FechaCreacion
        DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_EstadosFactura
        PRIMARY KEY (IdEstadoFactura),

    CONSTRAINT UQ_EstadosFactura_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_EstadosFactura_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_EstadosFactura_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT CK_EstadosFactura_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
CREATE TABLE EstadosCompra
(
    IdEstadoCompra       INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,

    Activo               BIT NOT NULL
        CONSTRAINT DF_EstadosCompra_Activo DEFAULT 1,
    FechaBaja            DATETIME2 NULL,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_EstadosCompra_FechaCreacion
        DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_EstadosCompra
        PRIMARY KEY (IdEstadoCompra),

    CONSTRAINT UQ_EstadosCompra_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_EstadosCompra_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_EstadosCompra_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT CK_EstadosCompra_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
CREATE TABLE EstadosProceso
(
    IdEstadoProceso      INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,

    Activo               BIT NOT NULL
        CONSTRAINT DF_EstadosProceso_Activo DEFAULT 1,
    FechaBaja            DATETIME2 NULL,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_EstadosProceso_FechaCreacion
        DEFAULT SYSUTCDATETIME(),
    IdUsuarioUltimaModificacion INT NULL,
    FechaUltimaModificacion DATETIME2 NULL,

    CONSTRAINT PK_EstadosProceso
        PRIMARY KEY (IdEstadoProceso),

    CONSTRAINT UQ_EstadosProceso_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_EstadosProceso_UsuariosCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_EstadosProceso_UsuariosModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT CK_EstadosProceso_FechaBaja
        CHECK (
            (Activo = 1 AND FechaBaja IS NULL)
            OR
            (Activo = 0 AND FechaBaja IS NOT NULL)
        )
);
CREATE TABLE Monedas
(
    IdMoneda             INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(10) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,
    Simbolo              VARCHAR(10) NULL,
    CantidadDecimales    TINYINT NOT NULL
        CONSTRAINT DF_Monedas_CantidadDecimales DEFAULT 2,
    Activo               BIT NOT NULL
        CONSTRAINT DF_Monedas_Activo DEFAULT 1,
    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_Monedas_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_Monedas
        PRIMARY KEY (IdMoneda),

    CONSTRAINT UQ_Monedas_Codigo
        UNIQUE (Codigo),

    CONSTRAINT CK_Monedas_CantidadDecimales
        CHECK (CantidadDecimales BETWEEN 0 AND 6),

    CONSTRAINT FK_Monedas_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
);
CREATE TABLE MediosPago
(
    IdMedioPago          INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,

    EsEfectivo           BIT NOT NULL
        CONSTRAINT DF_MediosPago_EsEfectivo DEFAULT 0,

    RequiereReferencia   BIT NOT NULL
        CONSTRAINT DF_MediosPago_RequiereReferencia DEFAULT 0,

    Activo               BIT NOT NULL
        CONSTRAINT DF_MediosPago_Activo DEFAULT 1,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_MediosPago_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_MediosPago
        PRIMARY KEY (IdMedioPago),

    CONSTRAINT UQ_MediosPago_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_MediosPago_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
);
CREATE TABLE TiposMovimientoCaja
(
    IdTipoMovimientoCaja INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(30) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,
    Signo                SMALLINT NOT NULL,
    Activo               BIT NOT NULL
        CONSTRAINT DF_TiposMovimientoCaja_Activo DEFAULT 1,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_TiposMovimientoCaja_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_TiposMovimientoCaja
        PRIMARY KEY (IdTipoMovimientoCaja),

    CONSTRAINT UQ_TiposMovimientoCaja_Codigo
        UNIQUE (Codigo),

    CONSTRAINT CK_TiposMovimientoCaja_Signo
        CHECK (Signo IN (-1, 1)),

    CONSTRAINT FK_TiposMovimientoCaja_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
);
CREATE TABLE EstadosCaja
(
    IdEstadoCaja         INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,
    Activo               BIT NOT NULL
        CONSTRAINT DF_EstadosCaja_Activo DEFAULT 1,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_EstadosCaja_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_EstadosCaja
        PRIMARY KEY (IdEstadoCaja),

    CONSTRAINT UQ_EstadosCaja_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_EstadosCaja_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
);
CREATE TABLE CajaCabecera (
    [IdCaja]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [FechaCaja]            DATE            NOT NULL,
    [IdUsuarioResponsable] INT             NOT NULL,
    [IdEstadoCaja]         INT             NOT NULL,
    [SaldoInicial]         DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [SaldoFinal]           DECIMAL (18, 2) NULL,
    [FechaApertura]        DATETIME2 (7)   DEFAULT (sysutcdatetime()) NOT NULL,
    [FechaCierre]          DATETIME2 (7)   NULL,
    [Observaciones]        VARCHAR (500)   NULL,
    [IdUsuarioCreacion]    INT             NOT NULL,
    [FechaCreacion]        DATETIME2 (7)   DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_CajaCabecera] PRIMARY KEY CLUSTERED ([IdCaja] ASC),
    CONSTRAINT [FK_CajaCabecera_Responsable] FOREIGN KEY ([IdUsuarioResponsable]) REFERENCES [dbo].[Usuarios] ([IdUsuario]),
    CONSTRAINT [FK_CajaCabecera_Estado] FOREIGN KEY ([IdEstadoCaja]) REFERENCES [dbo].[EstadosCaja] ([IdEstadoCaja]),
    CONSTRAINT [FK_CajaCabecera_UsuarioCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario])
);
CREATE TABLE CajaDetalle
(
    IdCajaDetalle         BIGINT IDENTITY(1,1) NOT NULL,
    IdCaja                BIGINT NOT NULL,

    IdTipoMovimientoCaja  INT NOT NULL,
    IdMedioPago           INT NULL,
    IdMoneda              INT NULL,

    FechaMovimiento       DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    Concepto              VARCHAR(200) NOT NULL,
    Referencia            VARCHAR(100) NULL,

    Importe               DECIMAL(18,2) NOT NULL,

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_CajaDetalle
        PRIMARY KEY (IdCajaDetalle),

    CONSTRAINT FK_CajaDetalle_Cajas
        FOREIGN KEY (IdCaja)
        REFERENCES CajaCabecera(IdCaja),

    CONSTRAINT FK_CajaDetalle_TipoMovimiento
        FOREIGN KEY (IdTipoMovimientoCaja)
        REFERENCES TiposMovimientoCaja(IdTipoMovimientoCaja),

    CONSTRAINT FK_CajaDetalle_MedioPago
        FOREIGN KEY (IdMedioPago)
        REFERENCES MediosPago(IdMedioPago),

    CONSTRAINT FK_CajaDetalle_Moneda
        FOREIGN KEY (IdMoneda)
        REFERENCES Monedas(IdMoneda),

    CONSTRAINT FK_CajaDetalle_UsuarioCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT CK_CajaDetalle_Importe
        CHECK (Importe > 0)
);
CREATE TABLE Depositos
(
    IdDeposito            INT IDENTITY(1,1) NOT NULL,
    Codigo                VARCHAR(20) NOT NULL,
    Descripcion           VARCHAR(150) NOT NULL,
    Ubicacion             VARCHAR(200) NULL,
    Activo                BIT NOT NULL
        CONSTRAINT DF_Depositos_Activo DEFAULT 1,
    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL
        CONSTRAINT DF_Depositos_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_Depositos
        PRIMARY KEY (IdDeposito),

    CONSTRAINT UQ_Depositos_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_Depositos_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
);
CREATE TABLE TiposMovimientoStock
(
    IdTipoMovimientoStock INT IDENTITY(1,1) NOT NULL,
    Codigo                VARCHAR(30) NOT NULL,
    Descripcion           VARCHAR(100) NOT NULL,

    Signo                 SMALLINT NOT NULL,

    Activo                BIT NOT NULL
        CONSTRAINT DF_TiposMovimientoStock_Activo DEFAULT 1,

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL
        CONSTRAINT DF_TiposMovimientoStock_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_TiposMovimientoStock
        PRIMARY KEY (IdTipoMovimientoStock),

    CONSTRAINT UQ_TiposMovimientoStock_Codigo
        UNIQUE (Codigo),

    CONSTRAINT CK_TiposMovimientoStock_Signo
        CHECK (Signo IN (-1, 1)),

    CONSTRAINT FK_TiposMovimientoStock_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
);
CREATE TABLE Stock
(
    IdStock               BIGINT IDENTITY(1,1) NOT NULL,
    IdDeposito            INT NOT NULL,
    IdProducto            INT NOT NULL,
    Cantidad              DECIMAL(18,4) NOT NULL
        CONSTRAINT DF_Stock_Cantidad DEFAULT 0,
    FechaUltimoMovimiento DATETIME2 NULL,

    CONSTRAINT PK_Stock
        PRIMARY KEY (IdStock),

    CONSTRAINT FK_Stock_Depositos
        FOREIGN KEY (IdDeposito)
        REFERENCES Depositos(IdDeposito),

    CONSTRAINT FK_Stock_Productos
        FOREIGN KEY (IdProducto)
        REFERENCES Productos(IdProducto),

    CONSTRAINT UQ_Stock_DepositoProducto
        UNIQUE (IdDeposito, IdProducto)
);
CREATE TABLE MovimientosStock
(
    IdMovimientoStock     BIGINT IDENTITY(1,1) NOT NULL,

    IdDeposito            INT NOT NULL,
    IdTipoMovimientoStock INT NOT NULL,

    FechaMovimiento       DATETIME2 NOT NULL
        CONSTRAINT DF_MovimientosStock_FechaMovimiento
        DEFAULT SYSUTCDATETIME(),

    IdUsuarioResponsable  INT NOT NULL,

    Referencia            VARCHAR(100) NULL,
    Observaciones         VARCHAR(500) NULL,

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL
        CONSTRAINT DF_MovimientosStock_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_MovimientosStock
        PRIMARY KEY (IdMovimientoStock),

    CONSTRAINT FK_MovimientosStock_Depositos
        FOREIGN KEY (IdDeposito)
        REFERENCES Depositos(IdDeposito),

    CONSTRAINT FK_MovimientosStock_Tipo
        FOREIGN KEY (IdTipoMovimientoStock)
        REFERENCES TiposMovimientoStock(IdTipoMovimientoStock),

    CONSTRAINT FK_MovimientosStock_Responsable
        FOREIGN KEY (IdUsuarioResponsable)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_MovimientosStock_UsuarioCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
);
CREATE TABLE MovimientoStockDetalle
(
    IdMovimientoStockDetalle BIGINT IDENTITY(1,1) NOT NULL,

    IdMovimientoStock        BIGINT NOT NULL,
    IdProducto               INT NOT NULL,

    Cantidad                 DECIMAL(18,4) NOT NULL,

    IdUsuarioCreacion        INT NOT NULL,
    FechaCreacion            DATETIME2 NOT NULL
        CONSTRAINT DF_MovimientoStockDetalle_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_MovimientoStockDetalle
        PRIMARY KEY (IdMovimientoStockDetalle),

    CONSTRAINT FK_MovimientoStockDetalle_Movimiento
        FOREIGN KEY (IdMovimientoStock)
        REFERENCES MovimientosStock(IdMovimientoStock),

    CONSTRAINT FK_MovimientoStockDetalle_Producto
        FOREIGN KEY (IdProducto)
        REFERENCES Productos(IdProducto),

    CONSTRAINT FK_MovimientoStockDetalle_Usuario
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT CK_MovimientoStockDetalle_Cantidad
        CHECK (Cantidad > 0)
);



INSERT INTO dbo.Provincias (Codigo, Descripcion, IdUsuarioCreacion)
VALUES
    ('BA',   'BUENOS AIRES', 1),
    ('CABA', 'CIUDAD AUTÓNOMA DE BUENOS AIRES', 1),
    ('CAT',  'CATAMARCA', 1),
    ('CHA',  'CHACO', 1),
    ('CHU',  'CHUBUT', 1),
    ('CBA',  'CÓRDOBA', 1),
    ('CTES', 'CORRIENTES', 1),
    ('ER',   'ENTRE RÍOS', 1),
    ('FOR',  'FORMOSA', 1),
    ('JUJ',  'JUJUY', 1),
    ('LP',   'LA PAMPA', 1),
    ('LR',   'LA RIOJA', 1),
    ('MZA',  'MENDOZA', 1),
    ('MIS',  'MISIONES', 1),
    ('NEU',  'NEUQUÉN', 1),
    ('RN',   'RÍO NEGRO', 1),
    ('SAL',  'SALTA', 1),
    ('SJ',   'SAN JUAN', 1),
    ('SL',   'SAN LUIS', 1),
    ('SC',   'SANTA CRUZ', 1),
    ('SF',   'SANTA FE', 1),
    ('SDE',  'SANTIAGO DEL ESTERO', 1),
    ('TDF',  'TIERRA DEL FUEGO', 1),
    ('TUC',  'TUCUMÁN', 1);



    SELECT * FROM DBO.Provincias