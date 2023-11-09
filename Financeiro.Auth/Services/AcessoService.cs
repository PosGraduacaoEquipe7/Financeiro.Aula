using Financeiro.Auth.Entities;
using Financeiro.Auth.Interfaces.Repositories;
using Financeiro.Auth.Interfaces.Services;
using System.Security.Cryptography;

namespace Financeiro.Auth.Services
{
    public class AcessoService : IAcessoService
    {
        private readonly IAcessoRepository _acessoRepository;

        private readonly TimeSpan TEMPO_EXPIRACAO_TOKEN = TimeSpan.FromDays(30);

        public AcessoService(IAcessoRepository acessoRepository)
        {
            _acessoRepository = acessoRepository;
        }

        public async Task<Acesso> GerarAcesso(Usuario usuario)
        {
            var refreshToken = GerarRefreshToken();

            var acesso = new Acesso(
                    0,
                    DateTime.Now,
                    refreshToken,
                    DateTime.Now.Add(TEMPO_EXPIRACAO_TOKEN),
                    false,
                    usuario.Id);

            await _acessoRepository.Incluir(acesso);

            return acesso;
        }

        public async Task AtualizarRefreshToken(Acesso acesso)
        {
            var refreshToken = GerarRefreshToken();

            acesso.AtualizarRefreshToken(
                refreshToken,
                DateTime.Now.Add(TEMPO_EXPIRACAO_TOKEN));

            await _acessoRepository.Alterar(acesso);
        }

        private static string GerarRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}