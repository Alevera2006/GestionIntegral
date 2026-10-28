
:ON ERROR EXIT
USE GestionIntegralDB;
GO

-- Run in SQLCMD mode with -v SuperAdminClaveHash=<BCrypt hash generated outside SQL>.
-- Do not put a plaintext password in ClaveHash.
DECLARE @SuperAdminClaveHash NVARCHAR(500) = N'$(SuperAdminClaveHash)';
IF LEN(@SuperAdminClaveHash) <> 60
   OR LEFT(@SuperAdminClaveHash, 4) NOT IN (N'$2a$', N'$2b$', N'$2y$')
    THROW 50001, 'SuperAdminClaveHash must be a BCrypt hash.', 1;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    CREATE TABLE dbo.TiposUsuario (
        IdTipoUsuario INT IDENTITY(1,1) NOT NULL,
        Descripcion NVARCHAR(100) NOT NULL,
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

    INSERT INTO dbo.TiposUsuario
        (Descripcion, IdUsuarioCreacion)
    VALUES
        (N'SuperAdministrador', 0);

    DECLARE @IdTipoAdmin INT =
        CONVERT(INT, SCOPE_IDENTITY());

    INSERT INTO dbo.Usuarios (
        Nombre,
        Apellido,
        IdTipoUsuario,
        Usuario,
        ClaveHash
    )
    VALUES (
        N'Super',
        N'Admin',
        @IdTipoAdmin,
        N'SuperAdmin',
        N'$(SuperAdminClaveHash)'
    );

    DECLARE @IdSuperAdmin INT =
        CONVERT(INT, SCOPE_IDENTITY());

    UPDATE dbo.Usuarios
    SET IdUsuarioCreacion = @IdSuperAdmin
    WHERE IdUsuario = @IdSuperAdmin;

    UPDATE dbo.TiposUsuario
    SET IdUsuarioCreacion = @IdSuperAdmin
    WHERE IdTipoUsuario = @IdTipoAdmin;

    ALTER TABLE dbo.Usuarios
    ALTER COLUMN IdUsuarioCreacion INT NOT NULL;

    ALTER TABLE dbo.Usuarios
    ADD CONSTRAINT FK_Usuarios_UsuarioCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES dbo.Usuarios(IdUsuario);

    ALTER TABLE dbo.Usuarios
    ADD CONSTRAINT FK_Usuarios_UsuarioModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES dbo.Usuarios(IdUsuario);

    ALTER TABLE dbo.TiposUsuario
    ADD CONSTRAINT FK_TiposUsuario_UsuarioCreacion
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES dbo.Usuarios(IdUsuario);

    ALTER TABLE dbo.TiposUsuario
    ADD CONSTRAINT FK_TiposUsuario_UsuarioModificacion
        FOREIGN KEY (IdUsuarioUltimaModificacion)
        REFERENCES dbo.Usuarios(IdUsuario);

    COMMIT TRANSACTION;

    SELECT
        IdUsuario,
        Usuario,
        IdTipoUsuario,
        Activo,
        Bloqueado,
        IdUsuarioCreacion,
        FechaCreacion
    FROM dbo.Usuarios
    WHERE IdUsuario = @IdSuperAdmin;

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
    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_CondicionesPago
        PRIMARY KEY (IdCondicionPago),

    CONSTRAINT FK_CondicionesPago_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT UQ_CondicionesPago_Codigo
        UNIQUE (Codigo)
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

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_Clientes
        PRIMARY KEY (IdCliente),

    CONSTRAINT FK_Clientes_CondicionesPago
        FOREIGN KEY (IdCondicionPago)
        REFERENCES CondicionesPago(IdCondicionPago),

    CONSTRAINT FK_Clientes_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT UQ_Clientes_Codigo
        UNIQUE (Codigo)
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

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_Proveedores
        PRIMARY KEY (IdProveedor),

    CONSTRAINT FK_Proveedores_CondicionesPago
        FOREIGN KEY (IdCondicionPago)
        REFERENCES CondicionesPago(IdCondicionPago),

    CONSTRAINT FK_Proveedores_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT UQ_Proveedores_Codigo
        UNIQUE (Codigo)
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

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_Categorias
        PRIMARY KEY (IdCategoria),

    CONSTRAINT FK_Categorias_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT UQ_Categorias_Codigo
        UNIQUE (Codigo)
);
CREATE TABLE SubCategorias
(
    IdSubCategoria        INT IDENTITY(1,1) NOT NULL,
    IdCategoria           INT NOT NULL,

    Codigo                VARCHAR(20) NOT NULL,
    Descripcion           VARCHAR(100) NOT NULL,
    Activo                BIT NOT NULL DEFAULT 1,

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_SubCategorias
        PRIMARY KEY (IdSubCategoria),

    CONSTRAINT FK_SubCategorias_Categorias
        FOREIGN KEY (IdCategoria)
        REFERENCES Categorias(IdCategoria),

    CONSTRAINT FK_SubCategorias_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
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

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_Productos
        PRIMARY KEY (IdProducto),

    CONSTRAINT FK_Productos_Categorias
        FOREIGN KEY (IdCategoria)
        REFERENCES Categorias(IdCategoria),

    CONSTRAINT FK_Productos_SubCategorias
        FOREIGN KEY (IdSubCategoria)
        REFERENCES SubCategorias(IdSubCategoria),

    CONSTRAINT FK_Productos_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT UQ_Productos_Codigo
        UNIQUE (Codigo)
);
CREATE TABLE ListasPrecios
(
    IdListaPrecio         INT IDENTITY(1,1) NOT NULL,
    Codigo                VARCHAR(20) NOT NULL,
    Descripcion           VARCHAR(100) NOT NULL,

    FechaDesde            DATETIME2 NULL,
    FechaHasta            DATETIME2 NULL,

    Activo                BIT NOT NULL DEFAULT 1,

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_ListasPrecios
        PRIMARY KEY (IdListaPrecio),

    CONSTRAINT FK_ListasPrecios_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT UQ_ListasPrecios_Codigo
        UNIQUE (Codigo)
);
CREATE TABLE ListaPreciosDetalle
(
    IdListaPrecioDetalle  INT IDENTITY(1,1) NOT NULL,
    IdListaPrecio         INT NOT NULL,
    IdProducto            INT NOT NULL,

    Precio                DECIMAL(18,4) NOT NULL,

    IdUsuarioCreacion     INT NOT NULL,
    FechaCreacion         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_ListaPreciosDetalle
        PRIMARY KEY (IdListaPrecioDetalle),

    CONSTRAINT FK_ListaPreciosDetalle_ListasPrecios
        FOREIGN KEY (IdListaPrecio)
        REFERENCES ListasPrecios(IdListaPrecio),

    CONSTRAINT FK_ListaPreciosDetalle_Productos
        FOREIGN KEY (IdProducto)
        REFERENCES Productos(IdProducto),

    CONSTRAINT FK_ListaPreciosDetalle_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
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

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_TiposImpuesto_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_TiposImpuesto
        PRIMARY KEY (IdTipoImpuesto),

    CONSTRAINT UQ_TiposImpuesto_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_TiposImpuesto_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
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

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_AlicuotasImpuesto_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

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

    CONSTRAINT FK_AlicuotasImpuesto_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
);
CREATE TABLE CondicionesIVA
(
    IdCondicionIVA       INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,

    Activo               BIT NOT NULL
        CONSTRAINT DF_CondicionesIVA_Activo DEFAULT 1,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_CondicionesIVA_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_CondicionesIVA
        PRIMARY KEY (IdCondicionIVA),

    CONSTRAINT UQ_CondicionesIVA_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_CondicionesIVA_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
);
CREATE TABLE Provincias
(
    IdProvincia          INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,
    CodigoAFIP           VARCHAR(10) NULL,
    Activo               BIT NOT NULL
        CONSTRAINT DF_Provincias_Activo DEFAULT 1,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_Provincias_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_Provincias
        PRIMARY KEY (IdProvincia),

    CONSTRAINT UQ_Provincias_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_Provincias_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
);
CREATE TABLE TiposComprobante
(
    IdTipoComprobante    INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,

    CodigoAFIP           VARCHAR(10) NULL,

    Activo               BIT NOT NULL
        CONSTRAINT DF_TiposComprobante_Activo DEFAULT 1,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_TiposComprobante_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_TiposComprobante
        PRIMARY KEY (IdTipoComprobante),

    CONSTRAINT UQ_TiposComprobante_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_TiposComprobante_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
);
CREATE TABLE MotivosAnulacion
(
    IdMotivoAnulacion    INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(200) NOT NULL,

    Activo               BIT NOT NULL
        CONSTRAINT DF_MotivosAnulacion_Activo DEFAULT 1,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_MotivosAnulacion_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_MotivosAnulacion
        PRIMARY KEY (IdMotivoAnulacion),

    CONSTRAINT UQ_MotivosAnulacion_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_MotivosAnulacion_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
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

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_EstadosPedido_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_EstadosPedido
        PRIMARY KEY (IdEstadoPedido),

    CONSTRAINT UQ_EstadosPedido_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_EstadosPedido_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
);
CREATE TABLE EstadosFactura
(
    IdEstadoFactura      INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,

    Activo               BIT NOT NULL
        CONSTRAINT DF_EstadosFactura_Activo DEFAULT 1,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_EstadosFactura_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_EstadosFactura
        PRIMARY KEY (IdEstadoFactura),

    CONSTRAINT UQ_EstadosFactura_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_EstadosFactura_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
);
CREATE TABLE EstadosCompra
(
    IdEstadoCompra       INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,

    Activo               BIT NOT NULL
        CONSTRAINT DF_EstadosCompra_Activo DEFAULT 1,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_EstadosCompra_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_EstadosCompra
        PRIMARY KEY (IdEstadoCompra),

    CONSTRAINT UQ_EstadosCompra_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_EstadosCompra_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
);
CREATE TABLE EstadosProceso
(
    IdEstadoProceso      INT IDENTITY(1,1) NOT NULL,
    Codigo               VARCHAR(20) NOT NULL,
    Descripcion          VARCHAR(100) NOT NULL,

    Activo               BIT NOT NULL
        CONSTRAINT DF_EstadosProceso_Activo DEFAULT 1,

    IdUsuarioCreacion    INT NOT NULL,
    FechaCreacion        DATETIME2 NOT NULL
        CONSTRAINT DF_EstadosProceso_FechaCreacion
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_EstadosProceso
        PRIMARY KEY (IdEstadoProceso),

    CONSTRAINT UQ_EstadosProceso_Codigo
        UNIQUE (Codigo),

    CONSTRAINT FK_EstadosProceso_Usuarios
        FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuarios(IdUsuario)
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


