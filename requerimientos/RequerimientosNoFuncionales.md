Requerimientos No-Funcionales

1 Desempeño

- El sistema debe responder a las operaciones habituales (búsqueda de pacientes, listado de turnos, registro de cobros) en un tiempo menor a 3 segundos.

- El asistente de Inteligencia Artificial debe procesar consultas en lenguaje natural en menos de 5 segundos.

- Debe soportar el uso simultáneo de al menos 20 puestos administrativos y profesionales sin degradación del servicio.

2 Confiabilidad

- El sistema debe asegurar la integridad transaccional.

- Ante una interrupción de conexión o falla del sistema durante una operación (por ejemplo, un cobro o una reserva con seña), el sistema debe revertir los cambios para no generar datos inconsistentes.

- Se deben realizar copias de seguridad (backups) periódicas para evitar pérdida de datos.

3 Disponibilidad

- El sistema debe estar disponible ininterrumpidamente durante todo el horario de atención operativa de la clínica.

- Ante una caída, el tiempo de restablecimiento no debe superar las 2 horas para no afectar la operatoria diaria.

4 Seguridad

- El acceso al sistema requiere autenticación obligatoria mediante credenciales de usuario.

- Las contraseñas deben almacenarse encriptadas (hasheadas), nunca en texto plano.

- El acceso a la información clínica y de facturación estará estrictamente delimitado por los roles y permisos del usuario (Administrativo, Profesional, Paciente).

5 Mantenibilidad

- El sistema debe desarrollarse bajo una arquitectura en capas que separe la interfaz de usuario, la lógica de negocio y el acceso a datos, permitiendo corregir errores o escalar funcionalidades sin afectar módulos enteros.

- El código debe respetar estándares de organización consistentes.

6 Portabilidad

- La aplicación web debe ser compatible y completamente funcional en las versiones recientes de los navegadores web de mayor uso (Google Chrome, Microsoft Edge, Mozilla Firefox), sin depender de un sistema operativo específico del lado del cliente.



### Restricciones

(Resultantes de estándares, políticas o reglamentaciones de la empresa, limitaciones de hardware etc.)

- Restricciones Tecnológicas: El sistema debe desarrollarse como aplicación web en ASP.NET Web Forms sobre .NET Framework 4.8.

- Restricciones de Accesibilidad: La interfaz debe incorporar obligatoriamente opciones de alto contraste, lectura de texto a voz y ser compatible con lectores de pantalla para garantizar la inclusión.

- Restricciones de Proyecto: El desarrollo debe ajustarse al cronograma de entregas de la cursada. El equipo está limitado a los integrantes de "Stack Soluciones Digitales", sin posibilidad de sumar recursos externos ni presupuesto para licencias pagas (se utilizarán herramientas y pasarelas en versión gratuita o de prueba).

