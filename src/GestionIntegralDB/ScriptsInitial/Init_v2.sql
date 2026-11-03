

USE GestionIntegralDB;
GO

/* =========================================================
   INIT_V2 - COMPLETO AUTOCONTENIDO
   - Drop/Create de todas las tablas en orden
   - Bootstrap de TiposUsuario/Usuarios con alevera01
   - Seed final de Provincias
   ========================================================= */

/* Drop en orden inverso */
IF OBJECT_ID('dbo.Emails', 'U') IS NOT NULL DROP TABLE dbo.Emails;
IF OBJECT_ID('dbo.Telefonos', 'U') IS NOT NULL DROP TABLE dbo.Telefonos;
IF OBJECT_ID('dbo.Domicilios', 'U') IS NOT NULL DROP TABLE dbo.Domicilios;
IF OBJECT_ID('dbo.ClienteContactos', 'U') IS NOT NULL DROP TABLE dbo.ClienteContactos;
IF OBJECT_ID('dbo.Contactos', 'U') IS NOT NULL DROP TABLE dbo.Contactos;
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
IF OBJECT_ID('dbo.Proveedores', 'U') IS NOT NULL DROP TABLE dbo.Proveedores;
IF OBJECT_ID('dbo.Clientes', 'U') IS NOT NULL DROP TABLE dbo.Clientes;
IF OBJECT_ID('dbo.CondicionesPago', 'U') IS NOT NULL DROP TABLE dbo.CondicionesPago;

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

DECLARE @SuperAdminClaveHash NVARCHAR(500) = N'$2a$11$gk/lE5669hXTdIvf3SGj5OFgxu773hnnt4ub/XOUrqXuKDbFtbds.';
IF LEN(@SuperAdminClaveHash) <> 60 OR LEFT(@SuperAdminClaveHash, 4) NOT IN (N'$2a$', N'$2b$', N'$2y$')
	THROW 50001, 'SuperAdminClaveHash must be a BCrypt hash.', 1;
GO

/* Bootstrap TiposUsuario / Usuarios */
CREATE TABLE dbo.TiposUsuario (
	IdTipoUsuario INT IDENTITY(1,1) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	IdUsuarioCreacion INT NULL,
	FechaCreacion DATETIME2(3) NOT NULL CONSTRAINT DF_TiposUsuario_FechaCreacion DEFAULT (SYSUTCDATETIME()),
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2(3) NULL,
	Activo BIT NOT NULL CONSTRAINT DF_TiposUsuario_Activo DEFAULT (1),
	FechaBaja DATETIME2(3) NULL,
	CONSTRAINT PK_TiposUsuario PRIMARY KEY (IdTipoUsuario),
	CONSTRAINT UQ_TiposUsuario_Descripcion UNIQUE (Descripcion)
);
GO

CREATE TABLE dbo.Usuarios (
	IdUsuario INT IDENTITY(1,1) NOT NULL,
	Nombre NVARCHAR(100) NOT NULL,
	Apellido NVARCHAR(100) NOT NULL,
	IdTipoUsuario INT NOT NULL,
	Usuario NVARCHAR(100) NOT NULL,
	ClaveHash NVARCHAR(500) NOT NULL,
	Bloqueado BIT NOT NULL CONSTRAINT DF_Usuarios_Bloqueado DEFAULT (0),
	Activo BIT NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT (1),
	IdUsuarioCreacion INT NULL,
	FechaCreacion DATETIME2(3) NOT NULL CONSTRAINT DF_Usuarios_FechaCreacion DEFAULT (SYSUTCDATETIME()),
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2(3) NULL,
	FechaBaja DATETIME2(3) NULL,
	CONSTRAINT PK_Usuarios PRIMARY KEY (IdUsuario),
	CONSTRAINT UQ_Usuarios_Usuario UNIQUE (Usuario),
	CONSTRAINT FK_Usuarios_TiposUsuario FOREIGN KEY (IdTipoUsuario) REFERENCES dbo.TiposUsuario(IdTipoUsuario),
	CONSTRAINT FK_Usuarios_UsuarioCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_Usuarios_UsuarioModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);
