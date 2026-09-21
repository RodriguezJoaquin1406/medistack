### Consultar ficha e historial de turnos del paciente



6.1 Introducción

Permite a los profesionales y al personal administrativo consultar la ficha clínica completa de un paciente, visualizando sus datos personales, información de contacto, obra social y el historial detallado de turnos (pendientes, realizados y cancelados) para un seguimiento integral de su atención.

6.2 Inputs

- Criterio de búsqueda del paciente (DNI o Nombre/Apellido).

- Filtros de búsqueda para el historial (Rango de fechas: Fecha desde / Fecha hasta).

- Filtro por estado del turno (Todos / Pendiente / Realizado / Cancelado).

6.3 Processing

- El usuario (Profesional o Administrativo) ingresa el DNI o el nombre del paciente en el buscador del sistema.

- El sistema valida que el paciente se encuentre registrado en la base de datos (creado previamente mediante el módulo de gestión de pacientes).

- El sistema recupera y expone los datos personales y de contacto del paciente.

- El sistema realiza una consulta a la base de datos buscando todos los turnos asociados al identificador de ese paciente.

- El sistema aplica los filtros de fecha y estado seleccionados por el usuario, ordenando los resultados de forma cronológica descendente (del más reciente al más antiguo).

6.4 Outputs

- Pantalla con la ficha completa del paciente mostrando sus datos personales consolidados.

- Listado o grilla con el historial de turnos, detallando para cada uno: fecha, profesional, especialidad, motivo y estado actual.

6.5 Error Handling

- Paciente no encontrado: Si el DNI o nombre ingresado no coincide con ningún registro en la base de datos, el sistema interrumpe la búsqueda y muestra el mensaje: "Error: No se encontró ningún paciente con los datos ingresados. Verifique la información o proceda a darlo de alta".

- Historial vacío: Si el paciente existe pero no posee turnos que coincidan con los filtros aplicados, el sistema muestra la ficha personal junto con una tabla vacía y el aviso: "El paciente no posee turnos registrados para los criterios seleccionados".

- Datos de búsqueda en blanco: Si el usuario intenta ejecutar la búsqueda sin ingresar ningún parámetro, el sistema advierte: "Atención: Debe ingresar un DNI o nombre válido para buscar al paciente".

