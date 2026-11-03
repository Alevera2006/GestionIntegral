IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [dbo].[AlicuotasImpuesto] (
    [IdAlicuotaImpuesto] int NOT NULL IDENTITY,
    [IdTipoImpuesto] int NOT NULL,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [Porcentaje] decimal(8,4) NOT NULL,
    [FechaDesde] date NULL,
    [FechaHasta] date NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_AlicuotasImpuesto] PRIMARY KEY ([IdAlicuotaImpuesto])
);

CREATE TABLE [dbo].[CajaCabecera] (
    [IdCaja] bigint NOT NULL IDENTITY,
    [FechaCaja] date NOT NULL,
    [IdUsuarioResponsable] int NOT NULL,
    [IdEstadoCaja] int NOT NULL,
    [SaldoInicial] decimal(18,2) NOT NULL DEFAULT 0.0,
    [SaldoFinal] decimal(18,2) NULL,
    [FechaApertura] datetime2(3) NOT NULL DEFAULT (SYSUTCDATETIME()),
    [FechaCierre] datetime2(3) NULL,
    [Observaciones] nvarchar(500) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_CajaCabecera] PRIMARY KEY ([IdCaja])
);

CREATE TABLE [dbo].[CajaDetalle] (
    [IdCajaDetalle] bigint NOT NULL IDENTITY,
    [IdCaja] bigint NOT NULL,
    [IdTipoMovimientoCaja] int NOT NULL,
    [IdMedioPago] int NULL,
    [IdMoneda] int NULL,
    [FechaMovimiento] datetime2(3) NOT NULL DEFAULT (SYSUTCDATETIME()),
    [Concepto] nvarchar(200) NOT NULL,
    [Referencia] nvarchar(100) NULL,
    [Importe] decimal(18,2) NOT NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_CajaDetalle] PRIMARY KEY ([IdCajaDetalle]),
    CONSTRAINT [FK_CajaDetalle_Cajas] FOREIGN KEY ([IdCaja]) REFERENCES [dbo].[CajaCabecera] ([IdCaja]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Categorias] (
    [IdCategoria] int NOT NULL IDENTITY,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_Categorias] PRIMARY KEY ([IdCategoria])
);

CREATE TABLE [dbo].[ClienteContactos] (
    [IdCliente] int NOT NULL,
    [IdContacto] int NOT NULL,
    [EsPrincipal] bit NOT NULL DEFAULT CAST(0 AS bit),
    CONSTRAINT [PK_ClienteContactos] PRIMARY KEY ([IdCliente], [IdContacto])
);

CREATE TABLE [dbo].[Clientes] (
    [IdCliente] int NOT NULL IDENTITY,
    [Codigo] nvarchar(20) NOT NULL,
    [RazonSocial] nvarchar(150) NOT NULL,
    [NombreFantasia] nvarchar(150) NULL,
    [CUIT] nvarchar(13) NULL,
    [IdCondicionPago] int NULL,
    [Observaciones] nvarchar(500) NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([IdCliente])
);

CREATE TABLE [dbo].[CondicionesIVA] (
    [IdCondicionIVA] int NOT NULL IDENTITY,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_CondicionesIVA] PRIMARY KEY ([IdCondicionIVA])
);

CREATE TABLE [dbo].[CondicionesPago] (
    [IdCondicionPago] int NOT NULL IDENTITY,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [Dias] int NOT NULL DEFAULT 0,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_CondicionesPago] PRIMARY KEY ([IdCondicionPago])
);

CREATE TABLE [dbo].[Contactos] (
    [IdContacto] int NOT NULL IDENTITY,
    [Nombre] nvarchar(100) NOT NULL,
    [Apellido] nvarchar(100) NULL,
    [Cargo] nvarchar(100) NULL,
    [Telefono] nvarchar(50) NULL,
    [Celular] nvarchar(50) NULL,
    [Email] nvarchar(150) NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_Contactos] PRIMARY KEY ([IdContacto])
);

CREATE TABLE [dbo].[Depositos] (
    [IdDeposito] int NOT NULL IDENTITY,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(150) NOT NULL,
    [Ubicacion] nvarchar(200) NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_Depositos] PRIMARY KEY ([IdDeposito])
);

CREATE TABLE [dbo].[Domicilios] (
    [IdDomicilio] int NOT NULL IDENTITY,
    [TipoReferencia] nvarchar(1) NOT NULL,
    [IdReferencia] int NOT NULL,
    [Domicilio] nvarchar(250) NOT NULL,
    [Ciudad] nvarchar(100) NOT NULL,
    [Localidad] nvarchar(100) NULL,
    [IdProvincia] int NOT NULL,
    [CodigoPostal] nvarchar(20) NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_Domicilios] PRIMARY KEY ([IdDomicilio]),
    CONSTRAINT [CK_Domicilios_IdReferencia] CHECK (IdReferencia > 0),
    CONSTRAINT [CK_Domicilios_TipoReferencia] CHECK (TipoReferencia IN ('C','P'))
);

CREATE TABLE [dbo].[Emails] (
    [IdEmail] int NOT NULL IDENTITY,
    [TipoReferencia] nvarchar(1) NOT NULL,
    [IdReferencia] int NOT NULL,
    [Email] nvarchar(254) NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_Emails] PRIMARY KEY ([IdEmail]),
    CONSTRAINT [CK_Emails_IdReferencia] CHECK (IdReferencia > 0),
    CONSTRAINT [CK_Emails_TipoReferencia] CHECK (TipoReferencia IN ('C','P'))
);

CREATE TABLE [dbo].[EstadosCaja] (
    [IdEstadoCaja] int NOT NULL IDENTITY,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_EstadosCaja] PRIMARY KEY ([IdEstadoCaja])
);

CREATE TABLE [dbo].[EstadosCompra] (
    [IdEstadoCompra] int NOT NULL IDENTITY,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    CONSTRAINT [PK_EstadosCompra] PRIMARY KEY ([IdEstadoCompra])
);

CREATE TABLE [dbo].[EstadosFactura] (
    [IdEstadoFactura] int NOT NULL IDENTITY,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    CONSTRAINT [PK_EstadosFactura] PRIMARY KEY ([IdEstadoFactura])
);

