# Diccionario de Datos — Capa de Base de Datos (MediStack)

> **Requerimientos origen:** [Req 01 (Registrar Usuarios)](file:///C:/Users/ujr001/proyectos/medistack/requerimientos/01_RegistrarUsurios/requerimiento.md)  
> **Motor de Base de Datos:** Microsoft SQL Server 2019+  
> **Estado:** Aprobado (Verificado por el usuario)

---

## 1. Justificación de Identificadores (Claves Primarias)

En concordancia con las directrices de seguridad y anti-enumeración definidas en [`AGENTS.md`](file:///C:/Users/ujr001/proyectos/medistack/AGENTS.md):
- **`Usuarios.UsuarioId` (`UNIQUEIDENTIFIER` / `NEWSEQUENTIALID()`):** Los identificadores de usuarios se exponen en sesiones, tokens, URLs y transacciones clínicas. El uso de GUID previene ataques de enumeración (IDOR), scraping de perfiles y deducción secuencial de cuentas. Se utiliza `NEWSEQUENTIALID()` para mitigar la fragmentación de páginas en índices clustered.
- **`Roles.RolId` (`INT IDENTITY(1,1)`):** Catálogo estático e interno del sistema con un número reducido de registros fijos (Paciente, Profesional, Administrativo, Administrador). No representa riesgo de enumeración externo.
- **`AuditoriaAccesos.AuditoriaAccesoId` (`BIGINT IDENTITY(1,1)`):** Bitácora transaccional append-only de alto volumen interno para auditoría.

---

## 2. Catálogo de Tablas

### 2.1. Tabla: `dbo.Roles`
Almacena los roles del sistema que definen los permisos y niveles de acceso funcional.

| Columna | Tipo de Dato SQL Server | Nulo | Clave | Restricciones / Defaults | Descripción |
|---|---|---|---|---|---|
| `RolId` | `INT` | NO | PK | `IDENTITY(1,1)` | Identificador interno secuencial del rol. |
| `Codigo` | `NVARCHAR(50)` | NO | UQ | `UNIQUE` | Código alfanumérico estandarizado (ej. `'PACIENTE'`, `'PROFESIONAL'`, `'ADMINISTRATIVO'`). |
| `Nombre` | `NVARCHAR(100)` | NO | - | - | Nombre legible del rol para la interfaz. |
| `Descripcion` | `NVARCHAR(255)` | SÍ | - | - | Descripción de los alcances y responsabilidades del rol. |
| `Activo` | `BIT` | NO | - | `DEFAULT (1)` | Bandera de estado lógico del rol. |
| `FechaCreacion` | `DATETIME2(7)` | NO | - | `DEFAULT (SYSUTCDATETIME())` | Marca temporal UTC del registro. |

---

### 2.2. Tabla: `dbo.Usuarios`
Almacena las cuentas de usuario, credenciales con hashing seguro, datos personales básicos y estado de bloqueo.

| Columna | Tipo de Dato SQL Server | Nulo | Clave | Restricciones / Defaults | Descripción |
|---|---|---|---|---|---|
| `UsuarioId` | `UNIQUEIDENTIFIER` | NO | PK | `DEFAULT (NEWSEQUENTIALID())` | Identificador global único no predecible del usuario. |
| `Username` | `NVARCHAR(50)` | NO | UQ | `UNIQUE` | Nombre de usuario único para autenticación. |
| `Email` | `NVARCHAR(256)` | NO | UQ | `UNIQUE` | Correo electrónico normalizado (minúsculas). |
| `PasswordHash` | `NVARCHAR(256)` | NO | - | - | Hash criptográfico de la contraseña (PBKDF2/SHA-256 o BCrypt). |
| `PasswordSalt` | `NVARCHAR(128)` | NO | - | - | Sal criptográfica aleatoria única por usuario. |
| `Nombre` | `NVARCHAR(100)` | NO | - | - | Nombre(s) del titular de la cuenta. |
| `Apellido` | `NVARCHAR(100)` | NO | - | - | Apellido(s) del titular de la cuenta. |
| `NumeroDocumento` | `NVARCHAR(20)` | NO | UQ | `UNIQUE` | DNI / Cédula de identidad (sin puntos ni espacios). |
| `Telefono` | `NVARCHAR(30)` | SÍ | - | - | Número de contacto telefónico principal. |
| `RolId` | `INT` | NO | FK | `REFERENCES dbo.Roles(RolId)` | Rol asignado al usuario. |
| `Activo` | `BIT` | NO | - | `DEFAULT (1)` | Estado de habilitación operativa del usuario. |
| `IntentosFallidos` | `INT` | NO | - | `DEFAULT (0)` | Contador acumulativo de intentos fallidos de autenticación. |
| `BloqueadoHasta` | `DATETIME2(7)` | SÍ | - | - | Fecha UTC de expiración del bloqueo temporal por lockout. |
| `FechaRegistro` | `DATETIME2(7)` | NO | - | `DEFAULT (SYSUTCDATETIME())` | Fecha y hora UTC del alta del usuario. |
| `UltimoAcceso` | `DATETIME2(7)` | SÍ | - | - | Fecha y hora UTC de la última autenticación exitosa. |

---

### 2.3. Tabla: `dbo.AuditoriaAccesos`
Bitácora de seguridad y trazabilidad de eventos de autenticación, ingresos fallidos y bloqueos preventivos.

| Columna | Tipo de Dato SQL Server | Nulo | Clave | Restricciones / Defaults | Descripción |
|---|---|---|---|---|---|
| `AuditoriaAccesoId` | `BIGINT` | NO | PK | `IDENTITY(1,1)` | Identificador secuencial de la entrada de auditoría. |
| `UsuarioId` | `UNIQUEIDENTIFIER` | SÍ | FK | `REFERENCES dbo.Usuarios(UsuarioId) ON DELETE SET NULL` | Usuario asociado si fue identificado; NULL si no existía el usuario. |
| `IdentificadorIngresado` | `NVARCHAR(256)` | NO | - | - | Nombre de usuario o email ingresado en el formulario (sanitizado). |
| `FechaEvento` | `DATETIME2(7)` | NO | - | `DEFAULT (SYSUTCDATETIME())` | Marca temporal UTC del intento de acceso. |
| `DireccionIP` | `NVARCHAR(45)` | SÍ | - | - | Dirección IPv4 o IPv6 de origen de la solicitud. |
| `UserAgent` | `NVARCHAR(500)` | SÍ | - | - | Cabecera User-Agent del navegador o cliente emisor. |
| `Resultado` | `NVARCHAR(50)` | NO | - | - | Resultado: `'EXITOSO'`, `'CREDENCIALES_INVALIDAS'`, `'BLOQUEADO'`, `'USUARIO_INACTIVO'`. |
| `Detalle` | `NVARCHAR(255)` | SÍ | - | - | Información de diagnóstico o causa del bloqueo. |
