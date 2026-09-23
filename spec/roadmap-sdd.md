# Roadmap de Diseño y Especificación de Software

## Estado del documento

- **Estado:** Borrador de trabajo
- **Fase:** Fase 0 — Diseño y Especificación de Software (SDD)
- **Fuente de verdad:** `requerimientos/`
- **Documento excluido:** `personal/MediStack_Requerimientos.md`
- **Modo de trabajo:** Híbrido: decisiones en conversación y seguimiento persistente en este documento

## Decisiones aprobadas

### 1. Alcance de la Fase 0

Durante la Fase 0 no se implementará lógica de aplicación, endpoints, controladores, páginas funcionales ni integraciones operativas.

Sí se podrán crear artefactos técnicos documentales y demostrativos:

- Documentación funcional y técnica en Markdown.
- Diagramas conceptuales y de arquitectura en Mermaid.
- Diccionarios de datos.
- Clases de modelo de dominio C# descriptivas, sin lógica ejecutable.
- Contratos y firmas conceptuales de la BLL.
- Propuestas de tablas SQL Server.
- Scripts SQL descriptivos con sintaxis realista, incluyendo el patrón equivalente a `CREATE TABLE IF NOT EXISTS` de SQL Server.
- Mocks visuales estáticos en HTML/CSS, sin lógica de negocio ni conexión con backend.

Los scripts SQL y las clases C# serán propuestas de diseño. No se ejecutarán contra una base de datos real ni se tratarán como implementación de producción durante esta fase.

### 2. Identificadores de entidades

- Usar `GUID` en C# y `UNIQUEIDENTIFIER` en SQL Server para usuarios y entidades cuyos identificadores puedan exponerse o cuyo uso pueda permitir enumeración o acceso indebido.
- Usar `INT IDENTITY` para entidades internas sin riesgo relevante de exposición o enumeración.
- La clasificación deberá justificarse por entidad; no se aplicará de forma automática sin analizar el contexto.

### 3. Idioma e identificadores

- La documentación, los comentarios, los textos visibles y las explicaciones se redactarán en español.
- Los identificadores técnicos seguirán una convención única, pendiente de definir antes de documentar el primer módulo.

### 4. Aprobación

- Cada artefacto tendrá un estado explícito.
- La aprobación se registrará únicamente como decisión aprobada por el usuario, sin exigir fecha ni número de versión.
- La aprobación se reflejará tanto en el documento correspondiente como en este roadmap.

### 5. Estados de trabajo

Los estados acordados son:

- `Borrador`
- `En revisión`
- `Bloqueado por incógnitas`
- `Aprobado`
- `Rechazado`
- `Obsoleto`

### 6. Seguridad, datos e inteligencia artificial

Cada requerimiento deberá analizar, cuando corresponda:

- Permisos y roles.
- Datos personales o clínicos sensibles.
- Auditoría.
- Confirmación humana para acciones sensibles o críticas.
- Restricciones y permisos de las herramientas del asistente de IA.
- Riesgos de exposición, enumeración o uso indebido de identificadores.

### 7. Trazabilidad y aceptación

Cada requerimiento deberá incluir:

- Matriz de trazabilidad.
- Criterios de aceptación verificables con formato equivalente a Dado/Cuando/Entonces.

### 8. Mocks visuales

Los mocks deberán representar los estados funcionales relevantes, incluyendo cuando corresponda:

- Estado normal.
- Estado vacío.
- Carga.
- Validaciones y errores.
- Permisos insuficientes.
- Confirmación humana para acciones críticas.

Se aplicará accesibilidad básica y diseño responsive cuando corresponda.

### 9. Definición de módulo terminado

Un módulo podrá cerrarse cuando todos sus elementos estén completos y aprobados, o cuando los pendientes hayan sido aceptados explícitamente como tales:

- Requisito fuente identificado.
- Análisis funcional y de errores.
- Entidades y relaciones.
- Diccionario de datos.
- Propuesta de tablas.
- Modelos de dominio.
- Contratos BLL y funciones de persistencia conceptuales.
- Seguridad, permisos y auditoría.
- Incógnitas registradas.
- Criterios de aceptación.
- Matriz de trazabilidad.
- Especificación completa.
- Mock visual, cuando corresponda.
- Revisión final y aprobación.

## Convenciones pendientes de definir

- Convención de nombres para carpetas, archivos, entidades, tablas y columnas.
- Convención de nombres para clases y firmas BLL.
- Criterios detallados para clasificar una entidad como expuesta o sujeta a riesgo de enumeración.
- Estructura exacta de la matriz de trazabilidad.
- Plantilla definitiva de estados y errores por requerimiento.

## Seguimiento por requerimiento

| Requerimiento | Estado | Diccionario | Aprobación | Especificación | Mock | Trazabilidad | Incógnitas |
|---|---|---|---|---|---|---|---|
| 1. Registrar usuarios | Borrador | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente |
| 2. Gestionar turnos | Borrador | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente |
| 3. Gestionar pacientes | Borrador | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente |

## Procedimiento operativo acordado

1. Seleccionar un requerimiento de `requerimientos/`.
2. Analizar actores, permisos, entradas, flujo principal, flujos alternativos, salidas, errores y casos límite.
3. Identificar entidades, relaciones, estados, catálogos, auditoría y datos sensibles.
4. Proponer el diccionario de datos y detenerse para obtener aprobación humana.
5. Documentar tablas SQL Server y modelos de dominio C# conforme al alcance aprobado.
6. Definir contratos BLL y funciones de persistencia sin implementar lógica de aplicación.
7. Registrar incógnitas, supuestos, riesgos y decisiones pendientes.
8. Redactar criterios de aceptación y matriz de trazabilidad.
9. Elaborar la especificación completa.
10. Crear el mock visual estático cuando corresponda.
11. Ejecutar una revisión de consistencia y registrar la aprobación.
12. Actualizar este roadmap.