CREATE TABLE [dbo].[EstadosPedido] (
    [IdEstadoPedido] int NOT NULL IDENTITY,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    CONSTRAINT [PK_EstadosPedido] PRIMARY KEY ([IdEstadoPedido])
);

CREATE TABLE [dbo].[EstadosProceso] (
    [IdEstadoProceso] int NOT NULL IDENTITY,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    CONSTRAINT [PK_EstadosProceso] PRIMARY KEY ([IdEstadoProceso])
);

CREATE TABLE [dbo].[Facturas] (
    [IdFactura] bigint NOT NULL IDENTITY,
    [IdCliente] int NULL,
    [NombreCliente] nvarchar(150) NOT NULL,
    [IdTipoComprobante] int NOT NULL,
    [NumeroComprobante] nvarchar(20) NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [IdCondicionPago] int NOT NULL,
    [Subtotal] decimal(18,2) NOT NULL,
    [ImporteIva] decimal(18,2) NOT NULL,
    [Total] decimal(18,2) NOT NULL,
    [Observaciones] nvarchar(500) NOT NULL,
    [Activo] bit NOT NULL,
    [FechaBaja] datetime2 NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2 NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2 NULL,
    CONSTRAINT [PK_Facturas] PRIMARY KEY ([IdFactura]),
    CONSTRAINT [FK_Facturas_Clientes_IdCliente] FOREIGN KEY ([IdCliente]) REFERENCES [dbo].[Clientes] ([IdCliente]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Facturas_CondicionesPago_IdCondicionPago] FOREIGN KEY ([IdCondicionPago]) REFERENCES [dbo].[CondicionesPago] ([IdCondicionPago]) ON DELETE NO ACTION
);

CREATE TABLE [dbo].[FacturasDetalle] (
    [IdFacturaDetalle] bigint NOT NULL IDENTITY,
    [IdFactura] bigint NOT NULL,
    [IdProducto] int NOT NULL,
    [Descripcion] nvarchar(150) NOT NULL,
    [Cantidad] decimal(18,4) NOT NULL,
    [PrecioUnitario] decimal(18,4) NOT NULL,
    [PorcentajeIva] decimal(18,2) NOT NULL,
    [Subtotal] decimal(18,2) NOT NULL,
    [ImporteIva] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_FacturasDetalle] PRIMARY KEY ([IdFacturaDetalle]),
    CONSTRAINT [FK_FacturasDetalle_Facturas_IdFactura] FOREIGN KEY ([IdFactura]) REFERENCES [dbo].[Facturas] ([IdFactura]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[ListaPreciosDetalle] (
    [IdListaPrecioDetalle] int NOT NULL IDENTITY,
    [IdListaPrecio] int NOT NULL,
    [IdProducto] int NOT NULL,
    [Precio] decimal(18,4) NOT NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_ListaPreciosDetalle] PRIMARY KEY ([IdListaPrecioDetalle])
);

CREATE TABLE [dbo].[ListasPrecios] (
    [IdListaPrecio] int NOT NULL IDENTITY,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [FechaDesde] datetime2(3) NULL,
    [FechaHasta] datetime2(3) NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_ListasPrecios] PRIMARY KEY ([IdListaPrecio])
);

CREATE TABLE [dbo].[MediosPago] (
    [IdMedioPago] int NOT NULL IDENTITY,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [EsEfectivo] bit NOT NULL DEFAULT CAST(0 AS bit),
    [RequiereReferencia] bit NOT NULL DEFAULT CAST(0 AS bit),
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_MediosPago] PRIMARY KEY ([IdMedioPago])
);

CREATE TABLE [dbo].[PagosClientes] (
    [IdPagoCliente] bigint NOT NULL IDENTITY,
    [IdCliente] int NOT NULL,
    [IdFactura] bigint NULL,
    [Fecha] datetime2 NOT NULL,
    [Monto] decimal(18,2) NOT NULL,
    [IdMedioPago] int NOT NULL,
    [NumeroCheque] nvarchar(50) NULL,
    [Banco] nvarchar(100) NULL,
    [FechaVencimientoCheque] datetime2 NULL,
    [Observaciones] nvarchar(500) NOT NULL,
    [Activo] bit NOT NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2 NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2 NULL,
    CONSTRAINT [PK_PagosClientes] PRIMARY KEY ([IdPagoCliente]),
    CONSTRAINT [FK_PagosClientes_Clientes_IdCliente] FOREIGN KEY ([IdCliente]) REFERENCES [dbo].[Clientes] ([IdCliente]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PagosClientes_Facturas_IdFactura] FOREIGN KEY ([IdFactura]) REFERENCES [dbo].[Facturas] ([IdFactura]) ON DELETE SET NULL,
    CONSTRAINT [FK_PagosClientes_MediosPago_IdMedioPago] FOREIGN KEY ([IdMedioPago]) REFERENCES [dbo].[MediosPago] ([IdMedioPago]) ON DELETE NO ACTION
);

CREATE TABLE [dbo].[ValoresCartera] (
    [IdValorCartera] bigint NOT NULL IDENTITY,
    [IdPagoCliente] bigint NOT NULL,
    [IdCliente] int NOT NULL,
    [TipoValor] nvarchar(50) NOT NULL,
    [Numero] nvarchar(50) NOT NULL,
    [Banco] nvarchar(100) NULL,
    [FechaEmision] datetime2 NULL,
    [FechaVencimiento] datetime2 NULL,
    [Importe] decimal(18,2) NOT NULL,
    [Estado] nvarchar(50) NOT NULL,
    [Observaciones] nvarchar(500) NULL,
    [Activo] bit NOT NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2 NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2 NULL,
    CONSTRAINT [PK_ValoresCartera] PRIMARY KEY ([IdValorCartera]),
    CONSTRAINT [FK_ValoresCartera_Clientes_IdCliente] FOREIGN KEY ([IdCliente]) REFERENCES [dbo].[Clientes] ([IdCliente]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ValoresCartera_PagosClientes_IdPagoCliente] FOREIGN KEY ([IdPagoCliente]) REFERENCES [dbo].[PagosClientes] ([IdPagoCliente]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Monedas] (
    [IdMoneda] int NOT NULL IDENTITY,
    [Codigo] nvarchar(10) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [Simbolo] nvarchar(10) NULL,
    [CantidadDecimales] tinyint NOT NULL DEFAULT CAST(2 AS tinyint),
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_Monedas] PRIMARY KEY ([IdMoneda])
);

