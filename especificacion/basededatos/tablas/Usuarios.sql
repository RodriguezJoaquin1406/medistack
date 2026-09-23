-- ============================================================================
-- MediStack — Capa de Base de Datos
-- Requerimientos origen: Req 01 (Registrar Usuarios)
-- Descripción: Tabla principal de usuarios con credenciales, lockout y datos personales.
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Usuarios] (
        [UsuarioId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_Usuarios_UsuarioId] DEFAULT (NEWSEQUENTIALID()),
        [Username] NVARCHAR(50) NOT NULL,
        [Email] NVARCHAR(256) NOT NULL,
        [PasswordHash] NVARCHAR(256) NOT NULL,
        [PasswordSalt] NVARCHAR(128) NOT NULL,
        [Nombre] NVARCHAR(100) NOT NULL,
        [Apellido] NVARCHAR(100) NOT NULL,
        [NumeroDocumento] NVARCHAR(20) NOT NULL,
        [Telefono] NVARCHAR(30) NULL,
        [RolId] INT NOT NULL,
        [Activo] BIT NOT NULL CONSTRAINT [DF_Usuarios_Activo] DEFAULT (1),
        [IntentosFallidos] INT NOT NULL CONSTRAINT [DF_Usuarios_IntentosFallidos] DEFAULT (0),
        [BloqueadoHasta] DATETIME2(7) NULL,
        [FechaRegistro] DATETIME2(7) NOT NULL CONSTRAINT [DF_Usuarios_FechaRegistro] DEFAULT (SYSUTCDATETIME()),
        [UltimoAcceso] DATETIME2(7) NULL,

        CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED ([UsuarioId] ASC),
        CONSTRAINT [FK_Usuarios_Roles] FOREIGN KEY ([RolId]) REFERENCES [dbo].[Roles] ([RolId]),
        CONSTRAINT [UQ_Usuarios_Email] UNIQUE NONCLUSTERED ([Email] ASC),
        CONSTRAINT [UQ_Usuarios_NumeroDocumento] UNIQUE NONCLUSTERED ([NumeroDocumento] ASC),
        CONSTRAINT [UQ_Usuarios_Username] UNIQUE NONCLUSTERED ([Username] ASC)
    );

    CREATE NONCLUSTERED INDEX [IX_Usuarios_RolId] ON [dbo].[Usuarios] ([RolId] ASC);
    CREATE NONCLUSTERED INDEX [IX_Usuarios_Activo_BloqueadoHasta] ON [dbo].[Usuarios] ([Activo] ASC, [BloqueadoHasta] ASC);
END
GO
