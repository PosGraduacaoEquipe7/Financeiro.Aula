using Financeiro.Aula.Domain.Interfaces.Services;
using System.Security.Claims;

namespace Financeiro.Aula.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpContext _context;

        public bool UsuarioLogado => _context.User.Identity?.IsAuthenticated ?? false;

        public string UsuarioId => _context.User.Identity?.Name ?? throw new Exception("Usuário não logado");

        public string NomeUsuario => _context.User.FindFirst(ClaimTypes.GivenName)?.Value ?? throw new Exception("Usuário não logado");

        public AuthService(IHttpContextAccessor contextAccessor)
        {
            if (contextAccessor.HttpContext is null)
                throw new NullReferenceException($"{nameof(contextAccessor)} nulo");

            _context = contextAccessor.HttpContext;
        }
    }
}