CREATE TABLE [dbo].[MotivosAnulacion] (
    [IdMotivoAnulacion] int NOT NULL IDENTITY,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(200) NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    CONSTRAINT [PK_MotivosAnulacion] PRIMARY KEY ([IdMotivoAnulacion])
);

CREATE TABLE [dbo].[MovimientosStock] (
    [IdMovimientoStock] bigint NOT NULL IDENTITY,
    [IdDeposito] int NOT NULL,
    [IdTipoMovimientoStock] int NOT NULL,
    [FechaMovimiento] datetime2(3) NOT NULL DEFAULT (SYSUTCDATETIME()),
    [IdUsuarioResponsable] int NOT NULL,
    [Referencia] nvarchar(100) NULL,
    [Observaciones] nvarchar(500) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_MovimientosStock] PRIMARY KEY ([IdMovimientoStock]),
    CONSTRAINT [FK_MovimientosStock_Depositos] FOREIGN KEY ([IdDeposito]) REFERENCES [dbo].[Depositos] ([IdDeposito]) ON DELETE NO ACTION
);

CREATE TABLE [dbo].[MovimientoStockDetalle] (
    [IdMovimientoStockDetalle] bigint NOT NULL IDENTITY,
    [IdMovimientoStock] bigint NOT NULL,
    [IdProducto] int NOT NULL,
    [Cantidad] decimal(18,4) NOT NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_MovimientoStockDetalle] PRIMARY KEY ([IdMovimientoStockDetalle]),
    CONSTRAINT [FK_MovimientoStockDetalle_Movimiento] FOREIGN KEY ([IdMovimientoStock]) REFERENCES [dbo].[MovimientosStock] ([IdMovimientoStock]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[PagosProveedores] (
    [IdPagoProveedor] bigint NOT NULL IDENTITY,
    [IdProveedor] int NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [Monto] decimal(18,2) NOT NULL,
    [IdMedioPago] int NOT NULL,
    [NumeroCheque] nvarchar(50) NULL,
    [Banco] nvarchar(100) NULL,
    [FechaVencimientoCheque] datetime2 NULL,
    [Concepto] nvarchar(250) NOT NULL,
    [Observaciones] nvarchar(500) NOT NULL,
    [Activo] bit NOT NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2 NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2 NULL,
    CONSTRAINT [PK_PagosProveedores] PRIMARY KEY ([IdPagoProveedor]),
    CONSTRAINT [FK_PagosProveedores_MediosPago_IdMedioPago] FOREIGN KEY ([IdMedioPago]) REFERENCES [dbo].[MediosPago] ([IdMedioPago]) ON DELETE NO ACTION
);

CREATE TABLE [dbo].[Productos] (
    [IdProducto] int NOT NULL IDENTITY,
    [Codigo] nvarchar(50) NOT NULL,
    [Descripcion] nvarchar(200) NOT NULL,
    [IdCategoria] int NOT NULL,
    [IdSubCategoria] int NULL,
    [CodigoBarras] nvarchar(50) NULL,
    [Costo] decimal(18,4) NULL,
    [StockMinimo] decimal(18,4) NULL,
    [StockMaximo] decimal(18,4) NULL,
    [Observaciones] nvarchar(500) NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_Productos] PRIMARY KEY ([IdProducto]),
    CONSTRAINT [FK_Productos_Categorias] FOREIGN KEY ([IdCategoria]) REFERENCES [dbo].[Categorias] ([IdCategoria]) ON DELETE NO ACTION
);

CREATE TABLE [dbo].[Stock] (
    [IdStock] bigint NOT NULL IDENTITY,
    [IdDeposito] int NOT NULL,
    [IdProducto] int NOT NULL,
    [Cantidad] decimal(18,4) NOT NULL DEFAULT 0.0,
    [FechaUltimoMovimiento] datetime2 NULL,
    CONSTRAINT [PK_Stock] PRIMARY KEY ([IdStock]),
    CONSTRAINT [FK_Stock_Depositos] FOREIGN KEY ([IdDeposito]) REFERENCES [dbo].[Depositos] ([IdDeposito]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Stock_Productos] FOREIGN KEY ([IdProducto]) REFERENCES [dbo].[Productos] ([IdProducto]) ON DELETE NO ACTION
);

CREATE TABLE [dbo].[Proveedores] (
    [IdProveedor] int NOT NULL IDENTITY,
    [Codigo] nvarchar(20) NOT NULL,
    [RazonSocial] nvarchar(150) NOT NULL,
    [NombreFantasia] nvarchar(150) NULL,
    [CUIT] nvarchar(13) NULL,
    [IdCondicionPago] int NULL,
    [Observaciones] nvarchar(500) NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_Proveedores] PRIMARY KEY ([IdProveedor]),
    CONSTRAINT [FK_Proveedores_CondicionesPago] FOREIGN KEY ([IdCondicionPago]) REFERENCES [dbo].[CondicionesPago] ([IdCondicionPago]) ON DELETE NO ACTION
);

CREATE TABLE [dbo].[Provincias] (
    [IdProvincia] int NOT NULL IDENTITY,
    [CodigoAFIP] nvarchar(10) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    CONSTRAINT [PK_Provincias] PRIMARY KEY ([IdProvincia])
);

CREATE TABLE [dbo].[SubCategorias] (
    [IdSubCategoria] int NOT NULL IDENTITY,
    [IdCategoria] int NOT NULL,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_SubCategorias] PRIMARY KEY ([IdSubCategoria]),
    CONSTRAINT [FK_SubCategorias_Categorias] FOREIGN KEY ([IdCategoria]) REFERENCES [dbo].[Categorias] ([IdCategoria]) ON DELETE NO ACTION
);

