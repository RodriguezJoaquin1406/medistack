-- ============================================================================
-- MediStack — Capa de Base de Datos
-- Requerimientos origen: Req 02 (Gestionar Turnos)
-- Descripción: Catálogo estático de estados del ciclo de vida de un turno.
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstadosTurno]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[EstadosTurno] (
        [EstadoTurnoId] INT NOT NULL,
        [Codigo] NVARCHAR(50) NOT NULL,
        [Nombre] NVARCHAR(100) NOT NULL,
        [Descripcion] NVARCHAR(255) NULL,
        [EsEstadoFinal] BIT NOT NULL CONSTRAINT [DF_EstadosTurno_EsEstadoFinal] DEFAULT (0),
        [Activo] BIT NOT NULL CONSTRAINT [DF_EstadosTurno_Activo] DEFAULT (1),

        CONSTRAINT [PK_EstadosTurno] PRIMARY KEY CLUSTERED ([EstadoTurnoId] ASC),
        CONSTRAINT [UQ_EstadosTurno_Codigo] UNIQUE NONCLUSTERED ([Codigo] ASC)
    );

    -- Inserción de valores iniciales del catálogo
    INSERT INTO [dbo].[EstadosTurno] ([EstadoTurnoId], [Codigo], [Nombre], [Descripcion], [EsEstadoFinal], [Activo])
    VALUES
        (1, 'PENDIENTE_PAGO', 'Pendiente de Pago', 'Reserva temporal a la espera del abono de la seña (expira por TTL)', 0, 1),
        (2, 'CONFIRMADO', 'Confirmado', 'Turno validado y agendado (con seña abonada o cubierto por obra social)', 0, 1),
        (3, 'REPROGRAMADO', 'Reprogramado', 'El paciente reprogramó la fecha/hora; superseded por un nuevo turno', 1, 1),
        (4, 'CANCELADO_PACIENTE', 'Cancelado por el Paciente', 'Cancelación voluntaria realizada con >= 24h de anticipación', 1, 1),
        (5, 'CANCELADO_PROFESIONAL', 'Cancelado por el Profesional', 'Cancelación por fuerza mayor o ausencia médica con reintegro automático', 1, 1),
        (6, 'ATENDIDO', 'Atendido', 'El paciente se presentó y la consulta médica fue efectuada', 1, 1),
        (7, 'AUSENTE', 'Ausente', 'El paciente no se presentó en el horario asignado sin cancelar a tiempo', 1, 1);
END
GO
