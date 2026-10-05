using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace MediStack.Negocio
{
    public static class VerificadorPassword
    {
        private const int IteracionesMinimas = 10000;
        private const int IteracionesMaximas = 1000000;

        public static bool Verificar(string password, string formatoAlmacenado)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(formatoAlmacenado))
            {
                return false;
            }

            string[] partes = formatoAlmacenado.Split('$');
            int iteraciones;
            if (partes.Length != 4
                || partes[0] != "PBKDF2-SHA256"
                || !int.TryParse(partes[1], NumberStyles.None, CultureInfo.InvariantCulture, out iteraciones)
                || iteraciones < IteracionesMinimas
                || iteraciones > IteracionesMaximas)
            {
                return false;
            }

            try
            {
                byte[] salt = Convert.FromBase64String(partes[2]);
                byte[] hashEsperado = Convert.FromBase64String(partes[3]);
                if (salt.Length < 16 || salt.Length > 64 || hashEsperado.Length != 32)
                {
                    return false;
                }

                byte[] hashCalculado;
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                using (Rfc2898DeriveBytes derivador = new Rfc2898DeriveBytes(
                    passwordBytes, salt, iteraciones, HashAlgorithmName.SHA256))
                {
                    hashCalculado = derivador.GetBytes(hashEsperado.Length);
                }

                int diferencia = 0;
                for (int i = 0; i < hashEsperado.Length; i++)
                {
                    diferencia |= hashEsperado[i] ^ hashCalculado[i];
                }

                return diferencia == 0;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static string CrearHash(string password)
        {
            const int iteraciones = 120000;
            byte[] salt = new byte[16];
            using (RandomNumberGenerator generador = RandomNumberGenerator.Create())
            {
                generador.GetBytes(salt);
            }

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hash;
            using (Rfc2898DeriveBytes derivador = new Rfc2898DeriveBytes(
                passwordBytes, salt, iteraciones, HashAlgorithmName.SHA256))
            {
                hash = derivador.GetBytes(32);
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                "PBKDF2-SHA256${0}${1}${2}",
                iteraciones,
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash));
        }
    }
}