CREATE TABLE [dbo].[Telefonos] (
    [IdTelefono] int NOT NULL IDENTITY,
    [TipoReferencia] nvarchar(1) NOT NULL,
    [IdReferencia] int NOT NULL,
    [NumeroTelefono] nvarchar(50) NOT NULL,
    [TipoTelefono] nvarchar(7) NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_Telefonos] PRIMARY KEY ([IdTelefono]),
    CONSTRAINT [CK_Telefonos_IdReferencia] CHECK (IdReferencia > 0),
    CONSTRAINT [CK_Telefonos_TipoReferencia] CHECK (TipoReferencia IN ('C','P'))
);

CREATE TABLE [dbo].[TiposComprobante] (
    [IdTipoComprobante] int NOT NULL IDENTITY,
    [CodigoAFIP] nvarchar(10) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    CONSTRAINT [PK_TiposComprobante] PRIMARY KEY ([IdTipoComprobante])
);

CREATE TABLE [dbo].[TiposImpuesto] (
    [IdTipoImpuesto] int NOT NULL IDENTITY,
    [Codigo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_TiposImpuesto] PRIMARY KEY ([IdTipoImpuesto])
);

CREATE TABLE [dbo].[TiposMovimientoCaja] (
    [IdTipoMovimientoCaja] int NOT NULL IDENTITY,
    [Codigo] nvarchar(30) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [Signo] smallint NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_TiposMovimientoCaja] PRIMARY KEY ([IdTipoMovimientoCaja])
);

CREATE TABLE [dbo].[TiposMovimientoStock] (
    [IdTipoMovimientoStock] int NOT NULL IDENTITY,
    [Codigo] nvarchar(30) NOT NULL,
    [Descripcion] nvarchar(100) NOT NULL,
    [Signo] smallint NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_TiposMovimientoStock] PRIMARY KEY ([IdTipoMovimientoStock])
);

CREATE TABLE [dbo].[TiposUsuario] (
    [IdTipoUsuario] int NOT NULL IDENTITY,
    [Descripcion] nvarchar(100) NOT NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_TiposUsuario] PRIMARY KEY ([IdTipoUsuario])
);

CREATE TABLE [dbo].[Usuarios] (
    [IdUsuario] int NOT NULL IDENTITY,
    [Nombre] nvarchar(100) NOT NULL,
    [Apellido] nvarchar(100) NOT NULL,
    [IdTipoUsuario] int NOT NULL,
    [Usuario] nvarchar(100) NOT NULL,
    [ClaveHash] nvarchar(500) NOT NULL,
    [Bloqueado] bit NOT NULL DEFAULT CAST(0 AS bit),
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [FechaBaja] datetime2(3) NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(3) NOT NULL,
    [IdUsuarioUltimaModificacion] int NULL,
    [FechaUltimaModificacion] datetime2(3) NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([IdUsuario]),
    CONSTRAINT [FK_Usuarios_TiposUsuario] FOREIGN KEY ([IdTipoUsuario]) REFERENCES [dbo].[TiposUsuario] ([IdTipoUsuario]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Usuarios_UsuarioCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Usuarios_UsuarioModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION
);

CREATE INDEX [IX_AlicuotasImpuesto_IdTipoImpuesto] ON [dbo].[AlicuotasImpuesto] ([IdTipoImpuesto]);

CREATE INDEX [IX_AlicuotasImpuesto_IdUsuarioCreacion] ON [dbo].[AlicuotasImpuesto] ([IdUsuarioCreacion]);

CREATE INDEX [IX_AlicuotasImpuesto_IdUsuarioUltimaModificacion] ON [dbo].[AlicuotasImpuesto] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_AlicuotasImpuesto_Codigo] ON [dbo].[AlicuotasImpuesto] ([Codigo]);

CREATE INDEX [IX_CajaCabecera_IdEstadoCaja] ON [dbo].[CajaCabecera] ([IdEstadoCaja]);

CREATE INDEX [IX_CajaCabecera_IdUsuarioCreacion] ON [dbo].[CajaCabecera] ([IdUsuarioCreacion]);

CREATE INDEX [IX_CajaCabecera_IdUsuarioResponsable] ON [dbo].[CajaCabecera] ([IdUsuarioResponsable]);

CREATE INDEX [IX_CajaCabecera_IdUsuarioUltimaModificacion] ON [dbo].[CajaCabecera] ([IdUsuarioUltimaModificacion]);

CREATE INDEX [IX_CajaDetalle_IdCaja] ON [dbo].[CajaDetalle] ([IdCaja]);

CREATE INDEX [IX_CajaDetalle_IdMedioPago] ON [dbo].[CajaDetalle] ([IdMedioPago]);

CREATE INDEX [IX_CajaDetalle_IdMoneda] ON [dbo].[CajaDetalle] ([IdMoneda]);

CREATE INDEX [IX_CajaDetalle_IdTipoMovimientoCaja] ON [dbo].[CajaDetalle] ([IdTipoMovimientoCaja]);

CREATE INDEX [IX_CajaDetalle_IdUsuarioCreacion] ON [dbo].[CajaDetalle] ([IdUsuarioCreacion]);

CREATE INDEX [IX_CajaDetalle_IdUsuarioUltimaModificacion] ON [dbo].[CajaDetalle] ([IdUsuarioUltimaModificacion]);

CREATE INDEX [IX_Categorias_IdUsuarioCreacion] ON [dbo].[Categorias] ([IdUsuarioCreacion]);

CREATE INDEX [IX_Categorias_IdUsuarioUltimaModificacion] ON [dbo].[Categorias] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_Categorias_Codigo] ON [dbo].[Categorias] ([Codigo]);

CREATE INDEX [IX_ClienteContactos_IdContacto] ON [dbo].[ClienteContactos] ([IdContacto]);

CREATE INDEX [IX_Clientes_IdCondicionPago] ON [dbo].[Clientes] ([IdCondicionPago]);

CREATE INDEX [IX_Clientes_IdUsuarioCreacion] ON [dbo].[Clientes] ([IdUsuarioCreacion]);

CREATE INDEX [IX_Clientes_IdUsuarioUltimaModificacion] ON [dbo].[Clientes] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_Clientes_Codigo] ON [dbo].[Clientes] ([Codigo]);

CREATE INDEX [IX_CondicionesIVA_IdUsuarioCreacion] ON [dbo].[CondicionesIVA] ([IdUsuarioCreacion]);

