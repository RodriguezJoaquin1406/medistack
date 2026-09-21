### Gestionar pacientes



3.1 Introducción

Permite dar de alta, modificar, consultar y dar de baja los registros de pacientes en el sistema.

3.2 Inputs

- Datos personales del paciente (nombre, apellido, DNI, fecha de nacimiento, obra social, teléfono, dirección, email)

- Acción a realizar (alta / baja / modificación / consulta)

3.3 Processing

- Validar los datos ingresados

- Verificar que el DNI no esté duplicado en un alta

- Actualizar el registro en la base de datos según la acción solicitada

3.4 Outputs

- Confirmación de la operación realizada

- Listado o ficha del paciente actualizado

3.5 Error Handling

- Si el DNI ya existe, se rechaza el alta

- Si el paciente no existe, se informa que no fue encontrado

- Si faltan datos obligatorios, se solicita completarlos
