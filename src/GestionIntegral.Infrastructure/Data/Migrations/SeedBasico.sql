-- Seed básico: tipo de usuario administrador y usuario admin
-- Contraseña del usuario admin: Admin123!
-- El hash fue generado con BCrypt
USE GestionIntegralDB;

-- Deshabilitar temporalmente las FK circulares entre Usuarios y TiposUsuario
ALTER TABLE [dbo].[Usuarios] NOCHECK CONSTRAINT [FK_Usuarios_TiposUsuario];
ALTER TABLE [dbo].[TiposUsuario] NOCHECK CONSTRAINT [FK_TiposUsuario_UsuarioCreacion];
ALTER TABLE [dbo].[TiposUsuario] NOCHECK CONSTRAINT [FK_TiposUsuario_UsuarioModificacion];
ALTER TABLE [dbo].[Usuarios] NOCHECK CONSTRAINT [FK_Usuarios_UsuarioCreacion];
ALTER TABLE [dbo].[Usuarios] NOCHECK CONSTRAINT [FK_Usuarios_UsuarioModificacion];

SET IDENTITY_INSERT [dbo].[TiposUsuario] ON;

INSERT INTO [dbo].[TiposUsuario] (
    [IdTipoUsuario],
    [Descripcion],
    [Activo],
    [IdUsuarioCreacion],
    [FechaCreacion]
) VALUES (
    1,
    'Administrador',
    1,
    1,
    SYSUTCDATETIME()
);

SET IDENTITY_INSERT [dbo].[TiposUsuario] OFF; -- <- importante

SET IDENTITY_INSERT [dbo].[Usuarios] ON;

INSERT INTO [dbo].[Usuarios] (
    [IdUsuario],
    [Nombre],
    [Apellido],
    [IdTipoUsuario],
    [Usuario],
    [ClaveHash],
    [Bloqueado],
    [Activo],
    [IdUsuarioCreacion],
    [FechaCreacion]
) VALUES (
    1,
    'Administrador',
    'Sistema',
    1,
    'admin',
    '$2a$11$Xxc.nO06UGVwtB4WFHSXb.KrGc7.LH7GydtEL0zKv3OmEfR98aJMC',
    0,
    1,
    1,
    SYSUTCDATETIME()
);

SET IDENTITY_INSERT [dbo].[Usuarios] OFF;

-- Rehabilitar las FK
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_TiposUsuario];
ALTER TABLE [dbo].[TiposUsuario] CHECK CONSTRAINT [FK_TiposUsuario_UsuarioCreacion];
ALTER TABLE [dbo].[TiposUsuario] CHECK CONSTRAINT [FK_TiposUsuario_UsuarioModificacion];
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_UsuarioCreacion];
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_UsuarioModificacion];

select * from usuarios