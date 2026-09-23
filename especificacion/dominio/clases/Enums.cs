// ============================================================================
// MediStack — Capa de Dominio
// Requerimientos origen: Req 01 (Registrar Usuarios), Req 02 (Gestionar Turnos)
// Descripción: Enumeradores del sistema (roles, autenticación, ciclo de turnos y transacciones).
// ============================================================================

namespace MediStack.Domain.Enums
{
    /// <summary>
    /// Roles funcionales de acceso al sistema MediStack.
    /// </summary>
    public enum RolUsuario
    {
        Paciente = 1,
        Profesional = 2,
        Administrativo = 3,
        Administrador = 4
    }

    /// <summary>
    /// Resultados posibles de un intento de autenticación.
    /// </summary>
    public enum ResultadoAutenticacion
    {
        Exitoso = 1,
        CredencialesInvalidas = 2,
        UsuarioBloqueado = 3,
        UsuarioInactivo = 4,
        DatosIncompletos = 5
    }

    /// <summary>
    /// Estados del ciclo de vida de un turno clínico.
    /// </summary>
    public enum EstadoTurnoEnum
    {
        PendientePago = 1,
        Confirmado = 2,
        Reprogramado = 3,
        CanceladoPaciente = 4,
        CanceladoProfesional = 5,
        Atendido = 6,
        Ausente = 7
    }

    /// <summary>
    /// Tipo de transacción económica vinculada al turno.
    /// </summary>
    public enum TipoTransaccionTurno
    {
        PagoSena = 1,
        ReintegroSena = 2
    }

    /// <summary>
    /// Estado del procesamiento en pasarela de pagos.
    /// </summary>
    public enum EstadoTransaccionTurno
    {
        Aprobada = 1,
        Rechazada = 2,
        Reembolsada = 3
    }
}
