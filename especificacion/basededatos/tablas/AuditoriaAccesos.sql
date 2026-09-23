-- ============================================================================
-- MediStack — Capa de Base de Datos
-- Requerimientos origen: Req 01 (Registrar Usuarios)
-- Descripción: Registro de auditoría de intentos de acceso, autenticación y bloqueos.
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditoriaAccesos]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AuditoriaAccesos] (
        [AuditoriaAccesoId] BIGINT IDENTITY(1,1) NOT NULL,
        [UsuarioId] UNIQUEIDENTIFIER NULL,
        [IdentificadorIngresado] NVARCHAR(256) NOT NULL,
        [FechaEvento] DATETIME2(7) NOT NULL CONSTRAINT [DF_AuditoriaAccesos_FechaEvento] DEFAULT (SYSUTCDATETIME()),
        [DireccionIP] NVARCHAR(45) NULL,
        [UserAgent] NVARCHAR(500) NULL,
        [Resultado] NVARCHAR(50) NOT NULL, -- 'EXITOSO', 'CREDENCIALES_INVALIDAS', 'BLOQUEADO', 'USUARIO_INACTIVO'
        [Detalle] NVARCHAR(255) NULL,

        CONSTRAINT [PK_AuditoriaAccesos] PRIMARY KEY CLUSTERED ([AuditoriaAccesoId] ASC),
        CONSTRAINT [FK_AuditoriaAccesos_Usuarios] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuarios] ([UsuarioId]) ON DELETE SET NULL
    );

    CREATE NONCLUSTERED INDEX [IX_AuditoriaAccesos_UsuarioId_Fecha] ON [dbo].[AuditoriaAccesos] ([UsuarioId] ASC, [FechaEvento] DESC);
    CREATE NONCLUSTERED INDEX [IX_AuditoriaAccesos_Identificador_Fecha] ON [dbo].[AuditoriaAccesos] ([IdentificadorIngresado] ASC, [FechaEvento] DESC);
END
GO
