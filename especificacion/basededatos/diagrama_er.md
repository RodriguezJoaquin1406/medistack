# Diagrama Entidad-Relación — MediStack

> **Requerimientos origen:** [Req 01 (Registrar Usuarios)](file:///C:/Users/ujr001/proyectos/medistack/requerimientos/01_RegistrarUsurios/requerimiento.md), [Req 02 (Gestionar Turnos)](file:///C:/Users/ujr001/proyectos/medistack/requerimientos/02_GestionarTurnos/requerimiento.md)  
> **Estado:** Aprobado (Verificado por el usuario)

```mermaid
erDiagram
    Roles ||--o{ Usuarios : "asigna permisos a"
    Usuarios ||--o{ AuditoriaAccesos : "genera eventos en"
    Usuarios ||--o{ Turnos : "solicita como paciente"
    Usuarios ||--o{ Turnos : "atiende como profesional"
    EstadosTurno ||--o{ Turnos : "determina ciclo de vida de"
    Especialidades ||--o{ Turnos : "clasifica consulta de"
    Turnos ||--o{ TransaccionesTurno : "registra senas o reintegros en"
    Turnos |o--o| Turnos : "reprograma hacia nuevo turno"

    Roles {
        INT RolId PK "IDENTITY(1,1)"
        NVARCHAR Codigo UK "Código único (PACIENTE, etc.)"
        NVARCHAR Nombre "Nombre visible"
        NVARCHAR Descripcion "Alcance del rol"
        BIT Activo "Estado lógico"
        DATETIME2 FechaCreacion "Marca temporal UTC"
    }

    Usuarios {
        UNIQUEIDENTIFIER UsuarioId PK "NEWSEQUENTIALID()"
        NVARCHAR Username UK "Nombre de usuario único"
        NVARCHAR Email UK "Correo único"
        NVARCHAR PasswordHash "Hash seguro"
        NVARCHAR PasswordSalt "Sal aleatoria"
        NVARCHAR Nombre "Nombre"
        NVARCHAR Apellido "Apellido"
        NVARCHAR NumeroDocumento UK "DNI único"
        NVARCHAR Telefono "Contacto opcional"
        INT RolId FK "Referencia a Roles"
        BIT Activo "Habilitado"
        INT IntentosFallidos "Contador de lockout"
        DATETIME2 BloqueadoHasta "Fin de lockout temporal"
        DATETIME2 FechaRegistro "Fecha de alta"
        DATETIME2 UltimoAcceso "Última sesión válida"
    }

    AuditoriaAccesos {
        BIGINT AuditoriaAccesoId PK "IDENTITY(1,1)"
        UNIQUEIDENTIFIER UsuarioId FK "NULL si no existía usuario"
        NVARCHAR IdentificadorIngresado "Email o username ingresado"
        DATETIME2 FechaEvento "Marca temporal UTC"
        NVARCHAR DireccionIP "IPv4 o IPv6"
        NVARCHAR UserAgent "Navegador o cliente"
        NVARCHAR Resultado "EXITOSO / FALLIDO / BLOQUEADO"
        NVARCHAR Detalle "Motivo de rechazo"
    }

    EstadosTurno {
        INT EstadoTurnoId PK "Identificador de estado"
        NVARCHAR Codigo UK "PENDIENTE_PAGO, CONFIRMADO, etc."
        NVARCHAR Nombre "Nombre visible"
        NVARCHAR Descripcion "Alcance del estado"
        BIT EsEstadoFinal "Libera agenda si es final"
        BIT Activo "Habilitado"
    }

    Especialidades {
        INT EspecialidadId PK "IDENTITY(1,1)"
        NVARCHAR Codigo UK "CAR, DERM, etc."
        NVARCHAR Nombre "Nombre de la especialidad"
        NVARCHAR Descripcion "Alcance clínico"
        INT DuracionEstandarMinutos "Minutos estándar (default 30)"
        BIT Activa "Habilitada para reserva web"
        DATETIME2 FechaCreacion "Fecha de alta UTC"
    }

    Turnos {
        UNIQUEIDENTIFIER TurnoId PK "NEWSEQUENTIALID()"
        NVARCHAR CodigoTurno UK "TRN-2026-0001"
        UNIQUEIDENTIFIER PacienteUsuarioId FK "Usuario paciente"
        UNIQUEIDENTIFIER ProfesionalUsuarioId FK "Usuario médico"
        INT EspecialidadId FK "Referencia a Especialidades"
        DATETIME2 FechaHoraInicio "Inicio de la cita"
        DATETIME2 FechaHoraFin "Fin estimado"
        INT EstadoTurnoId FK "Referencia a EstadosTurno"
        NVARCHAR MotivoConsulta "Razón o síntoma del turno"
        BIT EsParticular "0=Con convenio, 1=Particular"
        INT ObraSocialId "FK futura a ObrasSociales"
        DECIMAL MontoConsulta "Arancel total"
        BIT RequiereSena "1 si requiere 50%"
        DECIMAL MontoSena "50% de MontoConsulta"
        BIT SenaPagada "1 si fue acreditada"
        DATETIME2 ExpiracionReservaTemporal "TTL de pago de seña"
        UNIQUEIDENTIFIER TurnoOrigenReprogramacionId FK "Turno previo si reprogramado"
        DATETIME2 FechaCreacion "Alta UTC"
        DATETIME2 FechaModificacion "Modificación UTC"
    }

    TransaccionesTurno {
        UNIQUEIDENTIFIER TransaccionTurnoId PK "NEWSEQUENTIALID()"
        UNIQUEIDENTIFIER TurnoId FK "Referencia a Turnos"
        NVARCHAR TipoTransaccion "PAGO_SENA / REINTEGRO_SENA"
        DECIMAL Monto "Monto liquidado"
        NVARCHAR Moneda "ARS"
        NVARCHAR MetodoPago "TARJETA_CREDITO, etc."
        NVARCHAR ReferenciaPasarela "ID externo pasarela"
        NVARCHAR EstadoTransaccion "APROBADA / RECHAZADA / REEMBOLSADA"
        DATETIME2 FechaTransaccion "Marca temporal UTC"
        NVARCHAR MotivoReintegro "Motivo si cancelación >= 24h"
        NVARCHAR PayloadRespuesta "JSON auditoría técnica"
    }
```
