using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using MediStack.Dominio;
using MediStack.Negocio;

namespace MediStack.Web
{
    public partial class AgendaPagina : PaginaProtegida
    {
        private readonly AgendaNegocio _negocio = new AgendaNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            bool esAdministrativo = TieneRol("ADMINISTRATIVO");
            bool esProfesional = TieneRol("PROFESIONAL");
            if (!esAdministrativo && !esProfesional)
            {
                Response.Redirect(ResolveUrl("~/Dashboard.aspx"), true);
            }

            GestionHorariosPanel.Visible = esAdministrativo;
            ProfesionalLabel.Visible = esAdministrativo;
            Profesional.Enabled = esAdministrativo;

            if (!IsPostBack)
            {
                EjecutarConManejoDeErrores("No se pudo cargar la agenda", () =>
                {
                    CargarProfesionales();
                    if (esProfesional)
                    {
                        Guid profesionalId = Guid.Parse(Convert.ToString(Session["UsuarioId"]));
                        SeleccionarProfesional(profesionalId);
                    }

                    Fecha.Text = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    CargarEspecialidadesHorario();
                    LimpiarFormularioHorario();
                    CargarDatosAgenda();
                });
            }
        }

        protected void Profesional_SelectedIndexChanged(object sender, EventArgs e)
        {
            EjecutarConManejoDeErrores("No se pudo cambiar el profesional", () =>
            {
                CargarEspecialidadesHorario();
                LimpiarFormularioHorario();
                CargarDatosAgenda();
            });
        }

        protected void Actualizar_Click(object sender, EventArgs e)
        {
            EjecutarConManejoDeErrores("No se pudo actualizar la agenda", CargarDatosAgenda);
        }

        protected void NuevoHorario_Click(object sender, EventArgs e)
        {
            LimpiarMensaje();
            EjecutarConManejoDeErrores("No se pudo preparar el formulario", () =>
            {
                CargarEspecialidadesHorario();
                LimpiarFormularioHorario();
            });
        }

