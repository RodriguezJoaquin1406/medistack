using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using MediStack.Dominio;
using MediStack.Negocio;

namespace MediStack.Web
{
    public partial class FichaPacientePagina : PaginaProtegida
    {
        private readonly FichaPacienteNegocio _negocio = new FichaPacienteNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            Guid pacienteId;
            if (!TryObtenerPacienteAutorizado(out pacienteId))
            {
                return;
            }

            try
            {
                MostrarFicha(_negocio.ObtenerFicha(pacienteId));
            }
            catch (InvalidOperationException ex)
            {
                MostrarError(404, ex.Message);
            }
            catch (SqlException ex)
            {
                MostrarError(500, "No se pudo cargar la ficha del paciente (error SQL " + ex.Number + ").");
            }
        }

        private bool TryObtenerPacienteAutorizado(out Guid pacienteId)
        {
            pacienteId = Guid.Empty;
            string rol = Convert.ToString(Session["RolCodigo"]);
            Guid usuarioActual;
            if (!Guid.TryParse(Convert.ToString(Session["UsuarioId"]), out usuarioActual))
            {
                MostrarError(403, "No se pudo validar el usuario de la sesion.");
                return false;
            }

            if (string.Equals(rol, "PACIENTE", StringComparison.OrdinalIgnoreCase))
            {
                Guid solicitado;
                if (!string.IsNullOrWhiteSpace(Request.QueryString["PacienteId"])
                    && (!Guid.TryParse(Request.QueryString["PacienteId"], out solicitado)
                        || solicitado != usuarioActual))
                {
                    MostrarError(403, "Solo puedes consultar tu propia ficha.");
                    return false;
                }

                pacienteId = usuarioActual;
                return true;
            }

            if (string.Equals(rol, "ADMINISTRATIVO", StringComparison.OrdinalIgnoreCase))
            {
                if (Guid.TryParse(Request.QueryString["PacienteId"], out pacienteId))
                {
                    return true;
                }

                MostrarError(400, "Selecciona un paciente valido.");
                return false;
            }

            if (string.Equals(rol, "PROFESIONAL", StringComparison.OrdinalIgnoreCase)
                && Guid.TryParse(Request.QueryString["PacienteId"], out pacienteId))
            {
                try
                {
                    if (_negocio.PuedeProfesionalVer(pacienteId, usuarioActual))
                    {
                        return true;
                    }
                }
                catch (SqlException ex)
                {
                    MostrarError(500, "No se pudo validar el acceso a la ficha (error SQL " + ex.Number + ").");
                    return false;
                }
            }

            MostrarError(403, "No tienes acceso a la ficha de este paciente.");
            return false;
        }

        private void MostrarFicha(FichaPaciente ficha)
        {
            NombrePacienteLiteral.Text = HttpUtility.HtmlEncode(ficha.Nombre + " " + ficha.Apellido);
            FechaAlta.Text = HttpUtility.HtmlEncode(ficha.FechaAlta.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("es-AR")));
            EstadoPaciente.Text = ficha.Activo
                ? "<span class=\"status-tag\">Activo</span>"
                : "<span class=\"status-tag\">Inactivo</span>";
            Documento.Text = Codificar(ficha.NumeroDocumento);
            Nacimiento.Text = ficha.FechaNacimiento.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("es-AR"));
            Email.Text = Codificar(ficha.Email);
            Telefono.Text = ValorOVacio(ficha.Telefono);
            ObraSocial.Text = ValorOVacio(ficha.ObraSocial);
            Afiliado.Text = ValorOVacio(ficha.NumeroAfiliado);
            EmergenciaNombre.Text = ValorOVacio(ficha.ContactoEmergenciaNombre);
            EmergenciaTelefono.Text = ValorOVacio(ficha.ContactoEmergenciaTelefono);
            HistorialGrid.DataSource = ficha.Historial;
            HistorialGrid.DataBind();
            CoberturasGrid.DataSource = ficha.Coberturas;
            CoberturasGrid.DataBind();
            CobrosGrid.DataSource = ficha.Cobros;
            CobrosGrid.DataBind();
            FichaPanel.Visible = true;
        }

        private void MostrarError(int codigo, string mensaje)
        {
            Response.StatusCode = codigo;
            Response.TrySkipIisCustomErrors = true;
            Mensaje.Text = HttpUtility.HtmlEncode(mensaje);
            Mensaje.CssClass = "alert alert-error";
            Mensaje.Visible = true;
            FichaPanel.Visible = false;
        }

        private static string Codificar(string valor)
        {
            return HttpUtility.HtmlEncode(valor ?? string.Empty);
        }

        private static string ValorOVacio(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? "No informado" : Codificar(valor);
        }
    }
}
