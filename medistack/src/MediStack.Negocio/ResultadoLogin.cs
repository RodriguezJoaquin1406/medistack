using MediStack.Dominio;

namespace MediStack.Negocio
{
    public class ResultadoLogin
    {
        public bool Exitoso { get; private set; }
        public string Mensaje { get; private set; }
        public Usuario Usuario { get; private set; }

        public static ResultadoLogin Correcto(Usuario usuario)
        {
            return new ResultadoLogin
            {
                Exitoso = true,
                Usuario = usuario,
                Mensaje = string.Empty
            };
        }

        public static ResultadoLogin Fallido()
        {
            return new ResultadoLogin
            {
                Exitoso = false,
                Usuario = null,
                Mensaje = "No se pudo iniciar sesión. Verifica tus datos o intenta nuevamente más tarde."
            };
        }
    }
}
