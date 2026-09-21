### Gestionar profesionales



4.1 Introducción

Permite dar de alta, modificar, consultar y dar de baja los registros de profesionales en el sistema, incluyendo sus especialidades y horarios de atención disponibles, que luego se utilizan para validar la reserva de turnos.

4.2 Inputs

- Datos personales del profesional (nombre, apellido, DNI, email, teléfono)

- Especialidad/es

- Matrícula profesional

- Horarios de atención disponibles (días y franjas horarias)

- Acción a realizar (alta / baja / modificación / consulta)

4.3 Processing

- Validar que los datos obligatorios estén completos

- Verificar que el DNI y la matrícula no estén duplicados en un alta

- Validar que los horarios cargados no se superpongan entre sí

- Actualizar el registro en la base de datos según la acción solicitada

4.4 Outputs

- Confirmación de la operación realizada

- Listado o ficha del profesional actualizado, con su especialidad y disponibilidad horaria

4.5 Error Handling

- Si el DNI o la matrícula ya existen, se rechaza el alta

- Si el profesional no existe, se informa que no fue encontrado

- Si faltan datos obligatorios, se solicita completarlos

- Si los horarios cargados se superponen, se informa el conflicto y no se guarda la carga
