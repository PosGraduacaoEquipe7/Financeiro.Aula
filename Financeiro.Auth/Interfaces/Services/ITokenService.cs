using Financeiro.Auth.Entities;

namespace Financeiro.Auth.Interfaces.Services
{
    public interface ITokenService
    {
         string GenerateToken(Usuario usuario);
    }
}