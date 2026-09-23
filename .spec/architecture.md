## 1. Estructura de Archivos y Carpetas del Proyecto

**RECORDATORIO**: Repositorio maneja unicamente SDD nada de codigo

```text
medistack/
├── AGENTS.md                               # Reglas, restricciones y directrices para IAs
├── spec/
│   ├── present.md                          # Estado actual del proyecto y estrategia operativa
│   ├── architecture.md                     # Archivo actual sobre estructura de carpetas del proyecto
│   ├── mission.md                          # Propuesta del proyecto
│   ├── roadmap.md                          # Checklist de Requerimientos ya procesado
│   ├── spec_guide.md                       # Reglas 
│   └── database/                           # Vacio
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