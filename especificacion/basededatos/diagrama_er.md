# Diagrama Entidad-Relación — MediStack

> **Requerimientos origen:** [Req 01 (Registrar Usuarios)](file:///C:/Users/ujr001/proyectos/medistack/requerimientos/01_RegistrarUsurios/requerimiento.md)  
> **Estado:** Aprobado (Verificado por el usuario)

```mermaid
erDiagram
    Roles ||--o{ Usuarios : "asigna permisos a"
    Usuarios ||--o{ AuditoriaAccesos : "genera eventos en"

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
```
