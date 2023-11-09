using Financeiro.Auth.Interfaces.Repositories;
using Financeiro.Auth.Interfaces.Services;
using Financeiro.Auth.Models.RefreshToken;
using MediatR;

namespace Financeiro.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IAcessoRepository _acessoRepository;
        private readonly ITokenService _tokenService;
        private readonly IAcessoService _acessoService;

        public RefreshTokenCommandHandler(IAcessoRepository acessoRepository, ITokenService tokenService, IAcessoService acessoService)
        {
            _acessoRepository = acessoRepository;
            _tokenService = tokenService;
            _acessoService = acessoService;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var idDoToken = _tokenService.ObterIdentifierDoToken(command.Request.Token);
            if (idDoToken == null)
            {
                return RefreshTokenResponse.MalSucedido();
            }

            var acesso = await _acessoRepository.ObterPeloId(idDoToken.Value);
            if (acesso == null)
            {
                return RefreshTokenResponse.MalSucedido();
            }

            if (!acesso.RefreshTokenEhValido(command.Request.RefreshToken))
            {
                return RefreshTokenResponse.MalSucedido();
            }

            await _acessoService.AtualizarRefreshToken(acesso);

            var usuario = acesso.Usuario;

            var token = _tokenService.GerarToken(
                acesso.Id.ToString(),
                usuario.Nome,
                usuario.Role);

            return RefreshTokenResponse.Sucedido(token, acesso.RefreshToken);
        }
    }
}