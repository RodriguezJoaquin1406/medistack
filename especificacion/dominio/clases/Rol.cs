// ============================================================================
// MediStack — Capa de Dominio
// Requerimientos origen: Req 01 (Registrar Usuarios)
// Descripción: Entidad descriptiva del Rol de usuario en el modelo de dominio.
// ============================================================================

using System;
using System.Collections.Generic;

namespace MediStack.Domain.Entities
{
    /// <summary>
    /// Representa un rol con permisos y alcance operativo en el sistema.
    /// </summary>
    public class Rol
    {
        public int RolId { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Propiedad de navegación
        public virtual ICollection<Usuario> Usuarios { get; set; }

        public Rol()
        {
            Usuarios = new HashSet<Usuario>();
        }
    }
}