GO



DECLARE @SuperAdminClaveHash NVARCHAR(500) = N'$2a$11$gk/lE5669hXTdIvf3SGj5OFgxu773hnnt4ub/XOUrqXuKDbFtbds.';

IF LEN(@SuperAdminClaveHash) <> 60
   OR LEFT(@SuperAdminClaveHash, 4) NOT IN (N'$2a$', N'$2b$', N'$2y$')
    THROW 50001, 'SuperAdminClaveHash must be a BCrypt hash.', 1;
GO

DECLARE @SuperAdminClaveHash NVARCHAR(500) = N'$2a$11$gk/lE5669hXTdIvf3SGj5OFgxu773hnnt4ub/XOUrqXuKDbFtbds.';
 
INSERT INTO dbo.Usuarios (Nombre, Apellido, IdTipoUsuario, Usuario, ClaveHash, Bloqueado, Activo, IdUsuarioCreacion)
SELECT N'Alex', N'Vera', tu.IdTipoUsuario, N'alevera01', @SuperAdminClaveHash, 0, 1, NULL
FROM dbo.TiposUsuario tu
WHERE tu.Descripcion = N'SUPERADMIN';

DECLARE @IdAlevera01 INT = (SELECT TOP 1 IdUsuario FROM dbo.Usuarios WHERE Usuario = N'alevera01');
UPDATE dbo.TiposUsuario SET IdUsuarioCreacion = @IdAlevera01 WHERE IdUsuarioCreacion IS NULL;
UPDATE dbo.Usuarios SET IdUsuarioCreacion = @IdAlevera01 WHERE IdUsuarioCreacion IS NULL;

ALTER TABLE dbo.TiposUsuario ALTER COLUMN IdUsuarioCreacion INT NOT NULL;
ALTER TABLE dbo.Usuarios ALTER COLUMN IdUsuarioCreacion INT NOT NULL;

ALTER TABLE dbo.TiposUsuario
ADD CONSTRAINT FK_TiposUsuario_UsuarioCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario);
ALTER TABLE dbo.TiposUsuario
ADD CONSTRAINT FK_TiposUsuario_UsuarioModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario);
GO