CREATE INDEX [IX_CondicionesIVA_IdUsuarioUltimaModificacion] ON [dbo].[CondicionesIVA] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_CondicionesIVA_Codigo] ON [dbo].[CondicionesIVA] ([Codigo]);

CREATE INDEX [IX_CondicionesPago_IdUsuarioCreacion] ON [dbo].[CondicionesPago] ([IdUsuarioCreacion]);

CREATE INDEX [IX_CondicionesPago_IdUsuarioUltimaModificacion] ON [dbo].[CondicionesPago] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_CondicionesPago_Codigo] ON [dbo].[CondicionesPago] ([Codigo]);

CREATE INDEX [IX_Contactos_IdUsuarioCreacion] ON [dbo].[Contactos] ([IdUsuarioCreacion]);

CREATE INDEX [IX_Contactos_IdUsuarioUltimaModificacion] ON [dbo].[Contactos] ([IdUsuarioUltimaModificacion]);

CREATE INDEX [IX_Depositos_IdUsuarioCreacion] ON [dbo].[Depositos] ([IdUsuarioCreacion]);

CREATE INDEX [IX_Depositos_IdUsuarioUltimaModificacion] ON [dbo].[Depositos] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_Depositos_Codigo] ON [dbo].[Depositos] ([Codigo]);

CREATE INDEX [IX_Domicilios_IdProvincia] ON [dbo].[Domicilios] ([IdProvincia]);

CREATE INDEX [IX_Domicilios_IdUsuarioCreacion] ON [dbo].[Domicilios] ([IdUsuarioCreacion]);

CREATE INDEX [IX_Domicilios_IdUsuarioUltimaModificacion] ON [dbo].[Domicilios] ([IdUsuarioUltimaModificacion]);

CREATE INDEX [IX_Domicilios_TipoReferencia_IdReferencia] ON [dbo].[Domicilios] ([TipoReferencia], [IdReferencia]);

CREATE INDEX [IX_Emails_IdUsuarioCreacion] ON [dbo].[Emails] ([IdUsuarioCreacion]);

CREATE INDEX [IX_Emails_IdUsuarioUltimaModificacion] ON [dbo].[Emails] ([IdUsuarioUltimaModificacion]);

CREATE INDEX [IX_Emails_TipoReferencia_IdReferencia] ON [dbo].[Emails] ([TipoReferencia], [IdReferencia]);

CREATE INDEX [IX_EstadosCaja_IdUsuarioCreacion] ON [dbo].[EstadosCaja] ([IdUsuarioCreacion]);

CREATE INDEX [IX_EstadosCaja_IdUsuarioUltimaModificacion] ON [dbo].[EstadosCaja] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_EstadosCaja_Codigo] ON [dbo].[EstadosCaja] ([Codigo]);

CREATE INDEX [IX_EstadosCompra_IdUsuarioCreacion] ON [dbo].[EstadosCompra] ([IdUsuarioCreacion]);

CREATE INDEX [IX_EstadosCompra_IdUsuarioUltimaModificacion] ON [dbo].[EstadosCompra] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_EstadosCompra_Codigo] ON [dbo].[EstadosCompra] ([Codigo]);

CREATE INDEX [IX_EstadosFactura_IdUsuarioCreacion] ON [dbo].[EstadosFactura] ([IdUsuarioCreacion]);

CREATE INDEX [IX_EstadosFactura_IdUsuarioUltimaModificacion] ON [dbo].[EstadosFactura] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_EstadosFactura_Codigo] ON [dbo].[EstadosFactura] ([Codigo]);

CREATE INDEX [IX_EstadosPedido_IdUsuarioCreacion] ON [dbo].[EstadosPedido] ([IdUsuarioCreacion]);

CREATE INDEX [IX_EstadosPedido_IdUsuarioUltimaModificacion] ON [dbo].[EstadosPedido] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_EstadosPedido_Codigo] ON [dbo].[EstadosPedido] ([Codigo]);

CREATE INDEX [IX_EstadosProceso_IdUsuarioCreacion] ON [dbo].[EstadosProceso] ([IdUsuarioCreacion]);

CREATE INDEX [IX_EstadosProceso_IdUsuarioUltimaModificacion] ON [dbo].[EstadosProceso] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_EstadosProceso_Codigo] ON [dbo].[EstadosProceso] ([Codigo]);

CREATE INDEX [IX_Facturas_IdCliente] ON [dbo].[Facturas] ([IdCliente]);

CREATE INDEX [IX_Facturas_IdCondicionPago] ON [dbo].[Facturas] ([IdCondicionPago]);

CREATE INDEX [IX_Facturas_IdTipoComprobante] ON [dbo].[Facturas] ([IdTipoComprobante]);

CREATE INDEX [IX_FacturasDetalle_IdFactura] ON [dbo].[FacturasDetalle] ([IdFactura]);

CREATE INDEX [IX_FacturasDetalle_IdProducto] ON [dbo].[FacturasDetalle] ([IdProducto]);

CREATE INDEX [IX_ListaPreciosDetalle_IdProducto] ON [dbo].[ListaPreciosDetalle] ([IdProducto]);

CREATE INDEX [IX_ListaPreciosDetalle_IdUsuarioCreacion] ON [dbo].[ListaPreciosDetalle] ([IdUsuarioCreacion]);

CREATE INDEX [IX_ListaPreciosDetalle_IdUsuarioUltimaModificacion] ON [dbo].[ListaPreciosDetalle] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_ListaPreciosDetalle] ON [dbo].[ListaPreciosDetalle] ([IdListaPrecio], [IdProducto]);

CREATE INDEX [IX_ListasPrecios_IdUsuarioCreacion] ON [dbo].[ListasPrecios] ([IdUsuarioCreacion]);

CREATE INDEX [IX_ListasPrecios_IdUsuarioUltimaModificacion] ON [dbo].[ListasPrecios] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_ListasPrecios_Codigo] ON [dbo].[ListasPrecios] ([Codigo]);

CREATE INDEX [IX_MediosPago_IdUsuarioCreacion] ON [dbo].[MediosPago] ([IdUsuarioCreacion]);

CREATE INDEX [IX_MediosPago_IdUsuarioUltimaModificacion] ON [dbo].[MediosPago] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_MediosPago_Codigo] ON [dbo].[MediosPago] ([Codigo]);

