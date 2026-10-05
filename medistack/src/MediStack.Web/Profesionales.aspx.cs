using System;
using System.Web.UI.WebControls;
using MediStack.Dominio;
using MediStack.Negocio;

namespace MediStack.Web
{
    public partial class Profesionales : PaginaAdministrativa
    {
        private readonly GestionClinicaNegocio _negocio = new GestionClinicaNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarConManejoDeErrores(Mensaje, "No se pudieron cargar los profesionales",
                    () =>
                    {
                        LimpiarFormulario();
                        CargarProfesionales();
                    });
            }
        }

        protected void Buscar_Click(object sender, EventArgs e)
        {
            CargarConManejoDeErrores(Mensaje, "No se pudieron buscar los profesionales", CargarProfesionales);
        }

        protected void VerTodos_Click(object sender, EventArgs e)
        {
            Busqueda.Text = string.Empty;
            CargarConManejoDeErrores(Mensaje, "No se pudieron cargar los profesionales", CargarProfesionales);
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
                DateTime fecha;
                if (!DateTime.TryParse(Nacimiento.Text, out fecha))
                {
                    MostrarError(Mensaje, "Ingresa una fecha de nacimiento valida.");
                    return;
                }

                Profesional profesional = new Profesional
                {
                    UsuarioId = ViewState["ProfesionalId"] == null ? Guid.Empty : (Guid)ViewState["ProfesionalId"],
                    NombreUsuario = Usuario.Text,
                    Nombre = Nombre.Text,
                    Apellido = Apellido.Text,
                    NumeroDocumento = Documento.Text,
                    FechaNacimiento = fecha,
                    Email = Email.Text,
                    Telefono = Telefono.Text,
                    MatriculaProfesional = Matricula.Text
                };

                _negocio.GuardarProfesional(profesional, Contrasena.Text);
                LimpiarFormulario();
                CargarProfesionales();
                MostrarExito(Mensaje, "Los datos del profesional se guardaron correctamente.");
            }
            catch (InvalidOperationException ex)
            {
                MostrarError(Mensaje, ex.Message);
            }
            catch (FormatException)
            {
                MostrarError(Mensaje, "El profesional seleccionado no es valido. Actualiza la pagina e intenta nuevamente.");
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MostrarError(Mensaje, "No se pudieron cargar o guardar los datos del profesional (error SQL " + ex.Number + ").");
            }
        }

        protected void ProfesionalesGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int indice;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out indice)
                || indice < 0 || indice >= ProfesionalesGrid.DataKeys.Count)
            {
                return;
            }

            try
            {
                Guid id = GuidDataKey(ProfesionalesGrid, indice, "ProfesionalId");
                if (e.CommandName == "Editar")
                {
                    CargarProfesionalEnFormulario(indice);
                }
                else if (e.CommandName == "CambiarEstado")
                {
                    bool activo = Convert.ToBoolean(ProfesionalesGrid.DataKeys[indice].Values["Activo"]);
                    _negocio.CambiarEstadoProfesional(id, !activo);
                    CargarProfesionales();
                    MostrarExito(Mensaje, activo ? "El profesional fue desactivado." : "El profesional fue reactivado.");
                }
            }
            catch (InvalidOperationException ex)
            {
                MostrarError(Mensaje, ex.Message);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MostrarError(Mensaje, "No se pudo completar la operacion (error SQL " + ex.Number + ").");
            }
        }

        private void CargarProfesionales()
        {
            ProfesionalesGrid.DataSource = _negocio.BuscarProfesionales(Busqueda.Text);
            ProfesionalesGrid.DataBind();
        }

        private void CargarProfesionalEnFormulario(int indice)
        {
            ViewState["ProfesionalId"] = GuidDataKey(ProfesionalesGrid, indice, "ProfesionalId");
            Nombre.Text = TextoDataKey(ProfesionalesGrid, indice, "Nombre");
            Apellido.Text = TextoDataKey(ProfesionalesGrid, indice, "Apellido");
            Documento.Text = TextoDataKey(ProfesionalesGrid, indice, "NumeroDocumento");
            Nacimiento.Text = FechaDataKey(ProfesionalesGrid, indice, "FechaNacimiento").ToString("yyyy-MM-dd");
            Email.Text = TextoDataKey(ProfesionalesGrid, indice, "Email");
            Telefono.Text = TextoDataKey(ProfesionalesGrid, indice, "Telefono");
            Matricula.Text = TextoDataKey(ProfesionalesGrid, indice, "MatriculaProfesional");
            Usuario.Text = TextoDataKey(ProfesionalesGrid, indice, "NombreUsuario");
            Usuario.Enabled = false;
            CuentaNueva.Visible = false;
            TituloFormulario.Text = "Editar profesional";
            Guardar.Text = "Guardar cambios";
            LimpiarMensaje(Mensaje);
        }

        private void LimpiarFormulario()
        {
            ViewState.Remove("ProfesionalId");
            Nombre.Text = string.Empty;
            Apellido.Text = string.Empty;
            Documento.Text = string.Empty;
            Nacimiento.Text = string.Empty;
            Email.Text = string.Empty;
            Telefono.Text = string.Empty;
            Matricula.Text = string.Empty;
            Usuario.Text = string.Empty;
            Usuario.Enabled = true;
            Contrasena.Text = string.Empty;
            CuentaNueva.Visible = true;
            TituloFormulario.Text = "Alta de profesional";
            Guardar.Text = "Guardar profesional";
        }
    }
}
