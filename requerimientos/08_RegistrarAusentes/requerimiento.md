### Registrar turnos ausentes

8.1 Introducción

Permite al equipo administrativo revisar, al cierre del día, los turnos que no fueron marcados como atendidos ni cancelados, y confirmar manualmente cuáles corresponden a pacientes ausentes, dado que los horarios reales de atención no siempre coinciden con la hora asignada al turno.

8.2 Inputs

- Fecha del día a revisar

- Listado de turnos del día que no quedaron en estado “Cobrado” ni “Cancelado”

- Confirmación del administrativo por cada turno de la lista (Ausente/Pendiente de carga)

8.3 Processing

- Al cierre de caja diaria, el sistema genera la lista de turnos del día que no fueron marcados como “Cobrado” ni “Cancelado”

- El equipo administrativo coteja esa lista contra el registro real de pacientes atendidos en el día

- Por cada turno de la lista, el administrativo confirma si el paciente fue atendido (falta cargar el cobro) o si estuvo ausente

- El sistema actualiza el estado del turno según lo confirmado

8.4 Outputs

- Turnos actualizados a “Ausente” o dejados pendientes de cobro, según la confirmación del administrativo

- Listado final de ausentes del día

8.5 Error Handling

- Si la revisión no se completa antes del cierre, los turnos sin confirmar quedan pendientes de revisión para el día siguiente

- Si se intenta marcar como ausente un turno que ya fue cobrado, el sistema rechaza el cambio de estado


