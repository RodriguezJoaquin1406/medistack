-- ============================================================================
-- MediStack — Capa de Base de Datos
-- Requerimientos origen: Req 02 (Gestionar Turnos)
-- Descripción: Catálogo de especialidades médicas de la institución clínica.
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Especialidades]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Especialidades] (
        [EspecialidadId] INT IDENTITY(1,1) NOT NULL,
        [Codigo] NVARCHAR(50) NOT NULL,
        [Nombre] NVARCHAR(100) NOT NULL,
        [Descripcion] NVARCHAR(255) NULL,
        [DuracionEstandarMinutos] INT NOT NULL CONSTRAINT [DF_Especialidades_DuracionMinutos] DEFAULT (30),
        [Activa] BIT NOT NULL CONSTRAINT [DF_Especialidades_Activa] DEFAULT (1),
        [FechaCreacion] DATETIME2(7) NOT NULL CONSTRAINT [DF_Especialidades_FechaCreacion] DEFAULT (SYSUTCDATETIME()),

        CONSTRAINT [PK_Especialidades] PRIMARY KEY CLUSTERED ([EspecialidadId] ASC),
        CONSTRAINT [UQ_Especialidades_Codigo] UNIQUE NONCLUSTERED ([Codigo] ASC)
    );
END
GO
