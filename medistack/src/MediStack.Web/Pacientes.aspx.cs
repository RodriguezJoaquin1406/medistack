using System;
using System.Data;
using System.Web.UI.WebControls;
using MediStack.Dominio;
using MediStack.Negocio;

namespace MediStack.Web
{
    public partial class Pacientes : PaginaAdministrativa
    {
        private readonly GestionClinicaNegocio _negocio = new GestionClinicaNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarConManejoDeErrores(Mensaje, "No se pudieron cargar los pacientes",
                    () =>
                    {
                        LimpiarFormulario();
                        CargarPacientes();
                    });
            }
        }

        protected void Buscar_Click(object sender, EventArgs e)
        {
            CargarConManejoDeErrores(Mensaje, "No se pudieron buscar los pacientes", CargarPacientes);
        }

        protected void LimpiarBusqueda_Click(object sender, EventArgs e)
        {
            Busqueda.Text = string.Empty;
            CargarConManejoDeErrores(Mensaje, "No se pudieron cargar los pacientes", CargarPacientes);
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

                int obraSocialId;
                Paciente paciente = new Paciente
                {
                    UsuarioId = ViewState["PacienteId"] == null ? Guid.Empty : (Guid)ViewState["PacienteId"],
                    NombreUsuario = Usuario.Text,
                    Nombre = Nombre.Text,
                    Apellido = Apellido.Text,
                    NumeroDocumento = Documento.Text,
                    FechaNacimiento = fecha,
                    Email = Email.Text,
                    Telefono = Telefono.Text,
                    ObraSocialId = int.TryParse(ObraSocial.SelectedValue, out obraSocialId) ? (int?)obraSocialId : null,
                    NumeroAfiliado = Afiliado.Text,
                    ContactoEmergenciaNombre = EmergenciaNombre.Text,
                    ContactoEmergenciaTelefono = EmergenciaTelefono.Text
                };

                _negocio.GuardarPaciente(paciente, Contrasena.Text);
                LimpiarFormulario();
                CargarPacientes();
                MostrarExito(Mensaje, "Los datos del paciente se guardaron correctamente.");
            }
            catch (InvalidOperationException ex)
            {
                MostrarError(Mensaje, ex.Message);
            }
            catch (FormatException)
            {
                MostrarError(Mensaje, "El paciente seleccionado no es valido. Actualiza la pagina e intenta nuevamente.");
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MostrarError(Mensaje, "No se pudieron cargar o guardar los datos del paciente (error SQL " + ex.Number + ").");
            }
        }

        protected void PacientesGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int indice;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out indice)
                || indice < 0 || indice >= PacientesGrid.DataKeys.Count)
            {
                return;
            }

            try
            {
                Guid id = GuidDataKey(PacientesGrid, indice, "PacienteId");
                if (e.CommandName == "Editar")
                {
                    CargarPacienteEnFormulario(indice);
                }
                else if (e.CommandName == "CambiarEstado")
                {
                    bool activo = Convert.ToBoolean(PacientesGrid.DataKeys[indice].Values["Activo"]);
                    _negocio.CambiarEstadoPaciente(id, !activo);
                    CargarPacientes();
                    MostrarExito(Mensaje, activo ? "El paciente fue desactivado." : "El paciente fue reactivado.");
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

        private void CargarPacientes()
        {
            PacientesGrid.DataSource = _negocio.BuscarPacientes(Busqueda.Text);
            PacientesGrid.DataBind();
        }

        private void CargarObrasSociales()
        {
            ObraSocial.DataSource = _negocio.ObtenerOpcionesObraSocialPaciente();
            ObraSocial.DataTextField = "Nombre";
            ObraSocial.DataValueField = "ObraSocialId";
            ObraSocial.DataBind();
            ObraSocial.Items.Insert(0, new ListItem("Particular / sin obra social", string.Empty));
        }

        private void CargarPacienteEnFormulario(int indice)
        {
            DataKey datos = PacientesGrid.DataKeys[indice];
            ViewState["PacienteId"] = Guid.Parse(Convert.ToString(datos.Values["PacienteId"]));
            Nombre.Text = TextoDataKey(PacientesGrid, indice, "Nombre");
            Apellido.Text = TextoDataKey(PacientesGrid, indice, "Apellido");
            Documento.Text = TextoDataKey(PacientesGrid, indice, "NumeroDocumento");
            Nacimiento.Text = FechaDataKey(PacientesGrid, indice, "FechaNacimiento").ToString("yyyy-MM-dd");
            Email.Text = TextoDataKey(PacientesGrid, indice, "Email");
            Telefono.Text = TextoDataKey(PacientesGrid, indice, "Telefono");
            Afiliado.Text = TextoDataKey(PacientesGrid, indice, "NumeroAfiliado");
            EmergenciaNombre.Text = TextoDataKey(PacientesGrid, indice, "ContactoEmergenciaNombre");
            EmergenciaTelefono.Text = TextoDataKey(PacientesGrid, indice, "ContactoEmergenciaTelefono");
            string obraSocialId = TextoDataKey(PacientesGrid, indice, "ObraSocialId");
            if (!string.IsNullOrEmpty(obraSocialId) && ObraSocial.Items.FindByValue(obraSocialId) == null)
            {
                ObraSocial.Items.Add(new ListItem(
                    TextoDataKey(PacientesGrid, indice, "ObraSocial") + " (inactiva)", obraSocialId));
            }

            SeleccionarValor(ObraSocial, obraSocialId);
            Usuario.Text = TextoDataKey(PacientesGrid, indice, "NombreUsuario");
            Usuario.Enabled = false;
            CuentaNueva.Visible = false;
            TituloFormulario.Text = "Editar paciente";
            Guardar.Text = "Guardar cambios";
            LimpiarMensaje(Mensaje);
        }

        private void LimpiarFormulario()
        {
            CargarObrasSociales();
            ViewState.Remove("PacienteId");
            Nombre.Text = string.Empty;
            Apellido.Text = string.Empty;
            Documento.Text = string.Empty;
            Nacimiento.Text = string.Empty;
            Email.Text = string.Empty;
            Telefono.Text = string.Empty;
            Afiliado.Text = string.Empty;
            EmergenciaNombre.Text = string.Empty;
            EmergenciaTelefono.Text = string.Empty;
            Usuario.Text = string.Empty;
            Usuario.Enabled = true;
            Contrasena.Text = string.Empty;
            SeleccionarValor(ObraSocial, string.Empty);
            CuentaNueva.Visible = true;
            TituloFormulario.Text = "Alta de paciente";
            Guardar.Text = "Guardar paciente";
        }

        private static void SeleccionarValor(DropDownList lista, string valor)
        {
            ListItem opcion = lista.Items.FindByValue(valor ?? string.Empty);
            lista.ClearSelection();
            if (opcion != null)
            {
                opcion.Selected = true;
            }
        }
    }
}
