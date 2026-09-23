Pendientes importantes

1. Convención de nombres
Definir antes del primer módulo:
• Carpetas y archivos.
se documenta como un requerimiento en el desarrollo seguir una arquitectura por capas pero los nombres de carpetas quedan pendientes corresponden a una fase mas futura del proyecto

• Entidades y clases C#.
En español y SnakeCase siempre

• Tablas y columnas SQL.
En español y SnakeCase siempre

• Claves primarias y foráneas.
En español y SnakeCase siempre 

• Métodos y contratos BLL.
En español y SnakeCase siempre

• Idioma de identificadores.
En español y SnakeCase siempre

2. Estructura estándar de cada requerimiento
Cada carpeta debería tener una estructura fija, por ejemplo:
requerimientos/1_RegistrarUsuarios/
├── requerimiento.md
├── present.md   (ademas de tener un checklist)
├── spec.md      (Todo aqui dentro)
└── mock/

3. Plantillas obligatorias
Para eso deje un ejemplo de requerimiento procesado por mi en `requerimientos\ejemploRequerimiento1` ahi esta hecho actualmente esta desactualizado ya lo actualizaré.
4. Regla de conflictos
Ya definiste que la fuente válida es  requerimientos/ , pero falta indicar qué sucede si dos archivos dentro de esa carpeta se contradicen.
*el conflicto queda registrado como incógnita y el módulo no puede pasar a  Aprobado  hasta resolverlo.*
5. Control de cambios
Aunque decidiste no exigir fecha ni versión para una aprobación, el flujo necesita alguna forma de distinguir qué fue aprobado antes y qué cambió después.
Como mínimo debería registrarse:
Decisión: Aprobado
Aprobado por: Usuario
Alcance aprobado: Diccionario de datos de Paciente
La ausencia total de fecha o versión dificulta auditar cambios posteriores. No necesariamente tiene que ser una regla obligatoria, pero técnicamente sería recomendable conservarlas.
6. Separación física entre documentación y artefactos técnicos
Debe quedar claro dónde pueden existir:
• Clases C# conceptuales.
• Scripts SQL propuestos.
• HTML/CSS estático.
• Código que nunca debe ejecutarse.
Recomiendo ubicar los artefactos dentro de cada carpeta de requerimiento y evitar colocarlos en carpetas que parezcan de producción como  src/ ,  App_Code/ ,  Database/  o  Migrations/ .

7. Criterio preciso para GUID frente a INT
La regla general está bien, pero falta decidir qué entidades estarán expuestas. Por ejemplo:
• Usuarios: GUID.
• Pacientes: probablemente GUID por tratarse de datos sensibles.
• Turnos: posiblemente GUID si se exponen mediante URLs, APIs o asistentes.
• Catálogos internos: normalmente  INT IDENTITY .
La decisión debería quedar justificada en el diccionario de cada entidad.

8. Dependencias entre requerimientos
El roadmap debería registrar si un módulo depende de otro. Por ejemplo:
Gestionar turnos
depende de:
- Usuarios
- Profesionales
- Pacientes
- Agendas
Sin esto, se podría especificar un módulo sobre entidades todavía no definidas.
9. Registro de supuestos
Las incógnitas no son suficientes. También deben registrarse los supuestos temporales, por ejemplo:
Supuesto: un usuario puede tener varios roles.
Estado: pendiente de confirmación.
Impacto: autorización y modelo de datos.
Un supuesto no confirmado no debe convertirse automáticamente en regla de negocio.
10. Revisión de consistencia
Antes de aprobar un módulo, el flujo debería verificar formalmente:
• Toda regla tiene origen.
• Toda entidad tiene diccionario.
• Toda relación está documentada en ambos lados.
• Todo campo sensible tiene protección y auditoría definida.
• Todo flujo tiene errores y permisos.
• Toda acción crítica del asistente tiene confirmación humana.
• No existen reglas inventadas por el agente.
• No hay contradicciones entre Markdown, SQL, C# y mock.
11. Gestión de módulos obsoletos
Si un requerimiento cambia, no debería sobrescribirse silenciosamente. El módulo debe pasar a  Obsoleto  o volver a  En revisión , dejando constancia del impacto sobre:
• Entidades.
• Tablas.
• Relaciones.
• Mocks.
• Criterios de aceptación.
• Otros requerimientos dependientes.
12. Seguridad específica del asistente de IA
Además de permisos y confirmación humana, cada herramienta debería especificar:
• Si es de lectura o escritura.
• Roles autorizados.
• Datos que puede recibir.
• Datos que puede devolver.
• Si requiere confirmación.
• Qué auditoría genera.
• Qué ocurre ante ambigüedad o falta de autorización.
• Qué ocurre si el modelo solicita una acción peligrosa o incompleta.