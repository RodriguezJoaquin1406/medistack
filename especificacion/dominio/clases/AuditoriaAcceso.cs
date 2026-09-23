// ============================================================================
// MediStack — Capa de Dominio
// Requerimientos origen: Req 01 (Registrar Usuarios)
// Descripción: Entidad descriptiva del evento de auditoría de acceso en el modelo de dominio.
// ============================================================================

using System;
using MediStack.Domain.Enums;

namespace MediStack.Domain.Entities
{
    /// <summary>
    /// Registra intentos de autenticación y eventos de seguridad para trazabilidad y detección de intrusiones.
    /// </summary>
    public class AuditoriaAcceso
    {
        public long AuditoriaAccesoId { get; set; }
        public Guid? UsuarioId { get; set; }
        public string IdentificadorIngresado { get; set; }
        public DateTime FechaEvento { get; set; }
        public string DireccionIP { get; set; }
        public string UserAgent { get; set; }
        public ResultadoAutenticacion Resultado { get; set; }
        public string Detalle { get; set; }

        // Propiedad de navegación
        public virtual Usuario Usuario { get; set; }

        public AuditoriaAcceso()
        {
            FechaEvento = DateTime.UtcNow;
        }
    }
}
