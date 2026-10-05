using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MediStack.Datos;
using MediStack.Dominio;

namespace MediStack.Negocio
{
    public sealed class AgendaNegocio
    {
        private readonly GestionClinicaDatos _datos = new GestionClinicaDatos();

        public DataTable ObtenerProfesionales()
        {
            return _datos.ObtenerProfesionalesAgenda();
        }

        public DataTable ObtenerEspecialidades(Guid profesionalId)
        {
            if (profesionalId == Guid.Empty)
            {
                return new DataTable();
            }

            return _datos.ObtenerEspecialidadesProfesionalAgenda(profesionalId);
        }

        public DataTable ObtenerHorarios(Guid? profesionalId)
        {
            return _datos.ObtenerHorariosAtencion(profesionalId);
        }

        public DataTable ObtenerTurnos(Guid profesionalId, DateTime fecha)
        {
            ValidarSeleccionAgenda(profesionalId, fecha);
            return _datos.ObtenerTurnosAgenda(profesionalId, fecha);
        }

        public IList<FranjaAgenda> ObtenerDisponibilidad(Guid profesionalId, DateTime fecha)
        {
            ValidarSeleccionAgenda(profesionalId, fecha);
            int diaSemana = ((int)fecha.DayOfWeek + 6) % 7 + 1;
            DataTable horarios = _datos.ObtenerHorariosAtencionDia(profesionalId, diaSemana);
            IList<TurnoAgenda> turnos = CargarTurnos(_datos.ObtenerTurnosAgenda(profesionalId, fecha));
            List<FranjaAgenda> disponibilidad = new List<FranjaAgenda>();

            foreach (DataRow fila in horarios.Rows)
            {
                TimeSpan inicio = (TimeSpan)fila["HoraInicio"];
                TimeSpan fin = (TimeSpan)fila["HoraFin"];
                int duracion = Convert.ToInt32(fila["DuracionEstandarMinutos"]);
                DateTime hora = fecha.Date.Add(inicio);
                DateTime finHorario = fecha.Date.Add(fin);

                while (hora.AddMinutes(duracion) <= finHorario)
                {
                    DateTime finFranja = hora.AddMinutes(duracion);
                    List<TurnoAgenda> ocupantes = turnos
                        .Where(turno => !string.Equals(turno.Estado, "Cancelado", StringComparison.OrdinalIgnoreCase)
                            && turno.FechaHora < finFranja
                            && turno.FechaHora.AddMinutes(turno.DuracionMinutos) > hora)
                        .ToList();

                    FranjaAgenda franja = new FranjaAgenda
                    {
                        FechaHora = hora,
                        Fin = finFranja,
                        Especialidad = Convert.ToString(fila["Especialidad"]),
                        Disponible = ocupantes.Count == 0,
                        Estado = ocupantes.Count == 0 ? "Disponible" :
                            ocupantes.Count == 1 ? ocupantes[0].Estado : "Conflicto de turnos"
                    };

                    if (ocupantes.Count == 1)
                    {
                        franja.TurnoId = ocupantes[0].TurnoId;
                        franja.PacienteId = ocupantes[0].PacienteId;
                        franja.Paciente = ocupantes[0].Paciente;
                        franja.Motivo = ocupantes[0].Motivo;
                    }
                    else if (ocupantes.Count > 1)
                    {
                        franja.Motivo = "Hay mas de un turno que ocupa esta franja. Revisa los turnos del dia.";
                    }

                    disponibilidad.Add(franja);
                    hora = finFranja;
                }
            }

            return disponibilidad;
        }

        public void GuardarHorario(HorarioAtencion horario)
        {
            if (horario == null || horario.ProfesionalId == Guid.Empty
                || horario.EspecialidadId <= 0 || horario.HorarioAtencionId < 0)
            {
                throw new InvalidOperationException("Selecciona un profesional y una especialidad validos.");
            }

            if (horario.DiaSemana < 1 || horario.DiaSemana > 7)
            {
                throw new InvalidOperationException("Selecciona un dia de la semana valido.");
            }

            if (horario.HoraInicio < TimeSpan.Zero || horario.HoraFin > TimeSpan.FromDays(1)
                || horario.HoraInicio >= horario.HoraFin)
            {
                throw new InvalidOperationException("La hora de fin debe ser posterior a la hora de inicio.");
            }

            try
            {
                _datos.GuardarHorarioAtencion(horario);
            }
            catch (SqlException ex)
            {
                LanzarErrorHorario(ex);
            }
        }

        public void CambiarEstadoHorario(int horarioAtencionId, bool activo)
        {
            if (horarioAtencionId <= 0)
            {
                throw new InvalidOperationException("Selecciona un horario valido.");
            }

            try
            {
                _datos.CambiarEstadoHorarioAtencion(horarioAtencionId, activo);
            }
            catch (SqlException ex)
            {
                LanzarErrorHorario(ex);
            }
        }

        private static IList<TurnoAgenda> CargarTurnos(DataTable datos)
        {
            List<TurnoAgenda> items = new List<TurnoAgenda>(datos.Rows.Count);
            foreach (DataRow fila in datos.Rows)
            {
                items.Add(new TurnoAgenda
                {
                    TurnoId = Convert.ToInt32(fila["TurnoId"]),
                    PacienteId = (Guid)fila["PacienteId"],
                    Paciente = Convert.ToString(fila["Paciente"]),
                    FechaHora = Convert.ToDateTime(fila["FechaHora"]),
                    Estado = Convert.ToString(fila["Estado"]),
                    Motivo = fila.IsNull("Motivo") ? string.Empty : Convert.ToString(fila["Motivo"]),
                    Especialidad = Convert.ToString(fila["Especialidad"]),
                    DuracionMinutos = Convert.ToInt16(fila["DuracionEstandarMinutos"]),
                    EstadoSena = Convert.ToString(fila["EstadoSena"]),
                    MontoSena = Convert.ToDecimal(fila["MontoSena"])
                });
            }

            return items;
        }

        private static void ValidarSeleccionAgenda(Guid profesionalId, DateTime fecha)
        {
            if (profesionalId == Guid.Empty || fecha.Date < new DateTime(1900, 1, 1)
                || fecha.Date > new DateTime(9998, 12, 31))
            {
                throw new InvalidOperationException("Selecciona un profesional y una fecha validos.");
            }
        }

        private static void LanzarErrorHorario(SqlException ex)
        {
            if (ex.Number == 51001 || ex.Number == 51002 || ex.Number == 51003 || ex.Number == 51004)
            {
                throw new InvalidOperationException(ex.Message);
            }

            if (ex.Number == 2601 || ex.Number == 2627)
            {
                throw new InvalidOperationException("Ya existe un horario con ese profesional, especialidad, dia y hora de inicio.");
            }

            if (ex.Number == 547)
            {
                throw new InvalidOperationException("El horario requiere una relacion activa entre el profesional y la especialidad.");
            }

            throw new InvalidOperationException("No se pudo guardar el horario en MediStackDB (error SQL " + ex.Number + ").");
        }
    }
}
