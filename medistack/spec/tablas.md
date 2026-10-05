# Diseno de la base de datos MediStackDB

El script ejecutable y fuente del esquema es [`database/01_MediStackDB.sql`](../database/01_MediStackDB.sql). Se puede ejecutar en una ventana de consulta de SSMS conectada a una instancia de SQL Server. La instalacion inicial requiere que la base no tenga ya las tablas de MediStack; el script no borra bases ni datos existentes.

## Tablas y finalidad

| Tabla | Finalidad |
| --- | --- |
| `Roles` | Roles Paciente, Profesional y Administrativo. |
| `Usuarios` | Credenciales, datos personales, contacto, estado y datos de acceso. `PasswordHash` almacena un hash PBKDF2, no la contrasena original. |
| `Pacientes` | Datos propios del paciente y su obra social. Su clave tambien identifica al usuario asociado. |
| `Profesionales` | Matricula y estado del profesional. Su clave tambien identifica al usuario asociado. |
| `Especialidades` | Catalogo de especialidades y duracion estandar de consulta. |
| `ProfesionalesEspecialidades` | Relacion N a N entre profesionales y especialidades; incluye valor de consulta y esquema de honorarios. |
| `HorariosAtencion` | Horarios semanales asociados a una combinacion profesional/especialidad. `DiaSemana` usa 1=lunes hasta 7=domingo. |
| `ObrasSociales` | Obras sociales y CUIT/codigo. |
| `CoberturasEspecialidades` | Porcentaje cubierto por cada obra social para cada especialidad. |
| `Convenios` | Convenios activos o historicos entre obra social y combinacion profesional/especialidad, con vigencia. |
| `Turnos` | Reserva, estado, motivo y datos de la sena. |
| `RegistrosClinicos` | Motivo, diagnostico y observaciones asociados a un turno atendido. |
| `Cobros` | Movimientos de sena o consulta, desglose de cobertura/copago, medio de pago, importe recibido y vuelto. |
| `CierresCaja` | Resumen diario por medio de pago, cantidad de cobros y diferencia de efectivo. |
| `LiquidacionesHonorarios` | Total liquidado por profesional y periodo. |
| `InteraccionesIA` | Registro de canal, consulta, funcion ejecutada, respuesta y tokens. |

## Relaciones principales

- `Usuarios.RolId` -> `Roles.RolId`.
- `Pacientes.PacienteId` y `Profesionales.ProfesionalId` son claves compartidas que referencian `Usuarios.UsuarioId`.
- `Pacientes.ObraSocialId` -> `ObrasSociales.ObraSocialId`.
- `ProfesionalesEspecialidades` enlaza profesionales con especialidades y es referenciada por horarios, convenios y turnos.
- `CoberturasEspecialidades` enlaza obras sociales con especialidades; `Convenios` referencia tanto la cobertura como la especialidad del profesional.
- `Turnos` referencia paciente y profesional/especialidad. `RegistrosClinicos` y `Cobros` dependen del turno.
- `CierresCaja` referencia al usuario administrativo; `LiquidacionesHonorarios` referencia al profesional; `InteraccionesIA` referencia al usuario.

## Reglas implementadas en SQL

- No se permiten dos turnos en estado `Solicitado` o `Confirmado` del mismo paciente para la misma especialidad.
- No se permiten dos turnos vigentes para el mismo profesional en la misma fecha y hora.
- Un turno solo puede asociar a un profesional con la especialidad que tiene asignada.
- Porcentajes de cobertura y de honorarios se limitan al rango 0-100; los importes, duraciones y rangos de fechas tienen restricciones.
- Se admite como maximo un cobro de tipo `Sena` y uno de tipo `Consulta` por turno.
- Se valida el desglose de importes del cobro y la suma del cierre de caja.
- Los estados de turno, sena, canal de IA y medios de pago se limitan a valores definidos.

La regla de un turno vigente se expresa mediante indices filtrados para estados `Solicitado` y `Confirmado`; al cancelar o atender un turno deja de ocupar esa unicidad. La validacion de que la hora cae dentro del horario semanal, de que no hay solapamiento de duraciones y de que un profesional solo consulta fichas de pacientes con turno ese dia corresponde a la capa de negocio, que todavia no se implementa.

## Datos de prueba

El script crea cuentas de demostracion para un administrativo, cuatro pacientes y dos profesionales, ademas de especialidades, obras sociales, coberturas, convenios, horarios, turnos futuros e historicos, señas, un cobro de consulta, un registro clinico, un cierre de caja, liquidaciones e interacciones de IA.

Todas las cuentas usan la contrasena de desarrollo `MediStack2026!`; cada cuenta tiene un salt PBKDF2 distinto. El formato almacenado es `PBKDF2-SHA256$120000$<salt-base64>$<hash-base64>`. La futura autenticacion de Web Forms debe verificar este formato con PBKDF2-SHA256 y no comparar contrasenas en texto plano. Cambiar o eliminar estas cuentas demo antes de usar datos reales.
