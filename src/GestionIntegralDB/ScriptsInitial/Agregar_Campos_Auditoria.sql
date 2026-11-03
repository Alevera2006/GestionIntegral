-- Script de migración para agregar/ajustar campos de auditoría faltantes.
-- Verifica que cada columna no exista antes de agregarla.
-- No modifica Stock ni ClienteContactos porque no heredan de EntityBase.

USE GestionIntegralDB;
GO

-- =====================================================
-- Tablas paramétricas: agregar FechaBaja si falta
-- =====================================================

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Depositos') AND name = 'FechaBaja')
    ALTER TABLE dbo.Depositos ADD FechaBaja DATETIME2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.EstadosCaja') AND name = 'FechaBaja')
    ALTER TABLE dbo.EstadosCaja ADD FechaBaja DATETIME2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.MediosPago') AND name = 'FechaBaja')
    ALTER TABLE dbo.MediosPago ADD FechaBaja DATETIME2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Monedas') AND name = 'FechaBaja')
    ALTER TABLE dbo.Monedas ADD FechaBaja DATETIME2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.TiposMovimientoCaja') AND name = 'FechaBaja')
    ALTER TABLE dbo.TiposMovimientoCaja ADD FechaBaja DATETIME2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.TiposMovimientoStock') AND name = 'FechaBaja')
    ALTER TABLE dbo.TiposMovimientoStock ADD FechaBaja DATETIME2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Contactos') AND name = 'FechaBaja')
    ALTER TABLE dbo.Contactos ADD FechaBaja DATETIME2(3) NULL;

-- =====================================================
-- Tablas de movimiento/caja/stock: agregar modificación
-- =====================================================

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.CajaCabecera') AND name = 'IdUsuarioUltimaModificacion')
    ALTER TABLE dbo.CajaCabecera ADD IdUsuarioUltimaModificacion INT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.CajaCabecera') AND name = 'FechaUltimaModificacion')
    ALTER TABLE dbo.CajaCabecera ADD FechaUltimaModificacion DATETIME2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.CajaDetalle') AND name = 'IdUsuarioUltimaModificacion')
    ALTER TABLE dbo.CajaDetalle ADD IdUsuarioUltimaModificacion INT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.CajaDetalle') AND name = 'FechaUltimaModificacion')
    ALTER TABLE dbo.CajaDetalle ADD FechaUltimaModificacion DATETIME2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.MovimientosStock') AND name = 'IdUsuarioUltimaModificacion')
    ALTER TABLE dbo.MovimientosStock ADD IdUsuarioUltimaModificacion INT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.MovimientosStock') AND name = 'FechaUltimaModificacion')
    ALTER TABLE dbo.MovimientosStock ADD FechaUltimaModificacion DATETIME2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.MovimientoStockDetalle') AND name = 'IdUsuarioUltimaModificacion')
    ALTER TABLE dbo.MovimientoStockDetalle ADD IdUsuarioUltimaModificacion INT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.MovimientoStockDetalle') AND name = 'FechaUltimaModificacion')
    ALTER TABLE dbo.MovimientoStockDetalle ADD FechaUltimaModificacion DATETIME2(3) NULL;

-- =====================================================
-- Tablas de relación/contacto
-- =====================================================

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Contactos') AND name = 'IdUsuarioUltimaModificacion')
    ALTER TABLE dbo.Contactos ADD IdUsuarioUltimaModificacion INT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Contactos') AND name = 'FechaUltimaModificacion')
    ALTER TABLE dbo.Contactos ADD FechaUltimaModificacion DATETIME2(3) NULL;

-- =====================================================
-- Tablas paramétricas: agregar modificación si falta
-- =====================================================

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.MediosPago') AND name = 'IdUsuarioUltimaModificacion')
    ALTER TABLE dbo.MediosPago ADD IdUsuarioUltimaModificacion INT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.MediosPago') AND name = 'FechaUltimaModificacion')
    ALTER TABLE dbo.MediosPago ADD FechaUltimaModificacion DATETIME2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Monedas') AND name = 'IdUsuarioUltimaModificacion')
    ALTER TABLE dbo.Monedas ADD IdUsuarioUltimaModificacion INT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Monedas') AND name = 'FechaUltimaModificacion')
    ALTER TABLE dbo.Monedas ADD FechaUltimaModificacion DATETIME2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.TiposMovimientoCaja') AND name = 'IdUsuarioUltimaModificacion')
    ALTER TABLE dbo.TiposMovimientoCaja ADD IdUsuarioUltimaModificacion INT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.TiposMovimientoCaja') AND name = 'FechaUltimaModificacion')
    ALTER TABLE dbo.TiposMovimientoCaja ADD FechaUltimaModificacion DATETIME2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.TiposMovimientoStock') AND name = 'IdUsuarioUltimaModificacion')
    ALTER TABLE dbo.TiposMovimientoStock ADD IdUsuarioUltimaModificacion INT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.TiposMovimientoStock') AND name = 'FechaUltimaModificacion')
    ALTER TABLE dbo.TiposMovimientoStock ADD FechaUltimaModificacion DATETIME2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.EstadosCaja') AND name = 'IdUsuarioUltimaModificacion')
    ALTER TABLE dbo.EstadosCaja ADD IdUsuarioUltimaModificacion INT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.EstadosCaja') AND name = 'FechaUltimaModificacion')
    ALTER TABLE dbo.EstadosCaja ADD FechaUltimaModificacion DATETIME2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Depositos') AND name = 'IdUsuarioUltimaModificacion')
    ALTER TABLE dbo.Depositos ADD IdUsuarioUltimaModificacion INT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Depositos') AND name = 'FechaUltimaModificacion')
    ALTER TABLE dbo.Depositos ADD FechaUltimaModificacion DATETIME2(3) NULL;

PRINT 'Campos de auditoría agregados correctamente.';
GO
