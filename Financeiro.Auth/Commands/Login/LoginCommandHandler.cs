using Financeiro.Auth.Interfaces.Repositories;
using Financeiro.Auth.Interfaces.Services;
using Financeiro.Auth.Models.Login;
using Financeiro.Auth.Services;
using MediatR;

namespace Financeiro.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUsuarioRepository _repository;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IUsuarioRepository repository, ITokenService tokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var usuario = await _repository.ObterUsuario(command.Request.Email, command.Request.Senha);

            if (usuario == null)
                return LoginResponse.MalSucedido();

            var token = _tokenService.GenerateToken(usuario);

            return LoginResponse.Sucedido(token);
        }
    }
}