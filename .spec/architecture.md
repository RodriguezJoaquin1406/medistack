## 1. Estructura de Archivos y Carpetas del Proyecto

> [!NOTE]
> **RECORDATORIO**: El repositorio maneja únicamente SDD (diseño y especificación técnica), sin código ejecutable operativo (Regla Cero).

```text
medistack/
├── AGENTS.md                               # Reglas normativas, restricciones y directrices para IAs
├── .spec/                                  # Metadocumentación del proyecto y gobernanza de especificación
│   ├── present.md                          # Estado actual del proyecto y estrategia operativa entre sesiones
│   ├── architecture.md                     # Estructura del repositorio y arquitectura de especificación
│   ├── mission.md                          # Propuesta, alcance y objetivos del proyecto
│   ├── roadmap.md                          # Checklist y seguimiento de requerimientos procesados
│   └── spec_guide.md                       # Estándares técnicos, arquitectura por capas y Definition of Done
├── especificacion/                         # Especificación técnica consolidada y agrupada por capas
│   ├── basededatos/                        # Diccionarios de datos, diagramas ER y scripts SQL DDL descriptivos
│   ├── dominio/                            # Modelos de dominio descriptivos en C#
│   ├── paginas/                            # Prototipos visuales y páginas HTML/CSS estáticas
│   └── incognitas/                         # Registro centralizado de incógnitas, supuestos y decisiones técnicas
└── requerimientos/                         # Insumos funcionales base (solo lectura / sin enriquecimiento técnico interno)
    ├── 01_RegistrarUsurios/                # Req 1: Autenticación, roles y credenciales
    ├── 02_GestionarTurnos/                 # Req 2: Reserva, señas 50%, cancelación y reprogramación
    ├── 03_GestionPacientes/                # Req 3: Padrón y gestión de pacientes
    ├── 04_GestionProfesionales/            # Req 4: Matrículas, especialidades y disponibilidad
    ├── 05_GestionarObrasSociales/          # Req 5: Coberturas y convenios tripartitos
    ├── 06_FichaMedicaPaciente/             # Req 6: Ficha clínica e historial cronológico
    ├── 07_CobrosCajaDiaria/                # Req 7: Cobros, copagos, señas y caja diaria
    ├── 08_RegistrarAusentes/               # Req 8: Conciliación y registro de ausencias
    ├── 09_LiquidacionHonorarios/           # Req 9: Liquidación a profesionales
    ├── 10_ReportesAdministrativos/         # Req 10: Métricas y reportes administrativos
    ├── 11_Agente_IA/                       # Req 11: Asistente Clínico de IA
    └── RequerimientosNoFuncionales.md      # Desempeño, seguridad, disponibilidad y accesibilidad
```

---

## 2. Flujo de Especificación Agrupada

El flujo de trabajo desacopla los requerimientos funcionales de la especificación técnica:

1. **Lectura Funcional:** Se toma el requerimiento base desde `requerimientos/<modulo>/requerimiento.md`.
2. **Traspaso Técnico:** Se extraen las definiciones de ingeniería (entidades, tablas SQL, clases C#, prototipos de páginas e incógnitas) y se incorporan en la carpeta `especificacion/` dentro de su respectiva capa (`basededatos/`, `dominio/`, `paginas/`, `incognitas/`).
3. **Cierre:** Se marca el requerimiento como finalizado en [`.spec/roadmap.md`](file:///C:/Users/ujr001/proyectos/medistack/.spec/roadmap.md).