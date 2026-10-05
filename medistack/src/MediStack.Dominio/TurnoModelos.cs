using System;

namespace MediStack.Dominio
{
    public sealed class TurnoDisponible
    {
        public Guid ProfesionalId { get; set; }
        public int EspecialidadId { get; set; }
        public string Especialidad { get; set; }
        public DateTime FechaHora { get; set; }
        public DateTime Fin { get; set; }
        public short DuracionMinutos { get; set; }
        public bool Disponible { get; set; }
        public int? TurnoId { get; set; }
        public string Paciente { get; set; }
        public string Estado { get; set; }
    }

    public sealed class SolicitudTurno
    {
        public Guid PacienteId { get; set; }
        public Guid ProfesionalId { get; set; }
        public int EspecialidadId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; }
    }

    public sealed class TurnoDetalle
    {
        public int TurnoId { get; set; }
        public Guid PacienteId { get; set; }
        public Guid ProfesionalId { get; set; }
        public int EspecialidadId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; }
        public string Motivo { get; set; }
    }
}
