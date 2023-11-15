namespace Financeiro.Auth.Interfaces.Services
{
    public interface ITokenService
    {
        string GerarToken(string identifier, string usuarioId, string nomeUsuario, string role);

        long? ObterIdentifierDoToken(string token);
    }
}