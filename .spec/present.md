# Contexto y Estado del Proyecto — MediStack (Fase 0)

> **Documento de Contexto Operativo para Nuevas Sesiones e Interacciones con IAs**  
> *Este archivo es el puente de continuidad entre sesiones. Todo agente que inicie una nueva sesión DEBE leer este archivo y [`.spec/roadmap.md`](file:///C:/Users/ujr001/proyectos/medistack/.spec/roadmap.md) para poder continuar con el trabajo sin pérdida de contexto.*

## Estado Actual del Repositorio

- [`AGENTS.md`](file:///C:/Users/ujr001/proyectos/medistack/AGENTS.md): Documento normativo central que define las reglas de comportamiento, restricciones inviolables, Regla Cero y directrices para las IAs.
- [`.spec/spec_guide.md`](file:///C:/Users/ujr001/proyectos/medistack/.spec/spec_guide.md): Guía de estándares técnicos, arquitectura de especificación por capas, Definition of Done y checklist maestro.
- [`.spec/architecture.md`](file:///C:/Users/ujr001/proyectos/medistack/.spec/architecture.md): Estructura del repositorio y arquitectura técnica global.
- [`.spec/mission.md`](file:///C:/Users/ujr001/proyectos/medistack/.spec/mission.md): Declaración de misión, alcance y objetivos del sistema.
- [`.spec/roadmap.md`](file:///C:/Users/ujr001/proyectos/medistack/.spec/roadmap.md): Checklist de seguimiento del avance por requerimiento. **Módulo 01 completado y verificado.**
- `requerimientos/`: Contiene los requerimientos funcionales desglosados en subdirectorios modulares (`01_RegistrarUsurios/`, `02_GestionarTurnos/`, etc.). Son insumos de lectura funcional; **no se enriquecen técnicamente dentro de estas carpetas**.
- `especificacion/`: Carpeta técnica centralizada que aloja la especificación técnica agrupada y estructurada por capas de documentación en español (`basededatos/`, `dominio/`, `paginas/`, `incognitas/`).
  - **Módulo 01 (Usuarios y Autenticación):** Especificado y aprobado en sus 4 capas técnicas.

---

## Flujo de Trabajo Vigente

El flujo operativo acordado consiste en:
1. **Leer requerimiento:** Tomar el requerimiento funcional de `requerimientos/<modulo>/requerimiento.md`.
2. **Pasar información a `especificacion/`:** Volcar la información técnica (entidades, tablas SQL, clases C#, prototipos de páginas e incógnitas) a la carpeta `especificacion/` agrupada en sus capas (`basededatos/`, `dominio/`, `paginas/`, `incognitas/`).
3. **Marcar requerimiento como finalizado:** Actualizar el estado en [`.spec/roadmap.md`](file:///C:/Users/ujr001/proyectos/medistack/.spec/roadmap.md).

---

## Próxima Tarea Inmediata para la Siguiente Conversación

Al continuar con la especificación:
1. Asumir el rol de **Arquitecto de Software y Analista Funcional Senior**.
2. Respetar la **Regla Cero** (cero código ejecutable operativo; solo especificaciones descriptivas, modelos y prototipos estáticos).
3. Iniciar el procesamiento del **Módulo 02: Gestión Integral de Turnos** ([`requerimientos/02_GestionarTurnos/requerimiento.md`](file:///C:/Users/ujr001/proyectos/medistack/requerimientos/02_GestionarTurnos/requerimiento.md)):
   - Leer requerimiento funcional (estados de turnos, reserva con seña 50%, cancelación, reprogramación).
   - Incorporar o evolucionar tablas y tipos SQL en `especificacion/basededatos/`.
   - Incorporar o evolucionar clases de dominio C# en `especificacion/dominio/`.
   - Desarrollar prototipos visuales en `especificacion/paginas/`.
   - Documentar supuestos y decisiones en `especificacion/incognitas/`.
   - Presentar el lote para aprobación y marcar el avance en [`.spec/roadmap.md`](file:///C:/Users/ujr001/proyectos/medistack/.spec/roadmap.md).