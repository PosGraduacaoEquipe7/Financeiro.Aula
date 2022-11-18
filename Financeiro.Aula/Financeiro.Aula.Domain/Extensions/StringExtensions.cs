using System.Text;

namespace Financeiro.Aula.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string EncodeToBase64(this string text)
        {
            try
            {
                byte[] textAsBytes = Encoding.ASCII.GetBytes(text);
                return Convert.ToBase64String(textAsBytes);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //converte de base64 para texto
        public static string DecodeFrom64(this string data)
        {
            try
            {
                byte[] dataAsBytes = Convert.FromBase64String(data);
                return Encoding.ASCII.GetString(dataAsBytes);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}