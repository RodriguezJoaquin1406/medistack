### Gestionar obras sociales



5.1 Introducción

Permite administrar el catálogo de obras sociales del sistema, definir el porcentaje de cobertura que cada obra social aplica según la especialidad, y gestionar los convenios entre profesionales y obras sociales. Cada convenio se establece a nivel de profesional, especialidad y obra social, ya que un mismo profesional puede tener más de una especialidad y no necesariamente todas están cubiertas por las mismas obras sociales. Esta información es utilizada posteriormente por el módulo de cobros, para calcular el copago del paciente al momento de abonar un turno, y por el módulo de liquidación de honorarios profesionales, para determinar el valor a liquidar según si el turno fue atendido por convenio o de forma particular.

5.2 Inputs

- Datos de la obra social (nombre, código/CUIT)

- Porcentaje de cobertura de la obra social, por especialidad

- Profesional, especialidad y obra social para dar de alta un convenio

- Acción a realizar (alta / baja / modificación / consulta), tanto para obras sociales como para convenios

- Consulta de vigencia de convenio para una combinación de profesional, especialidad y obra social (utilizada por el módulo de gestión de turnos al momento de la reserva)

5.3 Processing

- Validar que los datos obligatorios de la obra social estén completos y que el nombre no esté duplicado en un alta

- Validar que el porcentaje de cobertura ingresado sea un valor numérico entre 0% y 100%, para cada especialidad

- Al dar de alta un convenio, validar que el profesional tenga habilitada esa especialidad y que la obra social exista

- Validar que no exista ya un convenio activo para esa combinación de profesional, especialidad y obra social

- Actualizar el registro correspondiente (obra social, cobertura o convenio) según la acción solicitada

5.4 Outputs

- Confirmación de la operación realizada

- Listado de obras sociales con sus porcentajes de cobertura por especialidad

- Listado de convenios activos, discriminados por profesional y especialidad

- Resultado de la consulta de convenio vigente (cubierto / no cubierto), utilizado por el módulo de gestión de turnos para determinar si corresponde solicitar una seña

5.5 Error Handling

- Obra social duplicada: Si el nombre de la obra social ya existe, se rechaza el alta. "Ya existe una obra social registrada con ese nombre."

- Porcentaje inválido: Si el porcentaje de cobertura ingresado no es un valor entre 0% y 100%, se rechaza la carga. "El porcentaje de cobertura debe ser un valor entre 0% y 100%."

- Convenio duplicado: Si ya existe un convenio activo para esa combinación de profesional, especialidad y obra social, se rechaza el alta. "Ya existe un convenio activo entre este profesional, esta especialidad y esta obra social."

- Especialidad no habilitada: Si el profesional seleccionado no tiene habilitada la especialidad indicada, se informa el error. "El profesional seleccionado no tiene habilitada la especialidad indicada."

- Datos incompletos: Si falta seleccionar el profesional, la especialidad o la obra social, se solicita completarlos. "Debe seleccionar un profesional, una especialidad y una obra social válidos."
