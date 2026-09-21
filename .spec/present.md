# Contexto y Estado del Proyecto — MediStack (Fase 0)

> **Documento de Contexto Operativo para Nuevas Sesiones e Interacciones con IAs**  
> *Este archivo es el puente de continuidad entre sesiones, todo agente que inicie una nueva sesion DEBE leer este archivo y `.spec\constitution\roadmap.md` para poder continuar con el trabajo sin perdida de contexto.*

## Estado Actual del Repositorio

- [`AGENTS.md`](file:///C:/Users/ujr001/proyectos/medistack/AGENTS.md): Documento normativo central que define las reglas de comportamiento, restricciones inviolables y directrices para las IAs.
- [`spec/guia-especificacion.md`](file:///C:/Users/ujr001/proyectos/medistack/spec/guia-especificacion.md): Guía de arquitectura, plantilla estándar SDD, Capa de Negocio conceptual, Definition of Done y checklist maestro.
- [`personal/MediStack_Requerimientos.md`](file:///C:/Users/ujr001/proyectos/medistack/personal/MediStack_Requerimientos.md): Documento monolítico original (fuente de verdad / SSOT histórica). **No debe leerse de golpe en contexto para no saturar tokens ni generar confusión**.
- `requerimientos/`: Contiene los requerimientos desglosados en subdirectorios modulares (`1_RegistrarUsurios/`, `2_GestionarTurnos/`, etc.).
- `personal/anotaciones.md`: Anotaciones del usuario sobre la estrategia de desglose.



## 5. Próxima Tarea Inmediata para la Siguiente Conversación

Al iniciar la próxima conversación, la IA asistente debe:
1. Asumir el rol de **Arquitecto de Software y Analista Funcional Senior**.
2. Respetar la **Regla Cero** (cero código ejecutable, solo especificación y mocks HTML).
3. Enfocarse en el primer lote: **Grupo 1-3** (`requerimientos/1-3/requerimientos_1_3.md`):
   - Módulo 1: Registrar e iniciar sesión de usuarios (migrar y pulir lo iniciado en `req1/`).
   - Módulo 2: Gestión integral de turnos (solicitud, señas 50%, validación de 24hs).
   - Módulo 3: Gestión del padrón de pacientes.
4. Consultar y validar con el usuario antes de avanzar a los siguientes grupos.