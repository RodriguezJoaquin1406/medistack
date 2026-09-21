### Gestionar acciones y consultas al asistente de inteligencia artificial

11.1 Introducción

Este requerimiento permite a pacientes, profesionales y personal administrativo interactuar con el sistema MediStack mediante un asistente de inteligencia artificial que interpreta consultas en lenguaje natural, tanto por texto como por voz. El asistente facilita la navegación del sitio, la consulta de información y la ejecución de acciones disponibles según el rol del usuario.

11.2 Inputs

- Consulta escrita o mediante voz.

11.3 Processing

- Recibir la consulta ingresada por texto o voz.

- Convertir la entrada de voz a texto cuando corresponda.

- Analizar la intención del usuario mediante inteligencia artificial.

- Identificar la funcionalidad solicitada.

- Validar permisos según el rol del usuario autenticado.

- Consultar la información necesaria en la base de datos.

- Ejecutar la acción correspondiente cuando la solicitud sea válida.

- Solicitar confirmación antes de realizar acciones críticas, tales como:

- Reservar turnos, Reprogramar turnos, Cancelar turnos, Registrar pagos.

- Generar una respuesta comprensible en lenguaje natural.

- Registrar la interacción para fines de auditoría y mejora del servicio.

11.4 Outputs

- Respuestas conversacionales en lenguaje natural.

- Confirmación de acciones realizadas.

- Información sobre disponibilidad de turnos.

- Listado de profesionales disponibles.

- Información sobre convenios con obras sociales.

- Confirmación de reservas de turnos.

- Confirmación de reprogramaciones.

- Confirmación de cancelaciones.

- Mensajes de orientación para utilizar funciones del sistema.

- Respuestas por texto y, cuando corresponda, por voz.

11. 5 Error Handling

- Consulta del usuario no comprendida, se le pide al usuario que reformule su consulta.

- Usuario sin permisos para determinada acción, se le informa que no es capaz de realizar tal acción.