        protected void GuardarHorario_Click(object sender, EventArgs e)
        {
            try
            {
                Guid profesionalId;
                int especialidadId;
                int diaSemana;
                TimeSpan horaInicio;
                TimeSpan horaFin;
                bool esEdicion = ViewState["HorarioAtencionId"] != null;
                if (!int.TryParse(DiaSemana.SelectedValue, out diaSemana)
                    || !TimeSpan.TryParseExact(HoraInicio.Text, new[] { @"hh\:mm", @"hh\:mm\:ss" },
                        CultureInfo.InvariantCulture, out horaInicio)
                    || !TimeSpan.TryParseExact(HoraFin.Text, new[] { @"hh\:mm", @"hh\:mm\:ss" },
                        CultureInfo.InvariantCulture, out horaFin))
                {
                    MostrarError("Completa el día y el rango horario.");
                    return;
                }

                if (esEdicion)
                {
                    profesionalId = (Guid)ViewState["HorarioProfesionalId"];
                    especialidadId = (int)ViewState["HorarioEspecialidadId"];
                }
                else if (!Guid.TryParse(Profesional.SelectedValue, out profesionalId)
                    || !int.TryParse(EspecialidadHorario.SelectedValue, out especialidadId))
                {
                    MostrarError("Selecciona un profesional y una especialidad válidos.");
                    return;
                }
                else if (!EsPropioProfesional(profesionalId))
                {
                    MostrarError("No puedes modificar la agenda de otro profesional.");
                    return;
                }

                _negocio.GuardarHorario(new HorarioAtencion
                {
                    HorarioAtencionId = esEdicion ? (int)ViewState["HorarioAtencionId"] : 0,
                    ProfesionalId = profesionalId,
                    EspecialidadId = especialidadId,
                    DiaSemana = diaSemana,
                    HoraInicio = horaInicio,
                    HoraFin = horaFin
                });

                if (string.Equals(Profesional.SelectedValue, profesionalId.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    CargarDatosAgenda();
                }

                LimpiarFormularioHorario();
                MostrarExito("La franja semanal se guardo correctamente.");
            }
            catch (InvalidOperationException ex)
            {
                MostrarError(ex.Message);
            }
            catch (SqlException ex)
            {
                MostrarError("No se pudo guardar la franja semanal (error SQL " + ex.Number + ").");
            }
        }

        protected void HorariosGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int indice;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out indice)
                || indice < 0 || indice >= HorariosGrid.DataKeys.Count)
            {
                return;
            }

            try
            {
                DataKey datos = HorariosGrid.DataKeys[indice];
                Guid profesionalId = Guid.Parse(ObtenerTextoDataKey(indice, "ProfesionalId"));
                int horarioId = Convert.ToInt32(HorariosGrid.DataKeys[indice].Values["HorarioAtencionId"]);
                if (!EsPropioProfesional(profesionalId))
                {
                    MostrarError("No puedes modificar la agenda de otro profesional.");
                    return;
                }

                if (e.CommandName == "EditarHorario")
                {
                    EditarHorario(indice, datos, horarioId, profesionalId);
                }
                else if (e.CommandName == "CambiarEstadoHorario")
                {
                    bool activo = Convert.ToBoolean(datos.Values["Activo"]);
                    _negocio.CambiarEstadoHorario(horarioId, !activo);
                    CargarDatosAgenda();
                    MostrarExito(activo ? "La franja fue desactivada." : "La franja fue reactivada.");
                }
            }
            catch (InvalidOperationException ex)
            {
                MostrarError(ex.Message);
            }
            catch (SqlException ex)
            {
                MostrarError("No se pudo completar la operación (error SQL " + ex.Number + ").");
            }
        }

        protected string DiaSemanaTexto(object valor)
        {
            string[] dias = { string.Empty, "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };
            int numero;
            if (valor == null || valor == DBNull.Value || !int.TryParse(Convert.ToString(valor), out numero)
                || numero < 1 || numero >= dias.Length)
            {
                return string.Empty;
            }

            return dias[numero];
        }

        private void CargarProfesionales()
        {
            DataTable profesionales = _negocio.ObtenerProfesionales();
            Profesional.DataSource = profesionales;
            Profesional.DataTextField = "Nombre";
            Profesional.DataValueField = "ProfesionalId";
            Profesional.DataBind();
            Profesional.Items.Insert(0, new ListItem("Selecciona un profesional", string.Empty));

            foreach (DataRow fila in profesionales.Rows)
            {
                if (!Convert.ToBoolean(fila["Activo"]))
                {
                    ListItem item = Profesional.Items.FindByValue(Convert.ToString(fila["ProfesionalId"]));
                    if (item != null)
                    {
                        item.Text += " (inactivo)";
                    }
                }
            }

            if (TieneRol("ADMINISTRATIVO") && profesionales.Rows.Count > 0)
            {
                DataRow activo = profesionales.Select("Activo = true").Length > 0
                    ? profesionales.Select("Activo = true")[0] : profesionales.Rows[0];
                SeleccionarProfesional((Guid)activo["ProfesionalId"]);
            }
        }

        private void SeleccionarProfesional(Guid profesionalId)
        {
            ListItem item = Profesional.Items.FindByValue(profesionalId.ToString());
            if (item != null)
            {
                Profesional.ClearSelection();
                item.Selected = true;
            }
        }

        private void CargarEspecialidadesHorario()
        {
            Guid profesionalId;
            if (!Guid.TryParse(Profesional.SelectedValue, out profesionalId))
            {
                EspecialidadHorario.Items.Clear();
                EspecialidadHorario.Items.Insert(0, new ListItem("Selecciona una especialidad", string.Empty));
                return;
            }

            EspecialidadHorario.DataSource = _negocio.ObtenerEspecialidades(profesionalId);
            EspecialidadHorario.DataTextField = "Nombre";
            EspecialidadHorario.DataValueField = "EspecialidadId";
            EspecialidadHorario.DataBind();
            EspecialidadHorario.Items.Insert(0, new ListItem("Selecciona una especialidad", string.Empty));
        }

        private void CargarDatosAgenda()
        {
            Guid profesionalId;
            DateTime fecha;
            if (!Guid.TryParse(Profesional.SelectedValue, out profesionalId)
                || !DateTime.TryParseExact(Fecha.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out fecha))
            {
                DisponibilidadGrid.DataSource = null;
                DisponibilidadGrid.DataBind();
                TurnosGrid.DataSource = null;
                TurnosGrid.DataBind();
                HorariosGrid.DataSource = null;
                HorariosGrid.DataBind();
                ResumenAgenda.Text = string.Empty;
                MostrarError("Selecciona un profesional y una fecha válidos.");
                return;
            }

            if (!EsPropioProfesional(profesionalId))
            {
                MostrarError("No tienes acceso a la agenda de ese profesional.");
                return;
            }

            DisponibilidadGrid.DataSource = _negocio.ObtenerDisponibilidad(profesionalId, fecha);
            DisponibilidadGrid.DataBind();
            TurnosGrid.DataSource = _negocio.ObtenerTurnos(profesionalId, fecha);
            TurnosGrid.DataBind();
            HorariosGrid.DataSource = _negocio.ObtenerHorarios(profesionalId);
            HorariosGrid.DataBind();
            ResumenAgenda.Text = HttpUtility.HtmlEncode(
                "Franja horaria semanal para " + Profesional.SelectedItem.Text + " · " +
                fecha.ToString("dddd dd/MM/yyyy", CultureInfo.GetCultureInfo("es-AR")) + ".");
        }

        private void EditarHorario(int indice, DataKey datos, int horarioId, Guid profesionalId)
        {
            int especialidadId = Convert.ToInt32(datos.Values["EspecialidadId"]);
            CargarEspecialidadesHorario();
            if (EspecialidadHorario.Items.FindByValue(especialidadId.ToString()) == null)
            {
                EspecialidadHorario.Items.Add(new ListItem(
                    ObtenerTextoDataKey(indice, "Especialidad") + " (inactiva)",
                    especialidadId.ToString(CultureInfo.InvariantCulture)));
            }

            EspecialidadHorario.SelectedValue = especialidadId.ToString(CultureInfo.InvariantCulture);
            EspecialidadHorario.Enabled = false;
            Profesional.Enabled = false;
            ViewState["HorarioAtencionId"] = horarioId;
            ViewState["HorarioProfesionalId"] = profesionalId;
            ViewState["HorarioEspecialidadId"] = especialidadId;
            DiaSemana.SelectedValue = Convert.ToString(datos.Values["DiaSemana"]);
            HoraInicio.Text = ((TimeSpan)datos.Values["HoraInicio"]).ToString(@"hh\:mm");
            HoraFin.Text = ((TimeSpan)datos.Values["HoraFin"]).ToString(@"hh\:mm");
            TituloHorario.Text = "Editar franja semanal";
            GuardarHorario.Text = "Guardar cambios";
            LimpiarMensaje();
        }

        private void LimpiarFormularioHorario()
        {
            ViewState.Remove("HorarioAtencionId");
            ViewState.Remove("HorarioProfesionalId");
            ViewState.Remove("HorarioEspecialidadId");
            Profesional.Enabled = TieneRol("ADMINISTRATIVO");
            EspecialidadHorario.Enabled = true;
            if (EspecialidadHorario.Items.Count > 0)
            {
                EspecialidadHorario.SelectedIndex = 0;
            }

            DiaSemana.SelectedValue = "1";
            HoraInicio.Text = string.Empty;
            HoraFin.Text = string.Empty;
            TituloHorario.Text = "Nueva franja semanal";
            GuardarHorario.Text = "Guardar franja";
        }

        private bool EsPropioProfesional(Guid profesionalId)
        {
            if (TieneRol("ADMINISTRATIVO"))
            {
                return true;
            }

            Guid usuarioActual;
            return TieneRol("PROFESIONAL")
                && Guid.TryParse(Convert.ToString(Session["UsuarioId"]), out usuarioActual)
                && usuarioActual == profesionalId;
        }

        private string ObtenerTextoDataKey(int fila, string clave)
        {
            object valor = HorariosGrid.DataKeys[fila].Values[clave];
            return valor == null || valor == DBNull.Value ? string.Empty : Convert.ToString(valor);
        }

        private void EjecutarConManejoDeErrores(string contexto, Action accion)
        {
            try
            {
                accion();
            }
            catch (InvalidOperationException ex)
            {
                MostrarError(ex.Message);
            }
            catch (SqlException ex)
            {
                MostrarError(contexto + " (error SQL " + ex.Number + ").");
            }
        }

        private void MostrarExito(string mensaje)
        {
            Mensaje.Text = HttpUtility.HtmlEncode(mensaje);
            Mensaje.CssClass = "alert alert-success";
            Mensaje.Visible = true;
        }

        private void MostrarError(string mensaje)
        {
            Mensaje.Text = HttpUtility.HtmlEncode(mensaje);
            Mensaje.CssClass = "alert alert-error";
            Mensaje.Visible = true;
        }

        private void LimpiarMensaje()
        {
            Mensaje.Text = string.Empty;
            Mensaje.Visible = false;
        }
    }
}