CREATE INDEX [IX_Monedas_IdUsuarioCreacion] ON [dbo].[Monedas] ([IdUsuarioCreacion]);

CREATE INDEX [IX_Monedas_IdUsuarioUltimaModificacion] ON [dbo].[Monedas] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_Monedas_Codigo] ON [dbo].[Monedas] ([Codigo]);

CREATE INDEX [IX_MotivosAnulacion_IdUsuarioCreacion] ON [dbo].[MotivosAnulacion] ([IdUsuarioCreacion]);

CREATE INDEX [IX_MotivosAnulacion_IdUsuarioUltimaModificacion] ON [dbo].[MotivosAnulacion] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_MotivosAnulacion_Codigo] ON [dbo].[MotivosAnulacion] ([Codigo]);

CREATE INDEX [IX_MovimientosStock_IdDeposito] ON [dbo].[MovimientosStock] ([IdDeposito]);

CREATE INDEX [IX_MovimientosStock_IdTipoMovimientoStock] ON [dbo].[MovimientosStock] ([IdTipoMovimientoStock]);

CREATE INDEX [IX_MovimientosStock_IdUsuarioCreacion] ON [dbo].[MovimientosStock] ([IdUsuarioCreacion]);

CREATE INDEX [IX_MovimientosStock_IdUsuarioResponsable] ON [dbo].[MovimientosStock] ([IdUsuarioResponsable]);

CREATE INDEX [IX_MovimientosStock_IdUsuarioUltimaModificacion] ON [dbo].[MovimientosStock] ([IdUsuarioUltimaModificacion]);

CREATE INDEX [IX_MovimientoStockDetalle_IdMovimientoStock] ON [dbo].[MovimientoStockDetalle] ([IdMovimientoStock]);

CREATE INDEX [IX_MovimientoStockDetalle_IdProducto] ON [dbo].[MovimientoStockDetalle] ([IdProducto]);

CREATE INDEX [IX_MovimientoStockDetalle_IdUsuarioCreacion] ON [dbo].[MovimientoStockDetalle] ([IdUsuarioCreacion]);

CREATE INDEX [IX_MovimientoStockDetalle_IdUsuarioUltimaModificacion] ON [dbo].[MovimientoStockDetalle] ([IdUsuarioUltimaModificacion]);

CREATE INDEX [IX_PagosClientes_IdCliente] ON [dbo].[PagosClientes] ([IdCliente]);

CREATE INDEX [IX_PagosClientes_IdFactura] ON [dbo].[PagosClientes] ([IdFactura]);

CREATE INDEX [IX_PagosClientes_IdMedioPago] ON [dbo].[PagosClientes] ([IdMedioPago]);

CREATE INDEX [IX_PagosProveedores_IdMedioPago] ON [dbo].[PagosProveedores] ([IdMedioPago]);

CREATE INDEX [IX_PagosProveedores_IdProveedor] ON [dbo].[PagosProveedores] ([IdProveedor]);

CREATE INDEX [IX_Productos_IdCategoria] ON [dbo].[Productos] ([IdCategoria]);

CREATE INDEX [IX_Productos_IdSubCategoria] ON [dbo].[Productos] ([IdSubCategoria]);

CREATE INDEX [IX_Productos_IdUsuarioCreacion] ON [dbo].[Productos] ([IdUsuarioCreacion]);

CREATE INDEX [IX_Productos_IdUsuarioUltimaModificacion] ON [dbo].[Productos] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_Productos_Codigo] ON [dbo].[Productos] ([Codigo]);

CREATE INDEX [IX_Proveedores_IdCondicionPago] ON [dbo].[Proveedores] ([IdCondicionPago]);

CREATE INDEX [IX_Proveedores_IdUsuarioCreacion] ON [dbo].[Proveedores] ([IdUsuarioCreacion]);

CREATE INDEX [IX_Proveedores_IdUsuarioUltimaModificacion] ON [dbo].[Proveedores] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_Proveedores_Codigo] ON [dbo].[Proveedores] ([Codigo]);

CREATE INDEX [IX_Provincias_IdUsuarioCreacion] ON [dbo].[Provincias] ([IdUsuarioCreacion]);

CREATE INDEX [IX_Provincias_IdUsuarioUltimaModificacion] ON [dbo].[Provincias] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_Provincias_Codigo] ON [dbo].[Provincias] ([Codigo]);

CREATE INDEX [IX_Stock_IdProducto] ON [dbo].[Stock] ([IdProducto]);

CREATE UNIQUE INDEX [UQ_Stock_DepositoProducto] ON [dbo].[Stock] ([IdDeposito], [IdProducto]);

CREATE INDEX [IX_SubCategorias_IdCategoria] ON [dbo].[SubCategorias] ([IdCategoria]);

CREATE INDEX [IX_SubCategorias_IdUsuarioCreacion] ON [dbo].[SubCategorias] ([IdUsuarioCreacion]);

CREATE INDEX [IX_SubCategorias_IdUsuarioUltimaModificacion] ON [dbo].[SubCategorias] ([IdUsuarioUltimaModificacion]);

CREATE INDEX [IX_Telefonos_IdUsuarioCreacion] ON [dbo].[Telefonos] ([IdUsuarioCreacion]);

CREATE INDEX [IX_Telefonos_IdUsuarioUltimaModificacion] ON [dbo].[Telefonos] ([IdUsuarioUltimaModificacion]);

CREATE INDEX [IX_Telefonos_TipoReferencia_IdReferencia] ON [dbo].[Telefonos] ([TipoReferencia], [IdReferencia]);

CREATE INDEX [IX_TiposComprobante_IdUsuarioCreacion] ON [dbo].[TiposComprobante] ([IdUsuarioCreacion]);

CREATE INDEX [IX_TiposComprobante_IdUsuarioUltimaModificacion] ON [dbo].[TiposComprobante] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_TiposComprobante_Codigo] ON [dbo].[TiposComprobante] ([Codigo]);

CREATE INDEX [IX_TiposImpuesto_IdUsuarioCreacion] ON [dbo].[TiposImpuesto] ([IdUsuarioCreacion]);

