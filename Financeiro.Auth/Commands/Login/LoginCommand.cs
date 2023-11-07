using Financeiro.Auth.Models.Login;
using MediatR;

namespace Financeiro.Auth.Commands.Login
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public LoginRequest Request { get; private set; }

        public LoginCommand(LoginRequest request)
        {
            Request = request;
        }
    }
}
