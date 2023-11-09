namespace Financeiro.Auth.Models.Login
{
    public class LoginResponse
    {
        public bool Sucesso { get; private set; }
        public string? Token { get; private set; }
        public string? RefreshToken { get; private set; }

        public LoginResponse(bool sucesso, string? token, string? refreshToken)
        {
            Sucesso = sucesso;
            Token = token;
            RefreshToken = refreshToken;
        }

        public static LoginResponse Sucedido(string token, string? refreshToken) => new LoginResponse(true, token, refreshToken);

        public static LoginResponse MalSucedido() => new LoginResponse(false, null, null);
    }
}