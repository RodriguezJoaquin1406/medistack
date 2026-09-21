### Gestionar turnos

2.1 Introducción

Permite que los pacientes soliciten, reserven, reprogramen o cancelen turnos de forma digital, eligiendo el profesional con el que desean atenderse dentro de la especialidad y fecha solicitada. Si el profesional elegido no tiene convenio con la obra social del paciente (o el paciente no tiene obra social registrada), se solicita el pago de una seña equivalente al 50% del valor de la consulta para confirmar la reserva.

2.2 Inputs

- Datos personales del usuario (nombre, apellido, DNI, email, teléfono)

- Especialidad solicitada

- Fecha solicitada

- Profesional elegido, de la lista de disponibles para esa especialidad y fecha

- Nota / razón del turno

- Datos de pago de la seña, cuando corresponda

- Turno a reprogramar o cancelar, y nueva fecha (en caso de reprogramación)

2.3 Processing

- Validar que la fecha no esté ocupada y que el usuario no tenga otro turno pendiente

- Recuperar el listado de profesionales disponibles para la especialidad y fecha solicitada

- Para cada profesional de la lista, verificar si tiene convenio con la obra social del paciente para esa especialidad (ver Gestionar obras sociales) y marcarlo como cubierto o particular

- El paciente selecciona un profesional de la lista

- Si el profesional elegido no tiene convenio con la obra social del paciente, o el paciente no tiene obra social, calcular la seña (50% del valor de la consulta) y procesar el pago mediante la pasarela de pago integrada

- Si el profesional elegido tiene convenio, confirmar la reserva sin solicitar seña

- Al cancelar o reprogramar: validar que el turno pertenezca al paciente que solicita la operación, y que la operación se realice con al menos 24 horas de anticipación

- En caso de reprogramación, validar que la nueva fecha no esté ocupada y repetir la selección de profesional (y de seña, si corresponde)

- En caso de cancelación dentro del plazo permitido, reintegrar la seña pagada, si la hubo

- Actualizar el estado del turno en la base de datos

2.4 Outputs

- Confirmación de la reserva, con fecha, profesional, especialidad, motivo indicado, y si se cobró seña o no

- Turno registrado y visible en el historial del paciente

- Comprobante de pago de la seña, cuando corresponda

- Confirmación de la cancelación o reprogramación

- Confirmación del reintegro de la seña, cuando corresponda

2.5 Error Handling

- Si la fecha solicitada ya está ocupada, se informa y se sugieren fechas alternativas

- Si el usuario ya tiene un turno pendiente, se informa y no se permite reservar otro hasta resolverlo

- Si no hay profesionales disponibles para la especialidad y fecha solicitada, se informa

- Si el pago de la seña es rechazado, la reserva no se confirma

- Si el turno no pertenece al paciente, se rechaza la operación de cancelar o reprogramar

- Si se intenta cancelar o reprogramar con menos de 24 horas de anticipación, se rechaza la operación y se informa el motivo

- Si la nueva fecha solicitada para la reprogramación ya está ocupada, se informa y se sugieren alternativas
