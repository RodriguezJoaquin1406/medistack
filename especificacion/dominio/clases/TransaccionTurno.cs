// ============================================================================
// MediStack — Capa de Dominio
// Requerimientos origen: Req 02 (Gestionar Turnos)
// Descripción: Entidad descriptiva de transacciones económicas de seña y reintegros.
// ============================================================================

using System;
using MediStack.Domain.Enums;

namespace MediStack.Domain.Entities
{
    /// <summary>
    /// Representa un pago de seña o reintegro derivado de la reserva o cancelación de un turno.
    /// </summary>
    public class TransaccionTurno
    {
        public Guid TransaccionTurnoId { get; set; }
        public Guid TurnoId { get; set; }
        public TipoTransaccionTurno TipoTransaccion { get; set; }
        public decimal Monto { get; set; }
        public string Moneda { get; set; }
        public string MetodoPago { get; set; }
        public string ReferenciaPasarela { get; set; }
        public EstadoTransaccionTurno EstadoTransaccion { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public string MotivoReintegro { get; set; }
        public string PayloadRespuesta { get; set; }

        // Propiedad de navegación
        public virtual Turno Turno { get; set; }

        public TransaccionTurno()
        {
            TransaccionTurnoId = Guid.NewGuid();
            Moneda = "ARS";
            FechaTransaccion = DateTime.UtcNow;
            EstadoTransaccion = EstadoTransaccionTurno.Aprobada;
        }
    }
}
