using System.Text.RegularExpressions;

namespace Financeiro.Commom.ValueObjects
{
    public class Cep : IComparable<Cep>
    {
        public Cep(string valor)
        {
            _somenteNumeros = valor.Replace("-", "").Replace(".", "");
        }

        private readonly string _somenteNumeros;

        public string SomenteNumeros => _somenteNumeros;

        public string Formatado =>
            _somenteNumeros.Length != 8
                ? _somenteNumeros
                : $"{_somenteNumeros[0..5]}-{_somenteNumeros[5..3]}";

        public bool Valido => Validar(_somenteNumeros);

        public override string ToString() => Formatado;

        private static bool Validar(string cep)
        {
            Regex regex = new Regex(@"^\d{8}$");
            return regex.IsMatch(cep);
        }

        public int CompareTo(Cep? other)
        {
            if (other == null) return 0;

            return other.SomenteNumeros.CompareTo(SomenteNumeros);
        }

        public static implicit operator Cep(string cep)
        {
            return new Cep(cep);
        }
    }
}
