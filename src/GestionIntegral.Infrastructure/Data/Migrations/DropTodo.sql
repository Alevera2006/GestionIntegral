-- Script para eliminar todas las tablas del modelo en orden seguro (respetando dependencias)
-- Ejecutar contra la base de datos GestionIntegralDB

-- 1. Tablas dependientes de otras (sin referencias circulares entre si)
DROP TABLE IF EXISTS [dbo].[PagosProveedores];
DROP TABLE IF EXISTS [dbo].[ValoresCartera];
DROP TABLE IF EXISTS [dbo].[PagosClientes];
DROP TABLE IF EXISTS [dbo].[FacturasDetalle];
DROP TABLE IF EXISTS [dbo].[Facturas];
DROP TABLE IF EXISTS [dbo].[CajaDetalle];
DROP TABLE IF EXISTS [dbo].[CajaCabecera];
DROP TABLE IF EXISTS [dbo].[MovimientoStockDetalle];
DROP TABLE IF EXISTS [dbo].[MovimientosStock];
DROP TABLE IF EXISTS [dbo].[Stock];
DROP TABLE IF EXISTS [dbo].[ListaPreciosDetalle];
DROP TABLE IF EXISTS [dbo].[ListasPrecios];
DROP TABLE IF EXISTS [dbo].[Productos];
DROP TABLE IF EXISTS [dbo].[SubCategorias];
DROP TABLE IF EXISTS [dbo].[Categorias];
DROP TABLE IF EXISTS [dbo].[ClienteContactos];
DROP TABLE IF EXISTS [dbo].[Contactos];
DROP TABLE IF EXISTS [dbo].[Domicilios];
DROP TABLE IF EXISTS [dbo].[Emails];
DROP TABLE IF EXISTS [dbo].[Telefonos];
DROP TABLE IF EXISTS [dbo].[Clientes];
DROP TABLE IF EXISTS [dbo].[Proveedores];
DROP TABLE IF EXISTS [dbo].[AlicuotasImpuesto];
DROP TABLE IF EXISTS [dbo].[TiposImpuesto];
DROP TABLE IF EXISTS [dbo].[CondicionesIVA];
DROP TABLE IF EXISTS [dbo].[CondicionesPago];
DROP TABLE IF EXISTS [dbo].[Depositos];
DROP TABLE IF EXISTS [dbo].[EstadosCaja];
DROP TABLE IF EXISTS [dbo].[EstadosCompra];
DROP TABLE IF EXISTS [dbo].[EstadosFactura];
DROP TABLE IF EXISTS [dbo].[EstadosPedido];
DROP TABLE IF EXISTS [dbo].[EstadosProceso];
DROP TABLE IF EXISTS [dbo].[MediosPago];
DROP TABLE IF EXISTS [dbo].[Monedas];
DROP TABLE IF EXISTS [dbo].[MotivosAnulacion];
DROP TABLE IF EXISTS [dbo].[TiposComprobante];
DROP TABLE IF EXISTS [dbo].[TiposMovimientoCaja];
DROP TABLE IF EXISTS [dbo].[TiposMovimientoStock];
DROP TABLE IF EXISTS [dbo].[Provincias];

-- 2. Resolver referencia circular entre Usuarios y TiposUsuario
--    TiposUsuario referencia Usuarios (IdUsuarioCreacion/Modificacion)
--    Usuarios referencia TiposUsuario (IdTipoUsuario)
IF OBJECT_ID(N'[dbo].[FK_Usuarios_TiposUsuario]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[Usuarios] DROP CONSTRAINT [FK_Usuarios_TiposUsuario];

IF OBJECT_ID(N'[dbo].[FK_TiposUsuario_UsuarioCreacion]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[TiposUsuario] DROP CONSTRAINT [FK_TiposUsuario_UsuarioCreacion];

IF OBJECT_ID(N'[dbo].[FK_TiposUsuario_UsuarioModificacion]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[TiposUsuario] DROP CONSTRAINT [FK_TiposUsuario_UsuarioModificacion];

IF OBJECT_ID(N'[dbo].[FK_Usuarios_UsuarioCreacion]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[Usuarios] DROP CONSTRAINT [FK_Usuarios_UsuarioCreacion];

IF OBJECT_ID(N'[dbo].[FK_Usuarios_UsuarioModificacion]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[Usuarios] DROP CONSTRAINT [FK_Usuarios_UsuarioModificacion];

DROP TABLE IF EXISTS [dbo].[TiposUsuario];
DROP TABLE IF EXISTS [dbo].[Usuarios];
DROP TABLE IF EXISTS [dbo].[__EFMigrationsHistory];
