### Registrar e iniciar sesión de usuarios



1.1 Introducción

Permite que los distintos tipos de usuarios de la plataforma (pacientes, profesionales y personal administrativo) se registren y accedan al sistema mediante credenciales propias, con niveles de permiso diferenciados según su rol.

1.2 Inputs

- Datos personales del usuario (nombre, apellido, DNI, email, teléfono)

- Rol asignado (paciente / profesional / administrativo)

- Usuario y contraseña

1.3 Processing

- Validar que los datos obligatorios estén completos y que el email/DNI no estén duplicados

- Encriptar la contraseña antes de almacenarla

- Verificar las credenciales contra la base de datos al iniciar sesión

- Determinar el rol del usuario y habilitar los módulos correspondientes

1.4 Outputs

- Confirmación de registro exitoso

- Sesión iniciada con acceso al panel correspondiente al rol

- Token/sesión de usuario autenticado

1.5 Error Handling

- Si los datos obligatorios están incompletos, se informa el campo faltante

- Si el DNI o el email ya existen, se rechaza el registro

- Si las credenciales son incorrectas, se informa el error sin precisar si falló el usuario o la contraseña, por motivos de seguridad

- Se bloquea el acceso luego de una cantidad determinada de intentos fallidos
