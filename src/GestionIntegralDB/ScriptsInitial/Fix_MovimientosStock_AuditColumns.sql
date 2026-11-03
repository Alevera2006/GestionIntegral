USE GestionIntegralDB;
GO

/*
  Fix: Align dbo.MovimientosStock with EF model
  - Adds missing audit columns if they do not exist
  - Adds FK for IdUsuarioUltimaModificacion if missing
*/

IF COL_LENGTH('dbo.MovimientosStock', 'IdUsuarioUltimaModificacion') IS NULL
BEGIN
	ALTER TABLE dbo.MovimientosStock
	ADD IdUsuarioUltimaModificacion INT NULL;
END
GO

IF COL_LENGTH('dbo.MovimientosStock', 'FechaUltimaModificacion') IS NULL
BEGIN
	ALTER TABLE dbo.MovimientosStock
	ADD FechaUltimaModificacion DATETIME2(3) NULL;
END
GO

IF NOT EXISTS (
	SELECT 1
	FROM sys.foreign_keys
	WHERE name = 'FK_MovimientosStock_UsuarioModificacion'
	  AND parent_object_id = OBJECT_ID('dbo.MovimientosStock')
)
AND COL_LENGTH('dbo.MovimientosStock', 'IdUsuarioUltimaModificacion') IS NOT NULL
AND OBJECT_ID('dbo.Usuarios', 'U') IS NOT NULL
BEGIN
	ALTER TABLE dbo.MovimientosStock
	ADD CONSTRAINT FK_MovimientosStock_UsuarioModificacion
		FOREIGN KEY (IdUsuarioUltimaModificacion)
		REFERENCES dbo.Usuarios(IdUsuario);
END
GO
