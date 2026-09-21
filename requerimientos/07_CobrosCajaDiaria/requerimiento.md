### Registrar cobros y caja diaria



7.1 Introducción

Permite al personal administrativo registrar el pago de los turnos o consultas de los pacientes, calculando el arancel final o copago a abonar según el medio de pago, la obra social del paciente y la adhesión del profesional, acumulando las transacciones para el cierre y control de la caja diaria de la clínica.



7.2 Inputs

- dentificador del turno a cobrar (o DNI del paciente).

- Medio de pago (Efectivo, Tarjeta de Débito/Crédito, Transferencia).

- Datos de la transacción (N° de comprobante / código de autorización de tarjeta o transferencia).

- Monto ingresado por el cliente (en pagos con efectivo, para el cálculo del vuelto)

7.3 Processing

- El administrativo selecciona el turno a cobrar desde la lista de turnos del día o buscando por el DNI del paciente.

- El sistema recupera el valor base de la consulta médica correspondiente a la especialidad del profesional.

- El sistema verifica si el profesional tiene convenio con la obra social del paciente para la especialidad del turno:

- Si tiene convenio: El sistema recupera el porcentaje de cobertura asignado por la obra social para esa especialidad, calcula el monto que cubre la obra social y determina el copago restante a abonar por el paciente.

- Si no tiene convenio, o es paciente particular: El sistema establece que el 100% del valor de la consulta debe ser abonado por el paciente.

- Si el turno tuvo una seña pagada al momento de la reserva, el sistema descuenta ese monto del total a abonar.

- El administrativo selecciona el medio de pago e ingresa los datos del comprobante.

- En caso de pago en efectivo, el sistema calcula el vuelto a entregar restando el arancel al monto recibido.

- El sistema guarda la transacción, cambia el estado del turno a "Cobrado" e incrementa el saldo consolidado de la caja diaria en el monto abonado por el paciente.

7.4 Outputs

- Comprobante o recibo de cobro emitido por pantalla y/o imprimible (con el desglose de cobertura de obra social y copago del paciente).

- Turno actualizado a estado "Cobrado".

- Resumen del saldo parcial de caja diaria actualizado.

7.5 Error Handling

- Turno ya cobrado: Si se intenta cobrar un turno que ya posee un registro de pago activo, el sistema interrumpe el proceso y notifica: "Error: El turno seleccionado ya se encuentra cobrado".

- Monto recibido insuficiente: Si en un pago en efectivo el valor ingresado es menor al total a abonar, el sistema advierte: "Error: El monto recibido ($[Ingresado]) es inferior al saldo a cobrar ($[Total])".

- Rechazo de tarjeta o pago digital: Si la transacción electrónica no se aprueba, el sistema muestra el mensaje: "Transacción rechazada. Por favor, verifique los datos o seleccione otro medio de pago" y el turno permanece en estado "Pendiente de pago".

- Datos faltantes del medio de pago: Si no se ingresa el código de autorización o número de comprobante exigido para medios electrónicos, el sistema indica: "Atención: Debe completar los datos de autorización del pago para finalizar el cobro".
