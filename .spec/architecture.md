## 1. Estructura de Archivos y Carpetas del Proyecto

El repositorio desacopla el comportamiento de los agentes (`AGENTS.md`), las notas y fuente original (`personal/`), el estado operativo (`spec/`) y el trabajo modular de especificación (`requerimientos/`):

```text
medistack/
├── AGENTS.md                               # Reglas de gobierno, restricciones y directrices para IAs
├── README.md                               # Visión general y fases del proyecto
├── personal/
│   ├── MediStack_Requerimientos.md         # Documento monolítico original (SSOT histórico)
│   └── anotaciones.md                      # Notas y acuerdos de trabajo
├── spec/
│   ├── present.md                          # Estado actual del proyecto y estrategia operativa
│   ├── guia-especificacion.md              # Esta guía (arquitectura, plantilla SDD y DoD)
│   └── database/                           # Esquema global consolidado y ERD maestro
└── requerimientos/                         # Módulos desglosados para especificación
    ├── 1_RegistrarUsurios/                 # Req 1: Autenticación, roles y credenciales
    ├── 2_GestionarTurnos/                  # Req 2: Reserva, señas 50%, cancelación y reprogramación
    ├── 3_GestionPacientes/                 # Req 3: Padrón y gestión de pacientes
    ├── 4_GestionProfesionales/             # Req 4: Matrículas, especialidades y disponibilidad
    ├── 5_GestionarObrasSociales/           # Req 5: Coberturas y convenios tripartitos
    ├── 6_FichaMedicaPaciente/              # Req 6: Ficha clínica e historial cronológico
    ├── 7_CobrosCajaDiaria/                 # Req 7: Cobros, copagos, señas y caja diaria
    ├── 8_RegistrarAusentes/                # Req 8: Conciliación y registro de ausencias
    ├── 9_LiquidacionHonorarios/            # Req 9: Liquidación a profesionales
    ├── 10_ReportesAdministrativos/         # Req 10: Métricas y reportes administrativos
    ├── 11_Agente_IA/                       # Req 11: Orquestación del Asistente Clínico de IA
    ├── RequerimientosNoFuncionales.md      # Desempeño, seguridad, disponibilidad y accesibilidad
    └── ejemploRequerimiento1/              # Referencia de trabajo (req1.md y req1.html)
```

---