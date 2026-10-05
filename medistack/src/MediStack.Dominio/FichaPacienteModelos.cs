using System;
using System.Collections.Generic;

namespace MediStack.Dominio
{
    public sealed class FichaPaciente
    {
        public Guid PacienteId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string ObraSocial { get; set; }
        public string NumeroAfiliado { get; set; }
        public string ContactoEmergenciaNombre { get; set; }
        public string ContactoEmergenciaTelefono { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }
        public IList<HistorialPacienteItem> Historial { get; set; }
        public IList<CobroPacienteItem> Cobros { get; set; }
        public IList<CoberturaPacienteItem> Coberturas { get; set; }
    }

    public sealed class HistorialPacienteItem
    {
        public int TurnoId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Profesional { get; set; }
        public string Especialidad { get; set; }
        public string Estado { get; set; }
        public string Motivo { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string MotivoConsulta { get; set; }
        public string Diagnostico { get; set; }
        public string Observaciones { get; set; }
        public string EstadoSena { get; set; }
        public decimal MontoSena { get; set; }
    }

    public sealed class CobroPacienteItem
    {
        public int CobroId { get; set; }
        public int TurnoId { get; set; }
        public DateTime FechaHoraCobro { get; set; }
        public string TipoCobro { get; set; }
        public string MedioPago { get; set; }
        public decimal MontoCobrado { get; set; }
    }

    public sealed class CoberturaPacienteItem
    {
        public string Especialidad { get; set; }
        public decimal PorcentajeCobertura { get; set; }
    }
}
