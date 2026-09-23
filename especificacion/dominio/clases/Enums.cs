// ============================================================================
// MediStack — Capa de Dominio
// Requerimientos origen: Req 01 (Registrar Usuarios)
// Descripción: Enumeradores de roles de usuario y estados de autenticación/auditoría.
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
}
