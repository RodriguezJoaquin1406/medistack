using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using MediStack.Dominio;
using MediStack.Negocio;

namespace MediStack.Web
{
    public partial class Convenios : PaginaAdministrativa
    {
        private readonly GestionClinicaNegocio _negocio = new GestionClinicaNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarConManejoDeErrores(Mensaje, "No se pudieron cargar los convenios",
                    () =>
                    {
                        CargarListas();
                        LimpiarFormulario();
                        CargarConvenios();
                    });
            }
        }

        protected void Buscar_Click(object sender, EventArgs e)
        {
            CargarConManejoDeErrores(Mensaje, "No se pudieron buscar los convenios", CargarConvenios);
        }

        protected void VerTodos_Click(object sender, EventArgs e)
        {
            Busqueda.Text = string.Empty;
            CargarConManejoDeErrores(Mensaje, "No se pudieron cargar los convenios", CargarConvenios);
        }

        protected void Nuevo_Click(object sender, EventArgs e)
        {
            LimpiarMensaje(Mensaje);
            LimpiarFormulario();
        }

        protected void Guardar_Click(object sender, EventArgs e)
        {
            try
            {
                Guid profesionalId;
                DateTime fechaDesde;
                DateTime? fechaHasta = null;
                decimal valorConsulta;
                decimal esquemaValor;

                if (!Guid.TryParse(Profesional.SelectedValue, out profesionalId)
                    || !DateTime.TryParseExact(FechaDesde.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out fechaDesde)
                    || !decimal.TryParse(ValorConsulta.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out valorConsulta)
                    || !decimal.TryParse(EsquemaValor.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out esquemaValor))
                {
                    MostrarError(Mensaje, "Completa los campos del convenio con valores validos.");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(FechaHasta.Text))
                {
                    DateTime fechaFin;
                    if (!DateTime.TryParseExact(FechaHasta.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out fechaFin))
                    {
                        MostrarError(Mensaje, "Ingresa una fecha de fin valida.");
                        return;
                    }

                    fechaHasta = fechaFin;
                }

                string[] coberturaElegida = Cobertura.SelectedValue.Split('|');
                int obraSocialId;
                int especialidadId;
                if (coberturaElegida.Length != 2
                    || !int.TryParse(coberturaElegida[0], out obraSocialId)
                    || !int.TryParse(coberturaElegida[1], out especialidadId))
                {
                    MostrarError(Mensaje, "Selecciona una cobertura valida.");
                    return;
                }

                bool esNuevo = ViewState["ConvenioId"] == null;
                Convenio convenio = new Convenio
                {
                    ConvenioId = esNuevo ? 0 : (int)ViewState["ConvenioId"],
                    ProfesionalId = profesionalId,
                    EspecialidadId = especialidadId,
                    ObraSocialId = obraSocialId,
                    FechaDesde = fechaDesde,
                    FechaHasta = fechaHasta,
                    Activo = true,
                    ValorConsulta = valorConsulta,
                    EsquemaTipo = EsquemaTipo.SelectedValue,
                    EsquemaValor = esquemaValor,
                    EsEspecialidadPrincipal = Principal.Checked
                };

                _negocio.GuardarConvenio(convenio, esNuevo);
                LimpiarFormulario();
                CargarConvenios();
                MostrarExito(Mensaje, "El convenio se guardo correctamente.");
            }
            catch (InvalidOperationException ex)
            {
                MostrarError(Mensaje, ex.Message);
            }
            catch (FormatException)
            {
                MostrarError(Mensaje, "El convenio seleccionado no es valido. Actualiza la pagina e intenta nuevamente.");
            }
            catch (SqlException ex)
            {
                MostrarError(Mensaje, "No se pudo guardar el convenio (error SQL " + ex.Number + ").");
            }
        }

        protected void ConveniosGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int indice;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out indice)
                || indice < 0 || indice >= ConveniosGrid.DataKeys.Count)
            {
                return;
            }

            try
            {
                int id = EnteroDataKey(ConveniosGrid, indice, "ConvenioId");
                if (e.CommandName == "Editar")
                {
                    CargarConvenioEnFormulario(id);
                }
                else if (e.CommandName == "CambiarEstado")
                {
                    bool activo = Convert.ToBoolean(ConveniosGrid.DataKeys[indice].Values["Activo"]);
                    _negocio.CambiarEstadoConvenio(id, !activo);
                    CargarConvenios();
                    MostrarExito(Mensaje, activo ? "El convenio fue desactivado." : "El convenio fue reactivado.");
                }
            }
            catch (InvalidOperationException ex)
            {
                MostrarError(Mensaje, ex.Message);
            }
            catch (SqlException ex)
            {
                MostrarError(Mensaje, "No se pudo completar la operacion (error SQL " + ex.Number + ").");
            }
        }

        private void CargarConvenios()
        {
            ConveniosGrid.DataSource = _negocio.BuscarConvenios(Busqueda.Text);
            ConveniosGrid.DataBind();
        }

        private void CargarListas()
        {
            Profesional.DataSource = _negocio.ObtenerProfesionalesParaConvenio();
            Profesional.DataTextField = "Nombre";
            Profesional.DataValueField = "ProfesionalId";
            Profesional.DataBind();
            Profesional.Items.Insert(0, new ListItem("Selecciona un profesional", string.Empty));

            DataTable coberturas = _negocio.ObtenerCoberturasParaConvenio();
            Cobertura.DataSource = coberturas;
            Cobertura.DataTextField = "Nombre";
            Cobertura.DataValueField = "Clave";
            Cobertura.DataBind();

            Cobertura.Items.Insert(0, new ListItem("Selecciona una cobertura", string.Empty));
        }

        private void CargarConvenioEnFormulario(int convenioId)
        {
            DataTable datos = _negocio.ObtenerConvenio(convenioId);
            if (datos.Rows.Count == 0)
            {
                MostrarError(Mensaje, "El convenio ya no existe. Actualiza la pagina.");
                CargarConvenios();
                return;
            }

            DataRow convenio = datos.Rows[0];
            ViewState["ConvenioId"] = Convert.ToInt32(convenio["ConvenioId"]);
            Profesional.SelectedValue = Convert.ToString(convenio["ProfesionalId"]);
            Cobertura.SelectedValue = convenio["ObraSocialId"] + "|" + convenio["EspecialidadId"];
            Profesional.Enabled = false;
            Cobertura.Enabled = false;
            FechaDesde.Text = Convert.ToDateTime(convenio["FechaDesde"]).ToString("yyyy-MM-dd");
            FechaHasta.Text = convenio["FechaHasta"] == DBNull.Value
                ? string.Empty : Convert.ToDateTime(convenio["FechaHasta"]).ToString("yyyy-MM-dd");
            ValorConsulta.Text = Convert.ToDecimal(convenio["ValorConsulta"])
                .ToString("0.00", CultureInfo.InvariantCulture);
            EsquemaTipo.SelectedValue = Convert.ToString(convenio["EsquemaTipo"]);
            EsquemaValor.Text = Convert.ToDecimal(convenio["EsquemaValor"])
                .ToString("0.00", CultureInfo.InvariantCulture);
            Principal.Checked = Convert.ToBoolean(convenio["EsEspecialidadPrincipal"]);
            TituloFormulario.Text = "Editar convenio";
            Guardar.Text = "Guardar cambios";
            LimpiarMensaje(Mensaje);
        }

        private void LimpiarFormulario()
        {
            ViewState.Remove("ConvenioId");
            Profesional.Enabled = true;
            Cobertura.Enabled = true;
            Profesional.SelectedIndex = Profesional.Items.Count == 0 ? -1 : 0;
            Cobertura.SelectedIndex = Cobertura.Items.Count == 0 ? -1 : 0;
            FechaDesde.Text = string.Empty;
            FechaHasta.Text = string.Empty;
            ValorConsulta.Text = string.Empty;
            EsquemaTipo.SelectedValue = "Porcentaje";
            EsquemaValor.Text = string.Empty;
            Principal.Checked = false;
            TituloFormulario.Text = "Alta de convenio";
            Guardar.Text = "Guardar convenio";
        }
    }
}
