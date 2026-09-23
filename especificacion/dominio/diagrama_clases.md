# Diagrama de Clases de Dominio — MediStack

> **Requerimientos origen:** [Req 01 (Registrar Usuarios)](file:///C:/Users/ujr001/proyectos/medistack/requerimientos/01_RegistrarUsurios/requerimiento.md), [Req 02 (Gestionar Turnos)](file:///C:/Users/ujr001/proyectos/medistack/requerimientos/02_GestionarTurnos/requerimiento.md)  
> **Estado:** Aprobado (Verificado por el usuario)

```mermaid
classDiagram
    class Rol {
        +int RolId
        +string Codigo
        +string Nombre
        +string Descripcion
        +bool Activo
        +DateTime FechaCreacion
        +ICollection~Usuario~ Usuarios
    }

    class Usuario {
        +Guid UsuarioId
        +string Username
        +string Email
        +string PasswordHash
        +string PasswordSalt
        +string Nombre
        +string Apellido
        +string NumeroDocumento
        +string Telefono
        +int RolId
        +bool Activo
        +int IntentosFallidos
        +DateTime? BloqueadoHasta
        +DateTime FechaRegistro
        +DateTime? UltimoAcceso
        +Rol Rol
        +ICollection~AuditoriaAcceso~ AuditoriasAcceso
        +ICollection~Turno~ TurnosComoPaciente
        +ICollection~Turno~ TurnosComoProfesional
    }

    class AuditoriaAcceso {
        +long AuditoriaAccesoId
        +Guid? UsuarioId
        +string IdentificadorIngresado
        +DateTime FechaEvento
        +string DireccionIP
        +string UserAgent
        +ResultadoAutenticacion Resultado
        +string Detalle
        +Usuario Usuario
    }

    class EstadoTurno {
        +int EstadoTurnoId
        +string Codigo
        +string Nombre
        +string Descripcion
        +bool EsEstadoFinal
        +bool Activo
        +ICollection~Turno~ Turnos
    }

    class Especialidad {
        +int EspecialidadId
        +string Codigo
        +string Nombre
        +string Descripcion
        +int DuracionEstandarMinutos
        +bool Activa
        +DateTime FechaCreacion
        +ICollection~Turno~ Turnos
    }

    class Turno {
        +Guid TurnoId
        +string CodigoTurno
        +Guid PacienteUsuarioId
        +Guid ProfesionalUsuarioId
        +int EspecialidadId
        +DateTime FechaHoraInicio
        +DateTime FechaHoraFin
        +int EstadoTurnoId
        +string MotivoConsulta
        +bool EsParticular
        +int? ObraSocialId
        +decimal MontoConsulta
        +bool RequiereSena
        +decimal MontoSena
        +bool SenaPagada
        +DateTime? ExpiracionReservaTemporal
        +Guid? TurnoOrigenReprogramacionId
        +DateTime FechaCreacion
        +DateTime? FechaModificacion
        +Usuario PacienteUsuario
        +Usuario ProfesionalUsuario
        +Especialidad Especialidad
        +EstadoTurno EstadoTurno
        +Turno TurnoOrigenReprogramacion
        +ICollection~TransaccionTurno~ Transacciones
    }

    class TransaccionTurno {
        +Guid TransaccionTurnoId
        +Guid TurnoId
        +TipoTransaccionTurno TipoTransaccion
        +decimal Monto
        +string Moneda
        +string MetodoPago
        +string ReferenciaPasarela
        +EstadoTransaccionTurno EstadoTransaccion
        +DateTime FechaTransaccion
        +string MotivoReintegro
        +string PayloadRespuesta
        +Turno Turno
    }

    class RolUsuario {
        <<enumeration>>
        Paciente = 1
        Profesional = 2
        Administrativo = 3
        Administrador = 4
    }

    class ResultadoAutenticacion {
        <<enumeration>>
        Exitoso = 1
        CredencialesInvalidas = 2
        UsuarioBloqueado = 3
        UsuarioInactivo = 4
        DatosIncompletos = 5
    }

    class EstadoTurnoEnum {
        <<enumeration>>
        PendientePago = 1
        Confirmado = 2
        Reprogramado = 3
        CanceladoPaciente = 4
        CanceladoProfesional = 5
        Atendido = 6
        Ausente = 7
    }

    class TipoTransaccionTurno {
        <<enumeration>>
        PagoSena = 1
        ReintegroSena = 2
    }

    class EstadoTransaccionTurno {
        <<enumeration>>
        Aprobada = 1
        Rechazada = 2
        Reembolsada = 3
    }

    Rol "1" o-- "0..*" Usuario : asigna permisos a
    Usuario "0..1" o-- "0..*" AuditoriaAcceso : registra
    Usuario "1" o-- "0..*" Turno : solicita como paciente
    Usuario "1" o-- "0..*" Turno : atiende como profesional
    EstadoTurno "1" o-- "0..*" Turno : clasifica estado de
    Especialidad "1" o-- "0..*" Turno : categoriza consulta de
    Turno "1" o-- "0..*" TransaccionTurno : registra cobros/reintegros en
    Turno "0..1" o-- "0..1" Turno : reprograma a

    Usuario ..> RolUsuario : clasificado por
    AuditoriaAcceso ..> ResultadoAutenticacion : categorizado con
    Turno ..> EstadoTurnoEnum : mapeado con
    TransaccionTurno ..> TipoTransaccionTurno : clasificado por
    TransaccionTurno ..> EstadoTransaccionTurno : registrado con
```
