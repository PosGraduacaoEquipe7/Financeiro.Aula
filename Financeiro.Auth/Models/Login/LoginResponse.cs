namespace Financeiro.Auth.Models.Login
{
    public class LoginResponse
    {
        public bool Sucesso { get; private set; }
        public string? Token { get; private set; }

        public LoginResponse(bool sucesso, string? token)
        {
            Sucesso = sucesso;
            Token = token;
        }

        public static LoginResponse Sucedido(string token) => new LoginResponse(true, token);

        public static LoginResponse MalSucedido() => new LoginResponse(false, null);
    }
}