# Registro Unificado de Incógnitas, Riesgos y Supuestos Técnicos — MediStack

> **Requerimientos origen:** [Req 01 (Registrar Usuarios)](file:///C:/Users/ujr001/proyectos/medistack/requerimientos/01_RegistrarUsurios/requerimiento.md), [Req 02 (Gestionar Turnos)](file:///C:/Users/ujr001/proyectos/medistack/requerimientos/02_GestionarTurnos/requerimiento.md)  
> **Estado:** Aprobado (Verificado por el usuario)

---

## 1. Catálogo de Incógnitas y Riesgos Identificados

### INC-01-01: Riesgo Severo de Elevación de Privilegios en Autoregistro Público
- **Origen:** Req 01, sección 1.1 y 1.2: *"Permite que los distintos tipos de usuarios... (pacientes, profesionales y personal administrativo) se registren... Rol asignado (paciente / profesional / administrativo)"*.
- **Riesgo:** Si el formulario de autoregistro web público permite a cualquier usuario anónimo seleccionar el rol `Profesional` o `Administrativo`, cualquier persona podría acceder inmediatamente a agendas clínicas, historias médicas confidenciales de pacientes o datos de recaudación de caja.
- **Postura del Arquitecto (Disidencia Técnica):**
  - **Opción Recomendada:** El formulario de registro público en la web debe permitir **únicamente el alta de Pacientes** (`Rol = Paciente`). Los usuarios con rol `Profesional` o `Administrativo` deben ser creados exclusivamente por un `Administrador` del centro médico desde un módulo interno de gestión de personal, o bien requerir un flujo de validación manual donde la cuenta queda en estado `Inactivo` hasta que el administrador verifique la matrícula médica y autorice el acceso.
- **Estado:** Pendiente de definición y aprobación humana.

---

### INC-01-02: Flujo de Recuperación de Contraseñas no especificado
- **Origen:** Req 01 no contempla el caso de uso en que un usuario olvida su contraseña o su cuenta resulta bloqueada por intentos fallidos.
- **Supuesto de Diseño:** Para la Fase 0 se asume que:
  1. Si la cuenta es bloqueada por exceso de intentos fallidos, el desbloqueo temporal es automático transcurridos 15 minutos, o manual vía un Administrador.
  2. El reseteo de contraseñas por correo electrónico (con enlace seguro de un solo uso y expiración de 1 hora) se especificará como requerimiento complementario.
- **Estado:** Supuesto documentado.

---

### INC-01-03: Esquema de Gestión de Sesiones en ASP.NET Web Forms
- **Origen:** Requerimientos no funcionales (Web Forms .NET 4.8).
- **Análisis:** En ASP.NET Web Forms se debe definir el mecanismo de autenticación y persistencia de sesión:
  - Opción A: `FormsAuthentication` clásico de cookies encriptadas de ASP.NET.
  - Opción B: Middleware OWIN / Katana con cookies de autenticación de ASP.NET Identity.
- **Propuesta de Arquitectura:** Utilizar autenticación mediante cookies seguras (`HttpOnly`, `SameSite=Lax`, `Secure`) en .NET Framework 4.8.
- **Estado:** Pendiente de confirmación.

---

### INC-02-01: Restricción Restrictiva de "Un Solo Turno Pendiente por Paciente"
- **Origen:** Req 02, secciones 2.3 y 2.5: *"Validar que la fecha no esté ocupada y que el usuario no tenga otro turno pendiente"*, *"Si el usuario ya tiene un turno pendiente, se informa y no se permite reservar otro hasta resolverlo"*.
- **Riesgo / Falla de Usabilidad:** En un policlínico o centro médico ambulatorio, es un caso de uso común y legítimo que un paciente requiera turnos pendientes en simultáneo para diferentes especialidades (ej. Cardiología y Odontología, o consulta clínica y posterior control con especialista). Bloquear de forma global la reserva de cualquier turno si existe una cita agendada para el mes próximo reduce drásticamente la utilidad del portal de autoservicio y obliga al paciente a recurrir a la atención telefónica o presencial.
- **Postura del Arquitecto (Disidencia Técnica):**
  - **Opción Recomendada:** Acotar la regla de negocio a: *"El paciente no puede tener más de un turno pendiente **para la misma especialidad**"* o permitir un cupo máximo global (ej. hasta 3 turnos activos).
  - **Estado:** Documentado como incógnita abierta. Para el modelo de datos y validaciones iniciales se modela la regla estricta requerida por el insumo funcional, permitiendo su flexibilización por configuración de negocio.

---

### INC-02-02: Bloqueo de Horarios y Prevención de Condiciones de Carrera durante el Pago de la Seña
- **Origen:** Req 02, sección 2.3: *"Si el profesional elegido no tiene convenio con la obra social del paciente... calcular la seña (50% del valor de la consulta) y procesar el pago mediante la pasarela de pago integrada"*.
- **Riesgo:** El proceso de pago en una pasarela electrónica externa toma entre 1 y 5 minutos (interacción del usuario, validación 3D Secure, OTP bancario). Si el slot del turno no se bloquea temporalmente al momento de iniciar el checkout, dos pacientes podrían pagar la seña por el mismo turno médico (doble cobro y conflicto de agenda). Si se bloquea permanentemente sin confirmación, un abandono del carrito bloquearía el turno a otros pacientes.
- **Solución de Arquitectura:**
  - Se incorpora el estado `PENDIENTE_PAGO` con un campo `ExpiracionReservaTemporal` (TTL de 15 minutos).
  - Si el pago es exitoso vía webhook de la pasarela, el estado cambia atómicamente a `CONFIRMADO`.
  - Si expira el TTL sin confirmación, un worker o validación en consulta considera el slot liberado y disponible para otros pacientes.
- **Estado:** Solución de diseño incorporada en el modelo de datos y tablas.

---

### INC-02-03: Destino de la Seña y Flujo Operativo ante Cancelaciones con menos de 24 Horas
- **Origen:** Req 02, secciones 2.3 y 2.5: *"Si se intenta cancelar o reprogramar con menos de 24 horas de anticipación, se rechaza la operación y se informa el motivo"*.
- **Riesgo / Duda de Proceso:** El autoservicio web bloquea la cancelación cuando faltan menos de 24 horas. Sin embargo, si el paciente notifica su imposibilidad de concurrir o simplemente no asiste:
  1. ¿La seña del 50% abonada queda definitivamente retenida por la clínica / profesional en concepto de resarcimiento por slot bloqueado?
  2. ¿Qué estado final adquiere el turno: `AUSENTE` o `CANCELADO_TARDIO`?
- **Postura del Arquitecto:** La retención del 50% de la seña en cancelaciones fuera de término actúa como mecanismo de desincentivo al ausentismo (*no-show*). Se asume que el turno no asistido derivará en estado `AUSENTE` (procesado en el Módulo 08) sin reintegro de seña, impactando favorablemente en la liquidación del profesional (Módulo 09).
- **Estado:** Supuesto documentado para validación en Módulos 08 y 09.
