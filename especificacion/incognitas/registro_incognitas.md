# Registro Unificado de Incógnitas, Riesgos y Supuestos Técnicos — MediStack

> **Requerimientos origen:** [Req 01 (Registrar Usuarios)](file:///C:/Users/ujr001/proyectos/medistack/requerimientos/01_RegistrarUsurios/requerimiento.md)  
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
