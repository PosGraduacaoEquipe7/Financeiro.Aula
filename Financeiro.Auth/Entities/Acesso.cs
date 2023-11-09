namespace Financeiro.Auth.Entities
{
    public class Acesso
    {
        public long Id { get; private set; }
        public DateTime DataHoraAcesso { get; private set; }
        public string RefreshToken { get; private set; }
        public DateTime ExpiracaoRefreshToken { get; private set; }
        public bool Encerrado { get; private set; }
        public long UsuarioId { get; private set; }
        public virtual Usuario Usuario { get; private set; } = null!;

        public Acesso(long id, DateTime dataHoraAcesso, string refreshToken, DateTime expiracaoRefreshToken, bool encerrado, long usuarioId)
        {
            Id = id;
            DataHoraAcesso = dataHoraAcesso;
            RefreshToken = refreshToken;
            ExpiracaoRefreshToken = expiracaoRefreshToken;
            Encerrado = encerrado;
            UsuarioId = usuarioId;
        }

        public void AtualizarRefreshToken(string refreshToken, DateTime expiracaoRefreshToken)
        {
            RefreshToken = refreshToken;
            ExpiracaoRefreshToken = expiracaoRefreshToken;
        }

        public bool RefreshTokenEhValido(string refreshToken)
        {
            return
                RefreshToken == refreshToken
                && ExpiracaoRefreshToken >= DateTime.Now
                && !Encerrado;
        }
    }
}