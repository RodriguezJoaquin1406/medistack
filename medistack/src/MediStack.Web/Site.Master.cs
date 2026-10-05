using System;
using System.Web.UI;

namespace MediStack.Web
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.User.Identity.IsAuthenticated && Session["UsuarioId"] != null)
            {
                UsuarioActual.Visible = true;
                NombreActual.Text = Server.HtmlEncode(Convert.ToString(Session["NombreCompleto"]));
                RolActual.Text = Server.HtmlEncode(Convert.ToString(Session["RolNombre"]));
                NavegacionAdministrativa.Visible = string.Equals(
                    Convert.ToString(Session["RolCodigo"]),
                    "ADMINISTRATIVO",
                    StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
