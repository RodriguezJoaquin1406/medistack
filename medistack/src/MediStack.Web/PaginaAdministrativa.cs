using System;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;

namespace MediStack.Web
{
    public class PaginaAdministrativa : PaginaProtegida
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!TieneRol("ADMINISTRATIVO"))
            {
                Response.Redirect(ResolveUrl("~/Dashboard.aspx"), true);
            }
        }

        protected void MostrarExito(System.Web.UI.WebControls.Label etiqueta, string mensaje)
        {
            etiqueta.Text = HttpUtility.HtmlEncode(mensaje);
            etiqueta.CssClass = "alert alert-success";
            etiqueta.Visible = true;
        }

        protected void MostrarError(System.Web.UI.WebControls.Label etiqueta, string mensaje)
        {
            etiqueta.Text = HttpUtility.HtmlEncode(mensaje);
            etiqueta.CssClass = "alert alert-error";
            etiqueta.Visible = true;
        }

        protected void LimpiarMensaje(System.Web.UI.WebControls.Label etiqueta)
        {
            etiqueta.Text = string.Empty;
            etiqueta.Visible = false;
        }

        protected bool CargarConManejoDeErrores(
            System.Web.UI.WebControls.Label etiqueta,
            string contexto,
            Action cargar)
        {
            try
            {
                cargar();
                return true;
            }
            catch (InvalidOperationException ex)
            {
                MostrarError(etiqueta, ex.Message);
            }
            catch (SqlException ex)
            {
                MostrarError(etiqueta, contexto + " (error SQL " + ex.Number + ").");
            }

            return false;
        }

        protected static string TextoDataKey(System.Web.UI.WebControls.GridView grid, int fila, string clave)
        {
            object valor = grid.DataKeys[fila].Values[clave];
            return valor == null || valor == DBNull.Value ? string.Empty : Convert.ToString(valor);
        }

        protected static DateTime FechaDataKey(System.Web.UI.WebControls.GridView grid, int fila, string clave)
        {
            return Convert.ToDateTime(grid.DataKeys[fila].Values[clave]);
        }

        protected static int EnteroDataKey(System.Web.UI.WebControls.GridView grid, int fila, string clave)
        {
            object valor = grid.DataKeys[fila].Values[clave];
            return valor == null || valor == DBNull.Value ? 0 : Convert.ToInt32(valor);
        }

        protected static Guid GuidDataKey(System.Web.UI.WebControls.GridView grid, int fila, string clave)
        {
            return Guid.Parse(TextoDataKey(grid, fila, clave));
        }
    }
}
