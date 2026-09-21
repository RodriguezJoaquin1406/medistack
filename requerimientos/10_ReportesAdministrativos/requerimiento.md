### Generar reportes administrativos

10.1 Introducción

Permite a los administradores generar y visualizar estadísticas y reportes consolidados sobre la operatoria clínica, como balances de caja y volumen de atención.



10.2 Inputs

- Tipo de reporte a generar (ej. Balance de caja diaria, Turnos por especialidad, Pacientes por obra social).

- Rango de fechas.

10.3 Processing

- El sistema ejecuta las consultas en la base de datos según los filtros ingresados.

- El sistema agrupa y totaliza los datos transaccionales (sumatoria de cobros, conteo de turnos).

- El sistema formatea los datos para su visualización.

10.4 Outputs

- Reporte visualizado en pantalla con gráficos o tablas de datos.

- Opción para exportar o imprimir el reporte resultante.

10.5 Error Handling

- Si no hay registros en la base de datos para el período seleccionado, el sistema muestra el mensaje: "No se encontraron datos para generar el reporte en el período seleccionado".

