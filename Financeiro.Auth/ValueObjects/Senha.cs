namespace Financeiro.Auth.ValueObjects
{
    public class Senha
    {
        private const int PASSWORD_SALT = 12;

        public string SenhaCifrada { get; private set; }

        private Senha()
        {
            SenhaCifrada = string.Empty;
        }

        private Senha(string value)
        {
            SenhaCifrada = BCrypt.Net.BCrypt.HashPassword(value, BCrypt.Net.BCrypt.GenerateSalt(PASSWORD_SALT));
        }

        public static implicit operator Senha(string value)
        {
            return new Senha(value);
        }

        public bool Check(string senha)
        {
            return BCrypt.Net.BCrypt.Verify(senha, SenhaCifrada);
        }

        public override string ToString()
        {
            return SenhaCifrada;
        }

        public static Senha FromHashed(string senhaHashed)
        {
            return new Senha() { SenhaCifrada = senhaHashed };
        }
    }
}