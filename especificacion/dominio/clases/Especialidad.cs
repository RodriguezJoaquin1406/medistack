// ============================================================================
// MediStack — Capa de Dominio
// Requerimientos origen: Req 02 (Gestionar Turnos)
// Descripción: Entidad descriptiva de Especialidad médica en el modelo de dominio.
// ============================================================================

using System;
using System.Collections.Generic;

namespace MediStack.Domain.Entities
{
    /// <summary>
    /// Representa una rama o especialidad de la medicina clínica en MediStack.
    /// </summary>
    public class Especialidad
    {
        public int EspecialidadId { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int DuracionEstandarMinutos { get; set; }
        public bool Activa { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Propiedades de navegación
        public virtual ICollection<Turno> Turnos { get; set; }

        public Especialidad()
        {
            DuracionEstandarMinutos = 30;
            Activa = true;
            FechaCreacion = DateTime.UtcNow;
            Turnos = new HashSet<Turno>();
        }
    }
}