/* Resto de tablas */
CREATE TABLE dbo.CondicionesPago (
	IdCondicionPago INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	Dias INT NOT NULL CONSTRAINT DF_CondicionesPago_Dias DEFAULT (0),
	Activo BIT NOT NULL CONSTRAINT DF_CondicionesPago_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_CondicionesPago PRIMARY KEY (IdCondicionPago),
	CONSTRAINT UQ_CondicionesPago_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_CondicionesPago_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_CondicionesPago_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.Clientes (
	IdCliente INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	RazonSocial NVARCHAR(150) NOT NULL,
	NombreFantasia NVARCHAR(150) NULL,
	CUIT NVARCHAR(13) NULL,
	IdCondicionPago INT NULL,
	Observaciones NVARCHAR(500) NULL,
	Activo BIT NOT NULL CONSTRAINT DF_Clientes_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_Clientes PRIMARY KEY (IdCliente),
	CONSTRAINT UQ_Clientes_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_Clientes_CondicionesPago FOREIGN KEY (IdCondicionPago) REFERENCES dbo.CondicionesPago(IdCondicionPago),
	CONSTRAINT FK_Clientes_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_Clientes_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.Proveedores (
	IdProveedor INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	RazonSocial NVARCHAR(150) NOT NULL,
	NombreFantasia NVARCHAR(150) NULL,
	CUIT NVARCHAR(13) NULL,
	IdCondicionPago INT NULL,
	Observaciones NVARCHAR(500) NULL,
	Activo BIT NOT NULL CONSTRAINT DF_Proveedores_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_Proveedores PRIMARY KEY (IdProveedor),
	CONSTRAINT UQ_Proveedores_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_Proveedores_CondicionesPago FOREIGN KEY (IdCondicionPago) REFERENCES dbo.CondicionesPago(IdCondicionPago),
	CONSTRAINT FK_Proveedores_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_Proveedores_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.Categorias (
	IdCategoria INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	Activo BIT NOT NULL CONSTRAINT DF_Categorias_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_Categorias PRIMARY KEY (IdCategoria),
	CONSTRAINT UQ_Categorias_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_Categorias_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_Categorias_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.SubCategorias (
	IdSubCategoria INT IDENTITY(1,1) NOT NULL,
	IdCategoria INT NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	Activo BIT NOT NULL CONSTRAINT DF_SubCategorias_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_SubCategorias PRIMARY KEY (IdSubCategoria),
	CONSTRAINT FK_SubCategorias_Categorias FOREIGN KEY (IdCategoria) REFERENCES dbo.Categorias(IdCategoria),
	CONSTRAINT FK_SubCategorias_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_SubCategorias_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.Productos (
	IdProducto INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(50) NOT NULL,
	Descripcion NVARCHAR(200) NOT NULL,
	IdCategoria INT NOT NULL,
	IdSubCategoria INT NULL,
	CodigoBarras NVARCHAR(50) NULL,
	Costo DECIMAL(18,4) NULL,
	StockMinimo DECIMAL(18,4) NULL,
	StockMaximo DECIMAL(18,4) NULL,
	Observaciones NVARCHAR(500) NULL,
	Activo BIT NOT NULL CONSTRAINT DF_Productos_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_Productos PRIMARY KEY (IdProducto),
	CONSTRAINT UQ_Productos_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_Productos_Categorias FOREIGN KEY (IdCategoria) REFERENCES dbo.Categorias(IdCategoria),
	CONSTRAINT FK_Productos_SubCategorias FOREIGN KEY (IdSubCategoria) REFERENCES dbo.SubCategorias(IdSubCategoria),
	CONSTRAINT FK_Productos_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_Productos_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.ListasPrecios (
	IdListaPrecio INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	FechaDesde DATETIME2 NULL,
	FechaHasta DATETIME2 NULL,
	Activo BIT NOT NULL CONSTRAINT DF_ListasPrecios_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_ListasPrecios PRIMARY KEY (IdListaPrecio),
	CONSTRAINT UQ_ListasPrecios_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_ListasPrecios_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_ListasPrecios_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.ListaPreciosDetalle (
	IdListaPrecioDetalle INT IDENTITY(1,1) NOT NULL,
	IdListaPrecio INT NOT NULL,
	IdProducto INT NOT NULL,
	Precio DECIMAL(18,4) NOT NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_ListaPreciosDetalle PRIMARY KEY (IdListaPrecioDetalle),
	CONSTRAINT UQ_ListaPreciosDetalle UNIQUE (IdListaPrecio, IdProducto),
	CONSTRAINT FK_ListaPreciosDetalle_ListasPrecios FOREIGN KEY (IdListaPrecio) REFERENCES dbo.ListasPrecios(IdListaPrecio) ON DELETE CASCADE,
	CONSTRAINT FK_ListaPreciosDetalle_Productos FOREIGN KEY (IdProducto) REFERENCES dbo.Productos(IdProducto),
	CONSTRAINT FK_ListaPreciosDetalle_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_ListaPreciosDetalle_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.TiposImpuesto (
	IdTipoImpuesto INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	Activo BIT NOT NULL CONSTRAINT DF_TiposImpuesto_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_TiposImpuesto PRIMARY KEY (IdTipoImpuesto),
	CONSTRAINT UQ_TiposImpuesto_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_TiposImpuesto_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_TiposImpuesto_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.AlicuotasImpuesto (
	IdAlicuotaImpuesto INT IDENTITY(1,1) NOT NULL,
	IdTipoImpuesto INT NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	Porcentaje DECIMAL(8,4) NOT NULL,
	FechaDesde DATE NULL,
	FechaHasta DATE NULL,
	Activo BIT NOT NULL CONSTRAINT DF_AlicuotasImpuesto_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_AlicuotasImpuesto PRIMARY KEY (IdAlicuotaImpuesto),
	CONSTRAINT UQ_AlicuotasImpuesto_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_AlicuotasImpuesto_TiposImpuesto FOREIGN KEY (IdTipoImpuesto) REFERENCES dbo.TiposImpuesto(IdTipoImpuesto),
	CONSTRAINT FK_AlicuotasImpuesto_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_AlicuotasImpuesto_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.CondicionesIVA (
	IdCondicionIVA INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	Activo BIT NOT NULL CONSTRAINT DF_CondicionesIVA_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_CondicionesIVA PRIMARY KEY (IdCondicionIVA),
	CONSTRAINT UQ_CondicionesIVA_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_CondicionesIVA_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_CondicionesIVA_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.Provincias (
	IdProvincia INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	CodigoAFIP NVARCHAR(10) NULL,
	Activo BIT NOT NULL CONSTRAINT DF_Provincias_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_Provincias PRIMARY KEY (IdProvincia),
	CONSTRAINT UQ_Provincias_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_Provincias_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_Provincias_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.TiposComprobante (
	IdTipoComprobante INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	CodigoAFIP NVARCHAR(10) NULL,
	Activo BIT NOT NULL CONSTRAINT DF_TiposComprobante_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_TiposComprobante PRIMARY KEY (IdTipoComprobante),
	CONSTRAINT UQ_TiposComprobante_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_TiposComprobante_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_TiposComprobante_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.MotivosAnulacion (
	IdMotivoAnulacion INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(200) NOT NULL,
	Activo BIT NOT NULL CONSTRAINT DF_MotivosAnulacion_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_MotivosAnulacion PRIMARY KEY (IdMotivoAnulacion),
	CONSTRAINT UQ_MotivosAnulacion_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_MotivosAnulacion_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_MotivosAnulacion_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.EstadosPedido (
	IdEstadoPedido INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	Activo BIT NOT NULL CONSTRAINT DF_EstadosPedido_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_EstadosPedido PRIMARY KEY (IdEstadoPedido),
	CONSTRAINT UQ_EstadosPedido_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_EstadosPedido_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_EstadosPedido_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.EstadosFactura (
	IdEstadoFactura INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	Activo BIT NOT NULL CONSTRAINT DF_EstadosFactura_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_EstadosFactura PRIMARY KEY (IdEstadoFactura),
	CONSTRAINT UQ_EstadosFactura_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_EstadosFactura_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_EstadosFactura_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.EstadosCompra (
	IdEstadoCompra INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	Activo BIT NOT NULL CONSTRAINT DF_EstadosCompra_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_EstadosCompra PRIMARY KEY (IdEstadoCompra),
	CONSTRAINT UQ_EstadosCompra_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_EstadosCompra_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_EstadosCompra_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.EstadosProceso (
	IdEstadoProceso INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	Activo BIT NOT NULL CONSTRAINT DF_EstadosProceso_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_EstadosProceso PRIMARY KEY (IdEstadoProceso),
	CONSTRAINT UQ_EstadosProceso_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_EstadosProceso_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_EstadosProceso_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.Monedas (
	IdMoneda INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(10) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	Simbolo NVARCHAR(10) NULL,
	CantidadDecimales TINYINT NOT NULL CONSTRAINT DF_Monedas_CantidadDecimales DEFAULT (2),
	Activo BIT NOT NULL CONSTRAINT DF_Monedas_Activo DEFAULT (1),
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2(3) NULL,
	CONSTRAINT PK_Monedas PRIMARY KEY (IdMoneda),
	CONSTRAINT UQ_Monedas_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_Monedas_Usuarios FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.MediosPago (
	IdMedioPago INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	EsEfectivo BIT NOT NULL CONSTRAINT DF_MediosPago_EsEfectivo DEFAULT (0),
	RequiereReferencia BIT NOT NULL CONSTRAINT DF_MediosPago_RequiereReferencia DEFAULT (0),
	Activo BIT NOT NULL CONSTRAINT DF_MediosPago_Activo DEFAULT (1),
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	CONSTRAINT PK_MediosPago PRIMARY KEY (IdMedioPago),
	CONSTRAINT UQ_MediosPago_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_MediosPago_Usuarios FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.TiposMovimientoCaja (
	IdTipoMovimientoCaja INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(30) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	Signo INT NOT NULL,
	Activo BIT NOT NULL CONSTRAINT DF_TiposMovimientoCaja_Activo DEFAULT (1),
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	CONSTRAINT PK_TiposMovimientoCaja PRIMARY KEY (IdTipoMovimientoCaja),
	CONSTRAINT UQ_TiposMovimientoCaja_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_TiposMovimientoCaja_Usuarios FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.EstadosCaja (
	IdEstadoCaja INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	Activo BIT NOT NULL CONSTRAINT DF_EstadosCaja_Activo DEFAULT (1),
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	CONSTRAINT PK_EstadosCaja PRIMARY KEY (IdEstadoCaja),
	CONSTRAINT UQ_EstadosCaja_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_EstadosCaja_Usuarios FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.CajaCabecera (
	IdCaja INT IDENTITY(1,1) NOT NULL,
	FechaCaja DATE NOT NULL,
	IdUsuarioResponsable INT NOT NULL,
	IdEstadoCaja INT NOT NULL,
	SaldoInicial DECIMAL(18,2) NOT NULL CONSTRAINT DF_CajaCabecera_SaldoInicial DEFAULT (0),
	SaldoFinal DECIMAL(18,2) NULL,
	FechaApertura DATETIME2 NOT NULL CONSTRAINT DF_CajaCabecera_FechaApertura DEFAULT (SYSUTCDATETIME()),
	FechaCierre DATETIME2 NULL,
	Observaciones NVARCHAR(500) NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	CONSTRAINT PK_CajaCabecera PRIMARY KEY (IdCaja),
	CONSTRAINT FK_CajaCabecera_Responsable FOREIGN KEY (IdUsuarioResponsable) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_CajaCabecera_Estado FOREIGN KEY (IdEstadoCaja) REFERENCES dbo.EstadosCaja(IdEstadoCaja),
	CONSTRAINT FK_CajaCabecera_UsuarioCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.CajaDetalle (
	IdCajaDetalle INT IDENTITY(1,1) NOT NULL,
	IdCaja INT NOT NULL,
	IdTipoMovimientoCaja INT NOT NULL,
	IdMedioPago INT NULL,
	IdMoneda INT NULL,
	FechaMovimiento DATETIME2 NOT NULL CONSTRAINT DF_CajaDetalle_FechaMovimiento DEFAULT (SYSUTCDATETIME()),
	Concepto NVARCHAR(200) NOT NULL,
	Referencia NVARCHAR(100) NULL,
	Importe DECIMAL(18,2) NOT NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	CONSTRAINT PK_CajaDetalle PRIMARY KEY (IdCajaDetalle),
	CONSTRAINT FK_CajaDetalle_Cajas FOREIGN KEY (IdCaja) REFERENCES dbo.CajaCabecera(IdCaja) ON DELETE CASCADE,
	CONSTRAINT FK_CajaDetalle_TipoMovimiento FOREIGN KEY (IdTipoMovimientoCaja) REFERENCES dbo.TiposMovimientoCaja(IdTipoMovimientoCaja),
	CONSTRAINT FK_CajaDetalle_MedioPago FOREIGN KEY (IdMedioPago) REFERENCES dbo.MediosPago(IdMedioPago),
	CONSTRAINT FK_CajaDetalle_Moneda FOREIGN KEY (IdMoneda) REFERENCES dbo.Monedas(IdMoneda),
	CONSTRAINT FK_CajaDetalle_UsuarioCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.Depositos (
	IdDeposito INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(20) NOT NULL,
	Descripcion NVARCHAR(150) NOT NULL,
	Ubicacion NVARCHAR(200) NULL,
	Activo BIT NOT NULL CONSTRAINT DF_Depositos_Activo DEFAULT (1),
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT  NULL,
	FechaUltimaModificacion DATETIME2 NOT NULL,
	CONSTRAINT PK_Depositos PRIMARY KEY (IdDeposito),
	CONSTRAINT UQ_Depositos_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_Depositos_Usuarios FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.TiposMovimientoStock (
	IdTipoMovimientoStock INT IDENTITY(1,1) NOT NULL,
	Codigo NVARCHAR(30) NOT NULL,
	Descripcion NVARCHAR(100) NOT NULL,
	Signo INT NOT NULL,
	Activo BIT NOT NULL CONSTRAINT DF_TiposMovimientoStock_Activo DEFAULT (1),
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT  NULL,
	FechaUltimaModificacion DATETIME2 NOT NULL,
	CONSTRAINT PK_TiposMovimientoStock PRIMARY KEY (IdTipoMovimientoStock),
	CONSTRAINT UQ_TiposMovimientoStock_Codigo UNIQUE (Codigo),
	CONSTRAINT FK_TiposMovimientoStock_Usuarios FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.Stock (
	IdStock INT IDENTITY(1,1) NOT NULL,
	IdDeposito INT NOT NULL,
	IdProducto INT NOT NULL,
	Cantidad DECIMAL(18,4) NOT NULL CONSTRAINT DF_Stock_Cantidad DEFAULT (0),
	FechaUltimoMovimiento DATETIME2 NULL,
	CONSTRAINT PK_Stock PRIMARY KEY (IdStock),
	CONSTRAINT UQ_Stock_DepositoProducto UNIQUE (IdDeposito, IdProducto),
	CONSTRAINT FK_Stock_Depositos FOREIGN KEY (IdDeposito) REFERENCES dbo.Depositos(IdDeposito),
	CONSTRAINT FK_Stock_Productos FOREIGN KEY (IdProducto) REFERENCES dbo.Productos(IdProducto)
);

CREATE TABLE dbo.MovimientosStock (
	IdMovimientoStock INT IDENTITY(1,1) NOT NULL,
	IdDeposito INT NOT NULL,
	IdTipoMovimientoStock INT NOT NULL,
	FechaMovimiento DATETIME2 NOT NULL CONSTRAINT DF_MovimientosStock_FechaMovimiento DEFAULT (SYSUTCDATETIME()),
	IdUsuarioResponsable INT NOT NULL,
	Referencia NVARCHAR(100) NULL,
	Observaciones NVARCHAR(500) NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	CONSTRAINT PK_MovimientosStock PRIMARY KEY (IdMovimientoStock),
	CONSTRAINT FK_MovimientosStock_Depositos FOREIGN KEY (IdDeposito) REFERENCES dbo.Depositos(IdDeposito),
	CONSTRAINT FK_MovimientosStock_Tipo FOREIGN KEY (IdTipoMovimientoStock) REFERENCES dbo.TiposMovimientoStock(IdTipoMovimientoStock),
	CONSTRAINT FK_MovimientosStock_Responsable FOREIGN KEY (IdUsuarioResponsable) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_MovimientosStock_UsuarioCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.MovimientoStockDetalle (
	IdMovimientoStockDetalle INT IDENTITY(1,1) NOT NULL,
	IdMovimientoStock INT NOT NULL,
	IdProducto INT NOT NULL,
	Cantidad DECIMAL(18,4) NOT NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	CONSTRAINT PK_MovimientoStockDetalle PRIMARY KEY (IdMovimientoStockDetalle),
	CONSTRAINT FK_MovimientoStockDetalle_Movimiento FOREIGN KEY (IdMovimientoStock) REFERENCES dbo.MovimientosStock(IdMovimientoStock) ON DELETE CASCADE,
	CONSTRAINT FK_MovimientoStockDetalle_Producto FOREIGN KEY (IdProducto) REFERENCES dbo.Productos(IdProducto),
	CONSTRAINT FK_MovimientoStockDetalle_Usuario FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.Contactos (
	IdContacto INT IDENTITY(1,1) NOT NULL,
	Nombre NVARCHAR(100) NOT NULL,
	Apellido NVARCHAR(100) NULL,
	Cargo NVARCHAR(100) NULL,
	Telefono NVARCHAR(50) NULL,
	Celular NVARCHAR(50) NULL,
	Email NVARCHAR(150) NULL,
	Activo BIT NOT NULL CONSTRAINT DF_Contactos_Activo DEFAULT (1),
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	CONSTRAINT PK_Contactos PRIMARY KEY (IdContacto),
	CONSTRAINT FK_Contactos_Usuarios FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.ClienteContactos (
	IdCliente INT NOT NULL,
	IdContacto INT NOT NULL,
	EsPrincipal BIT NOT NULL CONSTRAINT DF_ClienteContactos_EsPrincipal DEFAULT (0),
	CONSTRAINT PK_ClienteContactos PRIMARY KEY (IdCliente, IdContacto),
	CONSTRAINT FK_ClienteContactos_Clientes FOREIGN KEY (IdCliente) REFERENCES dbo.Clientes(IdCliente) ON DELETE CASCADE,
	CONSTRAINT FK_ClienteContactos_Contactos FOREIGN KEY (IdContacto) REFERENCES dbo.Contactos(IdContacto) ON DELETE CASCADE
);

CREATE TABLE dbo.Domicilios (
	IdDomicilio INT IDENTITY(1,1) NOT NULL,
	TipoReferencia NVARCHAR(1) NOT NULL,
	IdReferencia INT NOT NULL,
	Domicilio NVARCHAR(250) NOT NULL,
	Ciudad NVARCHAR(100) NOT NULL,
	Localidad NVARCHAR(100) NULL,
	IdProvincia INT NOT NULL,
	CodigoPostal NVARCHAR(20) NULL,
	Activo BIT NOT NULL CONSTRAINT DF_Domicilios_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_Domicilios PRIMARY KEY (IdDomicilio),
	CONSTRAINT CK_Domicilios_TipoReferencia CHECK (TipoReferencia IN ('C','P')),
	CONSTRAINT CK_Domicilios_IdReferencia CHECK (IdReferencia > 0),
	CONSTRAINT FK_Domicilios_Provincias FOREIGN KEY (IdProvincia) REFERENCES dbo.Provincias(IdProvincia),
	CONSTRAINT FK_Domicilios_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_Domicilios_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);
CREATE INDEX IX_Domicilios_TipoReferencia_IdReferencia ON dbo.Domicilios(TipoReferencia, IdReferencia);
CREATE INDEX IX_Domicilios_IdProvincia ON dbo.Domicilios(IdProvincia);

CREATE TABLE dbo.Telefonos (
	IdTelefono INT IDENTITY(1,1) NOT NULL,
	TipoReferencia NVARCHAR(1) NOT NULL,
	IdReferencia INT NOT NULL,
	NumeroTelefono NVARCHAR(50) NOT NULL,
	TipoTelefono NVARCHAR(7) NOT NULL,
	Activo BIT NOT NULL CONSTRAINT DF_Telefonos_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_Telefonos PRIMARY KEY (IdTelefono),
	CONSTRAINT CK_Telefonos_TipoReferencia CHECK (TipoReferencia IN ('C','P')),
	CONSTRAINT CK_Telefonos_IdReferencia CHECK (IdReferencia > 0),
	CONSTRAINT FK_Telefonos_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_Telefonos_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);
CREATE INDEX IX_Telefonos_TipoReferencia_IdReferencia ON dbo.Telefonos(TipoReferencia, IdReferencia);

CREATE TABLE dbo.Emails (
	IdEmail INT IDENTITY(1,1) NOT NULL,
	TipoReferencia NVARCHAR(1) NOT NULL,
	IdReferencia INT NOT NULL,
	Email NVARCHAR(254) NOT NULL,
	Activo BIT NOT NULL CONSTRAINT DF_Emails_Activo DEFAULT (1),
	FechaBaja DATETIME2 NULL,
	IdUsuarioCreacion INT NOT NULL,
	FechaCreacion DATETIME2 NOT NULL,
	IdUsuarioUltimaModificacion INT NULL,
	FechaUltimaModificacion DATETIME2 NULL,
	CONSTRAINT PK_Emails PRIMARY KEY (IdEmail),
	CONSTRAINT CK_Emails_TipoReferencia CHECK (TipoReferencia IN ('C','P')),
	CONSTRAINT CK_Emails_IdReferencia CHECK (IdReferencia > 0),
	CONSTRAINT FK_Emails_UsuariosCreacion FOREIGN KEY (IdUsuarioCreacion) REFERENCES dbo.Usuarios(IdUsuario),
	CONSTRAINT FK_Emails_UsuariosModificacion FOREIGN KEY (IdUsuarioUltimaModificacion) REFERENCES dbo.Usuarios(IdUsuario)
);
CREATE INDEX IX_Emails_TipoReferencia_IdReferencia ON dbo.Emails(TipoReferencia, IdReferencia);
GO

/* Seed Provincias */
DECLARE @IdUsuarioSistema INT = (SELECT TOP (1) IdUsuario FROM dbo.Usuarios WHERE Usuario = N'alevera01');
IF @IdUsuarioSistema IS NULL SET @IdUsuarioSistema = 1;

;WITH ProvinciasSeed AS
(
	SELECT * FROM (VALUES
		(N'01', N'Buenos Aires',                N'01'),
		(N'02', N'Catamarca',                   N'02'),
		(N'03', N'Chaco',                       N'03'),
		(N'04', N'Chubut',                      N'04'),
		(N'05', N'Córdoba',                     N'05'),
		(N'06', N'Corrientes',                  N'06'),
		(N'07', N'Entre Ríos',                  N'07'),
		(N'08', N'Formosa',                     N'08'),
		(N'09', N'Jujuy',                       N'09'),
		(N'10', N'La Pampa',                    N'10'),
		(N'11', N'La Rioja',                    N'11'),
		(N'12', N'Mendoza',                     N'12'),
		(N'13', N'Misiones',                    N'13'),
		(N'14', N'Neuquén',                     N'14'),
		(N'15', N'Río Negro',                   N'15'),
		(N'16', N'Salta',                       N'16'),
		(N'17', N'San Juan',                    N'17'),
		(N'18', N'San Luis',                    N'18'),
		(N'19', N'Santa Cruz',                  N'19'),
		(N'20', N'Santa Fe',                    N'20'),
		(N'21', N'Santiago del Estero',         N'21'),
		(N'22', N'Tierra del Fuego',            N'22'),
		(N'23', N'Tucumán',                     N'23'),
		(N'24', N'Ciudad Autónoma de Buenos Aires', N'24')
	) s(Codigo, Descripcion, CodigoAFIP)
)
MERGE dbo.Provincias AS target
USING ProvinciasSeed AS source
ON target.Codigo = source.Codigo
WHEN MATCHED THEN
	UPDATE SET
		target.Descripcion = source.Descripcion,
		target.CodigoAFIP = source.CodigoAFIP,
		target.Activo = 1,
		target.FechaBaja = NULL,
		target.IdUsuarioUltimaModificacion = @IdUsuarioSistema,
		target.FechaUltimaModificacion = SYSUTCDATETIME()
WHEN NOT MATCHED BY TARGET THEN
	INSERT (Codigo, Descripcion, CodigoAFIP, Activo, FechaBaja, IdUsuarioCreacion, FechaCreacion, IdUsuarioUltimaModificacion, FechaUltimaModificacion)
	VALUES (source.Codigo, source.Descripcion, source.CodigoAFIP, 1, NULL, @IdUsuarioSistema, SYSUTCDATETIME(), NULL, NULL);
GO
