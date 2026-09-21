### Liquidar honorarios profesionales



9.1 Introducción

Permite calcular y generar la liquidación de honorarios mensual correspondiente a cada profesional de la clínica, basándose en los turnos atendidos y cobrados, discriminando los montos según si la atención fue particular o mediante convenio con obra social.

9.2 Inputs

- Profesional a liquidar.

- Período a liquidar (Fecha desde / Fecha hasta).

- Esquema de honorarios (porcentaje o monto fijo acordado con el profesional).

9.3 Processing

- El sistema recupera todos los turnos del profesional que se encuentren en estado "Cobrado" dentro del período seleccionado.

- El sistema verifica, turno por turno, el medio de atención (Particular o por Convenio con Obra Social).

- El sistema aplica el esquema de honorarios correspondiente al monto base o al copago de cada turno.

- El sistema suma los montos y calcula el total a liquidar.



9.4 Outputs

- Detalle de liquidación por profesional y período generado en pantalla.

- Comprobante de liquidación exportable.

9.5 Error Handling

- Si no existen cobros registrados en el período seleccionado, el sistema informa: "No hay honorarios a liquidar para este profesional en el período indicado".

- Si el esquema de honorarios no está configurado, el sistema solicita: "Debe definir el esquema de honorarios del profesional antes de procesar la liquidación".

