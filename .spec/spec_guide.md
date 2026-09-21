# Guía de Especificación Técnica (SDD), Arquitectura y Definition of Done — MediStack

Este documento centraliza los estándares de ingeniería de software, arquitectura de especificación, estructura del repositorio, plantilla estandarizada SDD, Definition of Done (DoD) y el checklist de avance para la **Fase 0 (Diseño y Especificación)** de MediStack.

---

## 1. Plantilla Estándar Obligatoria para Especificaciones (SDD)

Cada documento de especificación dentro de su respectivo módulo de `requerimientos/` debe estructurarse siguiendo obligatoriamente este formato:

```markdown
# SDD - [Número]: [Nombre del Módulo]

## 1. Introducción y Alcance
- Descripción del objetivo del módulo dentro del contexto de MediStack.
- Roles de usuario involucrados (Paciente, Profesional, Personal Administrativo).

## 2. Requerimiento Funcional Detallado
- **Inputs:** Datos de entrada, formatos y orígenes.
- **Processing:** Lógica de validación, cálculos (ej. copagos, 50% seña), transiciones de estado.
- **Outputs:** Respuestas del sistema, comprobantes, vistas generadas.
- **Error Handling:** Matriz exhaustiva de validaciones fallidas y mensajes de error específicos.

## 3. Modelo de Dominio y Base de Datos (SQL Server)
- **Diagrama Entidad-Relación:** Mermaid `erDiagram`.
- **Diccionario de Datos:**
  | Campo | Tipo SQL Server | Nullability | PK/FK | Restricciones / Descripción |
  |-------|-----------------|-------------|-------|-----------------------------|
- **Propuesta de Stored Procedures y Triggers:**
  - `sp_NombreProcedimiento` (Parámetros, propósito, lógica transaccional).

## 4. Capa de Negocio (BLL en C#) — Firmas Conceptuales
- Definición de interfaces y contratos de servicios esperados (DTOs, firmas de métodos):
  ```csharp
  // Ejemplo conceptual de firma
  public interface ITurnoService {
      RespuestaOperacion<TurnoDTO> ReservarTurno(SolicitudTurnoDTO solicitud);
  }
  ```

## 5. Integración con el Asistente Clínico de IA
- **Herramientas/Acciones expuestas para el Asistente (Function/Tool Calling):**
  - Nombre de la acción (ej: `ConsultarDisponibilidad`, `SolicitarTurno`).
  - Roles autorizados para ejecutarla.
  - ¿Requiere confirmación humana explícita? (Sí / No).
  - Flujo de interacción por texto/voz.

## 6. Prototipo Visual (Mockup)
- Enlace y descripción de la vista estática en HTML (`.html`).
- Elementos clave de la interfaz, componentes accesibles y diseño responsivo.
```

---

## 3. Definition of Done (DoD) para cada Especificación

Un módulo o requerimiento se considerará **finalizado (Done)** en Fase 0 únicamente si cumple con todas las siguientes condiciones:

1. [ ] El documento de especificación sigue rigurosamente la **Plantilla Estándar Obligatoria** con todas sus secciones completadas.
2. [ ] Se incluye un diagrama Mermaid sintácticamente válido (`erDiagram` y/o `sequenceDiagram`).
3. [ ] Se detallan los tipos de datos nativos de SQL Server (`INT`, `BIGINT`, `NVARCHAR`, `DATETIME2`, `DECIMAL(18,2)`, `BIT`) con sus restricciones y nulabilidad.
4. [ ] Se definen las firmas conceptuales de la Capa de Negocio (`BLL`) en C# sin código de implementación.
5. [ ] Se define la sección de integración con el Asistente de IA (herramientas, roles autorizados y requerimiento de confirmación humana).
6. [ ] Se incluye el prototipo visual estático en HTML/CSS (`.html`), visualizable en navegador, ordenado y accesible, sin lógica de backend.
7. [ ] Se actualiza el checklist maestro de este documento marcando la casilla correspondiente con `[x]`.

---

## 4. Checklist Maestro de Avance (Fase 0)

- [ ] **Módulo 00:** Esquema Entidad-Relación global y Diccionario de Datos maestro consolidado (`spec/database/esquema-general.md`).
- [ ] **Módulo 01:** Registrar e iniciar sesión de usuarios (`requerimientos/1_RegistrarUsurios/`).
- [ ] **Módulo 02:** Gestión integral de turnos (`requerimientos/2_GestionarTurnos/`).
- [ ] **Módulo 03:** Gestión y padrón de pacientes (`requerimientos/3_GestionPacientes/`).
- [ ] **Módulo 04:** Gestión de profesionales, matrículas y disponibilidad (`requerimientos/4_GestionProfesionales/`).
- [ ] **Módulo 05:** Obras sociales y convenios tripartitos (`requerimientos/5_GestionarObrasSociales/`).
- [ ] **Módulo 06:** Ficha médica e historial cronológico de turnos (`requerimientos/6_FichaMedicaPaciente/`).
- [ ] **Módulo 07:** Cobros, copagos, señas y caja diaria (`requerimientos/7_CobrosCajaDiaria/`).
- [ ] **Módulo 08:** Conciliación y registro de ausencias (`requerimientos/8_RegistrarAusentes/`).
- [ ] **Módulo 09:** Liquidación periódica de honorarios profesionales (`requerimientos/9_LiquidacionHonorarios/`).
- [ ] **Módulo 10:** Reportes y métricas de gestión administrativa (`requerimientos/10_ReportesAdministrativos/`).
- [ ] **Módulo 11:** Arquitectura y orquestación del Asistente Clínico de IA (`requerimientos/11_Agente_IA/`).
- [ ] **Requerimientos No Funcionales:** Desempeño, seguridad, disponibilidad y accesibilidad (`requerimientos/RequerimientosNoFuncionales.md`).
