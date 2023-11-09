namespace Financeiro.Auth.Models.RefreshToken
{
    public class RefreshTokenResponse
    {
        public bool Sucesso { get; private set; }
        public string? Token { get; private set; }
        public string? RefreshToken { get; private set; }

        public RefreshTokenResponse(bool sucesso, string? token, string? refreshToken)
        {
            Sucesso = sucesso;
            Token = token;
            RefreshToken = refreshToken;
        }

        public static RefreshTokenResponse Sucedido(string token, string? refreshToken) => new RefreshTokenResponse(true, token, refreshToken);

        public static RefreshTokenResponse MalSucedido() => new RefreshTokenResponse(false, null, null);
    }
}