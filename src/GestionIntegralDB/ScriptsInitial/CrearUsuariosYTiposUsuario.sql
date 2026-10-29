-- Script para crear/recuperar las tablas TiposUsuario y Usuarios.
-- Util cuando se ejecuto BorrarUsuariosYTiposUsuario.sql y se quieren
-- recrear solo estas dos tablas con el SuperAdmin inicial.
--
-- Run in SQLCMD mode with -v SuperAdminClaveHash=<BCrypt hash>.
-- Ejemplo:
-- sqlcmd -S tuServidor -d GestionIntegralDB -i CrearUsuariosYTiposUsuario.sql -v SuperAdminClaveHash="$2a$11$..."

SET XACT_ABORT ON;
USE GestionIntegralDB;
GO

DECLARE @SuperAdminClaveHash NVARCHAR(500) = N'$$2a$11$OEcVjIdhJRAEGYdJt8SKLeBcbL0SWLcU1NU6bL4IMZ4AARg1kqLeC';

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

    DECLARE @SuperAdminClaveHash NVARCHAR(500) = N'$(SuperAdminClaveHash)';
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
            N'Super',
            N'Admin',
            @IdTipoAdmin,
            N'SuperAdmin',
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
        u.Usuario,
        u.IdTipoUsuario,
        tu.Descripcion AS TipoUsuarioDescripcion,
        u.Activo,
        u.Bloqueado,
        u.IdUsuarioCreacion,
        u.FechaCreacion
    FROM dbo.Usuarios u
    INNER JOIN dbo.TiposUsuario tu ON u.IdTipoUsuario = tu.IdTipoUsuario
    WHERE u.Usuario = N'SuperAdmin';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
