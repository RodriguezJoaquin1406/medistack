using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using MediStack.Dominio;
using MediStack.Negocio;

namespace MediStack.Web
{
    public partial class Especialidades : PaginaAdministrativa
    {
        private readonly GestionClinicaNegocio _negocio = new GestionClinicaNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarConManejoDeErrores(Mensaje, "No se pudieron cargar las especialidades",
                    () =>
                    {
                        LimpiarFormulario();
                        CargarEspecialidades();
                    });
            }
        }

        protected void Buscar_Click(object sender, EventArgs e)
        {
            CargarConManejoDeErrores(Mensaje, "No se pudieron buscar las especialidades", CargarEspecialidades);
        }

        protected void VerTodas_Click(object sender, EventArgs e)
        {
            Busqueda.Text = string.Empty;
            CargarConManejoDeErrores(Mensaje, "No se pudieron cargar las especialidades", CargarEspecialidades);
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
                short duracion;
                if (!short.TryParse(Duracion.Text, out duracion))
                {
                    MostrarError(Mensaje, "Ingresa una duracion valida en minutos.");
                    return;
                }

                Especialidad especialidad = new Especialidad
                {
                    EspecialidadId = ViewState["EspecialidadId"] == null ? 0 : (int)ViewState["EspecialidadId"],
                    Codigo = Codigo.Text,
                    Nombre = Nombre.Text,
                    Descripcion = Descripcion.Text,
                    DuracionEstandarMinutos = duracion
                };

                _negocio.GuardarEspecialidad(especialidad);
                LimpiarFormulario();
                CargarEspecialidades();
                MostrarExito(Mensaje, "La especialidad se guardo correctamente.");
            }
            catch (InvalidOperationException ex)
            {
                MostrarError(Mensaje, ex.Message);
            }
            catch (FormatException)
            {
                MostrarError(Mensaje, "La especialidad seleccionada no es valida. Actualiza la pagina e intenta nuevamente.");
            }
            catch (SqlException ex)
            {
                MostrarError(Mensaje, "No se pudieron cargar o guardar las especialidades (error SQL " + ex.Number + ").");
            }
        }

        protected void EspecialidadesGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int indice;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out indice)
                || indice < 0 || indice >= EspecialidadesGrid.DataKeys.Count)
            {
                return;
            }

            try
            {
                int id = EnteroDataKey(EspecialidadesGrid, indice, "EspecialidadId");
                if (e.CommandName == "Editar")
                {
                    CargarEspecialidadEnFormulario(indice);
                }
                else if (e.CommandName == "CambiarEstado")
                {
                    bool activa = Convert.ToBoolean(EspecialidadesGrid.DataKeys[indice].Values["Activa"]);
                    _negocio.CambiarEstadoEspecialidad(id, !activa);
                    CargarEspecialidades();
                    MostrarExito(Mensaje, activa ? "La especialidad fue desactivada." : "La especialidad fue reactivada.");
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

        private void CargarEspecialidades()
        {
            EspecialidadesGrid.DataSource = _negocio.ObtenerEspecialidades(Busqueda.Text);
            EspecialidadesGrid.DataBind();
        }

        private void CargarEspecialidadEnFormulario(int indice)
        {
            ViewState["EspecialidadId"] = EnteroDataKey(EspecialidadesGrid, indice, "EspecialidadId");
            Codigo.Text = TextoDataKey(EspecialidadesGrid, indice, "Codigo");
            Nombre.Text = TextoDataKey(EspecialidadesGrid, indice, "Nombre");
            Descripcion.Text = TextoDataKey(EspecialidadesGrid, indice, "Descripcion");
            Duracion.Text = Convert.ToString(EspecialidadesGrid.DataKeys[indice].Values["DuracionEstandarMinutos"]);
            TituloFormulario.Text = "Editar especialidad";
            Guardar.Text = "Guardar cambios";
            LimpiarMensaje(Mensaje);
        }

        private void LimpiarFormulario()
        {
            ViewState.Remove("EspecialidadId");
            Codigo.Text = string.Empty;
            Nombre.Text = string.Empty;
            Descripcion.Text = string.Empty;
            Duracion.Text = "30";
            TituloFormulario.Text = "Alta de especialidad";
            Guardar.Text = "Guardar especialidad";
        }
    }
}
