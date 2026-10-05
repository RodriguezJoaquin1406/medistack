using System;
using System.Web.UI;

namespace MediStack.Web
{
    public partial class DefaultPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string destino = Context.User.Identity.IsAuthenticated
                ? "~/Dashboard.aspx"
                : "~/Account/Login.aspx";

            Response.Redirect(ResolveUrl(destino), false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
