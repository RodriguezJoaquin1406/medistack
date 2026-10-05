using System;
using MediStack.Datos;
using MediStack.Dominio;

namespace MediStack.Negocio
{
    public class UsuarioNegocio
    {
        private readonly UsuarioDatos _usuarioDatos;

        public UsuarioNegocio()
        {
            _usuarioDatos = new UsuarioDatos();
        }

        public ResultadoLogin IniciarSesion(string nombreUsuario, string password)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario)
                || nombreUsuario.Trim().Length > 50
                || string.IsNullOrEmpty(password)
                || password.Length > 128)
            {
                return ResultadoLogin.Fallido();
            }

            Usuario usuario = _usuarioDatos.ObtenerPorNombreUsuario(nombreUsuario.Trim());
            if (usuario == null || !usuario.Activo)
            {
                return ResultadoLogin.Fallido();
            }

            if (usuario.BloqueadoHasta.HasValue && usuario.BloqueadoHasta.Value > DateTime.Now)
            {
                return ResultadoLogin.Fallido();
            }

            if (!VerificadorPassword.Verificar(password, usuario.PasswordHash))
            {
                _usuarioDatos.RegistrarIntentoFallido(usuario.UsuarioId);
                return ResultadoLogin.Fallido();
            }

            _usuarioDatos.RegistrarAccesoCorrecto(usuario.UsuarioId);
            return ResultadoLogin.Correcto(usuario);
        }
    }
}
