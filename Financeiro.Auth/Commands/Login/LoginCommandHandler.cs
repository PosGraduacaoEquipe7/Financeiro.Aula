using Financeiro.Auth.Interfaces.Repositories;
using Financeiro.Auth.Interfaces.Services;
using Financeiro.Auth.Models.Login;
using MediatR;

namespace Financeiro.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUsuarioRepository _repository;
        private readonly ITokenService _tokenService;
        private readonly IAcessoService _acessoService;

        public LoginCommandHandler(IUsuarioRepository repository, ITokenService tokenService, IAcessoService acessoService)
        {
            _repository = repository;
            _tokenService = tokenService;
            _acessoService = acessoService;
        }

        public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var usuario = await _repository.ObterUsuarioPeloEmail(command.Request.Email);

            if (usuario == null || !usuario.Senha.Check(command.Request.Senha))
                return LoginResponse.MalSucedido();

            var acesso = await _acessoService.GerarAcesso(usuario);

            var token = _tokenService.GerarToken(
                acesso.Id.ToString(),
                usuario.Id.ToString(),
                usuario.Nome,
                usuario.Role);

            return LoginResponse.Sucedido(token, acesso.RefreshToken);
        }
    }
}