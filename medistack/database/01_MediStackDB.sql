/*
    MediStackDB - SQL Server
    Ejecutar desde SQL Server Management Studio en una instancia de SQL Server.
    El script crea la base si no existe y no elimina datos existentes.
    Cuentas demo: admin, ana.perez, luis.gomez, carla.ruiz, martin.diaz
    Contrasena para todas: MediStack2026!
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET NUMERIC_ROUNDABORT OFF;
GO

IF DB_ID(N'MediStackDB') IS NULL
BEGIN
    EXEC(N'CREATE DATABASE [MediStackDB]');
END;
GO

USE [MediStackDB];
GO

IF OBJECT_ID(N'dbo.Roles', N'U') IS NOT NULL
BEGIN
    THROW 51000, N'MediStackDB ya contiene tablas. Use una base vacia para instalar el esquema inicial.', 1;
END;
GO

CREATE TABLE dbo.Roles
(
    RolId INT IDENTITY(1,1) NOT NULL,
    Codigo NVARCHAR(30) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(255) NULL,
    CONSTRAINT PK_Roles PRIMARY KEY (RolId),
    CONSTRAINT UQ_Roles_Codigo UNIQUE (Codigo),
    CONSTRAINT UQ_Roles_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.Usuarios
(
    UsuarioId UNIQUEIDENTIFIER NOT NULL,
    RolId INT NOT NULL,
    NombreUsuario NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(256) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    NumeroDocumento NVARCHAR(20) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Email NVARCHAR(256) NOT NULL,
    Telefono NVARCHAR(30) NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT (1),
    IntentosFallidos INT NOT NULL CONSTRAINT DF_Usuarios_IntentosFallidos DEFAULT (0),
    BloqueadoHasta DATETIME2(0) NULL,
    FechaRegistro DATETIME2(0) NOT NULL CONSTRAINT DF_Usuarios_FechaRegistro DEFAULT (SYSDATETIME()),
    UltimoAcceso DATETIME2(0) NULL,
    CONSTRAINT PK_Usuarios PRIMARY KEY (UsuarioId),
    CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (RolId) REFERENCES dbo.Roles (RolId),
    CONSTRAINT UQ_Usuarios_NombreUsuario UNIQUE (NombreUsuario),
    CONSTRAINT UQ_Usuarios_NumeroDocumento UNIQUE (NumeroDocumento),
    CONSTRAINT UQ_Usuarios_Email UNIQUE (Email),
    CONSTRAINT CK_Usuarios_IntentosFallidos CHECK (IntentosFallidos >= 0)
);
GO

CREATE TABLE dbo.ObrasSociales
(
    ObraSocialId INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    CodigoCUIT NVARCHAR(20) NOT NULL,
    Activa BIT NOT NULL CONSTRAINT DF_ObrasSociales_Activa DEFAULT (1),
    CONSTRAINT PK_ObrasSociales PRIMARY KEY (ObraSocialId),
    CONSTRAINT UQ_ObrasSociales_CodigoCUIT UNIQUE (CodigoCUIT),
    CONSTRAINT UQ_ObrasSociales_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.Pacientes
(
    PacienteId UNIQUEIDENTIFIER NOT NULL,
    ObraSocialId INT NULL,
    NumeroAfiliado NVARCHAR(50) NULL,
    ContactoEmergenciaNombre NVARCHAR(100) NULL,
    ContactoEmergenciaTelefono NVARCHAR(30) NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Pacientes_Activo DEFAULT (1),
    FechaAlta DATETIME2(0) NOT NULL CONSTRAINT DF_Pacientes_FechaAlta DEFAULT (SYSDATETIME()),
    CONSTRAINT PK_Pacientes PRIMARY KEY (PacienteId),
    CONSTRAINT FK_Pacientes_Usuarios FOREIGN KEY (PacienteId) REFERENCES dbo.Usuarios (UsuarioId),
    CONSTRAINT FK_Pacientes_ObrasSociales FOREIGN KEY (ObraSocialId) REFERENCES dbo.ObrasSociales (ObraSocialId)
);
GO

CREATE TABLE dbo.Profesionales
(
    ProfesionalId UNIQUEIDENTIFIER NOT NULL,
    MatriculaProfesional NVARCHAR(50) NOT NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Profesionales_Activo DEFAULT (1),
    FechaAlta DATETIME2(0) NOT NULL CONSTRAINT DF_Profesionales_FechaAlta DEFAULT (SYSDATETIME()),
    CONSTRAINT PK_Profesionales PRIMARY KEY (ProfesionalId),
    CONSTRAINT FK_Profesionales_Usuarios FOREIGN KEY (ProfesionalId) REFERENCES dbo.Usuarios (UsuarioId),
    CONSTRAINT UQ_Profesionales_Matricula UNIQUE (MatriculaProfesional)
);
GO

CREATE TABLE dbo.Especialidades
(
    EspecialidadId INT IDENTITY(1,1) NOT NULL,
    Codigo NVARCHAR(30) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255) NULL,
    DuracionEstandarMinutos SMALLINT NOT NULL CONSTRAINT DF_Especialidades_Duracion DEFAULT (30),
    Activa BIT NOT NULL CONSTRAINT DF_Especialidades_Activa DEFAULT (1),
    CONSTRAINT PK_Especialidades PRIMARY KEY (EspecialidadId),
    CONSTRAINT UQ_Especialidades_Codigo UNIQUE (Codigo),
    CONSTRAINT UQ_Especialidades_Nombre UNIQUE (Nombre),
    CONSTRAINT CK_Especialidades_Duracion CHECK (DuracionEstandarMinutos > 0)
);
GO

CREATE TABLE dbo.ProfesionalesEspecialidades
(
    ProfesionalId UNIQUEIDENTIFIER NOT NULL,
    EspecialidadId INT NOT NULL,
    EsEspecialidadPrincipal BIT NOT NULL CONSTRAINT DF_ProfEsp_Principal DEFAULT (0),
    ValorConsulta DECIMAL(12,2) NOT NULL,
    EsquemaTipo NVARCHAR(20) NOT NULL,
    EsquemaValor DECIMAL(7,2) NOT NULL,
    Activa BIT NOT NULL CONSTRAINT DF_ProfEsp_Activa DEFAULT (1),
    FechaAsignacion DATE NOT NULL CONSTRAINT DF_ProfEsp_FechaAsignacion DEFAULT (CONVERT(date, SYSDATETIME())),
    CONSTRAINT PK_ProfesionalesEspecialidades PRIMARY KEY (ProfesionalId, EspecialidadId),
    CONSTRAINT FK_ProfEsp_Profesionales FOREIGN KEY (ProfesionalId) REFERENCES dbo.Profesionales (ProfesionalId),
    CONSTRAINT FK_ProfEsp_Especialidades FOREIGN KEY (EspecialidadId) REFERENCES dbo.Especialidades (EspecialidadId),
    CONSTRAINT CK_ProfEsp_ValorConsulta CHECK (ValorConsulta >= 0),
    CONSTRAINT CK_ProfEsp_EsquemaTipo CHECK (EsquemaTipo IN (N'Porcentaje', N'Fijo')),
    CONSTRAINT CK_ProfEsp_EsquemaValor CHECK
    (
        (EsquemaTipo = N'Porcentaje' AND EsquemaValor BETWEEN 0 AND 100)
        OR (EsquemaTipo = N'Fijo' AND EsquemaValor >= 0)
    )
);
GO

CREATE TABLE dbo.HorariosAtencion
(
    HorarioAtencionId INT IDENTITY(1,1) NOT NULL,
    ProfesionalId UNIQUEIDENTIFIER NOT NULL,
    EspecialidadId INT NOT NULL,
    DiaSemana TINYINT NOT NULL,
    HoraInicio TIME(0) NOT NULL,
    HoraFin TIME(0) NOT NULL,
    Activo BIT NOT NULL CONSTRAINT DF_HorariosAtencion_Activo DEFAULT (1),
    CONSTRAINT PK_HorariosAtencion PRIMARY KEY (HorarioAtencionId),
    CONSTRAINT FK_HorariosAtencion_ProfEsp FOREIGN KEY (ProfesionalId, EspecialidadId)
        REFERENCES dbo.ProfesionalesEspecialidades (ProfesionalId, EspecialidadId),
    CONSTRAINT CK_HorariosAtencion_Dia CHECK (DiaSemana BETWEEN 1 AND 7),
    CONSTRAINT CK_HorariosAtencion_Rango CHECK (HoraFin > HoraInicio),
    CONSTRAINT UQ_HorariosAtencion_Inicio UNIQUE (ProfesionalId, EspecialidadId, DiaSemana, HoraInicio)
);
GO

CREATE TABLE dbo.CoberturasEspecialidades
(
    ObraSocialId INT NOT NULL,
    EspecialidadId INT NOT NULL,
    PorcentajeCobertura DECIMAL(5,2) NOT NULL,
    CONSTRAINT PK_CoberturasEspecialidades PRIMARY KEY (ObraSocialId, EspecialidadId),
    CONSTRAINT FK_Coberturas_ObrasSociales FOREIGN KEY (ObraSocialId) REFERENCES dbo.ObrasSociales (ObraSocialId),
    CONSTRAINT FK_Coberturas_Especialidades FOREIGN KEY (EspecialidadId) REFERENCES dbo.Especialidades (EspecialidadId),
    CONSTRAINT CK_Coberturas_Porcentaje CHECK (PorcentajeCobertura BETWEEN 0 AND 100)
);
GO

CREATE TABLE dbo.Convenios
(
    ConvenioId INT IDENTITY(1,1) NOT NULL,
    ProfesionalId UNIQUEIDENTIFIER NOT NULL,
    EspecialidadId INT NOT NULL,
    ObraSocialId INT NOT NULL,
    FechaDesde DATE NOT NULL,
    FechaHasta DATE NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Convenios_Activo DEFAULT (1),
    CONSTRAINT PK_Convenios PRIMARY KEY (ConvenioId),
    CONSTRAINT FK_Convenios_ProfEsp FOREIGN KEY (ProfesionalId, EspecialidadId)
        REFERENCES dbo.ProfesionalesEspecialidades (ProfesionalId, EspecialidadId),
    CONSTRAINT FK_Convenios_Cobertura FOREIGN KEY (ObraSocialId, EspecialidadId)
        REFERENCES dbo.CoberturasEspecialidades (ObraSocialId, EspecialidadId),
    CONSTRAINT CK_Convenios_Fechas CHECK (FechaHasta IS NULL OR FechaHasta >= FechaDesde),
    CONSTRAINT UQ_Convenios_ProfEspObraDesde UNIQUE (ProfesionalId, EspecialidadId, ObraSocialId, FechaDesde)
);
GO

CREATE TABLE dbo.Turnos
(
    TurnoId INT IDENTITY(1,1) NOT NULL,
    PacienteId UNIQUEIDENTIFIER NOT NULL,
    ProfesionalId UNIQUEIDENTIFIER NOT NULL,
    EspecialidadId INT NOT NULL,
    FechaHora DATETIME2(0) NOT NULL,
    Estado NVARCHAR(20) NOT NULL CONSTRAINT DF_Turnos_Estado DEFAULT (N'Solicitado'),
    Motivo NVARCHAR(500) NULL,
    MontoSena DECIMAL(12,2) NOT NULL CONSTRAINT DF_Turnos_MontoSena DEFAULT (0),
    EstadoSena NVARCHAR(20) NOT NULL CONSTRAINT DF_Turnos_EstadoSena DEFAULT (N'NoRequerida'),
    FechaHoraSena DATETIME2(0) NULL,
    CONSTRAINT PK_Turnos PRIMARY KEY (TurnoId),
    CONSTRAINT UQ_Turnos_TurnoPacienteProfesional UNIQUE (TurnoId, PacienteId, ProfesionalId),
    CONSTRAINT FK_Turnos_Pacientes FOREIGN KEY (PacienteId) REFERENCES dbo.Pacientes (PacienteId),
    CONSTRAINT FK_Turnos_ProfEsp FOREIGN KEY (ProfesionalId, EspecialidadId)
        REFERENCES dbo.ProfesionalesEspecialidades (ProfesionalId, EspecialidadId),
    CONSTRAINT CK_Turnos_Estado CHECK (Estado IN (N'Solicitado', N'Confirmado', N'Atendido', N'Ausente', N'Cancelado')),
    CONSTRAINT CK_Turnos_EstadoSena CHECK (EstadoSena IN (N'NoRequerida', N'Pendiente', N'Pagada', N'Reembolsada')),
    CONSTRAINT CK_Turnos_MontoSena CHECK (MontoSena >= 0)
);
GO

CREATE UNIQUE INDEX UX_Turnos_Paciente_Especialidad_Vigente
    ON dbo.Turnos (PacienteId, EspecialidadId)
    WHERE Estado IN (N'Solicitado', N'Confirmado');

CREATE UNIQUE INDEX UX_Turnos_Profesional_FechaHora_Vigente
    ON dbo.Turnos (ProfesionalId, FechaHora)
    WHERE Estado IN (N'Solicitado', N'Confirmado');

CREATE INDEX IX_Turnos_FechaHora_Estado
    ON dbo.Turnos (FechaHora, Estado);
GO

CREATE TABLE dbo.RegistrosClinicos
(
    TurnoId INT NOT NULL,
    FechaRegistro DATETIME2(0) NOT NULL CONSTRAINT DF_RegistrosClinicos_Fecha DEFAULT (SYSDATETIME()),
    MotivoConsulta NVARCHAR(500) NULL,
    Diagnostico NVARCHAR(1000) NULL,
    Observaciones NVARCHAR(MAX) NULL,
    CONSTRAINT PK_RegistrosClinicos PRIMARY KEY (TurnoId),
    CONSTRAINT FK_RegistrosClinicos_Turno FOREIGN KEY (TurnoId) REFERENCES dbo.Turnos (TurnoId)
);
GO

CREATE TABLE dbo.Cobros
(
    CobroId INT IDENTITY(1,1) NOT NULL,
    TurnoId INT NOT NULL,
    TipoCobro NVARCHAR(20) NOT NULL,
    MontoBase DECIMAL(12,2) NOT NULL CONSTRAINT DF_Cobros_MontoBase DEFAULT (0),
    MontoObraSocial DECIMAL(12,2) NOT NULL CONSTRAINT DF_Cobros_MontoObraSocial DEFAULT (0),
    Copago DECIMAL(12,2) NOT NULL CONSTRAINT DF_Cobros_Copago DEFAULT (0),
    SenaDescontada DECIMAL(12,2) NOT NULL CONSTRAINT DF_Cobros_SenaDescontada DEFAULT (0),
    MontoCobrado DECIMAL(12,2) NOT NULL,
    MedioPago NVARCHAR(20) NOT NULL,
    MontoRecibido DECIMAL(12,2) NOT NULL,
    Vuelto DECIMAL(12,2) NOT NULL CONSTRAINT DF_Cobros_Vuelto DEFAULT (0),
    FechaHoraCobro DATETIME2(0) NOT NULL CONSTRAINT DF_Cobros_Fecha DEFAULT (SYSDATETIME()),
    UsuarioCobradorId UNIQUEIDENTIFIER NULL,
    CONSTRAINT PK_Cobros PRIMARY KEY (CobroId),
    CONSTRAINT FK_Cobros_Turnos FOREIGN KEY (TurnoId) REFERENCES dbo.Turnos (TurnoId),
    CONSTRAINT FK_Cobros_Usuarios FOREIGN KEY (UsuarioCobradorId) REFERENCES dbo.Usuarios (UsuarioId),
    CONSTRAINT UQ_Cobros_TurnoTipo UNIQUE (TurnoId, TipoCobro),
    CONSTRAINT CK_Cobros_Tipo CHECK (TipoCobro IN (N'Sena', N'Consulta')),
    CONSTRAINT CK_Cobros_MedioPago CHECK (MedioPago IN (N'Efectivo', N'Tarjeta', N'Transferencia')),
    CONSTRAINT CK_Cobros_Importes CHECK
    (
        MontoBase >= 0 AND MontoObraSocial >= 0 AND Copago >= 0
        AND SenaDescontada >= 0 AND MontoCobrado >= 0
        AND MontoRecibido >= MontoCobrado AND Vuelto >= 0
        AND MontoRecibido - MontoCobrado = Vuelto
        AND MontoBase = MontoObraSocial + Copago
        AND SenaDescontada <= Copago
        AND
        (
            (TipoCobro = N'Sena' AND MontoBase = 0 AND MontoObraSocial = 0
                AND Copago = 0 AND SenaDescontada = 0 AND MontoCobrado > 0)
            OR
            (TipoCobro = N'Consulta' AND MontoCobrado = Copago - SenaDescontada)
        )
        AND (MedioPago = N'Efectivo' OR Vuelto = 0)
    )
);
GO

CREATE INDEX IX_Cobros_FechaHoraCobro_MedioPago
    ON dbo.Cobros (FechaHoraCobro, MedioPago)
    INCLUDE (MontoCobrado);
GO

CREATE TABLE dbo.CierresCaja
(
    CierreCajaId INT IDENTITY(1,1) NOT NULL,
    Fecha DATE NOT NULL,
    AdministrativoId UNIQUEIDENTIFIER NOT NULL,
    TotalEfectivo DECIMAL(12,2) NOT NULL,
    TotalTarjeta DECIMAL(12,2) NOT NULL,
    TotalTransferencia DECIMAL(12,2) NOT NULL,
    TotalGeneral DECIMAL(12,2) NOT NULL,
    CantidadCobros INT NOT NULL,
    EfectivoContado DECIMAL(12,2) NOT NULL,
    Diferencia DECIMAL(12,2) NOT NULL,
    FechaHoraCierre DATETIME2(0) NOT NULL CONSTRAINT DF_CierresCaja_FechaHora DEFAULT (SYSDATETIME()),
    CONSTRAINT PK_CierresCaja PRIMARY KEY (CierreCajaId),
    CONSTRAINT UQ_CierresCaja_Fecha UNIQUE (Fecha),
    CONSTRAINT FK_CierresCaja_Administrativo FOREIGN KEY (AdministrativoId) REFERENCES dbo.Usuarios (UsuarioId),
    CONSTRAINT CK_CierresCaja_Importes CHECK
    (
        TotalEfectivo >= 0 AND TotalTarjeta >= 0 AND TotalTransferencia >= 0
        AND TotalGeneral = TotalEfectivo + TotalTarjeta + TotalTransferencia
        AND CantidadCobros >= 0 AND EfectivoContado >= 0
        AND Diferencia = EfectivoContado - TotalEfectivo
    )
);
GO

CREATE TABLE dbo.LiquidacionesHonorarios
(
    LiquidacionHonorariosId INT IDENTITY(1,1) NOT NULL,
    ProfesionalId UNIQUEIDENTIFIER NOT NULL,
    PeriodoDesde DATE NOT NULL,
    PeriodoHasta DATE NOT NULL,
    TotalLiquidado DECIMAL(12,2) NOT NULL,
    FechaLiquidacion DATETIME2(0) NOT NULL CONSTRAINT DF_Liquidaciones_Fecha DEFAULT (SYSDATETIME()),
    CONSTRAINT PK_LiquidacionesHonorarios PRIMARY KEY (LiquidacionHonorariosId),
    CONSTRAINT FK_Liquidaciones_Profesionales FOREIGN KEY (ProfesionalId) REFERENCES dbo.Profesionales (ProfesionalId),
    CONSTRAINT UQ_Liquidaciones_ProfesionalPeriodo UNIQUE (ProfesionalId, PeriodoDesde, PeriodoHasta),
    CONSTRAINT CK_Liquidaciones_Periodo CHECK (PeriodoHasta >= PeriodoDesde),
    CONSTRAINT CK_Liquidaciones_Total CHECK (TotalLiquidado >= 0)
);
GO

CREATE TABLE dbo.InteraccionesIA
(
    InteraccionIAId BIGINT IDENTITY(1,1) NOT NULL,
    UsuarioId UNIQUEIDENTIFIER NOT NULL,
    Canal NVARCHAR(10) NOT NULL,
    Consulta NVARCHAR(MAX) NOT NULL,
    FuncionEjecutada NVARCHAR(100) NULL,
    Respuesta NVARCHAR(MAX) NULL,
    Fecha DATETIME2(0) NOT NULL CONSTRAINT DF_InteraccionesIA_Fecha DEFAULT (SYSDATETIME()),
    GastoTokens INT NOT NULL CONSTRAINT DF_InteraccionesIA_Tokens DEFAULT (0),
    CONSTRAINT PK_InteraccionesIA PRIMARY KEY (InteraccionIAId),
    CONSTRAINT FK_InteraccionesIA_Usuarios FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuarios (UsuarioId),
    CONSTRAINT CK_InteraccionesIA_Canal CHECK (Canal IN (N'Texto', N'Voz')),
    CONSTRAINT CK_InteraccionesIA_Tokens CHECK (GastoTokens >= 0)
);
GO

CREATE INDEX IX_InteraccionesIA_UsuarioFecha
    ON dbo.InteraccionesIA (UsuarioId, Fecha DESC);
GO

INSERT INTO dbo.Roles (Codigo, Nombre, Descripcion)
VALUES
    (N'PACIENTE', N'Paciente', N'Gestion de turnos e informacion personal.'),
    (N'PROFESIONAL', N'Profesional', N'Consulta de agenda y atencion de pacientes.'),
    (N'ADMINISTRATIVO', N'Administrativo', N'Gestion administrativa de la clinica.');

DECLARE @RolPaciente INT = (SELECT RolId FROM dbo.Roles WHERE Codigo = N'PACIENTE');
DECLARE @RolProfesional INT = (SELECT RolId FROM dbo.Roles WHERE Codigo = N'PROFESIONAL');
DECLARE @RolAdministrativo INT = (SELECT RolId FROM dbo.Roles WHERE Codigo = N'ADMINISTRATIVO');

DECLARE @Admin UNIQUEIDENTIFIER = '11111111-1111-4111-8111-111111111111';
DECLARE @Paciente1 UNIQUEIDENTIFIER = '22222222-2222-4222-8222-222222222221';
DECLARE @Paciente2 UNIQUEIDENTIFIER = '22222222-2222-4222-8222-222222222222';
DECLARE @Paciente3 UNIQUEIDENTIFIER = '22222222-2222-4222-8222-222222222223';
DECLARE @Paciente4 UNIQUEIDENTIFIER = '22222222-2222-4222-8222-222222222224';
DECLARE @Profesional1 UNIQUEIDENTIFIER = '33333333-3333-4333-8333-333333333331';
DECLARE @Profesional2 UNIQUEIDENTIFIER = '33333333-3333-4333-8333-333333333332';

INSERT INTO dbo.Usuarios
(
    UsuarioId, RolId, NombreUsuario, PasswordHash, Nombre, Apellido,
    NumeroDocumento, FechaNacimiento, Email, Telefono
)
VALUES
    (@Admin, @RolAdministrativo, N'admin',
     N'PBKDF2-SHA256$120000$uQAATWSh0/U7vy2FX4WbPA==$4DtXmtzJXrYmDWXH9YW1HaP4X4gsnL6x5DA+1ClDyOk=',
     N'Paula', N'Admin', N'20111222', '1985-04-12', N'admin@medistack.test', N'1100000001'),
    (@Paciente1, @RolPaciente, N'ana.perez',
     N'PBKDF2-SHA256$120000$3atg7HvMRWfSyLe4lgb+og==$GWDONAmj7fwGgA6lufS9ONUull6nR+OOvL9DQWTAHfQ=',
     N'Ana', N'Perez', N'30111222', '1990-06-18', N'ana.perez@medistack.test', N'1100000002'),
    (@Paciente2, @RolPaciente, N'luis.gomez',
     N'PBKDF2-SHA256$120000$UAkQ6SAhwxTpFDgax3nVIw==$3o4uezGCRslziz+B30TFxf+LvBYYMNmkrXAlAJLtEgQ=',
     N'Luis', N'Gomez', N'28999888', '1988-02-24', N'luis.gomez@medistack.test', N'1100000003'),
    (@Paciente3, @RolPaciente, N'carla.ruiz',
     N'PBKDF2-SHA256$120000$QaNuNj3VjpIsEGlu6Ao4uA==$j2OD2XXK19UhqHqF48SVobI2P88Ab0qn8dhiiN6qXLo=',
     N'Carla', N'Ruiz', N'33444555', '1993-11-03', N'carla.ruiz@medistack.test', N'1100000004'),
    (@Paciente4, @RolPaciente, N'martin.diaz',
     N'PBKDF2-SHA256$120000$lx7CvBSbW2Bgc0SJYeoeww==$/wIiYu805RXc3xfFtwljXCAIuFVB3SkBqYWTGX443Bg=',
     N'Martin', N'Diaz', N'32123456', '1991-09-15', N'martin.diaz@medistack.test', N'1100000005'),
    (@Profesional1, @RolProfesional, N'dra.lopez',
     N'PBKDF2-SHA256$120000$hD74nYjsHJ+9OyEOysdCsQ==$hOwIiVvwBYnKIS7Of9mBIlX+J2kaGXs+eTs1lj6+Uh0=',
     N'Sofia', N'Lopez', N'25111222', '1979-03-20', N'sofia.lopez@medistack.test', N'1100000006'),
    (@Profesional2, @RolProfesional, N'dr.fernandez',
     N'PBKDF2-SHA256$120000$TKKjysxaIrlawL6FDm8SZA==$Xw6L+c1yfwWrXtuPZo9XBTDv0Ur6ijN2wWqjJ/9WJ1I=',
     N'Marcos', N'Fernandez', N'26123456', '1981-07-08', N'marcos.fernandez@medistack.test', N'1100000007');

INSERT INTO dbo.ObrasSociales (Nombre, CodigoCUIT)
VALUES
    (N'Salud Union', N'30-70000001-1'),
    (N'Cobertura Federal', N'30-70000002-2');

DECLARE @ObraSocial1 INT = (SELECT ObraSocialId FROM dbo.ObrasSociales WHERE CodigoCUIT = N'30-70000001-1');
DECLARE @ObraSocial2 INT = (SELECT ObraSocialId FROM dbo.ObrasSociales WHERE CodigoCUIT = N'30-70000002-2');

INSERT INTO dbo.Pacientes (PacienteId, ObraSocialId, NumeroAfiliado, ContactoEmergenciaNombre, ContactoEmergenciaTelefono)
VALUES
    (@Paciente1, @ObraSocial1, N'SU-10001', N'Juan Perez', N'1100000012'),
    (@Paciente2, @ObraSocial2, N'CF-20002', N'Maria Gomez', N'1100000013'),
    (@Paciente3, NULL, NULL, N'Pedro Ruiz', N'1100000014'),
    (@Paciente4, @ObraSocial1, N'SU-10004', N'Elena Diaz', N'1100000015');

INSERT INTO dbo.Profesionales (ProfesionalId, MatriculaProfesional)
VALUES
    (@Profesional1, N'MN-45821'),
    (@Profesional2, N'MN-58314');

INSERT INTO dbo.Especialidades (Codigo, Nombre, Descripcion, DuracionEstandarMinutos)
VALUES
    (N'CARDIO', N'Cardiologia', N'Consulta y seguimiento cardiovascular.', 30),
    (N'DERMA', N'Dermatologia', N'Consulta dermatologica general.', 30),
    (N'PED', N'Pediatria', N'Atencion pediatrica.', 30);

DECLARE @Cardiologia INT = (SELECT EspecialidadId FROM dbo.Especialidades WHERE Codigo = N'CARDIO');
DECLARE @Dermatologia INT = (SELECT EspecialidadId FROM dbo.Especialidades WHERE Codigo = N'DERMA');
DECLARE @Pediatria INT = (SELECT EspecialidadId FROM dbo.Especialidades WHERE Codigo = N'PED');

INSERT INTO dbo.ProfesionalesEspecialidades
    (ProfesionalId, EspecialidadId, EsEspecialidadPrincipal, ValorConsulta, EsquemaTipo, EsquemaValor)
VALUES
    (@Profesional1, @Cardiologia, 1, 30000.00, N'Porcentaje', 70.00),
    (@Profesional1, @Pediatria, 0, 25000.00, N'Fijo', 15000.00),
    (@Profesional2, @Dermatologia, 1, 28000.00, N'Porcentaje', 65.00);

INSERT INTO dbo.CoberturasEspecialidades (ObraSocialId, EspecialidadId, PorcentajeCobertura)
VALUES
    (@ObraSocial1, @Cardiologia, 60.00),
    (@ObraSocial1, @Pediatria, 50.00),
    (@ObraSocial2, @Dermatologia, 70.00),
    (@ObraSocial2, @Cardiologia, 40.00);

INSERT INTO dbo.Convenios (ProfesionalId, EspecialidadId, ObraSocialId, FechaDesde)
VALUES
    (@Profesional1, @Cardiologia, @ObraSocial1, '2024-01-01'),
    (@Profesional1, @Pediatria, @ObraSocial1, '2024-01-01'),
    (@Profesional2, @Dermatologia, @ObraSocial2, '2024-01-01');

INSERT INTO dbo.HorariosAtencion (ProfesionalId, EspecialidadId, DiaSemana, HoraInicio, HoraFin)
VALUES
    (@Profesional1, @Cardiologia, 1, '08:00', '12:00'),
    (@Profesional1, @Cardiologia, 3, '13:00', '17:00'),
    (@Profesional1, @Pediatria, 2, '08:00', '12:00'),
    (@Profesional1, @Pediatria, 4, '13:00', '17:00'),
    (@Profesional2, @Dermatologia, 1, '09:00', '13:00'),
    (@Profesional2, @Dermatologia, 5, '13:00', '17:00');

DECLARE @Hoy DATE = CONVERT(date, SYSDATETIME());
DECLARE @ProximoLunes DATE = DATEADD(day, 7 - (DATEDIFF(day, CONVERT(date, '19000101'), @Hoy) % 7), @Hoy);
DECLARE @LunesAnterior DATE = DATEADD(day, -14, @ProximoLunes);

INSERT INTO dbo.Turnos
    (PacienteId, ProfesionalId, EspecialidadId, FechaHora, Estado, Motivo, MontoSena, EstadoSena, FechaHoraSena)
VALUES
    (@Paciente1, @Profesional1, @Cardiologia, DATEADD(hour, 9, CONVERT(datetime2(0), @ProximoLunes)),
     N'Confirmado', N'Control cardiologico', 5000.00, N'Pagada', SYSDATETIME()),
    (@Paciente2, @Profesional1, @Cardiologia, DATEADD(hour, 10, CONVERT(datetime2(0), @ProximoLunes)),
     N'Solicitado', N'Consulta inicial', 5000.00, N'Pendiente', NULL),
    (@Paciente3, @Profesional2, @Dermatologia, DATEADD(hour, 11, CONVERT(datetime2(0), @ProximoLunes)),
     N'Confirmado', N'Control dermatologico', 3500.00, N'Pagada', SYSDATETIME()),
    (@Paciente2, @Profesional2, @Dermatologia, DATEADD(hour, 12, CONVERT(datetime2(0), @ProximoLunes)),
     N'Confirmado', N'Consulta dermatologica', 4500.00, N'Pagada', SYSDATETIME()),
    (@Paciente1, @Profesional1, @Cardiologia, DATEADD(hour, 9, CONVERT(datetime2(0), @LunesAnterior)),
     N'Atendido', N'Control periodico', 0.00, N'NoRequerida', NULL),
    (@Paciente4, @Profesional1, @Cardiologia, DATEADD(hour, 10, CONVERT(datetime2(0), @LunesAnterior)),
     N'Cancelado', N'Turno cancelado para prueba', 0.00, N'NoRequerida', NULL);

DECLARE @TurnoSenaTransferencia INT =
    (SELECT TurnoId FROM dbo.Turnos WHERE PacienteId = @Paciente1 AND FechaHora = DATEADD(hour, 9, CONVERT(datetime2(0), @ProximoLunes)));
DECLARE @TurnoSenaEfectivo INT =
    (SELECT TurnoId FROM dbo.Turnos WHERE PacienteId = @Paciente3 AND FechaHora = DATEADD(hour, 11, CONVERT(datetime2(0), @ProximoLunes)));
DECLARE @TurnoSenaTarjeta INT =
    (SELECT TurnoId FROM dbo.Turnos WHERE PacienteId = @Paciente2 AND EspecialidadId = @Dermatologia
        AND FechaHora = DATEADD(hour, 12, CONVERT(datetime2(0), @ProximoLunes)));
DECLARE @TurnoAtendido INT =
    (SELECT TurnoId FROM dbo.Turnos WHERE PacienteId = @Paciente1 AND Estado = N'Atendido'
        AND FechaHora = DATEADD(hour, 9, CONVERT(datetime2(0), @LunesAnterior)));

INSERT INTO dbo.Cobros
    (TurnoId, TipoCobro, MontoCobrado, MedioPago, MontoRecibido, Vuelto, UsuarioCobradorId, FechaHoraCobro)
VALUES
    (@TurnoSenaTransferencia, N'Sena', 5000.00, N'Transferencia', 5000.00, 0.00, @Admin, SYSDATETIME()),
    (@TurnoSenaEfectivo, N'Sena', 3500.00, N'Efectivo', 4000.00, 500.00, @Admin, SYSDATETIME()),
    (@TurnoSenaTarjeta, N'Sena', 4500.00, N'Tarjeta', 4500.00, 0.00, @Admin, SYSDATETIME());

INSERT INTO dbo.Cobros
    (TurnoId, TipoCobro, MontoBase, MontoObraSocial, Copago, SenaDescontada,
     MontoCobrado, MedioPago, MontoRecibido, Vuelto, UsuarioCobradorId, FechaHoraCobro)
VALUES
    (@TurnoAtendido, N'Consulta', 30000.00, 18000.00, 12000.00, 0.00,
     12000.00, N'Efectivo', 12000.00, 0.00, @Admin,
     DATEADD(hour, 10, CONVERT(datetime2(0), @LunesAnterior)));

INSERT INTO dbo.RegistrosClinicos (TurnoId, FechaRegistro, MotivoConsulta, Diagnostico, Observaciones)
VALUES
    (@TurnoAtendido, DATEADD(hour, 10, CONVERT(datetime2(0), @LunesAnterior)),
     N'Control periodico', N'Control sin hallazgos relevantes.',
     N'Continuar controles habituales y consultar ante nuevos sintomas.');

INSERT INTO dbo.CierresCaja
    (Fecha, AdministrativoId, TotalEfectivo, TotalTarjeta, TotalTransferencia,
     TotalGeneral, CantidadCobros, EfectivoContado, Diferencia)
VALUES
    (@Hoy, @Admin, 3500.00, 4500.00, 5000.00, 13000.00, 3, 3500.00, 0.00);

INSERT INTO dbo.LiquidacionesHonorarios (ProfesionalId, PeriodoDesde, PeriodoHasta, TotalLiquidado)
VALUES
    (@Profesional1, @LunesAnterior, DATEADD(day, 6, @LunesAnterior), 8400.00),
    (@Profesional2, @LunesAnterior, DATEADD(day, 6, @LunesAnterior), 4200.00);

INSERT INTO dbo.InteraccionesIA (UsuarioId, Canal, Consulta, FuncionEjecutada, Respuesta, GastoTokens)
VALUES
    (@Paciente1, N'Texto', N'Quiero consultar mis turnos.', N'ConsultarTurnos',
     N'Tenes un turno de cardiologia confirmado.', 145),
    (@Admin, N'Voz', N'Cuantos turnos hay para el lunes?', N'ConsultarAgenda',
     N'Hay turnos agendados para la proxima semana.', 210);
GO
