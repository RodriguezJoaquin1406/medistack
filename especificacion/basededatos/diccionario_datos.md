# Diccionario de Datos — Capa de Base de Datos (MediStack)

> **Requerimientos origen:** [Req 01 (Registrar Usuarios)](file:///C:/Users/ujr001/proyectos/medistack/requerimientos/01_RegistrarUsurios/requerimiento.md), [Req 02 (Gestionar Turnos)](file:///C:/Users/ujr001/proyectos/medistack/requerimientos/02_GestionarTurnos/requerimiento.md)  
> **Motor de Base de Datos:** Microsoft SQL Server 2019+  
> **Estado:** Aprobado (Verificado por el usuario)

---

## 1. Justificación de Identificadores (Claves Primarias)

En concordancia con las directrices de seguridad y anti-enumeración definidas en [`AGENTS.md`](file:///C:/Users/ujr001/proyectos/medistack/AGENTS.md):
- **`Usuarios.UsuarioId` (`UNIQUEIDENTIFIER` / `NEWSEQUENTIALID()`):** Los identificadores de usuarios se exponen en sesiones, tokens, URLs y transacciones clínicas. El uso de GUID previene ataques de enumeración (IDOR), scraping de perfiles y deducción secuencial de cuentas. Se utiliza `NEWSEQUENTIALID()` para mitigar la fragmentación de páginas en índices clustered.
- **`Roles.RolId` (`INT IDENTITY(1,1)`):** Catálogo estático e interno del sistema con un número reducido de registros fijos (Paciente, Profesional, Administrativo, Administrador). No representa riesgo de enumeración externo.
- **`AuditoriaAccesos.AuditoriaAccesoId` (`BIGINT IDENTITY(1,1)`):** Bitácora transaccional append-only de alto volumen interno para auditoría.
- **`EstadosTurno.EstadoTurnoId` (`INT`):** Catálogo discreto estático de estados del ciclo de vida del turno. Sin riesgo de enumeración.
- **`Especialidades.EspecialidadId` (`INT IDENTITY(1,1)`):** Catálogo maestro institucional de especialidades médicas (público y de bajo volumen).
- **`Turnos.TurnoId` (`UNIQUEIDENTIFIER` / `NEWSEQUENTIALID()`):** Identificador primario de alta sensibilidad expuesto en enlaces de confirmación, correos de recordatorio, solicitudes de cancelación/reprogramación y pasarelas de pago. El uso de GUID no predecible previene el acceso no autorizado o la manipulación de turnos ajenos (anti-IDOR).
- **`TransaccionesTurno.TransaccionTurnoId` (`UNIQUEIDENTIFIER` / `NEWSEQUENTIALID()`):** Registro transaccional financiero expuesto a integraciones con pasarelas de pago y comprobantes para el paciente.

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

---

### 2.4. Tabla: `dbo.EstadosTurno`
Catálogo estático de estados para modelar la máquina de estados del ciclo de vida del turno.

| Columna | Tipo de Dato SQL Server | Nulo | Clave | Restricciones / Defaults | Descripción |
|---|---|---|---|---|---|
| `EstadoTurnoId` | `INT` | NO | PK | - | Identificador numérico fijo del estado. |
| `Codigo` | `NVARCHAR(50)` | NO | UQ | `UNIQUE` | Clave interna (ej. `'PENDIENTE_PAGO'`, `'CONFIRMADO'`, `'CANCELADO_PACIENTE'`). |
| `Nombre` | `NVARCHAR(100)` | NO | - | - | Nombre legible en la interfaz de usuario. |
| `Descripcion` | `NVARCHAR(255)` | SÍ | - | - | Descripción funcional de las condiciones de este estado. |
| `EsEstadoFinal` | `BIT` | NO | - | `DEFAULT (0)` | Indica si es un estado terminal que libera el slot o concluye el turno. |
| `Activo` | `BIT` | NO | - | `DEFAULT (1)` | Bandera de habilitación en el sistema. |

---

### 2.5. Tabla: `dbo.Especialidades`
Catálogo institucional de especialidades médicas disponibles para filtrado y agendas.

| Columna | Tipo de Dato SQL Server | Nulo | Clave | Restricciones / Defaults | Descripción |
|---|---|---|---|---|---|
| `EspecialidadId` | `INT` | NO | PK | `IDENTITY(1,1)` | Identificador secuencial de la especialidad. |
| `Codigo` | `NVARCHAR(50)` | NO | UQ | `UNIQUE` | Código estandarizado de la especialidad (ej. `'CAR'`, `'DERM'`). |
| `Nombre` | `NVARCHAR(100)` | NO | - | - | Denominación clínica (ej. `'Cardiología'`, `'Dermatología'`). |
| `Descripcion` | `NVARCHAR(255)` | SÍ | - | - | Detalle o alcance de la atención. |
| `DuracionEstandarMinutos` | `INT` | NO | - | `DEFAULT (30)` | Duración de referencia en minutos para el slot de turno. |
| `Activa` | `BIT` | NO | - | `DEFAULT (1)` | Habilitación para reserva web. |
| `FechaCreacion` | `DATETIME2(7)` | NO | - | `DEFAULT (SYSUTCDATETIME())` | Fecha UTC de alta en el sistema. |

---

### 2.6. Tabla: `dbo.Turnos`
Tabla central de turnos clínicos agendados, reservas, cancelaciones y reprogramaciones.

| Columna | Tipo de Dato SQL Server | Nulo | Clave | Restricciones / Defaults | Descripción |
|---|---|---|---|---|---|
| `TurnoId` | `UNIQUEIDENTIFIER` | NO | PK | `DEFAULT (NEWSEQUENTIALID())` | Identificador único global no predecible del turno (anti-IDOR). |
| `CodigoTurno` | `NVARCHAR(20)` | NO | UQ | `UNIQUE` | Código alfanumérico visible de seguimiento (ej. `'TRN-2026-0001'`). |
| `PacienteUsuarioId` | `UNIQUEIDENTIFIER` | NO | FK | `REFERENCES dbo.Usuarios(UsuarioId)` | Usuario paciente que solicita el turno. |
| `ProfesionalUsuarioId` | `UNIQUEIDENTIFIER` | NO | FK | `REFERENCES dbo.Usuarios(UsuarioId)` | Profesional de la salud seleccionado para la atención. |
| `EspecialidadId` | `INT` | NO | FK | `REFERENCES dbo.Especialidades(EspecialidadId)` | Especialidad médica de la consulta. |
| `FechaHoraInicio` | `DATETIME2(7)` | NO | - | - | Fecha y hora UTC del inicio de la cita. |
| `FechaHoraFin` | `DATETIME2(7)` | NO | - | `CHECK (FechaHoraFin > FechaHoraInicio)` | Fecha y hora UTC pactada de finalización. |
| `EstadoTurnoId` | `INT` | NO | FK | `REFERENCES dbo.EstadosTurno(EstadoTurnoId)` | Estado actual del turno en su ciclo de vida. |
| `MotivoConsulta` | `NVARCHAR(500)` | SÍ | - | - | Nota o razón de la consulta descrita por el paciente. |
| `EsParticular` | `BIT` | NO | - | `DEFAULT (0)` | 1 = Consulta particular (sin convenio o sin cobertura); 0 = Con convenio. |
| `ObraSocialId` | `INT` | SÍ | - | - | Identificador de obra social seleccionada (se integra en Req 05). |
| `MontoConsulta` | `DECIMAL(12,2)` | NO | - | `CHECK (MontoConsulta >= 0)` | Arancel total pactado de la consulta médica. |
| `RequiereSena` | `BIT` | NO | - | `DEFAULT (0)` | 1 si el paciente debe pagar seña del 50% para confirmar reserva. |
| `MontoSena` | `DECIMAL(12,2)` | NO | - | `DEFAULT (0)` | Importe de la seña (50% de MontoConsulta en casos particulares). |
| `SenaPagada` | `BIT` | NO | - | `DEFAULT (0)` | 1 si la seña requerida fue acreditada exitosamente. |
| `ExpiracionReservaTemporal` | `DATETIME2(7)` | SÍ | - | - | TTL para abonar la seña antes de liberar el slot (ej. 15 min). |
| `TurnoOrigenReprogramacionId` | `UNIQUEIDENTIFIER` | SÍ | FK | `REFERENCES dbo.Turnos(TurnoId)` | Trazabilidad del turno precedente si este turno surge de reprogramación. |
| `FechaCreacion` | `DATETIME2(7)` | NO | - | `DEFAULT (SYSUTCDATETIME())` | Marca temporal UTC del registro. |
| `FechaModificacion` | `DATETIME2(7)` | SÍ | - | - | Marca temporal UTC de la última actualización de estado. |

---

### 2.7. Tabla: `dbo.TransaccionesTurno`
Registro de cobros de seña del 50% y reintegros financieros por cancelaciones dentro del plazo reglamentario.

| Columna | Tipo de Dato SQL Server | Nulo | Clave | Restricciones / Defaults | Descripción |
|---|---|---|---|---|---|
| `TransaccionTurnoId` | `UNIQUEIDENTIFIER` | NO | PK | `DEFAULT (NEWSEQUENTIALID())` | Identificador único de la operación de pago/reintegro. |
| `TurnoId` | `UNIQUEIDENTIFIER` | NO | FK | `REFERENCES dbo.Turnos(TurnoId)` | Turno médico al que se imputa la transacción. |
| `TipoTransaccion` | `NVARCHAR(30)` | NO | - | - | Tipo de movimiento: `'PAGO_SENA'`, `'REINTEGRO_SENA'`. |
| `Monto` | `DECIMAL(12,2)` | NO | - | `CHECK (Monto > 0)` | Monto liquidado en la operación. |
| `Moneda` | `NVARCHAR(3)` | NO | - | `DEFAULT ('ARS')` | Moneda ISO 4217 (ej. `'ARS'`). |
| `MetodoPago` | `NVARCHAR(50)` | NO | - | - | Canal de cobro (ej. `'TARJETA_CREDITO'`, `'MERCADOPAGO'`). |
| `ReferenciaPasarela` | `NVARCHAR(150)` | NO | - | - | Identificador unívoco provisto por la pasarela de pagos. |
| `EstadoTransaccion` | `NVARCHAR(50)` | NO | - | - | Estado de la operación: `'APROBADA'`, `'RECHAZADA'`, `'REEMBOLSADA'`. |
| `FechaTransaccion` | `DATETIME2(7)` | NO | - | `DEFAULT (SYSUTCDATETIME())` | Fecha y hora UTC del procesamiento en la pasarela. |
| `MotivoReintegro` | `NVARCHAR(255)` | SÍ | - | - | Justificación registrada en cancelaciones con reintegro (>= 24h). |
| `PayloadRespuesta` | `NVARCHAR(MAX)` | SÍ | - | - | JSON con el payload de respuesta de la pasarela para fines de auditoría técnica. |
