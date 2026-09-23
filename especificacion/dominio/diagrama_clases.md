# Diagrama de Clases de Dominio — MediStack

> **Requerimientos origen:** [Req 01 (Registrar Usuarios)](file:///C:/Users/ujr001/proyectos/medistack/requerimientos/01_RegistrarUsurios/requerimiento.md)  
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

    Rol "1" o-- "0..*" Usuario : contiene
    Usuario "0..1" o-- "0..*" AuditoriaAcceso : registra
    Usuario ..> RolUsuario : clasificado por
    AuditoriaAcceso ..> ResultadoAutenticacion : categorizado con
```