CREATE INDEX [IX_TiposImpuesto_IdUsuarioUltimaModificacion] ON [dbo].[TiposImpuesto] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_TiposImpuesto_Codigo] ON [dbo].[TiposImpuesto] ([Codigo]);

CREATE INDEX [IX_TiposMovimientoCaja_IdUsuarioCreacion] ON [dbo].[TiposMovimientoCaja] ([IdUsuarioCreacion]);

CREATE INDEX [IX_TiposMovimientoCaja_IdUsuarioUltimaModificacion] ON [dbo].[TiposMovimientoCaja] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_TiposMovimientoCaja_Codigo] ON [dbo].[TiposMovimientoCaja] ([Codigo]);

CREATE INDEX [IX_TiposMovimientoStock_IdUsuarioCreacion] ON [dbo].[TiposMovimientoStock] ([IdUsuarioCreacion]);

CREATE INDEX [IX_TiposMovimientoStock_IdUsuarioUltimaModificacion] ON [dbo].[TiposMovimientoStock] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_TiposMovimientoStock_Codigo] ON [dbo].[TiposMovimientoStock] ([Codigo]);

CREATE INDEX [IX_TiposUsuario_IdUsuarioCreacion] ON [dbo].[TiposUsuario] ([IdUsuarioCreacion]);

CREATE INDEX [IX_TiposUsuario_IdUsuarioUltimaModificacion] ON [dbo].[TiposUsuario] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_TiposUsuario_Descripcion] ON [dbo].[TiposUsuario] ([Descripcion]);

CREATE INDEX [IX_Usuarios_IdTipoUsuario] ON [dbo].[Usuarios] ([IdTipoUsuario]);

CREATE INDEX [IX_Usuarios_IdUsuarioCreacion] ON [dbo].[Usuarios] ([IdUsuarioCreacion]);

CREATE INDEX [IX_Usuarios_IdUsuarioUltimaModificacion] ON [dbo].[Usuarios] ([IdUsuarioUltimaModificacion]);

CREATE UNIQUE INDEX [UQ_Usuarios_Usuario] ON [dbo].[Usuarios] ([Usuario]);

CREATE INDEX [IX_ValoresCartera_IdCliente] ON [dbo].[ValoresCartera] ([IdCliente]);

CREATE UNIQUE INDEX [IX_ValoresCartera_IdPagoCliente] ON [dbo].[ValoresCartera] ([IdPagoCliente]);

