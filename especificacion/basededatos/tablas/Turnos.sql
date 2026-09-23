-- ============================================================================
-- MediStack — Capa de Base de Datos
-- Requerimientos origen: Req 02 (Gestionar Turnos)
-- Descripción: Tabla principal de turnos clínicos, reservas, reprogramaciones y estados.
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Turnos]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Turnos] (
        [TurnoId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_Turnos_TurnoId] DEFAULT (NEWSEQUENTIALID()),
        [CodigoTurno] NVARCHAR(20) NOT NULL,
        [PacienteUsuarioId] UNIQUEIDENTIFIER NOT NULL,
        [ProfesionalUsuarioId] UNIQUEIDENTIFIER NOT NULL,
        [EspecialidadId] INT NOT NULL,
        [FechaHoraInicio] DATETIME2(7) NOT NULL,
        [FechaHoraFin] DATETIME2(7) NOT NULL,
        [EstadoTurnoId] INT NOT NULL,
        [MotivoConsulta] NVARCHAR(500) NULL,
        [EsParticular] BIT NOT NULL CONSTRAINT [DF_Turnos_EsParticular] DEFAULT (0),
        [ObraSocialId] INT NULL,
        [MontoConsulta] DECIMAL(12,2) NOT NULL,
        [RequiereSena] BIT NOT NULL CONSTRAINT [DF_Turnos_RequiereSena] DEFAULT (0),
        [MontoSena] DECIMAL(12,2) NOT NULL CONSTRAINT [DF_Turnos_MontoSena] DEFAULT (0),
        [SenaPagada] BIT NOT NULL CONSTRAINT [DF_Turnos_SenaPagada] DEFAULT (0),
        [ExpiracionReservaTemporal] DATETIME2(7) NULL,
        [TurnoOrigenReprogramacionId] UNIQUEIDENTIFIER NULL,
        [FechaCreacion] DATETIME2(7) NOT NULL CONSTRAINT [DF_Turnos_FechaCreacion] DEFAULT (SYSUTCDATETIME()),
        [FechaModificacion] DATETIME2(7) NULL,

        CONSTRAINT [PK_Turnos] PRIMARY KEY CLUSTERED ([TurnoId] ASC),
        CONSTRAINT [FK_Turnos_PacienteUsuario] FOREIGN KEY ([PacienteUsuarioId]) REFERENCES [dbo].[Usuarios] ([UsuarioId]),
        CONSTRAINT [FK_Turnos_ProfesionalUsuario] FOREIGN KEY ([ProfesionalUsuarioId]) REFERENCES [dbo].[Usuarios] ([UsuarioId]),
        CONSTRAINT [FK_Turnos_Especialidades] FOREIGN KEY ([EspecialidadId]) REFERENCES [dbo].[Especialidades] ([EspecialidadId]),
        CONSTRAINT [FK_Turnos_EstadosTurno] FOREIGN KEY ([EstadoTurnoId]) REFERENCES [dbo].[EstadosTurno] ([EstadoTurnoId]),
        CONSTRAINT [FK_Turnos_TurnoOrigen] FOREIGN KEY ([TurnoOrigenReprogramacionId]) REFERENCES [dbo].[Turnos] ([TurnoId]),
        CONSTRAINT [UQ_Turnos_CodigoTurno] UNIQUE NONCLUSTERED ([CodigoTurno] ASC),
        CONSTRAINT [CK_Turnos_RangoHorario] CHECK ([FechaHoraFin] > [FechaHoraInicio]),
        CONSTRAINT [CK_Turnos_MontoSena] CHECK ([MontoSena] >= 0 AND [MontoSena] <= [MontoConsulta])
    );

    -- Evita solapamiento de turnos activos para un mismo profesional en una misma fecha/hora
    -- Los estados 3 (REPROGRAMADO), 4 (CANCELADO_PACIENTE) y 5 (CANCELADO_PROFESIONAL) liberan el slot.
    CREATE UNIQUE NONCLUSTERED INDEX [UQ_Turnos_Profesional_HorarioActivo]
    ON [dbo].[Turnos] ([ProfesionalUsuarioId] ASC, [FechaHoraInicio] ASC)
    WHERE [EstadoTurnoId] NOT IN (3, 4, 5);

    -- Optimización para validar regla de negocio: 'paciente con turno pendiente activo'
    CREATE NONCLUSTERED INDEX [IX_Turnos_Paciente_Estado]
    ON [dbo].[Turnos] ([PacienteUsuarioId] ASC, [EstadoTurnoId] ASC)
    INCLUDE ([FechaHoraInicio], [EspecialidadId]);

    -- Optimización de búsqueda por fechas para grillas de agenda
    CREATE NONCLUSTERED INDEX [IX_Turnos_FechaHoraInicio]
    ON [dbo].[Turnos] ([FechaHoraInicio] ASC)
    INCLUDE ([ProfesionalUsuarioId], [EstadoTurnoId]);
END
GO
