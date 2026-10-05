<%@ Page Title="Iniciar sesión" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MediStack.Web.Account.Login" %>
<asp:Content ID="LoginContent" ContentPlaceHolderID="MainContent" runat="server">
    <section class="login-layout" aria-labelledby="login-title">
        <div class="login-intro">
            <p class="eyebrow">Gestión clínica centralizada</p>
            <h1 id="login-title">Bienvenido a MediStack</h1>
            <p>Ingresa con tu cuenta para continuar.</p>
        </div>
        <div class="panel login-panel">
            <asp:Label ID="ErrorMessage" runat="server" CssClass="alert alert-error" role="alert" Visible="false" />
            <div class="form-group">
                <asp:Label ID="NombreUsuarioLabel" runat="server" AssociatedControlID="NombreUsuario" Text="Usuario" />
                <asp:TextBox ID="NombreUsuario" runat="server" CssClass="form-control" MaxLength="50" autocomplete="username" required="required" />
            </div>
            <div class="form-group">
                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Text="Contraseña" />
                <asp:TextBox ID="Password" runat="server" CssClass="form-control" TextMode="Password" MaxLength="128" autocomplete="current-password" required="required" />
            </div>
            <asp:Button ID="Ingresar" runat="server" Text="Iniciar sesión" CssClass="button button-primary button-wide" OnClick="Ingresar_Click" />
            <p class="form-note">Si no tienes una cuenta o necesitas recuperar el acceso, solicita ayuda al área administrativa.</p>
        </div>
    </section>
</asp:Content>
