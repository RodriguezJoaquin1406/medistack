using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace MediStack.Web
{
    public class PaginaProtegida : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!Context.User.Identity.IsAuthenticated || Session["UsuarioId"] == null)
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                Session.Abandon();
                Response.Redirect(ResolveUrl("~/Account/Login.aspx"), true);
            }
        }

        protected bool TieneRol(string codigoRol)
        {
            return string.Equals(
                Convert.ToString(Session["RolCodigo"]),
                codigoRol,
                StringComparison.OrdinalIgnoreCase);
        }

        protected string NombreCompleto
        {
            get { return HttpUtility.HtmlEncode(Convert.ToString(Session["NombreCompleto"])); }
        }

        protected string RolNombre
        {
            get { return HttpUtility.HtmlEncode(Convert.ToString(Session["RolNombre"])); }
        }
    }
}