ALTER TABLE [dbo].[AlicuotasImpuesto] ADD CONSTRAINT [FK_AlicuotasImpuesto_TiposImpuesto] FOREIGN KEY ([IdTipoImpuesto]) REFERENCES [dbo].[TiposImpuesto] ([IdTipoImpuesto]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[AlicuotasImpuesto] ADD CONSTRAINT [FK_AlicuotasImpuesto_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[AlicuotasImpuesto] ADD CONSTRAINT [FK_AlicuotasImpuesto_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[CajaCabecera] ADD CONSTRAINT [FK_CajaCabecera_Estado] FOREIGN KEY ([IdEstadoCaja]) REFERENCES [dbo].[EstadosCaja] ([IdEstadoCaja]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[CajaCabecera] ADD CONSTRAINT [FK_CajaCabecera_Responsable] FOREIGN KEY ([IdUsuarioResponsable]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[CajaCabecera] ADD CONSTRAINT [FK_CajaCabecera_UsuarioCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[CajaCabecera] ADD CONSTRAINT [FK_CajaCabecera_UsuarioModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[CajaDetalle] ADD CONSTRAINT [FK_CajaDetalle_MedioPago] FOREIGN KEY ([IdMedioPago]) REFERENCES [dbo].[MediosPago] ([IdMedioPago]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[CajaDetalle] ADD CONSTRAINT [FK_CajaDetalle_Moneda] FOREIGN KEY ([IdMoneda]) REFERENCES [dbo].[Monedas] ([IdMoneda]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[CajaDetalle] ADD CONSTRAINT [FK_CajaDetalle_TipoMovimiento] FOREIGN KEY ([IdTipoMovimientoCaja]) REFERENCES [dbo].[TiposMovimientoCaja] ([IdTipoMovimientoCaja]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[CajaDetalle] ADD CONSTRAINT [FK_CajaDetalle_UsuarioCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[CajaDetalle] ADD CONSTRAINT [FK_CajaDetalle_UsuarioModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Categorias] ADD CONSTRAINT [FK_Categorias_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Categorias] ADD CONSTRAINT [FK_Categorias_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[ClienteContactos] ADD CONSTRAINT [FK_ClienteContactos_Clientes] FOREIGN KEY ([IdCliente]) REFERENCES [dbo].[Clientes] ([IdCliente]) ON DELETE CASCADE;

ALTER TABLE [dbo].[ClienteContactos] ADD CONSTRAINT [FK_ClienteContactos_Contactos] FOREIGN KEY ([IdContacto]) REFERENCES [dbo].[Contactos] ([IdContacto]) ON DELETE CASCADE;

ALTER TABLE [dbo].[Clientes] ADD CONSTRAINT [FK_Clientes_CondicionesPago] FOREIGN KEY ([IdCondicionPago]) REFERENCES [dbo].[CondicionesPago] ([IdCondicionPago]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Clientes] ADD CONSTRAINT [FK_Clientes_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Clientes] ADD CONSTRAINT [FK_Clientes_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[CondicionesIVA] ADD CONSTRAINT [FK_CondicionesIVA_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[CondicionesIVA] ADD CONSTRAINT [FK_CondicionesIVA_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[CondicionesPago] ADD CONSTRAINT [FK_CondicionesPago_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[CondicionesPago] ADD CONSTRAINT [FK_CondicionesPago_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Contactos] ADD CONSTRAINT [FK_Contactos_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Contactos] ADD CONSTRAINT [FK_Contactos_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Depositos] ADD CONSTRAINT [FK_Depositos_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Depositos] ADD CONSTRAINT [FK_Depositos_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Domicilios] ADD CONSTRAINT [FK_Domicilios_Provincias] FOREIGN KEY ([IdProvincia]) REFERENCES [dbo].[Provincias] ([IdProvincia]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Domicilios] ADD CONSTRAINT [FK_Domicilios_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Domicilios] ADD CONSTRAINT [FK_Domicilios_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Emails] ADD CONSTRAINT [FK_Emails_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Emails] ADD CONSTRAINT [FK_Emails_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[EstadosCaja] ADD CONSTRAINT [FK_EstadosCaja_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[EstadosCaja] ADD CONSTRAINT [FK_EstadosCaja_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[EstadosCompra] ADD CONSTRAINT [FK_EstadosCompra_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[EstadosCompra] ADD CONSTRAINT [FK_EstadosCompra_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[EstadosFactura] ADD CONSTRAINT [FK_EstadosFactura_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[EstadosFactura] ADD CONSTRAINT [FK_EstadosFactura_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[EstadosPedido] ADD CONSTRAINT [FK_EstadosPedido_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[EstadosPedido] ADD CONSTRAINT [FK_EstadosPedido_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[EstadosProceso] ADD CONSTRAINT [FK_EstadosProceso_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[EstadosProceso] ADD CONSTRAINT [FK_EstadosProceso_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Facturas] ADD CONSTRAINT [FK_Facturas_TiposComprobante_IdTipoComprobante] FOREIGN KEY ([IdTipoComprobante]) REFERENCES [dbo].[TiposComprobante] ([IdTipoComprobante]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[FacturasDetalle] ADD CONSTRAINT [FK_FacturasDetalle_Productos_IdProducto] FOREIGN KEY ([IdProducto]) REFERENCES [dbo].[Productos] ([IdProducto]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[ListaPreciosDetalle] ADD CONSTRAINT [FK_ListaPreciosDetalle_ListasPrecios] FOREIGN KEY ([IdListaPrecio]) REFERENCES [dbo].[ListasPrecios] ([IdListaPrecio]) ON DELETE CASCADE;

ALTER TABLE [dbo].[ListaPreciosDetalle] ADD CONSTRAINT [FK_ListaPreciosDetalle_Productos] FOREIGN KEY ([IdProducto]) REFERENCES [dbo].[Productos] ([IdProducto]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[ListaPreciosDetalle] ADD CONSTRAINT [FK_ListaPreciosDetalle_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[ListaPreciosDetalle] ADD CONSTRAINT [FK_ListaPreciosDetalle_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[ListasPrecios] ADD CONSTRAINT [FK_ListasPrecios_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[ListasPrecios] ADD CONSTRAINT [FK_ListasPrecios_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[MediosPago] ADD CONSTRAINT [FK_MediosPago_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[MediosPago] ADD CONSTRAINT [FK_MediosPago_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Monedas] ADD CONSTRAINT [FK_Monedas_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Monedas] ADD CONSTRAINT [FK_Monedas_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[MotivosAnulacion] ADD CONSTRAINT [FK_MotivosAnulacion_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[MotivosAnulacion] ADD CONSTRAINT [FK_MotivosAnulacion_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[MovimientosStock] ADD CONSTRAINT [FK_MovimientosStock_Responsable] FOREIGN KEY ([IdUsuarioResponsable]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[MovimientosStock] ADD CONSTRAINT [FK_MovimientosStock_UsuarioCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[MovimientosStock] ADD CONSTRAINT [FK_MovimientosStock_UsuarioModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[MovimientosStock] ADD CONSTRAINT [FK_MovimientosStock_Tipo] FOREIGN KEY ([IdTipoMovimientoStock]) REFERENCES [dbo].[TiposMovimientoStock] ([IdTipoMovimientoStock]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[MovimientoStockDetalle] ADD CONSTRAINT [FK_MovimientoStockDetalle_Producto] FOREIGN KEY ([IdProducto]) REFERENCES [dbo].[Productos] ([IdProducto]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[MovimientoStockDetalle] ADD CONSTRAINT [FK_MovimientoStockDetalle_UsuarioCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[MovimientoStockDetalle] ADD CONSTRAINT [FK_MovimientoStockDetalle_UsuarioModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[PagosProveedores] ADD CONSTRAINT [FK_PagosProveedores_Proveedores_IdProveedor] FOREIGN KEY ([IdProveedor]) REFERENCES [dbo].[Proveedores] ([IdProveedor]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Productos] ADD CONSTRAINT [FK_Productos_SubCategorias] FOREIGN KEY ([IdSubCategoria]) REFERENCES [dbo].[SubCategorias] ([IdSubCategoria]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Productos] ADD CONSTRAINT [FK_Productos_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Productos] ADD CONSTRAINT [FK_Productos_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Proveedores] ADD CONSTRAINT [FK_Proveedores_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Proveedores] ADD CONSTRAINT [FK_Proveedores_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Provincias] ADD CONSTRAINT [FK_Provincias_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Provincias] ADD CONSTRAINT [FK_Provincias_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[SubCategorias] ADD CONSTRAINT [FK_SubCategorias_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[SubCategorias] ADD CONSTRAINT [FK_SubCategorias_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Telefonos] ADD CONSTRAINT [FK_Telefonos_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[Telefonos] ADD CONSTRAINT [FK_Telefonos_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[TiposComprobante] ADD CONSTRAINT [FK_TiposComprobante_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[TiposComprobante] ADD CONSTRAINT [FK_TiposComprobante_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[TiposImpuesto] ADD CONSTRAINT [FK_TiposImpuesto_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[TiposImpuesto] ADD CONSTRAINT [FK_TiposImpuesto_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[TiposMovimientoCaja] ADD CONSTRAINT [FK_TiposMovimientoCaja_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[TiposMovimientoCaja] ADD CONSTRAINT [FK_TiposMovimientoCaja_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[TiposMovimientoStock] ADD CONSTRAINT [FK_TiposMovimientoStock_UsuariosCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[TiposMovimientoStock] ADD CONSTRAINT [FK_TiposMovimientoStock_UsuariosModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[TiposUsuario] ADD CONSTRAINT [FK_TiposUsuario_UsuarioCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

ALTER TABLE [dbo].[TiposUsuario] ADD CONSTRAINT [FK_TiposUsuario_UsuarioModificacion] FOREIGN KEY ([IdUsuarioUltimaModificacion]) REFERENCES [dbo].[Usuarios] ([IdUsuario]) ON DELETE NO ACTION;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261102204007_Inicial', N'9.0.0');

COMMIT;
GO

