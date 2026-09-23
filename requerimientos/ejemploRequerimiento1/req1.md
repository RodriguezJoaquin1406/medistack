
# 1.	Registrar e iniciar sesión de usuarios

## 1.1 Introducción
Permite que los distintos tipos de usuarios de la plataforma (*pacientes*, *profesionales* y *personal administrativo*) se registren y accedan al sistema mediante *credenciales* propias, con niveles de permiso diferenciados según su *rol*.

> Entidades:
    - Usuario 
> Roles:
    - Pacientes, Profesionales y personal administrativo

 - Usuarios + roles + funcion de credenciales (registro, inicio de sesion, Cambio de contraseña, Vista perfil usuario)

## 1.2 Inputs
•	Datos personales del usuario *(nombre, apellido, DNI, email, teléfono)* Campos tabla usuario
•	Rol asignado *(paciente / profesional / administrativo)* Roles de usuario
•	Usuario y contraseña
## 1.3 Processing
•	Validar que los datos obligatorios estén completos y que el email/DNI no estén duplicados
•	Hashear la contraseña antes de almacenarla
•	Verificar las credenciales contra la base de datos al iniciar sesión
•	Determinar el rol del usuario y habilitar los módulos correspondientes
## 1.4 Outputs
•	Confirmación de registro exitoso
•	Sesión iniciada con acceso al panel correspondiente al rol
•	Token/sesión de usuario autenticado
## 1.5 Error Handling
•	Si los datos obligatorios están incompletos, se informa el campo faltante
•	Si el DNI o el email ya existen, se rechaza el registro
•	Si las credenciales son incorrectas, se informa el error sin precisar si falló el usuario o la contraseña, por motivos de seguridad
•	Se bloquea el acceso luego de una cantidad determinada de intentos fallidos

### Que debe cumplir

- Registro nuevo usuario
- Inicio de sesion
- Recuperacion de contraseñas

### Entidades

> Usuario

### Diccionario de datos (SIN TIPOS)
> Usuario
    - Nombre           
    - Apellido         
    - Contraseña       
    - Correo           
    - Dni              
    - Rol              
    - Nro telefono     
    - ? Obra social (esto deberia ser una tabla intermedia creo)

### Entidad

#### Class C# 

> Usuario
    - INT / GUID IdRol
    - INT / GUID IdUsuario        
    - String Nombre
    - String Apellido         
    - String ContraseñaHash   
    - String Telefono         
    - Bool Activo
    - DateTime FechaCreacion    
    - String Correo
    - String Dni

#### Tabla SQL

> Usuario
    - IdRol            UNIQUEIDENTIFIER      NOT NULL  
    - IdUsuario        UNIQUEIDENTIFIER      NOT NULL  
    - Nombre           NVARCHAR              NOT NULL  
    - Apellido         NVARCHAR              NOT NULL  
    - ContraseñaHash   NVARCHAR              NOT NULL  
    - Telefono         NVARCHAR              NOT NULL  UNIQUE
    - Activo           BIT DEFAULT 1         NOT NULL   
    - FechaCreacion    DATETIME              NOT NULL  
    - Correo           NVARCHAR              NOT NULL  UNIQUE
    - Dni              NVARCHAR              NOT NULL  UNIQUE

    + Tabla intermedia de obras sociales

### Funciones

- Bool/Usuario  Registrar(Usuario)
- Usuario       IniciarSesion(Correo, Contraseña)
* Bool          VerificarCorreo(Correo)

### Reglas de Negocio y logica

- Validar unicidad de DNI y Email.

- Hashear la contraseña (BCrypt).



