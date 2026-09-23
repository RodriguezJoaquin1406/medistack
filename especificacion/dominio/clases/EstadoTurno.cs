// ============================================================================
// MediStack — Capa de Dominio
// Requerimientos origen: Req 02 (Gestionar Turnos)
// Descripción: Entidad descriptiva del Estado de Turno en el catálogo de dominio.
// ============================================================================

using System.Collections.Generic;

namespace MediStack.Domain.Entities
{
    /// <summary>
    /// Representa un estado discreto en el ciclo de vida del turno médico.
    /// </summary>
    public class EstadoTurno
    {
        public int EstadoTurnoId { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool EsEstadoFinal { get; set; }
        public bool Activo { get; set; }

        // Propiedades de navegación
        public virtual ICollection<Turno> Turnos { get; set; }

        public EstadoTurno()
        {
            Activo = true;
            EsEstadoFinal = false;
            Turnos = new HashSet<Turno>();
        }
    }
}
