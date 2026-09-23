-- ============================================================================
-- MediStack — Capa de Base de Datos
-- Requerimientos origen: Req 02 (Gestionar Turnos)
-- Descripción: Registro de transacciones financieras vinculadas a turnos (pago de señas del 50% y reintegros).
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransaccionesTurno]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[TransaccionesTurno] (
        [TransaccionTurnoId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_TransaccionesTurno_TransaccionTurnoId] DEFAULT (NEWSEQUENTIALID()),
        [TurnoId] UNIQUEIDENTIFIER NOT NULL,
        [TipoTransaccion] NVARCHAR(30) NOT NULL, -- 'PAGO_SENA', 'REINTEGRO_SENA'
        [Monto] DECIMAL(12,2) NOT NULL,
        [Moneda] NVARCHAR(3) NOT NULL CONSTRAINT [DF_TransaccionesTurno_Moneda] DEFAULT ('ARS'),
        [MetodoPago] NVARCHAR(50) NOT NULL,
        [ReferenciaPasarela] NVARCHAR(150) NOT NULL,
        [EstadoTransaccion] NVARCHAR(50) NOT NULL, -- 'APROBADA', 'RECHAZADA', 'REEMBOLSADA'
        [FechaTransaccion] DATETIME2(7) NOT NULL CONSTRAINT [DF_TransaccionesTurno_FechaTransaccion] DEFAULT (SYSUTCDATETIME()),
        [MotivoReintegro] NVARCHAR(255) NULL,
        [PayloadRespuesta] NVARCHAR(MAX) NULL,

        CONSTRAINT [PK_TransaccionesTurno] PRIMARY KEY CLUSTERED ([TransaccionTurnoId] ASC),
        CONSTRAINT [FK_TransaccionesTurno_Turnos] FOREIGN KEY ([TurnoId]) REFERENCES [dbo].[Turnos] ([TurnoId]),
        CONSTRAINT [CK_TransaccionesTurno_Monto] CHECK ([Monto] > 0)
    );

    CREATE NONCLUSTERED INDEX [IX_TransaccionesTurno_TurnoId]
    ON [dbo].[TransaccionesTurno] ([TurnoId] ASC);

    CREATE NONCLUSTERED INDEX [IX_TransaccionesTurno_ReferenciaPasarela]
    ON [dbo].[TransaccionesTurno] ([ReferenciaPasarela] ASC);
END
GO
