-- ============================================================================
-- MediStack — Capa de Base de Datos
-- Requerimientos origen: Req 01 (Registrar Usuarios)
-- Descripción: Catálogo maestro de roles del sistema (RBAC).
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Roles] (
        [RolId] INT IDENTITY(1,1) NOT NULL,
        [Codigo] NVARCHAR(50) NOT NULL,
        [Nombre] NVARCHAR(100) NOT NULL,
        [Descripcion] NVARCHAR(255) NULL,
        [Activo] BIT NOT NULL CONSTRAINT [DF_Roles_Activo] DEFAULT (1),
        [FechaCreacion] DATETIME2(7) NOT NULL CONSTRAINT [DF_Roles_FechaCreacion] DEFAULT (SYSUTCDATETIME()),

        CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RolId] ASC),
        CONSTRAINT [UQ_Roles_Codigo] UNIQUE NONCLUSTERED ([Codigo] ASC)
    );
END
GO
