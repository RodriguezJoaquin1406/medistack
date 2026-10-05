using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using MediStack.Dominio;
using MediStack.Negocio;

namespace MediStack.Web
{
    public partial class ObrasSociales : PaginaAdministrativa
    {
        private readonly GestionClinicaNegocio _negocio = new GestionClinicaNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarConManejoDeErrores(Mensaje, "No se pudieron cargar las obras sociales",
                    () =>
                    {
                        LimpiarFormulario();
                        CargarObrasSociales();
                    });
            }
        }

        protected void Buscar_Click(object sender, EventArgs e)
        {
            CargarConManejoDeErrores(Mensaje, "No se pudieron buscar las obras sociales", CargarObrasSociales);
        }

        protected void VerTodas_Click(object sender, EventArgs e)
        {
            Busqueda.Text = string.Empty;
            CargarConManejoDeErrores(Mensaje, "No se pudieron cargar las obras sociales", CargarObrasSociales);
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
                ObraSocial obraSocial = new ObraSocial
                {
                    ObraSocialId = ViewState["ObraSocialId"] == null ? 0 : (int)ViewState["ObraSocialId"],
                    Nombre = Nombre.Text,
                    CodigoCUIT = CodigoCUIT.Text
                };

                _negocio.GuardarObraSocial(obraSocial);
                LimpiarFormulario();
                CargarObrasSociales();
                MostrarExito(Mensaje, "La obra social se guardo correctamente.");
            }
            catch (InvalidOperationException ex)
            {
                MostrarError(Mensaje, ex.Message);
            }
            catch (FormatException)
            {
                MostrarError(Mensaje, "La obra social seleccionada no es valida. Actualiza la pagina e intenta nuevamente.");
            }
            catch (SqlException ex)
            {
                MostrarError(Mensaje, "No se pudieron cargar o guardar las obras sociales (error SQL " + ex.Number + ").");
            }
        }

        protected void ObrasSocialesGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int indice;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out indice)
                || indice < 0 || indice >= ObrasSocialesGrid.DataKeys.Count)
            {
                return;
            }

            try
            {
                int id = EnteroDataKey(ObrasSocialesGrid, indice, "ObraSocialId");
                if (e.CommandName == "Editar")
                {
                    ViewState["ObraSocialId"] = id;
                    Nombre.Text = TextoDataKey(ObrasSocialesGrid, indice, "Nombre");
                    CodigoCUIT.Text = TextoDataKey(ObrasSocialesGrid, indice, "CodigoCUIT");
                    TituloFormulario.Text = "Editar obra social";
                    Guardar.Text = "Guardar cambios";
                    LimpiarMensaje(Mensaje);
                }
                else if (e.CommandName == "CambiarEstado")
                {
                    bool activa = Convert.ToBoolean(ObrasSocialesGrid.DataKeys[indice].Values["Activa"]);
                    _negocio.CambiarEstadoObraSocial(id, !activa);
                    CargarObrasSociales();
                    MostrarExito(Mensaje, activa ? "La obra social fue desactivada." : "La obra social fue reactivada.");
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

        private void CargarObrasSociales()
        {
            ObrasSocialesGrid.DataSource = _negocio.ObtenerObrasSociales(Busqueda.Text);
            ObrasSocialesGrid.DataBind();
        }

        private void LimpiarFormulario()
        {
            ViewState.Remove("ObraSocialId");
            Nombre.Text = string.Empty;
            CodigoCUIT.Text = string.Empty;
            TituloFormulario.Text = "Alta de obra social";
            Guardar.Text = "Guardar obra social";
        }
    }
}
