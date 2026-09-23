// ============================================================================
// MediStack — Capa de Dominio
// Requerimientos origen: Req 02 (Gestionar Turnos)
// Descripción: Entidad descriptiva del Turno clínico en el modelo de dominio.
// ============================================================================

using System;
using System.Collections.Generic;

namespace MediStack.Domain.Entities
{
    /// <summary>
    /// Representa un turno médico solicitado, reservado, confirmado, reprogramado o cancelado.
    /// Utiliza Guid como identificador seguro para evitar riesgos de enumeración (anti-IDOR).
    /// </summary>
    public class Turno
    {
        public Guid TurnoId { get; set; }
        public string CodigoTurno { get; set; }
        public Guid PacienteUsuarioId { get; set; }
        public Guid ProfesionalUsuarioId { get; set; }
        public int EspecialidadId { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public int EstadoTurnoId { get; set; }
        public string MotivoConsulta { get; set; }
        public bool EsParticular { get; set; }
        public int? ObraSocialId { get; set; }
        public decimal MontoConsulta { get; set; }
        public bool RequiereSena { get; set; }
        public decimal MontoSena { get; set; }
        public bool SenaPagada { get; set; }
        public DateTime? ExpiracionReservaTemporal { get; set; }
        public Guid? TurnoOrigenReprogramacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        // Propiedades de navegación
        public virtual Usuario PacienteUsuario { get; set; }
        public virtual Usuario ProfesionalUsuario { get; set; }
        public virtual Especialidad Especialidad { get; set; }
        public virtual EstadoTurno EstadoTurno { get; set; }
        public virtual Turno TurnoOrigenReprogramacion { get; set; }
        public virtual ICollection<TransaccionTurno> Transacciones { get; set; }

        public Turno()
        {
            TurnoId = Guid.NewGuid();
            EsParticular = false;
            RequiereSena = false;
            MontoSena = 0m;
            SenaPagada = false;
            FechaCreacion = DateTime.UtcNow;
            Transacciones = new HashSet<TransaccionTurno>();
        }
    }
}
