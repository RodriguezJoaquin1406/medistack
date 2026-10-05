using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using MediStack.Dominio;
using MediStack.Negocio;

namespace MediStack.Web
{
    public partial class Coberturas : PaginaAdministrativa
    {
        private readonly GestionClinicaNegocio _negocio = new GestionClinicaNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarConManejoDeErrores(Mensaje, "No se pudieron cargar las coberturas",
                    () =>
                    {
                        LimpiarFormulario();
                        CargarCoberturas();
                    });
            }
        }

        protected void Buscar_Click(object sender, EventArgs e)
        {
            CargarConManejoDeErrores(Mensaje, "No se pudieron buscar las coberturas", CargarCoberturas);
        }

        protected void VerTodas_Click(object sender, EventArgs e)
        {
            Busqueda.Text = string.Empty;
            CargarConManejoDeErrores(Mensaje, "No se pudieron cargar las coberturas", CargarCoberturas);
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
                decimal porcentaje;
                int obraSocialId;
                int especialidadId;
                if (!decimal.TryParse(Porcentaje.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out porcentaje)
                    || !int.TryParse(ObraSocial.SelectedValue, out obraSocialId)
                    || !int.TryParse(Especialidad.SelectedValue, out especialidadId))
                {
                    MostrarError(Mensaje, "Selecciona una obra social, una especialidad y un porcentaje valido.");
                    return;
                }

                bool esNueva = ViewState["CoberturaObraSocialId"] == null;
                CoberturaEspecialidad cobertura = new CoberturaEspecialidad
                {
                    ObraSocialId = esNueva ? obraSocialId : (int)ViewState["CoberturaObraSocialId"],
                    EspecialidadId = esNueva ? especialidadId : (int)ViewState["CoberturaEspecialidadId"],
                    PorcentajeCobertura = porcentaje
                };

                _negocio.GuardarCobertura(cobertura, esNueva);
                LimpiarFormulario();
                CargarCoberturas();
                MostrarExito(Mensaje, "La cobertura se guardo correctamente.");
            }
            catch (InvalidOperationException ex)
            {
                MostrarError(Mensaje, ex.Message);
            }
            catch (FormatException)
            {
                MostrarError(Mensaje, "La cobertura seleccionada no es valida. Actualiza la pagina e intenta nuevamente.");
            }
            catch (SqlException ex)
            {
                MostrarError(Mensaje, "No se pudo guardar la cobertura (error SQL " + ex.Number + ").");
            }
        }

        protected void CoberturasGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int indice;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out indice)
                || indice < 0 || indice >= CoberturasGrid.DataKeys.Count)
            {
                return;
            }

            try
            {
                DataKey datos = CoberturasGrid.DataKeys[indice];
                int obraSocialId = Convert.ToInt32(datos.Values["ObraSocialId"]);
                int especialidadId = Convert.ToInt32(datos.Values["EspecialidadId"]);
                if (e.CommandName == "Editar")
                {
                    EditarCobertura(indice, datos);
                }
                else if (e.CommandName == "Eliminar")
                {
                    _negocio.EliminarCobertura(obraSocialId, especialidadId);
                    CargarCoberturas();
                    MostrarExito(Mensaje, "La cobertura se elimino correctamente.");
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

        private void CargarCoberturas()
        {
            CoberturasGrid.DataSource = _negocio.BuscarCoberturas(Busqueda.Text);
            CoberturasGrid.DataBind();
        }

        private void CargarListas()
        {
            ObraSocial.DataSource = _negocio.ObtenerObrasSocialesActivas();
            ObraSocial.DataTextField = "Nombre";
            ObraSocial.DataValueField = "ObraSocialId";
            ObraSocial.DataBind();
            ObraSocial.Items.Insert(0, new ListItem("Selecciona una obra social", string.Empty));

            Especialidad.DataSource = _negocio.ObtenerEspecialidadesActivas();
            Especialidad.DataTextField = "Nombre";
            Especialidad.DataValueField = "EspecialidadId";
            Especialidad.DataBind();
            Especialidad.Items.Insert(0, new ListItem("Selecciona una especialidad", string.Empty));
        }

        private void EditarCobertura(int indice, DataKey datos)
        {
            int obraSocialId = Convert.ToInt32(datos.Values["ObraSocialId"]);
            int especialidadId = Convert.ToInt32(datos.Values["EspecialidadId"]);
            ViewState["CoberturaObraSocialId"] = obraSocialId;
            ViewState["CoberturaEspecialidadId"] = especialidadId;
            AgregarOpcionSiFalta(ObraSocial, obraSocialId.ToString(CultureInfo.InvariantCulture),
                TextoDataKey(CoberturasGrid, indice, "ObraSocial"));
            AgregarOpcionSiFalta(Especialidad, especialidadId.ToString(CultureInfo.InvariantCulture),
                TextoDataKey(CoberturasGrid, indice, "Especialidad"));
            ObraSocial.SelectedValue = obraSocialId.ToString(CultureInfo.InvariantCulture);
            Especialidad.SelectedValue = especialidadId.ToString(CultureInfo.InvariantCulture);
            ObraSocial.Enabled = false;
            Especialidad.Enabled = false;
            Porcentaje.Text = Convert.ToDecimal(datos.Values["PorcentajeCobertura"])
                .ToString("0.00", CultureInfo.InvariantCulture);
            TituloFormulario.Text = "Editar cobertura";
            Guardar.Text = "Guardar cambios";
            LimpiarMensaje(Mensaje);
        }

        private static void AgregarOpcionSiFalta(DropDownList lista, string valor, string texto)
        {
            if (lista.Items.FindByValue(valor) == null)
            {
                lista.Items.Add(new ListItem(texto + " (inactiva)", valor));
            }
        }

        private void LimpiarFormulario()
        {
            CargarListas();
            ViewState.Remove("CoberturaObraSocialId");
            ViewState.Remove("CoberturaEspecialidadId");
            ObraSocial.Enabled = true;
            Especialidad.Enabled = true;
            if (ObraSocial.Items.Count > 0)
            {
                ObraSocial.SelectedIndex = 0;
            }

            if (Especialidad.Items.Count > 0)
            {
                Especialidad.SelectedIndex = 0;
            }

            Porcentaje.Text = string.Empty;
            TituloFormulario.Text = "Alta de cobertura";
            Guardar.Text = "Guardar cobertura";
        }
    }
}
