using System;
using System.Collections.Generic;
using System.Data;
using MediStack.Datos;
using MediStack.Dominio;

namespace MediStack.Negocio
{
    public sealed class FichaPacienteNegocio
    {
        private readonly GestionClinicaDatos _datos = new GestionClinicaDatos();

        public FichaPaciente ObtenerFicha(Guid pacienteId)
        {
            if (pacienteId == Guid.Empty)
            {
                throw new InvalidOperationException("Selecciona un paciente valido.");
            }

            DataTable datosPaciente = _datos.ObtenerFichaPaciente(pacienteId);
            if (datosPaciente.Rows.Count == 0)
            {
                throw new InvalidOperationException("No se encontro la ficha del paciente.");
            }

            DataRow fila = datosPaciente.Rows[0];
            FichaPaciente ficha = new FichaPaciente
            {
                PacienteId = (Guid)fila["PacienteId"],
                Nombre = Convert.ToString(fila["Nombre"]),
                Apellido = Convert.ToString(fila["Apellido"]),
                NumeroDocumento = Convert.ToString(fila["NumeroDocumento"]),
                FechaNacimiento = Convert.ToDateTime(fila["FechaNacimiento"]),
                Email = Convert.ToString(fila["Email"]),
                Telefono = TextoNullable(fila["Telefono"]),
                ObraSocial = TextoNullable(fila["ObraSocial"]),
                NumeroAfiliado = TextoNullable(fila["NumeroAfiliado"]),
                ContactoEmergenciaNombre = TextoNullable(fila["ContactoEmergenciaNombre"]),
                ContactoEmergenciaTelefono = TextoNullable(fila["ContactoEmergenciaTelefono"]),
                Activo = Convert.ToBoolean(fila["Activo"]),
                FechaAlta = Convert.ToDateTime(fila["FechaAlta"]),
                Historial = CargarHistorial(_datos.ObtenerHistorialPaciente(pacienteId)),
                Cobros = CargarCobros(_datos.ObtenerCobrosPaciente(pacienteId)),
                Coberturas = CargarCoberturas(_datos.ObtenerCoberturasPaciente(pacienteId))
            };

            return ficha;
        }

        public bool PuedeProfesionalVer(Guid pacienteId, Guid profesionalId)
        {
            if (pacienteId == Guid.Empty || profesionalId == Guid.Empty)
            {
                return false;
            }

            return _datos.PuedeProfesionalVerFichaPaciente(pacienteId, profesionalId);
        }

        private static IList<HistorialPacienteItem> CargarHistorial(DataTable datos)
        {
            List<HistorialPacienteItem> items = new List<HistorialPacienteItem>(datos.Rows.Count);
            foreach (DataRow fila in datos.Rows)
            {
                items.Add(new HistorialPacienteItem
                {
                    TurnoId = Convert.ToInt32(fila["TurnoId"]),
                    FechaHora = Convert.ToDateTime(fila["FechaHora"]),
                    Profesional = Convert.ToString(fila["Profesional"]),
                    Especialidad = Convert.ToString(fila["Especialidad"]),
                    Estado = Convert.ToString(fila["Estado"]),
                    Motivo = TextoNullable(fila["Motivo"]),
                    FechaRegistro = fila.IsNull("FechaRegistro")
                        ? (DateTime?)null : Convert.ToDateTime(fila["FechaRegistro"]),
                    MotivoConsulta = TextoNullable(fila["MotivoConsulta"]),
                    Diagnostico = TextoNullable(fila["Diagnostico"]),
                    Observaciones = TextoNullable(fila["Observaciones"]),
                    EstadoSena = Convert.ToString(fila["EstadoSena"]),
                    MontoSena = Convert.ToDecimal(fila["MontoSena"])
                });
            }

            return items;
        }

        private static IList<CobroPacienteItem> CargarCobros(DataTable datos)
        {
            List<CobroPacienteItem> items = new List<CobroPacienteItem>(datos.Rows.Count);
            foreach (DataRow fila in datos.Rows)
            {
                items.Add(new CobroPacienteItem
                {
                    CobroId = Convert.ToInt32(fila["CobroId"]),
                    TurnoId = Convert.ToInt32(fila["TurnoId"]),
                    FechaHoraCobro = Convert.ToDateTime(fila["FechaHoraCobro"]),
                    TipoCobro = Convert.ToString(fila["TipoCobro"]),
                    MedioPago = Convert.ToString(fila["MedioPago"]),
                    MontoCobrado = Convert.ToDecimal(fila["MontoCobrado"])
                });
            }

            return items;
        }

        private static IList<CoberturaPacienteItem> CargarCoberturas(DataTable datos)
        {
            List<CoberturaPacienteItem> items = new List<CoberturaPacienteItem>(datos.Rows.Count);
            foreach (DataRow fila in datos.Rows)
            {
                items.Add(new CoberturaPacienteItem
                {
                    Especialidad = Convert.ToString(fila["Especialidad"]),
                    PorcentajeCobertura = Convert.ToDecimal(fila["PorcentajeCobertura"])
                });
            }

            return items;
        }

        private static string TextoNullable(object valor)
        {
            return valor == DBNull.Value ? string.Empty : Convert.ToString(valor);
        }
    }
}
