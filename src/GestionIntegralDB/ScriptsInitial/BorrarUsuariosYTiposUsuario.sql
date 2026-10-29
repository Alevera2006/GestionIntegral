SET XACT_ABORT ON;
USE GestionIntegralDB;
GO

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
