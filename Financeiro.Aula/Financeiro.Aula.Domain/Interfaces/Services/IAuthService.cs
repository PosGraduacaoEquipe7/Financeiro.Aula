namespace Financeiro.Aula.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        bool UsuarioLogado { get; }
        string UsuarioId { get; }
        string NomeUsuario { get; }
    }
}