// ============================================================================
// MediStack — Capa de Dominio
// Requerimientos origen: Req 01 (Registrar Usuarios)
// Descripción: Entidad descriptiva del Usuario en el modelo de dominio.
// ============================================================================

using System;
using System.Collections.Generic;

namespace MediStack.Domain.Entities
{
    /// <summary>
    /// Representa un usuario registrado en MediStack con credenciales y perfil base.
    /// Utiliza Guid como identificador seguro para evitar riesgos de enumeración (IDOR).
    /// </summary>
    public class Usuario
    {
        public Guid UsuarioId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NumeroDocumento { get; set; }
        public string Telefono { get; set; }
        public int RolId { get; set; }
        public bool Activo { get; set; }
        public int IntentosFallidos { get; set; }
        public DateTime? BloqueadoHasta { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? UltimoAcceso { get; set; }

        // Propiedades de navegación
        public virtual Rol Rol { get; set; }
        public virtual ICollection<AuditoriaAcceso> AuditoriasAcceso { get; set; }

        public Usuario()
        {
            UsuarioId = Guid.NewGuid();
            Activo = true;
            IntentosFallidos = 0;
            FechaRegistro = DateTime.UtcNow;
            AuditoriasAcceso = new HashSet<AuditoriaAcceso>();
        }
    }
}
