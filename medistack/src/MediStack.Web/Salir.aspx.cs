using System;
using System.Web.Security;
using System.Web.UI;

namespace MediStack.Web
{
    public partial class Salir : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            Response.Redirect(ResolveUrl("~/Account/Login.aspx"), false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
